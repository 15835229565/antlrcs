﻿/*
 * [The "BSD licence"]
 * Copyright (c) 2005-2008 Terence Parr
 * All rights reserved.
 *
 * Conversion to C#:
 * Copyright (c) 2008 Sam Harwell, Pixel Mine, Inc.
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the
 *    documentation and/or other materials provided with the distribution.
 * 3. The name of the author may not be used to endorse or promote products
 *    derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
 * IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 * DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 * THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

namespace Antlr3.Analysis
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Antlr.Runtime.JavaExtensions;
    using Antlr3.Tool;

    using CodeGenerator = Antlr3.Codegen.CodeGenerator;
    using IIntSet = Antlr3.Misc.IIntSet;
    using IIntStream = Antlr.Runtime.IIntStream;
    using IList = System.Collections.IList;
    using IntervalSet = Antlr3.Misc.IntervalSet;
    using StringBuilder = System.Text.StringBuilder;
    using StringTemplate = Antlr3.ST.StringTemplate;

    /** A DFA (converted from a grammar's NFA).
     *  DFAs are used as prediction machine for alternative blocks in all kinds
     *  of recognizers (lexers, parsers, tree walkers).
     */
    public class DFA
    {
        public const int REACHABLE_UNKNOWN = -2;
        public const int REACHABLE_BUSY = -1; // in process of computing
        public const int REACHABLE_NO = 0;
        public const int REACHABLE_YES = 1;

        /** Prevent explosion of DFA states during conversion. The max number
         *  of states per alt in a single decision's DFA.
        public const int MAX_STATES_PER_ALT_IN_DFA = 450;
         */

        /** Set to 0 to not terminate early (time in ms) */
        public static TimeSpan MAX_TIME_PER_DFA_CREATION = TimeSpan.FromSeconds( 1 );

        /** How many edges can each DFA state have before a "special" state
         *  is created that uses IF expressions instead of a table?
         */
        public static int MAX_STATE_TRANSITIONS_FOR_TABLE = 65534;

        /** What's the start state for this DFA? */
        public DFAState startState;

        /** This DFA is being built for which decision? */
        public int decisionNumber = 0;

        /** From what NFAState did we create the DFA? */
        public NFAState decisionNFAStartState;

        /** The printable grammar fragment associated with this DFA */
        public String description;

        /** A set of all uniquely-numbered DFA states.  Maps hash of DFAState
         *  to the actual DFAState object.  We use this to detect
         *  existing DFA states.  Map<DFAState,DFAState>.  Use Map so
         *  we can get old state back (Set only allows you to see if it's there).
         *  Not used during fixed k lookahead as it's a waste to fill it with
         *  a dup of states array.
         */
        protected IDictionary<DFAState, DFAState> uniqueStates = new Dictionary<DFAState, DFAState>();

        /** Maps the state number to the actual DFAState.  Use a Vector as it
         *  grows automatically when I set the ith element.  This contains all
         *  states, but the states are not unique.  s3 might be same as s1 so
         *  s3 -> s1 in this table.  This is how cycles occur.  If fixed k,
         *  then these states will all be unique as states[i] always points
         *  at state i when no cycles exist.
         *
         *  This is managed in parallel with uniqueStates and simply provides
         *  a way to go from state number to DFAState rather than via a
         *  hash lookup.
         */
        protected List<DFAState> states = new List<DFAState>();

        /** Unique state numbers per DFA */
        protected int stateCounter = 0;

        /** count only new states not states that were rejected as already present */
        protected int numberOfStates = 0;

        /** User specified max fixed lookahead.  If 0, nothing specified.  -1
         *  implies we have not looked at the options table yet to set k.
         */
        protected int user_k = -1;

        /** While building the DFA, track max lookahead depth if not cyclic */
        protected internal int max_k = -1;

        /** Is this DFA reduced?  I.e., can all states lead to an accept state? */
        protected bool reduced = true;

        /** Are there any loops in this DFA?
         *  Computed by doesStateReachAcceptState()
         */
        protected bool cyclic = false;

        /** Track whether this DFA has at least one sem/syn pred encountered
         *  during a closure operation.  This is useful for deciding whether
         *  to retry a non-LL(*) with k=1.  If no pred, it will not work w/o
         *  a pred so don't bother.  It would just give another error message.
         */
        public bool predicateVisible = false;

        public bool hasPredicateBlockedByAction = false;

        /** Each alt in an NFA derived from a grammar must have a DFA state that
         *  predicts it lest the parser not know what to do.  Nondeterminisms can
         *  lead to this situation (assuming no semantic predicates can resolve
         *  the problem) and when for some reason, I cannot compute the lookahead
         *  (which might arise from an error in the algorithm or from
         *  left-recursion etc...).  This list starts out with all alts contained
         *  and then in method doesStateReachAcceptState() I remove the alts I
         *  know to be uniquely predicted.
         */
        protected List<int> unreachableAlts;

        protected int nAlts = 0;

        /** We only want one accept state per predicted alt; track here */
        protected DFAState[] altToAcceptState;

        /** Track whether an alt discovers recursion for each alt during
         *  NFA to DFA conversion; >1 alt with recursion implies nonregular.
         */
        public IIntSet recursiveAltSet = new IntervalSet();

        /** Which NFA are we converting (well, which piece of the NFA)? */
        public NFA nfa;

        protected NFAToDFAConverter nfaConverter;

        /** This probe tells you a lot about a decision and is useful even
         *  when there is no error such as when a syntactic nondeterminism
         *  is solved via semantic predicates.  Perhaps a GUI would want
         *  the ability to show that.
         */
        public DecisionProbe probe;

        /** Track absolute time of the conversion so we can have a failsafe:
         *  if it takes too long, then terminate.  Assume bugs are in the
         *  analysis engine.
         */
        protected internal DateTime conversionStartTime;

        /** Map an edge transition table to a unique set number; ordered so
         *  we can push into the output template as an ordered list of sets
         *  and then ref them from within the transition[][] table.  Like this
         *  for C# target:
         *     public static readonly DFA30_transition0 =
         *     	new short[] { 46, 46, -1, 46, 46, -1, -1, -1, -1, -1, -1, -1,...};
         *         public static readonly DFA30_transition1 =
         *     	new short[] { 21 };
         *      public static readonly short[][] DFA30_transition = {
         *     	  DFA30_transition0,
         *     	  DFA30_transition0,
         *     	  DFA30_transition1,
         *     	  ...
         *      };
         */
        public IDictionary<int[], int?> edgeTransitionClassMap = new Dictionary<int[], int?>();

        /** The unique edge transition class number; every time we see a new
         *  set of edges emanating from a state, we number it so we can reuse
         *  if it's every seen again for another state.  For Java grammar,
         *  some of the big edge transition tables are seen about 57 times.
         */
        protected int edgeTransitionClass = 0;

        /* This DFA can be converted to a transition[state][char] table and
         * the following tables are filled by createStateTables upon request.
         * These are injected into the templates for code generation.
         * See March 25, 2006 entry for description:
         *   http://www.antlr.org/blog/antlr3/codegen.tml
         * Often using Vector as can't set ith position in a List and have
         * it extend list size; bizarre.
         */

        /** List of special DFAState objects */
        public IList specialStates;
        /** List of ST for special states. */
        public IList specialStateSTs;
        public const int EmptyValue = -1;
        public int[] accept;
        public int[] eot;
        public int[] eof;
        public int[] min;
        public int[] max;
        public int[] special;
        public int[][] transition;
        /** just the Vector<Integer> indicating which unique edge table is at
         *  position i.
         */
        public List<int?> transitionEdgeTables; // not used by java yet
        protected int uniqueCompressedSpecialStateNum = 0;

        /** Which generator to use if we're building state tables */
        protected CodeGenerator generator = null;

        protected DFA()
        {
            probe = new DecisionProbe( this );
        }

        public DFA( int decisionNumber, NFAState decisionStartState )
            : this()
        {
            this.decisionNumber = decisionNumber;
            this.decisionNFAStartState = decisionStartState;
            nfa = decisionStartState.nfa;
            nAlts = nfa.grammar.getNumberOfAltsForDecisionNFA( decisionStartState );
            //setOptions( nfa.grammar.getDecisionOptions(getDecisionNumber()) );
            initAltRelatedInfo();

            //long start = JSystem.currentTimeMillis();
            nfaConverter = new NFAToDFAConverter( this );
            try
            {
                nfaConverter.convert();

                // figure out if there are problems with decision
                verify();

                if ( !probe.IsDeterministic || probe.AnalysisOverflowed )
                {
                    probe.issueWarnings();
                }

                // must be after verify as it computes cyclic, needed by this routine
                // should be after warnings because early termination or something
                // will not allow the reset to operate properly in some cases.
                resetStateNumbersToBeContiguous();

                //long stop = JSystem.currentTimeMillis();
                //JSystem.@out.println("verify cost: "+(int)(stop-start)+" ms");
            }
            catch ( AnalysisTimeoutException /*at*/ )
            {
                probe.reportAnalysisTimeout();
                if ( !OkToRetryWithK1 )
                {
                    probe.issueWarnings();
                }
            }
            catch ( NonLLStarDecisionException /*nonLL*/ )
            {
                probe.reportNonLLStarDecision( this );
                // >1 alt recurses, k=* and no auto backtrack nor manual sem/syn
                if ( !OkToRetryWithK1 )
                {
                    probe.issueWarnings();
                }
            }
        }

        #region Properties
        public bool AnalysisTimedOut
        {
            get
            {
                return probe.AnalysisTimedOut;
            }
        }
        public bool CanInlineDecision
        {
            get
            {
                return !IsCyclic &&
                    !probe.IsNonLLStarDecision &&
                    NumberOfStates < CodeGenerator.MAX_ACYCLIC_DFA_STATES_INLINE;
            }
        }
        public bool AutoBacktrackMode
        {
            get
            {
                return getAutoBacktrackMode();
            }
        }
        public GrammarAST DecisionASTNode
        {
            get
            {
                return decisionNFAStartState.associatedASTNode;
            }
        }
        public int DecisionNumber
        {
            get
            {
                return decisionNFAStartState.DecisionNumber;
            }
        }
        public string Description
        {
            get
            {
                return description;
            }
        }
        public bool IsCyclic
        {
            get
            {
                return cyclic && UserMaxLookahead == 0;
            }
        }
        public bool IsGreedy
        {
            get
            {
                return isGreedy();
            }
        }
        public bool IsReduced
        {
            get
            {
                return reduced;
            }
        }
        public bool IsTokensRuleDecision
        {
            get
            {
                return isTokensRuleDecision();
            }
        }
        public int MaxLookaheadDepth
        {
            get
            {
                if ( IsCyclic )
                {
                    return int.MaxValue;
                }
                return max_k;
            }
        }
        public int MaxStateNumber
        {
            get
            {
                return states.Count - 1;
            }
        }
        public NFAState NFADecisionStartState
        {
            get
            {
                return decisionNFAStartState;
            }
        }
        public int NumberOfAlts
        {
            get
            {
                return nAlts;
            }
        }
        public int NumberOfStates
        {
            get
            {
                if ( UserMaxLookahead > 0 )
                {
                    // if using fixed lookahead then uniqueSets not set
                    return states.Count;
                }
                return numberOfStates;
            }
        }
        public bool OkToRetryWithK1
        {
            get
            {
                return okToRetryDFAWithK1();
            }
        }
        public string ReasonForFailure
        {
            get
            {
                return getReasonForFailure();
            }
        }
        public IDictionary<DFAState, DFAState> UniqueStates
        {
            get
            {
                return uniqueStates;
            }
        }
        public ICollection<int> UnreachableAlts
        {
            get
            {
                return unreachableAlts;
            }
        }
        public int UserMaxLookahead
        {
            get
            {
                return getUserMaxLookahead();
            }
        }
        #endregion

        /** Walk all states and reset their numbers to be a contiguous sequence
         *  of integers starting from 0.  Only cyclic DFA can have unused positions
         *  in states list.  State i might be identical to a previous state j and
         *  will result in states[i] == states[j].  We don't want to waste a state
         *  number on this.  Useful mostly for code generation in tables.
         *
         *  At the start of this routine, states[i].stateNumber &lt;= i by definition.
         *  If states[50].stateNumber is 50 then a cycle during conversion may
         *  try to add state 103, but we find that an identical DFA state, named
         *  50, already exists, hence, states[103]==states[50] and both have
         *  stateNumber 50 as they point at same object.  Afterwards, the set
         *  of state numbers from all states should represent a contiguous range
         *  from 0..n-1 where n is the number of unique states.
         */
        public virtual void resetStateNumbersToBeContiguous()
        {
            if ( UserMaxLookahead > 0 )
            {
                // all numbers are unique already; no states are thrown out.
                return;
            }

            // walk list of DFAState objects by state number,
            // setting state numbers to 0..n-1
            int snum = 0;
            for ( int i = 0; i <= MaxStateNumber; i++ )
            {
                DFAState s = getState( i );
                // some states are unused after creation most commonly due to cycles
                // or conflict resolution.
                if ( s == null )
                {
                    continue;
                }
                // state i is mapped to DFAState with state number set to i originally
                // so if it's less than i, then we renumbered it already; that
                // happens when states have been merged or cycles occurred I think.
                // states[50] will point to DFAState with s50 in it but
                // states[103] might also point at this same DFAState.  Since
                // 50 < 103 then it's already been renumbered as it points downwards.
                bool alreadyRenumbered = s.stateNumber < i;
                if ( !alreadyRenumbered )
                {
                    // state i is a valid state, reset it's state number
                    s.stateNumber = snum; // rewrite state numbers to be 0..n-1
                    snum++;
                }
            }
            if ( snum != NumberOfStates )
            {
                ErrorManager.internalError( "DFA " + decisionNumber + ": " +
                    decisionNFAStartState.Description + " num unique states " + NumberOfStates +
                    "!= num renumbered states " + snum );
            }
        }

        // JAVA-SPECIFIC Accessors!!!!!  It is so impossible to get arrays
        // or even consistently formatted strings acceptable to java that
        // I am forced to build the individual char elements here

        public virtual List<string> getJavaCompressedAccept()
        {
            return getRunLengthEncoding( accept );
        }
        public virtual List<string> getJavaCompressedEOT()
        {
            return getRunLengthEncoding( eot );
        }
        public virtual List<string> getJavaCompressedEOF()
        {
            return getRunLengthEncoding( eof );
        }
        public virtual List<string> getJavaCompressedMin()
        {
            return getRunLengthEncoding( min );
        }
        public virtual List<string> getJavaCompressedMax()
        {
            return getRunLengthEncoding( max );
        }
        public virtual List<string> getJavaCompressedSpecial()
        {
            return getRunLengthEncoding( special );
        }
        public virtual List<List<string>> getJavaCompressedTransition()
        {
            if ( transition == null || transition.Length == 0 )
            {
                return null;
            }
            List<List<string>> encoded = new List<List<string>>( transition.Length );
            // walk Vector<Vector<FormattedInteger>> which is the transition[][] table
            for ( int i = 0; i < transition.Length; i++ )
            {
                var transitionsForState = transition[i];
                encoded.Add( getRunLengthEncoding( transitionsForState ) );
            }
            return encoded;
        }

        /** Compress the incoming data list so that runs of same number are
         *  encoded as number,value pair sequences.  3 -1 -1 -1 28 is encoded
         *  as 1 3 3 -1 1 28.  I am pretty sure this is the lossless compression
         *  that GIF files use.  Transition tables are heavily compressed by
         *  this technique.  I got the idea from JFlex http://jflex.de/
         *
         *  Return List&lt;String&gt; where each string is either \xyz for 8bit char
         *  and \uFFFF for 16bit.  Hideous and specific to Java, but it is the
         *  only target bad enough to need it.
         */
        public virtual List<string> getRunLengthEncoding( int[] data )
        {
            if ( data == null || data.Length == 0 )
            {
                // for states with no transitions we want an empty string ""
                // to hold its place in the transitions array.
                List<string> empty = new List<string>();
                empty.Add( "" );
                return empty;
            }
            int size = Math.Max( 2, data.Length / 2 );
            List<string> encoded = new List<string>( size ); // guess at size
            // scan values looking for runs
            int i = 0;
            while ( i < data.Length )
            {
                int I = data[i];
                //if ( I == null )
                //{
                //    I = emptyValue;
                //}

                // count how many v there are?
                int n = 0;
                for ( int j = i; j < data.Length; j++ )
                {
                    int v = data[j];
                    //if ( v == null )
                    //{
                    //    v = emptyValue;
                    //}
                    if ( I.Equals( v ) )
                    {
                        n++;
                    }
                    else
                    {
                        break;
                    }
                }
                encoded.Add( generator.target.encodeIntAsCharEscape( (char)n ) );
                encoded.Add( generator.target.encodeIntAsCharEscape( (char)(int)I ) );
                i += n;
            }
            return encoded;
        }

        public virtual void createStateTables( CodeGenerator generator )
        {
            //JSystem.@out.println("createTables:\n"+this);
            this.generator = generator;
            description = NFADecisionStartState.Description;
            description =
                generator.target.getTargetStringLiteralFromString( description );

            // create all the tables
            //special = new List<int>( this.NumberOfStates ); // Vector<short>
            //special.setSize( this.NumberOfStates );
            special = Enumerable.Repeat( EmptyValue, NumberOfStates ).ToArray();
            specialStates = new List<object>();				// List<DFAState>
            specialStateSTs = new List<object>();				// List<ST>
            //accept = new List<int>( this.NumberOfStates ); // Vector<int>
            //accept.setSize( this.NumberOfStates );
            accept = Enumerable.Repeat( EmptyValue, NumberOfStates ).ToArray();
            //eot = new List<int>( this.NumberOfStates ); // Vector<int>
            //eot.setSize( this.NumberOfStates );
            eot = Enumerable.Repeat( EmptyValue, NumberOfStates ).ToArray();
            //eof = new List<int>( this.NumberOfStates ); // Vector<int>
            //eof.setSize( this.NumberOfStates );
            eof = Enumerable.Repeat( EmptyValue, NumberOfStates ).ToArray();
            //min = new List<int>( this.NumberOfStates ); // Vector<int>
            //min.setSize( this.NumberOfStates );
            min = Enumerable.Repeat( EmptyValue, NumberOfStates ).ToArray();
            //max = new List<int>( this.NumberOfStates ); // Vector<int>
            //max.setSize( this.NumberOfStates );
            max = Enumerable.Repeat( EmptyValue, NumberOfStates ).ToArray();
            transition = new int[NumberOfStates][]; // Vector<Vector<int>>
            //transition.setSize( this.NumberOfStates );
            transitionEdgeTables = new List<int?>( this.NumberOfStates ); // Vector<Vector<int>>
            transitionEdgeTables.setSize( this.NumberOfStates );

            // for each state in the DFA, fill relevant tables.
            IEnumerable<DFAState> it = null;
            if ( UserMaxLookahead > 0 )
            {
                it = states;
            }
            else
            {
                it = UniqueStates.Values;
            }
            foreach ( DFAState s in it )
            {
                if ( s == null )
                {
                    // ignore null states; some acylic DFA see this condition
                    // when inlining DFA (due to lacking of exit branch pruning?)
                    continue;
                }
                if ( s.IsAcceptState )
                {
                    // can't compute min,max,special,transition on accepts
                    accept[s.stateNumber] = s.getUniquelyPredictedAlt();
                }
                else
                {
                    createMinMaxTables( s );
                    createTransitionTableEntryForState( s );
                    createSpecialTable( s );
                    createEOTAndEOFTables( s );
                }
            }

            // now that we have computed list of specialStates, gen code for 'em
            for ( int i = 0; i < specialStates.Count; i++ )
            {
                DFAState ss = (DFAState)specialStates[i];
                StringTemplate stateST =
                    generator.generateSpecialState( ss );
                specialStateSTs.Add( stateST );
            }

            // check that the tables are not messed up by encode/decode
            /*
            testEncodeDecode(min);
            testEncodeDecode(max);
            testEncodeDecode(accept);
            testEncodeDecode(special);
            JSystem.@out.println("min="+min);
            JSystem.@out.println("max="+max);
            JSystem.@out.println("eot="+eot);
            JSystem.@out.println("eof="+eof);
            JSystem.@out.println("accept="+accept);
            JSystem.@out.println("special="+special);
            JSystem.@out.println("transition="+transition);
            */
        }

#if false
        private void testEncodeDecode( int[] data )
        {
            JSystem.@out.println( "data=" + data );
            var encoded = getRunLengthEncoding( data );
            StringBuilder buf = new StringBuilder();
            for ( int i = 0; i < encoded.size(); i++ )
            {
                String I = (String)encoded.get( i );
                int v = 0;
                if ( I.startsWith( "\\u" ) )
                {
                    v = int.Parse( I.substring( 2, I.length() ), NumberStyles.HexNumber );
                }
                else
                {
                    v = int.Parse( I.substring( 1, I.length() ), System.Globalization.NumberStyles.Octal );
                }
                buf.append( (char)v );
            }
            String encodedS = buf.ToString();
            short[] decoded = Antlr.Runtime.DFA.UnpackEncodedString( encodedS );
            //JSystem.@out.println("decoded:");
            for ( int i = 0; i < decoded.Length; i++ )
            {
                short x = decoded[i];
                if ( x != data[i] )
                {
                    Console.Error.WriteLine( "problem with encoding" );
                }
                //JSystem.@out.print(", "+x);
            }
            //JSystem.@out.println();
        }
#endif

        protected virtual void createMinMaxTables( DFAState s )
        {
            int smin = Label.MAX_CHAR_VALUE + 1;
            int smax = Label.MIN_ATOM_VALUE - 1;
            for ( int j = 0; j < s.NumberOfTransitions; j++ )
            {
                Transition edge = (Transition)s.transition( j );
                Label label = edge.label;
                if ( label.IsAtom )
                {
                    if ( label.Atom >= Label.MIN_CHAR_VALUE )
                    {
                        if ( label.Atom < smin )
                        {
                            smin = label.Atom;
                        }
                        if ( label.Atom > smax )
                        {
                            smax = label.Atom;
                        }
                    }
                }
                else if ( label.IsSet )
                {
                    IntervalSet labels = (IntervalSet)label.Set;
                    int lmin = labels.getMinElement();
                    // if valid char (don't do EOF) and less than current min
                    if ( lmin < smin && lmin >= Label.MIN_CHAR_VALUE )
                    {
                        smin = labels.getMinElement();
                    }
                    if ( labels.getMaxElement() > smax )
                    {
                        smax = labels.getMaxElement();
                    }
                }
            }

            if ( smax < 0 )
            {
                // must be predicates or pure EOT transition; just zero out min, max
                smin = Label.MIN_CHAR_VALUE;
                smax = Label.MIN_CHAR_VALUE;
            }

            min[s.stateNumber] = (char)smin;
            max[s.stateNumber] = (char)smax;

            if ( smax < 0 || smin > Label.MAX_CHAR_VALUE || smin < 0 )
            {
                ErrorManager.internalError( "messed up: min=" + min + ", max=" + max );
            }
        }

        protected virtual void createTransitionTableEntryForState( DFAState s )
        {
            /*
            JSystem.@out.println("createTransitionTableEntryForState s"+s.stateNumber+
                " dec "+s.dfa.decisionNumber+" cyclic="+s.dfa.isCyclic());
                */
            int smax = max[s.stateNumber];
            int smin = min[s.stateNumber];

            int[] stateTransitions = new int[smax - smin + 1];
            for ( int i = 0; i < stateTransitions.Length; i++ )
                stateTransitions[i] = EmptyValue;

            transition[s.stateNumber] = stateTransitions;
            for ( int j = 0; j < s.NumberOfTransitions; j++ )
            {
                Transition edge = s.transition( j );
                Label label = edge.label;
                if ( label.IsAtom && label.Atom >= Label.MIN_CHAR_VALUE )
                {
                    int labelIndex = label.Atom - smin; // offset from 0
                    stateTransitions[labelIndex] = edge.target.stateNumber;
                }
                else if ( label.IsSet )
                {
                    foreach ( var interval in ((IntervalSet)label.Set).Intervals )
                    {
                        for ( int i = Math.Max( interval.a, Label.MIN_CHAR_VALUE ); i <= interval.b; i++ )
                        {
                            stateTransitions[i - smin] = edge.target.stateNumber;
                        }
                    }
                }
            }
            // track unique state transition tables so we can reuse
            int? edgeClass; // = edgeTransitionClassMap.get( stateTransitions );
            if ( edgeTransitionClassMap.TryGetValue( stateTransitions, out edgeClass ) && edgeClass != null )
            {
                //JSystem.@out.println("we've seen this array before; size="+stateTransitions.size());
                transitionEdgeTables[s.stateNumber] = edgeClass;
            }
            else
            {
                edgeClass = edgeTransitionClass;
                transitionEdgeTables[s.stateNumber] = edgeClass;
                edgeTransitionClassMap[stateTransitions] = edgeClass;
                edgeTransitionClass++;
            }
        }

        /** Set up the EOT and EOF tables; we cannot put -1 min/max values so
         *  we need another way to test that in the DFA transition function.
         */
        protected virtual void createEOTAndEOFTables( DFAState s )
        {
            for ( int j = 0; j < s.NumberOfTransitions; j++ )
            {
                Transition edge = s.transition( j );
                Label label = edge.label;
                if ( label.IsAtom )
                {
                    if ( label.Atom == Label.EOT )
                    {
                        // eot[s] points to accept state
                        eot[s.stateNumber] = edge.target.stateNumber;
                    }
                    else if ( label.Atom == Label.EOF )
                    {
                        // eof[s] points to accept state
                        eof[s.stateNumber] = edge.target.stateNumber;
                    }
                }
                else if ( label.IsSet )
                {
                    if ( label.Set.member( Label.EOT ) )
                    {
                        eot[s.stateNumber] = edge.target.stateNumber;
                    }

                    if ( label.Set.member( Label.EOF ) )
                    {
                        eof[s.stateNumber] = edge.target.stateNumber;
                    }
                }
            }
        }

        protected virtual void createSpecialTable( DFAState s )
        {
            // number all special states from 0...n-1 instead of their usual numbers
            bool hasSemPred = false;

            // TODO this code is very similar to canGenerateSwitch.  Refactor to share
            for ( int j = 0; j < s.NumberOfTransitions; j++ )
            {
                Transition edge = (Transition)s.transition( j );
                Label label = edge.label;
                // can't do a switch if the edges have preds or are going to
                // require gated predicates
                if ( label.IsSemanticPredicate ||
                     ( (DFAState)edge.target ).getGatedPredicatesInNFAConfigurations() != null )
                {
                    hasSemPred = true;
                    break;
                }
            }
            // if has pred or too big for table, make it special
            int smax = max[s.stateNumber];
            int smin = min[s.stateNumber];
            if ( hasSemPred || smax - smin > MAX_STATE_TRANSITIONS_FOR_TABLE )
            {
                special[s.stateNumber] = uniqueCompressedSpecialStateNum;
                uniqueCompressedSpecialStateNum++;
                specialStates.Add( s );
            }
            else
            {
                special[s.stateNumber] = EmptyValue; // not special
            }
        }

        public virtual int predict( IIntStream input )
        {
            Interpreter interp = new Interpreter( nfa.grammar, input );
            return interp.predict( this );
        }

        /** Add a new DFA state to this DFA if not already present.
         *  To force an acyclic, fixed maximum depth DFA, just always
         *  return the incoming state.  By not reusing old states,
         *  no cycles can be created.  If we're doing fixed k lookahead
         *  don't updated uniqueStates, just return incoming state, which
         *  indicates it's a new state.
         */
        protected internal virtual DFAState addState( DFAState d )
        {
            if ( UserMaxLookahead > 0 )
            {
                return d;
            }
            // does a DFA state exist already with everything the same
            // except its state number?
            DFAState existing = (DFAState)uniqueStates.get( d );
            if ( existing != null )
            {
                /*
                JSystem.@out.println("state "+d.stateNumber+" exists as state "+
                    existing.stateNumber);
                    */
                // already there...get the existing DFA state
                return existing;
            }

            // if not there, then add new state.
            uniqueStates[d] = d;
            numberOfStates++;
            return d;
        }

        public void removeState( DFAState d )
        {
            DFAState it;
            if ( uniqueStates.TryGetValue( d, out it ) )
            {
                uniqueStates.Remove( d );
                if ( it != null )
                {
                    numberOfStates--;
                }
            }
        }

        [Obsolete]
        public IDictionary<DFAState, DFAState> getUniqueStates()
        {
            return UniqueStates;
        }

        /** What is the max state number ever created?  This may be beyond
         *  getNumberOfStates().
         */
        [Obsolete]
        public int getMaxStateNumber()
        {
            return MaxStateNumber;
        }

        public virtual DFAState getState( int stateNumber )
        {
            return (DFAState)states[stateNumber];
        }

        public virtual void setState( int stateNumber, DFAState d )
        {
            states[stateNumber] = d;
        }

        /** Is the DFA reduced?  I.e., does every state have a path to an accept
         *  state?  If not, don't delete as we need to generate an error indicating
         *  which paths are "dead ends".  Also tracks list of alts with no accept
         *  state in the DFA.  Must call verify() first before this makes sense.
         */
        [Obsolete]
        public virtual bool isReduced()
        {
            return IsReduced;
        }

        /** Is this DFA cyclic?  That is, are there any loops?  If not, then
         *  the DFA is essentially an LL(k) predictor for some fixed, max k value.
         *  We can build a series of nested IF statements to match this.  In the
         *  presence of cycles, we need to build a general DFA and interpret it
         *  to distinguish between alternatives.
         */
        [Obsolete]
        public virtual bool isCyclic()
        {
            return IsCyclic;
        }

        [Obsolete]
        public virtual bool canInlineDecision()
        {
            return CanInlineDecision;
        }

        /** Is this DFA derived from the NFA for the Tokens rule? */
        public virtual bool isTokensRuleDecision()
        {
            if ( nfa.grammar.type != Grammar.LEXER )
            {
                return false;
            }
            NFAState nfaStart = NFADecisionStartState;
            Rule r = nfa.grammar.getLocallyDefinedRule( Grammar.ARTIFICIAL_TOKENS_RULENAME );
            NFAState TokensRuleStart = r.startState;
            NFAState TokensDecisionStart =
                (NFAState)TokensRuleStart.transition[0].target;
            return nfaStart == TokensDecisionStart;
        }

        /** The user may specify a max, acyclic lookahead for any decision.  No
         *  DFA cycles are created when this value, k, is greater than 0.
         *  If this decision has no k lookahead specified, then try the grammar.
         */
        public virtual int getUserMaxLookahead()
        {
            if ( user_k >= 0 )
            { // cache for speed
                return user_k;
            }
            user_k = nfa.grammar.getUserMaxLookahead( decisionNumber );
            return user_k;
        }

        public virtual bool getAutoBacktrackMode()
        {
            return nfa.grammar.getAutoBacktrackMode( decisionNumber );
        }

        public virtual void setUserMaxLookahead( int k )
        {
            this.user_k = k;
        }

        /** Return k if decision is LL(k) for some k else return max int */
        [Obsolete]
        public virtual int getMaxLookaheadDepth()
        {
            return MaxLookaheadDepth;
        }

        /** Return a list of Integer alt numbers for which no lookahead could
         *  be computed or for which no single DFA accept state predicts those
         *  alts.  Must call verify() first before this makes sense.
         */
        [Obsolete]
        public virtual List<int> getUnreachableAlts()
        {
            return (List<int>)UnreachableAlts;
        }

        /** Once this DFA has been built, need to verify that:
         *
         *  1. it's reduced
         *  2. all alts have an accept state
         *
         *  Elsewhere, in the NFA converter, we need to verify that:
         *
         *  3. alts i and j have disjoint lookahead if no sem preds
         *  4. if sem preds, nondeterministic alts must be sufficiently covered
         *
         *  This is avoided if analysis bails out for any reason.
         */
        public virtual void verify()
        {
            doesStateReachAcceptState( startState );
        }

        /** figure out if this state eventually reaches an accept state and
         *  modify the instance variable 'reduced' to indicate if we find
         *  at least one state that cannot reach an accept state.  This implies
         *  that the overall DFA is not reduced.  This algorithm should be
         *  linear in the number of DFA states.
         *
         *  The algorithm also tracks which alternatives have no accept state,
         *  indicating a nondeterminism.
         *
         *  Also computes whether the DFA is cyclic.
         *
         *  TODO: I call getUniquelyPredicatedAlt too much; cache predicted alt
         */
        protected virtual bool doesStateReachAcceptState( DFAState d )
        {
            if ( d.IsAcceptState )
            {
                // accept states have no edges emanating from them so we can return
                d.AcceptStateReachable = REACHABLE_YES;
                // this alt is uniquely predicted, remove from nondeterministic list
                int predicts = d.getUniquelyPredictedAlt();
                unreachableAlts.Remove( predicts );
                return true;
            }

            // avoid infinite loops
            d.AcceptStateReachable = REACHABLE_BUSY;

            bool anEdgeReachesAcceptState = false;
            // Visit every transition, track if at least one edge reaches stop state
            // Cannot terminate when we know this state reaches stop state since
            // all transitions must be traversed to set status of each DFA state.
            for ( int i = 0; i < d.NumberOfTransitions; i++ )
            {
                Transition t = d.transition( i );
                DFAState edgeTarget = (DFAState)t.target;
                int targetStatus = edgeTarget.AcceptStateReachable;
                if ( targetStatus == REACHABLE_BUSY )
                { // avoid cycles; they say nothing
                    cyclic = true;
                    continue;
                }
                if ( targetStatus == REACHABLE_YES )
                { // avoid unnecessary work
                    anEdgeReachesAcceptState = true;
                    continue;
                }
                if ( targetStatus == REACHABLE_NO )
                {  // avoid unnecessary work
                    continue;
                }
                // target must be REACHABLE_UNKNOWN (i.e., unvisited)
                if ( doesStateReachAcceptState( edgeTarget ) )
                {
                    anEdgeReachesAcceptState = true;
                    // have to keep looking so don't break loop
                    // must cover all states even if we find a path for this state
                }
            }
            if ( anEdgeReachesAcceptState )
            {
                d.AcceptStateReachable = REACHABLE_YES;
            }
            else
            {
                d.AcceptStateReachable = REACHABLE_NO;
                reduced = false;
            }
            return anEdgeReachesAcceptState;
        }

        /** Walk all accept states and find the manually-specified synpreds.
         *  Gated preds are not always hoisted
         *  I used to do this in the code generator, but that is too late.
         *  This converter tries to avoid computing DFA for decisions in
         *  syntactic predicates that are not ever used such as those
         *  created by autobacktrack mode.
         */
        public virtual void findAllGatedSynPredsUsedInDFAAcceptStates()
        {
            int nAlts = NumberOfAlts;
            for ( int i = 1; i <= nAlts; i++ )
            {
                DFAState a = getAcceptState( i );
                //JSystem.@out.println("alt "+i+": "+a);
                if ( a != null )
                {
                    HashSet<SemanticContext> synpreds = a.getGatedSyntacticPredicatesInNFAConfigurations();
                    if ( synpreds != null )
                    {
                        // add all the predicates we find (should be just one, right?)
                        foreach ( SemanticContext semctx in synpreds )
                        {
                            // JSystem.@out.println("synpreds: "+semctx);
                            nfa.grammar.synPredUsedInDFA( this, semctx );
                        }
                    }
                }
            }
        }

        [Obsolete]
        public virtual NFAState getNFADecisionStartState()
        {
            return NFADecisionStartState;
        }

        public virtual DFAState getAcceptState( int alt )
        {
            return altToAcceptState[alt];
        }

        public virtual void setAcceptState( int alt, DFAState acceptState )
        {
            altToAcceptState[alt] = acceptState;
        }

        [Obsolete]
        public virtual String getDescription()
        {
            return Description;
        }

        [Obsolete]
        public virtual int getDecisionNumber()
        {
            return DecisionNumber;
        }

        /** If this DFA failed to finish during construction, we might be
         *  able to retry with k=1 but we need to know whether it will
         *  potentially succeed.  Can only succeed if there is a predicate
         *  to resolve the issue.  Don't try if k=1 already as it would
         *  cycle forever.  Timeout can retry with k=1 even if no predicate
         *  if k!=1.
         */
        public virtual bool okToRetryDFAWithK1()
        {
            bool nonLLStarOrOverflowAndPredicateVisible =
                ( probe.IsNonLLStarDecision || probe.AnalysisOverflowed ) &&
                predicateVisible; // auto backtrack or manual sem/syn
            return UserMaxLookahead != 1 &&
                 ( AnalysisTimedOut || nonLLStarOrOverflowAndPredicateVisible );
        }

        public virtual String getReasonForFailure()
        {
            StringBuilder buf = new StringBuilder();
            if ( probe.IsNonLLStarDecision )
            {
                buf.Append( "non-LL(*)" );
                if ( predicateVisible )
                {
                    buf.Append( " && predicate visible" );
                }
            }
            if ( probe.AnalysisOverflowed )
            {
                buf.Append( "recursion overflow" );
                if ( predicateVisible )
                {
                    buf.Append( " && predicate visible" );
                }
            }
            if ( AnalysisTimedOut )
            {
                if ( buf.Length > 0 )
                {
                    buf.Append( " && " );
                }
                buf.Append( "timed out (>" );
                buf.Append( DFA.MAX_TIME_PER_DFA_CREATION );
                buf.Append( "ms)" );
            }
            buf.Append( "\n" );
            return buf.ToString();
        }

        /** What GrammarAST node (derived from the grammar) is this DFA
         *  associated with?  It will point to the start of a block or
         *  the loop back of a (...)+ block etc...
         */
        [Obsolete]
        public virtual GrammarAST getDecisionASTNode()
        {
            return DecisionASTNode;
        }

        public virtual bool isGreedy()
        {
            GrammarAST blockAST = nfa.grammar.getDecisionBlockAST( decisionNumber );
            Object v = nfa.grammar.getBlockOption( blockAST, "greedy" );
            if ( v != null && v.Equals( "false" ) )
            {
                return false;
            }
            return true;

        }

        public virtual DFAState newState()
        {
            DFAState n = new DFAState( this );
            n.stateNumber = stateCounter;
            stateCounter++;
            states.setSize( n.stateNumber + 1 );
            states[n.stateNumber] = n; // track state num to state
            return n;
        }

        [Obsolete]
        public virtual int getNumberOfStates()
        {
            return NumberOfStates;
        }

        [Obsolete]
        public virtual int getNumberOfAlts()
        {
            return NumberOfAlts;
        }

        [Obsolete]
        public virtual bool analysisTimedOut()
        {
            return AnalysisTimedOut;
        }

        protected virtual void initAltRelatedInfo()
        {
            unreachableAlts = new List<int>();
            for ( int i = 1; i <= nAlts; i++ )
            {
                unreachableAlts.Add( i );
            }
            altToAcceptState = new DFAState[nAlts + 1];
        }

        public override string ToString()
        {
            FASerializer serializer = new FASerializer( nfa.grammar );
            if ( startState == null )
            {
                return "";
            }
            return serializer.serialize( startState, false );
        }

#if false
        /** EOT (end of token) is a label that indicates when the DFA conversion
         *  algorithm would "fall off the end of a lexer rule".  It normally
         *  means the default clause.  So for ('a'..'z')+ you would see a DFA
         *  with a state that has a..z and EOT emanating from it.  a..z would
         *  jump to a state predicting alt 1 and EOT would jump to a state
         *  predicting alt 2 (the exit loop branch).  EOT implies anything other
         *  than a..z.  If for some reason, the set is "all char" such as with
         *  the wildcard '.', then EOT cannot match anything.  For example,
         *
         *     BLOCK : '{' (.)* '}'
         *
         *  consumes all char until EOF when greedy=true.  When all edges are
         *  combined for the DFA state after matching '}', you will find that
         *  it is all char.  The EOT transition has nothing to match and is
         *  unreachable.  The findNewDFAStatesAndAddDFATransitions() method
         *  must know to ignore the EOT, so we simply remove it from the
         *  reachable labels.  Later analysis will find that the exit branch
         *  is not predicted by anything.  For greedy=false, we leave only
         *  the EOT label indicating that the DFA should stop immediately
         *  and predict the exit branch. The reachable labels are often a
         *  set of disjoint values like: [<EOT>, 42, {0..41, 43..65534}]
         *  due to DFA conversion so must construct a pure set to see if
         *  it is same as Label.ALLCHAR.
         *
         *  Only do this for Lexers.
         *
         *  If EOT coexists with ALLCHAR:
         *  1. If not greedy, modify the labels parameter to be EOT
         *  2. If greedy, remove EOT from the labels set
         */
        protected boolean reachableLabelsEOTCoexistsWithAllChar(OrderedHashSet labels)
        {
            Label eot = new Label(Label.EOT);
            if ( !labels.containsKey(eot) ) {
                return false;
            }
            JSystem.@out.println("### contains EOT");
            bool containsAllChar = false;
            IntervalSet completeVocab = new IntervalSet();
            int n = labels.size();
            for (int i=0; i<n; i++) {
                Label rl = (Label)labels.get(i);
                if ( !rl.Equals(eot) ) {
                    completeVocab.addAll(rl.Set());
                }
            }
            JSystem.@out.println("completeVocab="+completeVocab);
            if ( completeVocab.Equals(Label.ALLCHAR) ) {
                JSystem.@out.println("all char");
                containsAllChar = true;
            }
            return containsAllChar;
        }
#endif
    }

}
