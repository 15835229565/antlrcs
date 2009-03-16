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

namespace Antlr3.Misc
{
    using System.Collections.Generic;
    using System.Linq;
    using Antlr.Runtime.JavaExtensions;

    using ArgumentException = System.ArgumentException;
    using Array = System.Array;
    using Grammar = Antlr3.Tool.Grammar;
    using ICloneable = System.ICloneable;
    using IDictionary = System.Collections.IDictionary;
    using IEnumerable = System.Collections.IEnumerable;
    using IList = System.Collections.IList;
    using Label = Antlr3.Analysis.Label;
    using Math = System.Math;
    using NotImplementedException = System.NotImplementedException;
    using StringBuilder = System.Text.StringBuilder;

    /**A BitSet to replace java.util.BitSet.
     *
     * Primary differences are that most set operators return new sets
     * as opposed to oring and anding "in place".  Further, a number of
     * operations were added.  I cannot contain a BitSet because there
     * is no way to access the internal bits (which I need for speed)
     * and, because it is final, I cannot subclass to add functionality.
     * Consider defining set degree.  Without access to the bits, I must
     * call a method n times to test the ith bit...ack!
     *
     * Also seems like or() from util is wrong when size of incoming set is bigger
     * than this.bits.length.
     *
     * @author Terence Parr
     */
    public class BitSet : IIntSet, ICloneable
    {
        protected const int BITS = 64;    // number of bits / long
        protected const int LOG_BITS = 6; // 2^6 == 64

        /* We will often need to do a mod operator (i mod nbits).  Its
         * turns out that, for powers of two, this mod operation is
         * same as (i & (nbits-1)).  Since mod is slow, we use a
         * precomputed mod mask to do the mod instead.
         */
        protected const int MOD_MASK = BITS - 1;

        /** The actual data bits */
        protected ulong[] bits;

        /** Construct a bitset of size one word (64 bits) */
        public BitSet() :
            this( BITS )
        {
        }

        /** Construction from a static array of longs */
        public BitSet( ulong[] bits_ )
        {
            bits = bits_;
        }

        /** Construct a bitset given the size
         * @param nbits The size of the bitset in bits
         */
        public BitSet( int nbits )
        {
            bits = new ulong[( ( nbits - 1 ) >> LOG_BITS ) + 1];
        }

        #region Properties
        public bool IsNil
        {
            get
            {
                return isNil();
            }
        }
        public int LengthInLongWords
        {
            get
            {
                return lengthInLongWords();
            }
        }
        public int NumBits
        {
            get
            {
                return numBits();
            }
        }
        public int Size
        {
            get
            {
                return size();
            }
        }
        #endregion

        /** or this element into this set (grow as necessary to accommodate) */
        public virtual void add( int el )
        {
            //JSystem.@out.println("add("+el+")");
            int n = wordNumber( el );
            //JSystem.@out.println("word number is "+n);
            //JSystem.@out.println("bits.length "+bits.length);
            if ( n >= bits.Length )
            {
                growToInclude( el );
            }
            bits[n] |= bitMask( el );
        }

        public virtual void addAll( IIntSet set )
        {
            if ( set is BitSet )
            {
                this.orInPlace( (BitSet)set );
            }
            else if ( set is IntervalSet )
            {
                IntervalSet other = (IntervalSet)set;
                // walk set and add each interval
                foreach ( Interval I in other.intervals )
                {
                    this.orInPlace( BitSet.range( I.a, I.b ) );
                }
            }
            else
            {
                throw new ArgumentException( "can't add " +
                                                   set.GetType().Name +
                                                   " to BitSet" );
            }
        }

        public virtual void addAll( int[] elements )
        {
            if ( elements == null )
            {
                return;
            }
            for ( int i = 0; i < elements.Length; i++ )
            {
                int e = elements[i];
                add( e );
            }
        }

        public virtual void addAll( IEnumerable elements )
        {
            if ( elements == null )
            {
                return;
            }
            foreach ( object o in elements )
            {
                if ( !( o is int ) )
                {
                    throw new ArgumentException();
                }
                int eI = (int)o;
                add( eI );
            }
            /*
            int n = elements.size();
            for (int i = 0; i < n; i++) {
                Object o = elements.get(i);
                if ( !(o instanceof Integer) ) {
                    throw new IllegalArgumentException();
                }
                Integer eI = (Integer)o;
                add(eI.intValue());
            }
             */
        }

        public virtual IIntSet and( IIntSet a )
        {
            BitSet s = (BitSet)this.Clone();
            s.andInPlace( (BitSet)a );
            return s;
        }

        public virtual void andInPlace( BitSet a )
        {
            int min = Math.Min( bits.Length, a.bits.Length );
            for ( int i = min - 1; i >= 0; i-- )
            {
                bits[i] &= a.bits[i];
            }
            // clear all bits in this not present in a (if this bigger than a).
            for ( int i = min; i < bits.Length; i++ )
            {
                bits[i] = 0;
            }
        }

        private static ulong bitMask( int bitNumber )
        {
            int bitPosition = bitNumber & MOD_MASK; // bitNumber mod BITS
            return 1UL << bitPosition;
        }

        public virtual void clear()
        {
            for ( int i = bits.Length - 1; i >= 0; i-- )
            {
                bits[i] = 0;
            }
        }

        public virtual void clear( int el )
        {
            int n = wordNumber( el );
            if ( n >= bits.Length )
            {	// grow as necessary to accommodate
                growToInclude( el );
            }
            bits[n] &= ~bitMask( el );
        }

        public virtual object Clone()
        {
            return new BitSet( (ulong[])bits.Clone() );
        }
        //public Object clone()
        //{
        //    BitSet s;
        //    try
        //    {
        //        s = (BitSet)base.clone();
        //        s.bits = new long[bits.Length];
        //        JSystem.arraycopy( bits, 0, s.bits, 0, bits.Length );
        //    }
        //    catch ( CloneNotSupportedException e )
        //    {
        //        throw new InternalError();
        //    }
        //    return s;
        //}

        public virtual int size()
        {
            int deg = 0;
            for ( int i = bits.Length - 1; i >= 0; i-- )
            {
                ulong word = bits[i];
                if ( word != 0L )
                {
                    for ( int bit = BITS - 1; bit >= 0; bit-- )
                    {
                        if ( ( word & ( 1UL << bit ) ) != 0 )
                        {
                            deg++;
                        }
                    }
                }
            }
            return deg;
        }

        public override int GetHashCode()
        {
            throw new System.NotImplementedException();
        }

        public override bool Equals( object other )
        {
            if ( other == null || !( other is BitSet ) )
            {
                return false;
            }

            BitSet otherSet = (BitSet)other;

            int n = Math.Min( this.bits.Length, otherSet.bits.Length );

            // for any bits in common, compare
            for ( int i = 0; i < n; i++ )
            {
                if ( this.bits[i] != otherSet.bits[i] )
                {
                    return false;
                }
            }

            // make sure any extra bits are off

            if ( this.bits.Length > n )
            {
                for ( int i = n + 1; i < this.bits.Length; i++ )
                {
                    if ( this.bits[i] != 0 )
                    {
                        return false;
                    }
                }
            }
            else if ( otherSet.bits.Length > n )
            {
                for ( int i = n + 1; i < otherSet.bits.Length; i++ )
                {
                    if ( otherSet.bits[i] != 0 )
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /**
         * Grows the set to a larger number of bits.
         * @param bit element that must fit in set
         */
        public virtual void growToInclude( int bit )
        {
            int newSize = Math.Max( bits.Length << 1, numWordsToHold( bit ) );
            ulong[] newbits = new ulong[newSize];
            Array.Copy( bits, newbits, bits.Length );
            bits = newbits;
        }

        public virtual bool member( int el )
        {
            int n = wordNumber( el );
            if ( n >= bits.Length )
                return false;
            return ( bits[n] & bitMask( el ) ) != 0;
        }

        /** Get the first element you find and return it.  Return Label.INVALID
         *  otherwise.
         */
        public virtual int getSingleElement()
        {
            for ( int i = 0; i < ( bits.Length << LOG_BITS ); i++ )
            {
                if ( member( i ) )
                {
                    return i;
                }
            }
            return Label.INVALID;
        }

        public virtual bool isNil()
        {
            for ( int i = bits.Length - 1; i >= 0; i-- )
            {
                if ( bits[i] != 0 )
                    return false;
            }
            return true;
        }

        public virtual IIntSet complement()
        {
            BitSet s = (BitSet)this.Clone();
            s.notInPlace();
            return s;
        }

        public virtual IIntSet complement( IIntSet set )
        {
            if ( set == null )
            {
                return this.complement();
            }
            return set.subtract( this );
        }

        public virtual void notInPlace()
        {
            for ( int i = bits.Length - 1; i >= 0; i-- )
            {
                bits[i] = ~bits[i];
            }
        }

        /** complement bits in the range 0..maxBit. */
        public virtual void notInPlace( int maxBit )
        {
            notInPlace( 0, maxBit );
        }

        /** complement bits in the range minBit..maxBit.*/
        public virtual void notInPlace( int minBit, int maxBit )
        {
            // make sure that we have room for maxBit
            growToInclude( maxBit );
            for ( int i = minBit; i <= maxBit; i++ )
            {
                int n = wordNumber( i );
                bits[n] ^= bitMask( i );
            }
        }

        private /*final*/ int numWordsToHold( int el )
        {
            return ( el >> LOG_BITS ) + 1;
        }

        public static BitSet of( int el )
        {
            BitSet s = new BitSet( el + 1 );
            s.add( el );
            return s;
        }

        public static BitSet of<T>( T elements )
            where T : IEnumerable<int>
        {
            BitSet s = new BitSet();
            foreach ( int i in elements )
            {
                s.add( i );
            }
            return s;
        }

        public static BitSet of( IntervalSet set )
        {
            return of( (IIntSet)set );
        }
        public static BitSet of( IIntSet set )
        {
            if ( set == null )
            {
                return null;
            }

            if ( set is BitSet )
            {
                return (BitSet)set;
            }
            if ( set is IntervalSet )
            {
                BitSet s = new BitSet();
                s.addAll( set );
                return s;
            }
            throw new ArgumentException( "can't create BitSet from " + set.GetType().Name );
        }

        public static BitSet of( IDictionary elements )
        {
            return BitSet.of( elements.Keys.Cast<int>() );
        }

        public static BitSet of<TKey, TValue>( System.Collections.Generic.IDictionary<TKey, TValue> elements )
        {
            return BitSet.of( elements.Keys.Cast<int>() );
        }

        public static BitSet range( int a, int b )
        {
            BitSet s = new BitSet( b + 1 );
            for ( int i = a; i <= b; i++ )
            {
                int n = wordNumber( i );
                s.bits[n] |= bitMask( i );
            }
            return s;
        }

        /** return this | a in a new set */
        public virtual IIntSet or( IIntSet a )
        {
            if ( a == null )
            {
                return this;
            }
            BitSet s = (BitSet)this.Clone();
            s.orInPlace( (BitSet)a );
            return s;
        }

        public virtual void orInPlace( BitSet a )
        {
            if ( a == null )
            {
                return;
            }
            // If this is smaller than a, grow this first
            if ( a.bits.Length > bits.Length )
            {
                setSize( a.bits.Length );
            }
            int min = Math.Min( bits.Length, a.bits.Length );
            for ( int i = min - 1; i >= 0; i-- )
            {
                bits[i] |= a.bits[i];
            }
        }

        // remove this element from this set
        public virtual void remove( int el )
        {
            int n = wordNumber( el );
            if ( n >= bits.Length )
            {
                growToInclude( el );
            }
            bits[n] &= ~bitMask( el );
        }

        /**
         * Sets the size of a set.
         * @param nwords how many words the new set should be
         */
        private void setSize( int nwords )
        {
            ulong[] newbits = new ulong[nwords];
            int n = Math.Min( nwords, bits.Length );
            Array.Copy( bits, newbits, n );
            bits = newbits;
        }

        public virtual int numBits()
        {
            return bits.Length << LOG_BITS; // num words * bits per word
        }

        /** return how much space is being used by the bits array not
         *  how many actually have member bits on.
         */
        public virtual int lengthInLongWords()
        {
            return bits.Length;
        }

        /**Is this contained within a? */
        public virtual bool subset( BitSet a )
        {
            if ( a == null )
                return false;
            return this.and( a ).Equals( this );
        }

        /**Subtract the elements of 'a' from 'this' in-place.
         * Basically, just turn off all bits of 'this' that are in 'a'.
         */
        public virtual void subtractInPlace( BitSet a )
        {
            if ( a == null )
                return;
            // for all words of 'a', turn off corresponding bits of 'this'
            for ( int i = 0; i < bits.Length && i < a.bits.Length; i++ )
            {
                bits[i] &= ~a.bits[i];
            }
        }

        public virtual IIntSet subtract( IIntSet a )
        {
            if ( a == null || !( a is BitSet ) )
                return null;

            BitSet s = (BitSet)this.Clone();
            s.subtractInPlace( (BitSet)a );
            return s;
        }

        public virtual List<int> ToList()
        {
            throw new NotImplementedException( "BitSet.toList() unimplemented" );
        }

        public virtual int[] toArray()
        {
            int[] elems = new int[size()];
            int en = 0;
            for ( int i = 0; i < ( bits.Length << LOG_BITS ); i++ )
            {
                if ( member( i ) )
                {
                    elems[en++] = i;
                }
            }
            return elems;
        }

        public virtual ulong[] toPackedArray()
        {
            return bits;
        }

        public override string ToString()
        {
            return ToString( null );
        }

        /** Transform a bit set into a string by formatting each element as an integer
         * separator The string to put in between elements
         * @return A commma-separated list of values
         */
        public virtual string ToString( Grammar g )
        {
            StringBuilder buf = new StringBuilder();
            string separator = ",";
            bool havePrintedAnElement = false;
            buf.Append( '{' );

            for ( int i = 0; i < ( bits.Length << LOG_BITS ); i++ )
            {
                if ( member( i ) )
                {
                    if ( i > 0 && havePrintedAnElement )
                    {
                        buf.Append( separator );
                    }
                    if ( g != null )
                    {
                        buf.Append( g.getTokenDisplayName( i ) );
                    }
                    else
                    {
                        buf.Append( i );
                    }
                    havePrintedAnElement = true;
                }
            }
            buf.Append( '}' );
            return buf.ToString();
        }

        /**Create a string representation where instead of integer elements, the
         * ith element of vocabulary is displayed instead.  Vocabulary is a Vector
         * of Strings.
         * separator The string to put in between elements
         * @return A commma-separated list of character constants.
         */
        public virtual string ToString( string separator, IList vocabulary )
        {
            if ( vocabulary == null )
            {
                return ToString( null );
            }
            string str = "";
            for ( int i = 0; i < ( bits.Length << LOG_BITS ); i++ )
            {
                if ( member( i ) )
                {
                    if ( str.Length > 0 )
                    {
                        str += separator;
                    }
                    if ( i >= vocabulary.Count )
                    {
                        str += "'" + (char)i + "'";
                    }
                    else if ( vocabulary[i] == null )
                    {
                        str += "'" + (char)i + "'";
                    }
                    else
                    {
                        str += (string)vocabulary[i];
                    }
                }
            }
            return str;
        }

        /**
         * Dump a comma-separated list of the words making up the bit set.
         * Split each 64 bit number into two more manageable 32 bit numbers.
         * This generates a comma-separated list of C++-like unsigned long constants.
         */
        public virtual string ToStringOfHalfWords()
        {
            StringBuilder s = new StringBuilder();
            for ( int i = 0; i < bits.Length; i++ )
            {
                if ( i != 0 )
                    s.Append( ", " );
                ulong tmp = bits[i];
                tmp &= 0xFFFFFFFFL;
                s.Append( tmp );
                s.Append( "UL" );
                s.Append( ", " );
                tmp = bits[i] >> 32;
                tmp &= 0xFFFFFFFFL;
                s.Append( tmp );
                s.Append( "UL" );
            }
            return s.ToString();
        }

        /**
         * Dump a comma-separated list of the words making up the bit set.
         * This generates a comma-separated list of Java-like long int constants.
         */
        public virtual string ToStringOfWords()
        {
            StringBuilder s = new StringBuilder();
            for ( int i = 0; i < bits.Length; i++ )
            {
                if ( i != 0 )
                    s.Append( ", " );
                s.Append( bits[i] );
                s.Append( "L" );
            }
            return s.ToString();
        }

        public virtual string ToStringWithRanges()
        {
            return ToString();
        }

        private /*final*/ static int wordNumber( int bit )
        {
            return bit >> LOG_BITS; // bit / BITS
        }

        #region ICollection<int> Members

        void ICollection<int>.Add( int item )
        {
            throw new System.NotImplementedException();
        }

        void ICollection<int>.Clear()
        {
            throw new System.NotImplementedException();
        }

        bool ICollection<int>.Contains( int item )
        {
            throw new System.NotImplementedException();
        }

        void ICollection<int>.CopyTo( int[] array, int arrayIndex )
        {
            throw new System.NotImplementedException();
        }

        int ICollection<int>.Count
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        bool ICollection<int>.IsReadOnly
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        bool ICollection<int>.Remove( int item )
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region IEnumerable<int> Members

        IEnumerator<int> IEnumerable<int>.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
