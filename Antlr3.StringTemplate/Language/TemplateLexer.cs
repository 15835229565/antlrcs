// $ANTLR 3.1.2 Language\\Template.g3 2009-03-07 08:52:37

// The variable 'variable' is assigned but its value is never used.
#pragma warning disable 219
// Unreachable code detected.
#pragma warning disable 162


/*
 [The "BSD licence"]
 Copyright (c) 2005-2008 Terence Parr
 All rights reserved.

 Grammar conversion to ANTLR v3 and C#:
 Copyright (c) 2008 Sam Harwell, Pixel Mine, Inc.
 All rights reserved.

 Redistribution and use in source and binary forms, with or without
 modification, are permitted provided that the following conditions
 are met:
 1. Redistributions of source code must retain the above copyright
	notice, this list of conditions and the following disclaimer.
 2. Redistributions in binary form must reproduce the above copyright
	notice, this list of conditions and the following disclaimer in the
	documentation and/or other materials provided with the distribution.
 3. The name of the author may not be used to endorse or promote products
	derived from this software without specific prior written permission.

 THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR
 IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
 INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
 DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
 THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/


using System.Collections.Generic;
using Antlr.Runtime;
using Stack = System.Collections.Generic.Stack<object>;
using List = System.Collections.IList;
using ArrayList = System.Collections.Generic.List<object>;
using Map = System.Collections.IDictionary;
using HashMap = System.Collections.Generic.Dictionary<object, object>;
namespace Antlr3.ST.Language
{
public partial class TemplateLexer : Lexer
{
	public const int EOF=-1;
	public const int ACTION=4;
	public const int COMMENT=5;
	public const int ELSE=6;
	public const int ELSEIF=7;
	public const int ENDIF=8;
	public const int ESC=9;
	public const int ESC_CHAR=10;
	public const int EXPR=11;
	public const int HEX=12;
	public const int IF=13;
	public const int IF_EXPR=14;
	public const int INDENT=15;
	public const int LITERAL=16;
	public const int NESTED_PARENS=17;
	public const int NEWLINE=18;
	public const int REGION_DEF=19;
	public const int REGION_REF=20;
	public const int SUBTEMPLATE=21;
	public const int TEMPLATE=22;

    // delegates
    // delegators

	public TemplateLexer() {}
	public TemplateLexer( ICharStream input )
		: this( input, new RecognizerSharedState() )
	{
	}
	public TemplateLexer( ICharStream input, RecognizerSharedState state )
		: base( input, state )
	{

	}
	public override string GrammarFileName { get { return "Language\\Template.g3"; } }

	// $ANTLR start "IF"
	private void mIF()
	{
		try
		{
			// Language\\Template.g3:257:15: ()
			// Language\\Template.g3:257:15: 
			{


			}

		}
		finally
		{
		}
	}
	// $ANTLR end "IF"

	// $ANTLR start "ELSEIF"
	private void mELSEIF()
	{
		try
		{
			// Language\\Template.g3:258:19: ()
			// Language\\Template.g3:258:19: 
			{


			}

		}
		finally
		{
		}
	}
	// $ANTLR end "ELSEIF"

	// $ANTLR start "ELSE"
	private void mELSE()
	{
		try
		{
			// Language\\Template.g3:259:17: ()
			// Language\\Template.g3:259:17: 
			{


			}

		}
		finally
		{
		}
	}
	// $ANTLR end "ELSE"

	// $ANTLR start "ENDIF"
	private void mENDIF()
	{
		try
		{
			// Language\\Template.g3:260:18: ()
			// Language\\Template.g3:260:18: 
			{


			}

		}
		finally
		{
		}
	}
	// $ANTLR end "ENDIF"

	// $ANTLR start "REGION_DEF"
	private void mREGION_DEF()
	{
		try
		{
			// Language\\Template.g3:261:23: ()
			// Language\\Template.g3:261:23: 
			{


			}

		}
		finally
		{
		}
	}
	// $ANTLR end "REGION_DEF"

	// $ANTLR start "REGION_REF"
	private void mREGION_REF()
	{
		try
		{
			// Language\\Template.g3:262:23: ()
			// Language\\Template.g3:262:23: 
			{


			}

		}
		finally
		{
		}
	}
	// $ANTLR end "REGION_REF"

	// $ANTLR start "NEWLINE"
	private void mNEWLINE()
	{
		try
		{
			int _type = NEWLINE;
			int _channel = DEFAULT_TOKEN_CHANNEL;
			// Language\\Template.g3:265:4: ( ( '\\r' )? '\\n' )
			// Language\\Template.g3:265:4: ( '\\r' )? '\\n'
			{
			// Language\\Template.g3:265:4: ( '\\r' )?
			int alt1=2;
			int LA1_0 = input.LA(1);

			if ( (LA1_0=='\r') )
			{
				alt1=1;
			}
			switch ( alt1 )
			{
			case 1:
				// Language\\Template.g3:265:5: '\\r'
				{
				Match('\r'); if (state.failed) return ;

				}
				break;

			}

			Match('\n'); if (state.failed) return ;
			if ( state.backtracking == 0 )
			{
				currentIndent=null;
			}

			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
		}
	}
	// $ANTLR end "NEWLINE"

	// $ANTLR start "ACTION"
	private void mACTION()
	{
		try
		{
			int _type = ACTION;
			int _channel = DEFAULT_TOKEN_CHANNEL;
			IToken exp=null;
			int ch;


				int startCol = CharPositionInLine;
				System.Text.StringBuilder builder = new System.Text.StringBuilder();
				System.Text.StringBuilder buf = null;
				string subtext = string.Empty;
				char uc = '\0';
				bool atLeft = false;
				string t = null;

			// Language\\Template.g3:279:4: ( ( options {k=1; } :=> '$' ( ESC_CHAR[out uc] )+ '$' |=> COMMENT | (=> '$if' ( ' ' )* '(' exp= IF_EXPR ')$' ( ( '\\r' )? '\\n' )? |=> '$elseif' ( ' ' )* '(' exp= IF_EXPR ')$' ( ( '\\r' )? '\\n' )? |=> '$else$' ( ( '\\r' )? '\\n' )? |=> '$endif$' ({...}? => ( '\\r' )? '\\n' )? |=> '$@' (ch=~ ( '$' | '(' ) )+ ( '()$' | '$' (=> ( '\\r' )? '\\n' )? ({...}? => (=> ( '\\r' )? '\\n' |ch= . ) )+ (=> ( '\\r' )? '\\n' )? (=> '$@end$' | . ) ({...}? ( '\\r' )? '\\n' )? ) | '$' EXPR[out subtext] '$' ) ) )
			// Language\\Template.g3:279:4: ( options {k=1; } :=> '$' ( ESC_CHAR[out uc] )+ '$' |=> COMMENT | (=> '$if' ( ' ' )* '(' exp= IF_EXPR ')$' ( ( '\\r' )? '\\n' )? |=> '$elseif' ( ' ' )* '(' exp= IF_EXPR ')$' ( ( '\\r' )? '\\n' )? |=> '$else$' ( ( '\\r' )? '\\n' )? |=> '$endif$' ({...}? => ( '\\r' )? '\\n' )? |=> '$@' (ch=~ ( '$' | '(' ) )+ ( '()$' | '$' (=> ( '\\r' )? '\\n' )? ({...}? => (=> ( '\\r' )? '\\n' |ch= . ) )+ (=> ( '\\r' )? '\\n' )? (=> '$@end$' | . ) ({...}? ( '\\r' )? '\\n' )? ) | '$' EXPR[out subtext] '$' ) )
			{
			// Language\\Template.g3:279:4: ( options {k=1; } :=> '$' ( ESC_CHAR[out uc] )+ '$' |=> COMMENT | (=> '$if' ( ' ' )* '(' exp= IF_EXPR ')$' ( ( '\\r' )? '\\n' )? |=> '$elseif' ( ' ' )* '(' exp= IF_EXPR ')$' ( ( '\\r' )? '\\n' )? |=> '$else$' ( ( '\\r' )? '\\n' )? |=> '$endif$' ({...}? => ( '\\r' )? '\\n' )? |=> '$@' (ch=~ ( '$' | '(' ) )+ ( '()$' | '$' (=> ( '\\r' )? '\\n' )? ({...}? => (=> ( '\\r' )? '\\n' |ch= . ) )+ (=> ( '\\r' )? '\\n' )? (=> '$@end$' | . ) ({...}? ( '\\r' )? '\\n' )? ) | '$' EXPR[out subtext] '$' ) )
			int alt26=3;
			int LA26_0 = input.LA(1);

			if ( (LA26_0=='$') )
			{
				int LA26_1 = input.LA(2);

				if ( (synpred1_Template()) )
				{
					alt26=1;
				}
				else if ( (synpred2_Template()) )
				{
					alt26=2;
				}
				else if ( (true) )
				{
					alt26=3;
				}
				else
				{
					if (state.backtracking>0) {state.failed=true; return ;}
					NoViableAltException nvae = new NoViableAltException("", 26, 1, input);

					throw nvae;
				}
			}
			else
			{
				if (state.backtracking>0) {state.failed=true; return ;}
				NoViableAltException nvae = new NoViableAltException("", 26, 0, input);

				throw nvae;
			}
			switch ( alt26 )
			{
			case 1:
				// Language\\Template.g3:280:4: => '$' ( ESC_CHAR[out uc] )+ '$'
				{

				Match('$'); if (state.failed) return ;
				// Language\\Template.g3:282:8: ( ESC_CHAR[out uc] )+
				int cnt2=0;
				for ( ; ; )
				{
					int alt2=2;
					int LA2_0 = input.LA(1);

					if ( (LA2_0=='\\') )
					{
						alt2=1;
					}


					switch ( alt2 )
					{
					case 1:
						// Language\\Template.g3:282:9: ESC_CHAR[out uc]
						{
						mESC_CHAR(out uc); if (state.failed) return ;
						if ( state.backtracking == 0 )
						{
							builder.Append(uc);
						}

						}
						break;

					default:
						if ( cnt2 >= 1 )
							goto loop2;

						if (state.backtracking>0) {state.failed=true; return ;}
						EarlyExitException eee2 = new EarlyExitException( 2, input );
						throw eee2;
					}
					cnt2++;
				}
				loop2:
					;


				Match('$'); if (state.failed) return ;
				if ( state.backtracking == 0 )
				{

									Text = builder.ToString();
									_type = LITERAL;
								
				}

				}
				break;
			case 2:
				// Language\\Template.g3:287:5: => COMMENT
				{

				mCOMMENT(); if (state.failed) return ;
				if ( state.backtracking == 0 )
				{
					 _channel = HIDDEN; 
				}

				}
				break;
			case 3:
				// Language\\Template.g3:289:5: (=> '$if' ( ' ' )* '(' exp= IF_EXPR ')$' ( ( '\\r' )? '\\n' )? |=> '$elseif' ( ' ' )* '(' exp= IF_EXPR ')$' ( ( '\\r' )? '\\n' )? |=> '$else$' ( ( '\\r' )? '\\n' )? |=> '$endif$' ({...}? => ( '\\r' )? '\\n' )? |=> '$@' (ch=~ ( '$' | '(' ) )+ ( '()$' | '$' (=> ( '\\r' )? '\\n' )? ({...}? => (=> ( '\\r' )? '\\n' |ch= . ) )+ (=> ( '\\r' )? '\\n' )? (=> '$@end$' | . ) ({...}? ( '\\r' )? '\\n' )? ) | '$' EXPR[out subtext] '$' )
				{
				// Language\\Template.g3:289:5: (=> '$if' ( ' ' )* '(' exp= IF_EXPR ')$' ( ( '\\r' )? '\\n' )? |=> '$elseif' ( ' ' )* '(' exp= IF_EXPR ')$' ( ( '\\r' )? '\\n' )? |=> '$else$' ( ( '\\r' )? '\\n' )? |=> '$endif$' ({...}? => ( '\\r' )? '\\n' )? |=> '$@' (ch=~ ( '$' | '(' ) )+ ( '()$' | '$' (=> ( '\\r' )? '\\n' )? ({...}? => (=> ( '\\r' )? '\\n' |ch= . ) )+ (=> ( '\\r' )? '\\n' )? (=> '$@end$' | . ) ({...}? ( '\\r' )? '\\n' )? ) | '$' EXPR[out subtext] '$' )
				int alt25=6;
				int LA25_0 = input.LA(1);

				if ( (LA25_0=='$') )
				{
					int LA25_1 = input.LA(2);

					if ( (synpred3_Template()) )
					{
						alt25=1;
					}
					else if ( (synpred4_Template()) )
					{
						alt25=2;
					}
					else if ( (synpred5_Template()) )
					{
						alt25=3;
					}
					else if ( (synpred6_Template()) )
					{
						alt25=4;
					}
					else if ( (synpred7_Template()) )
					{
						alt25=5;
					}
					else if ( (true) )
					{
						alt25=6;
					}
					else
					{
						if (state.backtracking>0) {state.failed=true; return ;}
						NoViableAltException nvae = new NoViableAltException("", 25, 1, input);

						throw nvae;
					}
				}
				else
				{
					if (state.backtracking>0) {state.failed=true; return ;}
					NoViableAltException nvae = new NoViableAltException("", 25, 0, input);

					throw nvae;
				}
				switch ( alt25 )
				{
				case 1:
					// Language\\Template.g3:289:7: => '$if' ( ' ' )* '(' exp= IF_EXPR ')$' ( ( '\\r' )? '\\n' )?
					{

					Match("$if"); if (state.failed) return ;

					// Language\\Template.g3:290:11: ( ' ' )*
					for ( ; ; )
					{
						int alt3=2;
						int LA3_0 = input.LA(1);

						if ( (LA3_0==' ') )
						{
							alt3=1;
						}


						switch ( alt3 )
						{
						case 1:
							// Language\\Template.g3:290:12: ' '
							{
							Match(' '); if (state.failed) return ;

							}
							break;

						default:
							goto loop3;
						}
					}

					loop3:
						;


					Match('('); if (state.failed) return ;
					int expStart206 = GetCharIndex();
					mIF_EXPR(); if (state.failed) return ;
					exp = new CommonToken(input, TokenConstants.INVALID_TOKEN_TYPE, TokenConstants.DEFAULT_CHANNEL, expStart206, GetCharIndex()-1);
					Match(")$"); if (state.failed) return ;

					if ( state.backtracking == 0 )
					{

											Text = "if(" + (exp!=null?exp.Text:null) + ")";
											_type = TemplateParser.IF;
										
					}
					// Language\\Template.g3:295:5: ( ( '\\r' )? '\\n' )?
					int alt5=2;
					int LA5_0 = input.LA(1);

					if ( (LA5_0=='\n'||LA5_0=='\r') )
					{
						alt5=1;
					}
					switch ( alt5 )
					{
					case 1:
						// Language\\Template.g3:295:7: ( '\\r' )? '\\n'
						{
						// Language\\Template.g3:295:7: ( '\\r' )?
						int alt4=2;
						int LA4_0 = input.LA(1);

						if ( (LA4_0=='\r') )
						{
							alt4=1;
						}
						switch ( alt4 )
						{
						case 1:
							// Language\\Template.g3:295:8: '\\r'
							{
							Match('\r'); if (state.failed) return ;

							}
							break;

						}

						Match('\n'); if (state.failed) return ;

						}
						break;

					}


					}
					break;
				case 2:
					// Language\\Template.g3:296:6: => '$elseif' ( ' ' )* '(' exp= IF_EXPR ')$' ( ( '\\r' )? '\\n' )?
					{

					Match("$elseif"); if (state.failed) return ;

					// Language\\Template.g3:297:15: ( ' ' )*
					for ( ; ; )
					{
						int alt6=2;
						int LA6_0 = input.LA(1);

						if ( (LA6_0==' ') )
						{
							alt6=1;
						}


						switch ( alt6 )
						{
						case 1:
							// Language\\Template.g3:297:16: ' '
							{
							Match(' '); if (state.failed) return ;

							}
							break;

						default:
							goto loop6;
						}
					}

					loop6:
						;


					Match('('); if (state.failed) return ;
					int expStart265 = GetCharIndex();
					mIF_EXPR(); if (state.failed) return ;
					exp = new CommonToken(input, TokenConstants.INVALID_TOKEN_TYPE, TokenConstants.DEFAULT_CHANNEL, expStart265, GetCharIndex()-1);
					Match(")$"); if (state.failed) return ;

					if ( state.backtracking == 0 )
					{

											Text = "elseif(" + (exp!=null?exp.Text:null) + ")";
											_type = TemplateParser.ELSEIF;
										
					}
					// Language\\Template.g3:302:5: ( ( '\\r' )? '\\n' )?
					int alt8=2;
					int LA8_0 = input.LA(1);

					if ( (LA8_0=='\n'||LA8_0=='\r') )
					{
						alt8=1;
					}
					switch ( alt8 )
					{
					case 1:
						// Language\\Template.g3:302:7: ( '\\r' )? '\\n'
						{
						// Language\\Template.g3:302:7: ( '\\r' )?
						int alt7=2;
						int LA7_0 = input.LA(1);

						if ( (LA7_0=='\r') )
						{
							alt7=1;
						}
						switch ( alt7 )
						{
						case 1:
							// Language\\Template.g3:302:8: '\\r'
							{
							Match('\r'); if (state.failed) return ;

							}
							break;

						}

						Match('\n'); if (state.failed) return ;

						}
						break;

					}


					}
					break;
				case 3:
					// Language\\Template.g3:303:6: => '$else$' ( ( '\\r' )? '\\n' )?
					{

					Match("$else$"); if (state.failed) return ;

					if ( state.backtracking == 0 )
					{

											Text = "else";
											_type = (TemplateParser.ELSE);
										
					}
					// Language\\Template.g3:309:5: ( ( '\\r' )? '\\n' )?
					int alt10=2;
					int LA10_0 = input.LA(1);

					if ( (LA10_0=='\n'||LA10_0=='\r') )
					{
						alt10=1;
					}
					switch ( alt10 )
					{
					case 1:
						// Language\\Template.g3:309:7: ( '\\r' )? '\\n'
						{
						// Language\\Template.g3:309:7: ( '\\r' )?
						int alt9=2;
						int LA9_0 = input.LA(1);

						if ( (LA9_0=='\r') )
						{
							alt9=1;
						}
						switch ( alt9 )
						{
						case 1:
							// Language\\Template.g3:309:8: '\\r'
							{
							Match('\r'); if (state.failed) return ;

							}
							break;

						}

						Match('\n'); if (state.failed) return ;

						}
						break;

					}


					}
					break;
				case 4:
					// Language\\Template.g3:310:6: => '$endif$' ({...}? => ( '\\r' )? '\\n' )?
					{

					Match("$endif$"); if (state.failed) return ;

					if ( state.backtracking == 0 )
					{

											Text = "endif";
											_type = TemplateParser.ENDIF;
										
					}
					// Language\\Template.g3:316:5: ({...}? => ( '\\r' )? '\\n' )?
					int alt12=2;
					int LA12_0 = input.LA(1);

					if ( (LA12_0=='\n'||LA12_0=='\r') && ((startCol==0)))
					{
						alt12=1;
					}
					switch ( alt12 )
					{
					case 1:
						// Language\\Template.g3:316:7: {...}? => ( '\\r' )? '\\n'
						{
						if ( !((startCol==0)) )
						{
							if (state.backtracking>0) {state.failed=true; return ;}
							throw new FailedPredicateException(input, "ACTION", "startCol==0");
						}
						// Language\\Template.g3:316:25: ( '\\r' )?
						int alt11=2;
						int LA11_0 = input.LA(1);

						if ( (LA11_0=='\r') )
						{
							alt11=1;
						}
						switch ( alt11 )
						{
						case 1:
							// Language\\Template.g3:316:26: '\\r'
							{
							Match('\r'); if (state.failed) return ;

							}
							break;

						}

						Match('\n'); if (state.failed) return ;

						}
						break;

					}


					}
					break;
				case 5:
					// Language\\Template.g3:320:5: => '$@' (ch=~ ( '$' | '(' ) )+ ( '()$' | '$' (=> ( '\\r' )? '\\n' )? ({...}? => (=> ( '\\r' )? '\\n' |ch= . ) )+ (=> ( '\\r' )? '\\n' )? (=> '$@end$' | . ) ({...}? ( '\\r' )? '\\n' )? )
					{

					if ( state.backtracking == 0 )
					{

											builder = new System.Text.StringBuilder();
										
					}
					Match("$@"); if (state.failed) return ;

					// Language\\Template.g3:324:10: (ch=~ ( '$' | '(' ) )+
					int cnt13=0;
					for ( ; ; )
					{
						int alt13=2;
						int LA13_0 = input.LA(1);

						if ( ((LA13_0>='\u0000' && LA13_0<='#')||(LA13_0>='%' && LA13_0<='\'')||(LA13_0>=')' && LA13_0<='\uFFFF')) )
						{
							alt13=1;
						}


						switch ( alt13 )
						{
						case 1:
							// Language\\Template.g3:324:12: ch=~ ( '$' | '(' )
							{
							ch= input.LA(1);
							input.Consume();
							state.failed=false;
							if ( state.backtracking == 0 )
							{
								builder.Append((char)ch);
							}

							}
							break;

						default:
							if ( cnt13 >= 1 )
								goto loop13;

							if (state.backtracking>0) {state.failed=true; return ;}
							EarlyExitException eee13 = new EarlyExitException( 13, input );
							throw eee13;
						}
						cnt13++;
					}
					loop13:
						;


					if ( state.backtracking == 0 )
					{
						 t = builder.ToString(); 
					}
					// Language\\Template.g3:326:5: ( '()$' | '$' (=> ( '\\r' )? '\\n' )? ({...}? => (=> ( '\\r' )? '\\n' |ch= . ) )+ (=> ( '\\r' )? '\\n' )? (=> '$@end$' | . ) ({...}? ( '\\r' )? '\\n' )? )
					int alt24=2;
					int LA24_0 = input.LA(1);

					if ( (LA24_0=='(') )
					{
						alt24=1;
					}
					else if ( (LA24_0=='$') )
					{
						alt24=2;
					}
					else
					{
						if (state.backtracking>0) {state.failed=true; return ;}
						NoViableAltException nvae = new NoViableAltException("", 24, 0, input);

						throw nvae;
					}
					switch ( alt24 )
					{
					case 1:
						// Language\\Template.g3:326:7: '()$'
						{
						Match("()$"); if (state.failed) return ;

						if ( state.backtracking == 0 )
						{

													_type = TemplateParser.REGION_REF;
												
						}

						}
						break;
					case 2:
						// Language\\Template.g3:330:7: '$' (=> ( '\\r' )? '\\n' )? ({...}? => (=> ( '\\r' )? '\\n' |ch= . ) )+ (=> ( '\\r' )? '\\n' )? (=> '$@end$' | . ) ({...}? ( '\\r' )? '\\n' )?
						{
						Match('$'); if (state.failed) return ;
						if ( state.backtracking == 0 )
						{

													_type = TemplateParser.REGION_DEF;
													builder.Append("::=");
												
						}
						// Language\\Template.g3:335:6: (=> ( '\\r' )? '\\n' )?
						int alt15=2;
						int LA15_0 = input.LA(1);

						if ( (LA15_0=='\r') )
						{
							int LA15_1 = input.LA(2);

							if ( (LA15_1=='\n') )
							{
								int LA15_4 = input.LA(3);

								if ( (synpred8_Template()) )
								{
									alt15=1;
								}
							}
						}
						else if ( (LA15_0=='\n') )
						{
							int LA15_2 = input.LA(2);

							if ( (synpred8_Template()) )
							{
								alt15=1;
							}
						}
						switch ( alt15 )
						{
						case 1:
							// Language\\Template.g3:335:8: => ( '\\r' )? '\\n'
							{

							// Language\\Template.g3:335:23: ( '\\r' )?
							int alt14=2;
							int LA14_0 = input.LA(1);

							if ( (LA14_0=='\r') )
							{
								alt14=1;
							}
							switch ( alt14 )
							{
							case 1:
								// Language\\Template.g3:335:24: '\\r'
								{
								Match('\r'); if (state.failed) return ;

								}
								break;

							}

							Match('\n'); if (state.failed) return ;

							}
							break;

						}

						if ( state.backtracking == 0 )
						{
							atLeft = false;
						}
						// Language\\Template.g3:337:6: ({...}? => (=> ( '\\r' )? '\\n' |ch= . ) )+
						int cnt18=0;
						for ( ; ; )
						{
							int alt18=2;
							alt18 = dfa18.Predict(input);
							switch ( alt18 )
							{
							case 1:
								// Language\\Template.g3:337:8: {...}? => (=> ( '\\r' )? '\\n' |ch= . )
								{
								if ( !((!(upcomingAtEND(1) || ( input.LA(1) == '\n' && upcomingAtEND(2) ) || ( input.LA(1) == '\r' && input.LA(2) == '\n' && upcomingAtEND(3) )))) )
								{
									if (state.backtracking>0) {state.failed=true; return ;}
									throw new FailedPredicateException(input, "ACTION", "!(upcomingAtEND(1) || ( input.LA(1) == '\\n' && upcomingAtEND(2) ) || ( input.LA(1) == '\\r' && input.LA(2) == '\\n' && upcomingAtEND(3) ))");
								}
								// Language\\Template.g3:338:7: (=> ( '\\r' )? '\\n' |ch= . )
								int alt17=2;
								int LA17_0 = input.LA(1);

								if ( (LA17_0=='\r') )
								{
									int LA17_1 = input.LA(2);

									if ( (synpred9_Template()) )
									{
										alt17=1;
									}
									else if ( (true) )
									{
										alt17=2;
									}
									else
									{
										if (state.backtracking>0) {state.failed=true; return ;}
										NoViableAltException nvae = new NoViableAltException("", 17, 1, input);

										throw nvae;
									}
								}
								else if ( (LA17_0=='\n') )
								{
									int LA17_2 = input.LA(2);

									if ( (synpred9_Template()) )
									{
										alt17=1;
									}
									else if ( (true) )
									{
										alt17=2;
									}
									else
									{
										if (state.backtracking>0) {state.failed=true; return ;}
										NoViableAltException nvae = new NoViableAltException("", 17, 2, input);

										throw nvae;
									}
								}
								else if ( ((LA17_0>='\u0000' && LA17_0<='\t')||(LA17_0>='\u000B' && LA17_0<='\f')||(LA17_0>='\u000E' && LA17_0<='\uFFFF')) )
								{
									alt17=2;
								}
								else
								{
									if (state.backtracking>0) {state.failed=true; return ;}
									NoViableAltException nvae = new NoViableAltException("", 17, 0, input);

									throw nvae;
								}
								switch ( alt17 )
								{
								case 1:
									// Language\\Template.g3:338:9: => ( '\\r' )? '\\n'
									{

									// Language\\Template.g3:338:24: ( '\\r' )?
									int alt16=2;
									int LA16_0 = input.LA(1);

									if ( (LA16_0=='\r') )
									{
										alt16=1;
									}
									switch ( alt16 )
									{
									case 1:
										// Language\\Template.g3:338:25: '\\r'
										{
										Match('\r'); if (state.failed) return ;
										if ( state.backtracking == 0 )
										{
											builder.Append('\r');
										}

										}
										break;

									}

									Match('\n'); if (state.failed) return ;
									if ( state.backtracking == 0 )
									{
										builder.Append('\n'); atLeft = true;
									}

									}
									break;
								case 2:
									// Language\\Template.g3:339:9: ch= .
									{
									ch = input.LA(1);
									MatchAny(); if (state.failed) return ;
									if ( state.backtracking == 0 )
									{
										builder.Append((char)ch); atLeft = false;
									}

									}
									break;

								}


								}
								break;

							default:
								if ( cnt18 >= 1 )
									goto loop18;

								if (state.backtracking>0) {state.failed=true; return ;}
								EarlyExitException eee18 = new EarlyExitException( 18, input );
								throw eee18;
							}
							cnt18++;
						}
						loop18:
							;


						// Language\\Template.g3:342:6: (=> ( '\\r' )? '\\n' )?
						int alt20=2;
						int LA20_0 = input.LA(1);

						if ( (LA20_0=='\r') )
						{
							int LA20_1 = input.LA(2);

							if ( (LA20_1=='\n') )
							{
								int LA20_4 = input.LA(3);

								if ( (LA20_4=='$') && (synpred10_Template()))
								{
									alt20=1;
								}
								else if ( ((LA20_4>='\u0000' && LA20_4<='#')||(LA20_4>='%' && LA20_4<='\uFFFF')) && (synpred10_Template()))
								{
									alt20=1;
								}
							}
						}
						else if ( (LA20_0=='\n') )
						{
							int LA20_2 = input.LA(2);

							if ( (LA20_2=='$') && (synpred10_Template()))
							{
								alt20=1;
							}
							else if ( (LA20_2=='\r') )
							{
								int LA20_6 = input.LA(3);

								if ( (synpred10_Template()) )
								{
									alt20=1;
								}
							}
							else if ( (LA20_2=='\n') )
							{
								int LA20_7 = input.LA(3);

								if ( (synpred10_Template()) )
								{
									alt20=1;
								}
							}
							else if ( ((LA20_2>='\u0000' && LA20_2<='\t')||(LA20_2>='\u000B' && LA20_2<='\f')||(LA20_2>='\u000E' && LA20_2<='#')||(LA20_2>='%' && LA20_2<='\uFFFF')) && (synpred10_Template()))
							{
								alt20=1;
							}
						}
						switch ( alt20 )
						{
						case 1:
							// Language\\Template.g3:342:8: => ( '\\r' )? '\\n'
							{

							// Language\\Template.g3:342:23: ( '\\r' )?
							int alt19=2;
							int LA19_0 = input.LA(1);

							if ( (LA19_0=='\r') )
							{
								alt19=1;
							}
							switch ( alt19 )
							{
							case 1:
								// Language\\Template.g3:342:24: '\\r'
								{
								Match('\r'); if (state.failed) return ;

								}
								break;

							}

							Match('\n'); if (state.failed) return ;
							if ( state.backtracking == 0 )
							{
								atLeft = true;
							}

							}
							break;

						}

						// Language\\Template.g3:343:6: (=> '$@end$' | . )
						int alt21=2;
						int LA21_0 = input.LA(1);

						if ( (LA21_0=='$') )
						{
							int LA21_1 = input.LA(2);

							if ( (LA21_1=='@') && (synpred11_Template()))
							{
								alt21=1;
							}
							else
							{
								alt21=2;}
						}
						else if ( ((LA21_0>='\u0000' && LA21_0<='#')||(LA21_0>='%' && LA21_0<='\uFFFF')) )
						{
							alt21=2;
						}
						else
						{
							if (state.backtracking>0) {state.failed=true; return ;}
							NoViableAltException nvae = new NoViableAltException("", 21, 0, input);

							throw nvae;
						}
						switch ( alt21 )
						{
						case 1:
							// Language\\Template.g3:343:8: => '$@end$'
							{

							Match("$@end$"); if (state.failed) return ;


							}
							break;
						case 2:
							// Language\\Template.g3:344:8: .
							{
							MatchAny(); if (state.failed) return ;
							if ( state.backtracking == 0 )
							{
								self.error("missing region "+t+" $@end$ tag");
							}

							}
							break;

						}

						// Language\\Template.g3:346:6: ({...}? ( '\\r' )? '\\n' )?
						int alt23=2;
						int LA23_0 = input.LA(1);

						if ( (LA23_0=='\n'||LA23_0=='\r') )
						{
							alt23=1;
						}
						switch ( alt23 )
						{
						case 1:
							// Language\\Template.g3:346:8: {...}? ( '\\r' )? '\\n'
							{
							if ( !((atLeft)) )
							{
								if (state.backtracking>0) {state.failed=true; return ;}
								throw new FailedPredicateException(input, "ACTION", "atLeft");
							}
							// Language\\Template.g3:346:18: ( '\\r' )?
							int alt22=2;
							int LA22_0 = input.LA(1);

							if ( (LA22_0=='\r') )
							{
								alt22=1;
							}
							switch ( alt22 )
							{
							case 1:
								// Language\\Template.g3:346:19: '\\r'
								{
								Match('\r'); if (state.failed) return ;

								}
								break;

							}

							Match('\n'); if (state.failed) return ;

							}
							break;

						}


						}
						break;

					}

					if ( state.backtracking == 0 )
					{

											Text = builder.ToString();
										
					}

					}
					break;
				case 6:
					// Language\\Template.g3:351:6: '$' EXPR[out subtext] '$'
					{
					Match('$'); if (state.failed) return ;
					mEXPR(out subtext); if (state.failed) return ;
					Match('$'); if (state.failed) return ;
					if ( state.backtracking == 0 )
					{
						 Text = subtext; 
					}

					}
					break;

				}

				if ( state.backtracking == 0 )
				{

									//ChunkToken t = new ChunkToken(_type, Text, currentIndent);
									//state.token = t;
									state.token = new ChunkToken(_type, Text, currentIndent);
								
				}

				}
				break;

			}


			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
		}
	}
	// $ANTLR end "ACTION"

	// $ANTLR start "LITERAL"
	private void mLITERAL()
	{
		try
		{
			int _type = LITERAL;
			int _channel = DEFAULT_TOKEN_CHANNEL;
			IToken ind=null;
			int ch;


				int loopStartIndex = Text.Length;
				int col = CharPositionInLine;
				System.Text.StringBuilder builder = new System.Text.StringBuilder();

			// Language\\Template.g3:369:4: ( ( '\\\\' ( '$' | '\\\\' |ch=~ ( '$' | '\\\\' ) ) |ind= INDENT |ch=~ ( '$' | '\\r' | '\\n' | '\\\\' | ' ' | '\\t' ) )+ )
			// Language\\Template.g3:369:4: ( '\\\\' ( '$' | '\\\\' |ch=~ ( '$' | '\\\\' ) ) |ind= INDENT |ch=~ ( '$' | '\\r' | '\\n' | '\\\\' | ' ' | '\\t' ) )+
			{
			// Language\\Template.g3:369:4: ( '\\\\' ( '$' | '\\\\' |ch=~ ( '$' | '\\\\' ) ) |ind= INDENT |ch=~ ( '$' | '\\r' | '\\n' | '\\\\' | ' ' | '\\t' ) )+
			int cnt28=0;
			for ( ; ; )
			{
				int alt28=4;
				int LA28_0 = input.LA(1);

				if ( (LA28_0=='\\') )
				{
					alt28=1;
				}
				else if ( (LA28_0=='\t'||LA28_0==' ') )
				{
					alt28=2;
				}
				else if ( ((LA28_0>='\u0000' && LA28_0<='\b')||(LA28_0>='\u000B' && LA28_0<='\f')||(LA28_0>='\u000E' && LA28_0<='\u001F')||(LA28_0>='!' && LA28_0<='#')||(LA28_0>='%' && LA28_0<='[')||(LA28_0>=']' && LA28_0<='\uFFFF')) )
				{
					alt28=3;
				}


				switch ( alt28 )
				{
				case 1:
					// Language\\Template.g3:369:6: '\\\\' ( '$' | '\\\\' |ch=~ ( '$' | '\\\\' ) )
					{
					Match('\\'); if (state.failed) return ;
					// Language\\Template.g3:370:4: ( '$' | '\\\\' |ch=~ ( '$' | '\\\\' ) )
					int alt27=3;
					int LA27_0 = input.LA(1);

					if ( (LA27_0=='$') )
					{
						alt27=1;
					}
					else if ( (LA27_0=='\\') )
					{
						alt27=2;
					}
					else if ( ((LA27_0>='\u0000' && LA27_0<='#')||(LA27_0>='%' && LA27_0<='[')||(LA27_0>=']' && LA27_0<='\uFFFF')) )
					{
						alt27=3;
					}
					else
					{
						if (state.backtracking>0) {state.failed=true; return ;}
						NoViableAltException nvae = new NoViableAltException("", 27, 0, input);

						throw nvae;
					}
					switch ( alt27 )
					{
					case 1:
						// Language\\Template.g3:370:6: '$'
						{
						Match('$'); if (state.failed) return ;
						if ( state.backtracking == 0 )
						{
							builder.Append("$");
						}

						}
						break;
					case 2:
						// Language\\Template.g3:371:6: '\\\\'
						{
						Match('\\'); if (state.failed) return ;
						if ( state.backtracking == 0 )
						{
							builder.Append("\\");
						}

						}
						break;
					case 3:
						// Language\\Template.g3:372:6: ch=~ ( '$' | '\\\\' )
						{
						ch= input.LA(1);
						input.Consume();
						state.failed=false;
						if ( state.backtracking == 0 )
						{
							builder.Append("\\" + (char)ch);
						}

						}
						break;

					}


					}
					break;
				case 2:
					// Language\\Template.g3:374:5: ind= INDENT
					{
					int indStart759 = GetCharIndex();
					mINDENT(); if (state.failed) return ;
					ind = new CommonToken(input, TokenConstants.INVALID_TOKEN_TYPE, TokenConstants.DEFAULT_CHANNEL, indStart759, GetCharIndex()-1);
					if ( state.backtracking == 0 )
					{

										loopStartIndex = builder.Length;
										col = CharPositionInLine - (ind!=null?ind.Text:null).Length;

										builder.Append( (ind!=null?ind.Text:null) );
										if ( col==0 && input.LA(1)=='$' )
										{
											// store indent in ASTExpr not in a literal
											currentIndent=(ind!=null?ind.Text:null);
											//text.setLength(loopStartIndex); // reset length to wack text
											builder.Length = loopStartIndex; //= Text.Substring( 0, loopStartIndex );
										}
										else
										{
											currentIndent=null;
										}
									
					}

					}
					break;
				case 3:
					// Language\\Template.g3:392:5: ch=~ ( '$' | '\\r' | '\\n' | '\\\\' | ' ' | '\\t' )
					{
					ch= input.LA(1);
					input.Consume();
					state.failed=false;
					if ( state.backtracking == 0 )
					{
						builder.Append((char)ch);
					}

					}
					break;

				default:
					if ( cnt28 >= 1 )
						goto loop28;

					if (state.backtracking>0) {state.failed=true; return ;}
					EarlyExitException eee28 = new EarlyExitException( 28, input );
					throw eee28;
				}
				cnt28++;
			}
			loop28:
				;


			if ( state.backtracking == 0 )
			{
				Text = builder.ToString();
			}
			if ( state.backtracking == 0 )
			{
				if (Text.Length==0) {_channel = HIDDEN;}
			}

			}

			state.type = _type;
			state.channel = _channel;
		}
		finally
		{
		}
	}
	// $ANTLR end "LITERAL"

	// $ANTLR start "EXPR"
	private void mEXPR(out string _text)
	{
		try
		{
			IToken st=null;
			IToken ESC1=null;
			int ch;


				_text = string.Empty;
				string subtext = string.Empty;
				System.Text.StringBuilder builder = new System.Text.StringBuilder();

			// Language\\Template.g3:407:4: ( ( ESC |st= SUBTEMPLATE | ( '=' | '+' ) ( TEMPLATE[out subtext] |st= SUBTEMPLATE |ch=~ ( '\"' | '<' | '{' ) ) |ch=~ ( '\\\\' | '{' | '=' | '+' | '$' ) )+ )
			// Language\\Template.g3:407:4: ( ESC |st= SUBTEMPLATE | ( '=' | '+' ) ( TEMPLATE[out subtext] |st= SUBTEMPLATE |ch=~ ( '\"' | '<' | '{' ) ) |ch=~ ( '\\\\' | '{' | '=' | '+' | '$' ) )+
			{
			// Language\\Template.g3:407:4: ( ESC |st= SUBTEMPLATE | ( '=' | '+' ) ( TEMPLATE[out subtext] |st= SUBTEMPLATE |ch=~ ( '\"' | '<' | '{' ) ) |ch=~ ( '\\\\' | '{' | '=' | '+' | '$' ) )+
			int cnt31=0;
			for ( ; ; )
			{
				int alt31=5;
				int LA31_0 = input.LA(1);

				if ( (LA31_0=='\\') )
				{
					alt31=1;
				}
				else if ( (LA31_0=='{') )
				{
					alt31=2;
				}
				else if ( (LA31_0=='+'||LA31_0=='=') )
				{
					alt31=3;
				}
				else if ( ((LA31_0>='\u0000' && LA31_0<='#')||(LA31_0>='%' && LA31_0<='*')||(LA31_0>=',' && LA31_0<='<')||(LA31_0>='>' && LA31_0<='[')||(LA31_0>=']' && LA31_0<='z')||(LA31_0>='|' && LA31_0<='\uFFFF')) )
				{
					alt31=4;
				}


				switch ( alt31 )
				{
				case 1:
					// Language\\Template.g3:407:6: ESC
					{
					int ESC1Start821 = GetCharIndex();
					mESC(); if (state.failed) return ;
					ESC1 = new CommonToken(input, TokenConstants.INVALID_TOKEN_TYPE, TokenConstants.DEFAULT_CHANNEL, ESC1Start821, GetCharIndex()-1);
					if ( state.backtracking == 0 )
					{
						builder.Append((ESC1!=null?ESC1.Text:null));
					}

					}
					break;
				case 2:
					// Language\\Template.g3:408:5: st= SUBTEMPLATE
					{
					int stStart837 = GetCharIndex();
					mSUBTEMPLATE(); if (state.failed) return ;
					st = new CommonToken(input, TokenConstants.INVALID_TOKEN_TYPE, TokenConstants.DEFAULT_CHANNEL, stStart837, GetCharIndex()-1);
					if ( state.backtracking == 0 )
					{
						builder.Append((st!=null?st.Text:null));
					}

					}
					break;
				case 3:
					// Language\\Template.g3:409:5: ( '=' | '+' ) ( TEMPLATE[out subtext] |st= SUBTEMPLATE |ch=~ ( '\"' | '<' | '{' ) )
					{
					// Language\\Template.g3:409:5: ( '=' | '+' )
					int alt29=2;
					int LA29_0 = input.LA(1);

					if ( (LA29_0=='=') )
					{
						alt29=1;
					}
					else if ( (LA29_0=='+') )
					{
						alt29=2;
					}
					else
					{
						if (state.backtracking>0) {state.failed=true; return ;}
						NoViableAltException nvae = new NoViableAltException("", 29, 0, input);

						throw nvae;
					}
					switch ( alt29 )
					{
					case 1:
						// Language\\Template.g3:409:7: '='
						{
						Match('='); if (state.failed) return ;
						if ( state.backtracking == 0 )
						{
							builder.Append('=');
						}

						}
						break;
					case 2:
						// Language\\Template.g3:410:6: '+'
						{
						Match('+'); if (state.failed) return ;
						if ( state.backtracking == 0 )
						{
							builder.Append('+');
						}

						}
						break;

					}

					// Language\\Template.g3:412:4: ( TEMPLATE[out subtext] |st= SUBTEMPLATE |ch=~ ( '\"' | '<' | '{' ) )
					int alt30=3;
					int LA30_0 = input.LA(1);

					if ( (LA30_0=='\"'||LA30_0=='<') )
					{
						alt30=1;
					}
					else if ( (LA30_0=='{') )
					{
						alt30=2;
					}
					else if ( ((LA30_0>='\u0000' && LA30_0<='!')||(LA30_0>='#' && LA30_0<=';')||(LA30_0>='=' && LA30_0<='z')||(LA30_0>='|' && LA30_0<='\uFFFF')) )
					{
						alt30=3;
					}
					else
					{
						if (state.backtracking>0) {state.failed=true; return ;}
						NoViableAltException nvae = new NoViableAltException("", 30, 0, input);

						throw nvae;
					}
					switch ( alt30 )
					{
					case 1:
						// Language\\Template.g3:412:6: TEMPLATE[out subtext]
						{
						mTEMPLATE(out subtext); if (state.failed) return ;
						if ( state.backtracking == 0 )
						{
							builder.Append(subtext);
						}

						}
						break;
					case 2:
						// Language\\Template.g3:413:6: st= SUBTEMPLATE
						{
						int stStart895 = GetCharIndex();
						mSUBTEMPLATE(); if (state.failed) return ;
						st = new CommonToken(input, TokenConstants.INVALID_TOKEN_TYPE, TokenConstants.DEFAULT_CHANNEL, stStart895, GetCharIndex()-1);
						if ( state.backtracking == 0 )
						{
							builder.Append((st!=null?st.Text:null));
						}

						}
						break;
					case 3:
						// Language\\Template.g3:414:6: ch=~ ( '\"' | '<' | '{' )
						{
						ch= input.LA(1);
						input.Consume();
						state.failed=false;
						if ( state.backtracking == 0 )
						{
							builder.Append((char)ch);
						}

						}
						break;

					}


					}
					break;
				case 4:
					// Language\\Template.g3:416:5: ch=~ ( '\\\\' | '{' | '=' | '+' | '$' )
					{
					ch= input.LA(1);
					input.Consume();
					state.failed=false;
					if ( state.backtracking == 0 )
					{
						builder.Append((char)ch);
					}

					}
					break;

				default:
					if ( cnt31 >= 1 )
						goto loop31;

					if (state.backtracking>0) {state.failed=true; return ;}
					EarlyExitException eee31 = new EarlyExitException( 31, input );
					throw eee31;
				}
				cnt31++;
			}
			loop31:
				;


			if ( state.backtracking == 0 )
			{
				_text = builder.ToString();
			}

			}

		}
		finally
		{
		}
	}
	// $ANTLR end "EXPR"

	// $ANTLR start "TEMPLATE"
	private void mTEMPLATE(out string _text)
	{
		try
		{
			IToken ESC2=null;
			int ch;


				_text = string.Empty;
				System.Text.StringBuilder builder = new System.Text.StringBuilder();

			// Language\\Template.g3:428:4: ( '\"' ( ESC |ch=~ ( '\\\\' | '\"' ) )* '\"' | '<<' (=> ( '\\r' )? '\\n' )? (=> ( '\\r' )? '\\n' |ch= . )* '>>' )
			int alt37=2;
			int LA37_0 = input.LA(1);

			if ( (LA37_0=='\"') )
			{
				alt37=1;
			}
			else if ( (LA37_0=='<') )
			{
				alt37=2;
			}
			else
			{
				if (state.backtracking>0) {state.failed=true; return ;}
				NoViableAltException nvae = new NoViableAltException("", 37, 0, input);

				throw nvae;
			}
			switch ( alt37 )
			{
			case 1:
				// Language\\Template.g3:428:4: '\"' ( ESC |ch=~ ( '\\\\' | '\"' ) )* '\"'
				{
				Match('\"'); if (state.failed) return ;
				if ( state.backtracking == 0 )
				{
					builder.Append('"');
				}
				// Language\\Template.g3:430:3: ( ESC |ch=~ ( '\\\\' | '\"' ) )*
				for ( ; ; )
				{
					int alt32=3;
					int LA32_0 = input.LA(1);

					if ( (LA32_0=='\\') )
					{
						alt32=1;
					}
					else if ( ((LA32_0>='\u0000' && LA32_0<='!')||(LA32_0>='#' && LA32_0<='[')||(LA32_0>=']' && LA32_0<='\uFFFF')) )
					{
						alt32=2;
					}


					switch ( alt32 )
					{
					case 1:
						// Language\\Template.g3:430:5: ESC
						{
						int ESC2Start981 = GetCharIndex();
						mESC(); if (state.failed) return ;
						ESC2 = new CommonToken(input, TokenConstants.INVALID_TOKEN_TYPE, TokenConstants.DEFAULT_CHANNEL, ESC2Start981, GetCharIndex()-1);
						if ( state.backtracking == 0 )
						{
							builder.Append((ESC2!=null?ESC2.Text:null));
						}

						}
						break;
					case 2:
						// Language\\Template.g3:431:5: ch=~ ( '\\\\' | '\"' )
						{
						ch= input.LA(1);
						input.Consume();
						state.failed=false;
						if ( state.backtracking == 0 )
						{
							builder.Append((char)ch);
						}

						}
						break;

					default:
						goto loop32;
					}
				}

				loop32:
					;


				Match('\"'); if (state.failed) return ;
				if ( state.backtracking == 0 )
				{

								builder.Append('"');
								_text = builder.ToString();
							
				}

				}
				break;
			case 2:
				// Language\\Template.g3:438:4: '<<' (=> ( '\\r' )? '\\n' )? (=> ( '\\r' )? '\\n' |ch= . )* '>>'
				{
				Match("<<"); if (state.failed) return ;

				if ( state.backtracking == 0 )
				{

								builder.Append("<<");
							
				}
				// Language\\Template.g3:442:4: (=> ( '\\r' )? '\\n' )?
				int alt34=2;
				int LA34_0 = input.LA(1);

				if ( (LA34_0=='\r') )
				{
					int LA34_1 = input.LA(2);

					if ( (LA34_1=='\n') )
					{
						int LA34_2 = input.LA(3);

						if ( (synpred12_Template()) )
						{
							alt34=1;
						}
					}
				}
				else if ( (LA34_0=='\n') )
				{
					int LA34_2 = input.LA(2);

					if ( (synpred12_Template()) )
					{
						alt34=1;
					}
				}
				switch ( alt34 )
				{
				case 1:
					// Language\\Template.g3:442:6: => ( '\\r' )? '\\n'
					{

					// Language\\Template.g3:442:21: ( '\\r' )?
					int alt33=2;
					int LA33_0 = input.LA(1);

					if ( (LA33_0=='\r') )
					{
						alt33=1;
					}
					switch ( alt33 )
					{
					case 1:
						// Language\\Template.g3:442:22: '\\r'
						{
						Match('\r'); if (state.failed) return ;

						}
						break;

					}

					Match('\n'); if (state.failed) return ;

					}
					break;

				}

				// Language\\Template.g3:444:3: (=> ( '\\r' )? '\\n' |ch= . )*
				for ( ; ; )
				{
					int alt36=3;
					int LA36_0 = input.LA(1);

					if ( (LA36_0=='>') )
					{
						int LA36_1 = input.LA(2);

						if ( (LA36_1=='>') )
						{
							int LA36_5 = input.LA(3);

							if ( ((LA36_5>='\u0000' && LA36_5<='\uFFFF')) )
							{
								alt36=2;
							}


						}
						else if ( ((LA36_1>='\u0000' && LA36_1<='=')||(LA36_1>='?' && LA36_1<='\uFFFF')) )
						{
							alt36=2;
						}


					}
					else if ( (LA36_0=='\r') )
					{
						int LA36_2 = input.LA(2);

						if ( (synpred13_Template()) )
						{
							alt36=1;
						}
						else if ( (true) )
						{
							alt36=2;
						}


					}
					else if ( (LA36_0=='\n') )
					{
						int LA36_3 = input.LA(2);

						if ( (synpred13_Template()) )
						{
							alt36=1;
						}
						else if ( (true) )
						{
							alt36=2;
						}


					}
					else if ( ((LA36_0>='\u0000' && LA36_0<='\t')||(LA36_0>='\u000B' && LA36_0<='\f')||(LA36_0>='\u000E' && LA36_0<='=')||(LA36_0>='?' && LA36_0<='\uFFFF')) )
					{
						alt36=2;
					}


					switch ( alt36 )
					{
					case 1:
						// Language\\Template.g3:444:5: => ( '\\r' )? '\\n'
						{

						// Language\\Template.g3:444:23: ( '\\r' )?
						int alt35=2;
						int LA35_0 = input.LA(1);

						if ( (LA35_0=='\r') )
						{
							alt35=1;
						}
						switch ( alt35 )
						{
						case 1:
							// Language\\Template.g3:444:24: '\\r'
							{
							Match('\r'); if (state.failed) return ;

							}
							break;

						}

						Match('\n'); if (state.failed) return ;

						}
						break;
					case 2:
						// Language\\Template.g3:445:5: ch= .
						{
						ch = input.LA(1);
						MatchAny(); if (state.failed) return ;
						if ( state.backtracking == 0 )
						{
							builder.Append((char)ch);
						}

						}
						break;

					default:
						goto loop36;
					}
				}

				loop36:
					;


				Match(">>"); if (state.failed) return ;

				if ( state.backtracking == 0 )
				{

								builder.Append(">>");
								_text = builder.ToString();
							
				}

				}
				break;

			}
		}
		finally
		{
		}
	}
	// $ANTLR end "TEMPLATE"

	// $ANTLR start "IF_EXPR"
	private void mIF_EXPR()
	{
		try
		{
			// Language\\Template.g3:456:4: ( ( ESC | SUBTEMPLATE | NESTED_PARENS |~ ( '\\\\' | '{' | '(' | ')' ) )+ )
			// Language\\Template.g3:456:4: ( ESC | SUBTEMPLATE | NESTED_PARENS |~ ( '\\\\' | '{' | '(' | ')' ) )+
			{
			// Language\\Template.g3:456:4: ( ESC | SUBTEMPLATE | NESTED_PARENS |~ ( '\\\\' | '{' | '(' | ')' ) )+
			int cnt38=0;
			for ( ; ; )
			{
				int alt38=5;
				int LA38_0 = input.LA(1);

				if ( (LA38_0=='\\') )
				{
					alt38=1;
				}
				else if ( (LA38_0=='{') )
				{
					alt38=2;
				}
				else if ( (LA38_0=='(') )
				{
					alt38=3;
				}
				else if ( ((LA38_0>='\u0000' && LA38_0<='\'')||(LA38_0>='*' && LA38_0<='[')||(LA38_0>=']' && LA38_0<='z')||(LA38_0>='|' && LA38_0<='\uFFFF')) )
				{
					alt38=4;
				}


				switch ( alt38 )
				{
				case 1:
					// Language\\Template.g3:456:6: ESC
					{
					mESC(); if (state.failed) return ;

					}
					break;
				case 2:
					// Language\\Template.g3:457:5: SUBTEMPLATE
					{
					mSUBTEMPLATE(); if (state.failed) return ;

					}
					break;
				case 3:
					// Language\\Template.g3:458:5: NESTED_PARENS
					{
					mNESTED_PARENS(); if (state.failed) return ;

					}
					break;
				case 4:
					// Language\\Template.g3:459:5: ~ ( '\\\\' | '{' | '(' | ')' )
					{
					input.Consume();
					state.failed=false;

					}
					break;

				default:
					if ( cnt38 >= 1 )
						goto loop38;

					if (state.backtracking>0) {state.failed=true; return ;}
					EarlyExitException eee38 = new EarlyExitException( 38, input );
					throw eee38;
				}
				cnt38++;
			}
			loop38:
				;



			}

		}
		finally
		{
		}
	}
	// $ANTLR end "IF_EXPR"

	// $ANTLR start "ESC_CHAR"
	private void mESC_CHAR(out char uc)
	{
		try
		{
			IToken a=null;
			IToken b=null;
			IToken c=null;
			IToken d=null;


				uc = '\0';

			// Language\\Template.g3:469:4: ( '\\\\n' | '\\\\r' | '\\\\t' | '\\\\ ' | '\\\\u' a= HEX b= HEX c= HEX d= HEX )
			int alt39=5;
			int LA39_0 = input.LA(1);

			if ( (LA39_0=='\\') )
			{
				switch ( input.LA(2) )
				{
				case 'n':
					{
					alt39=1;
					}
					break;
				case 'r':
					{
					alt39=2;
					}
					break;
				case 't':
					{
					alt39=3;
					}
					break;
				case ' ':
					{
					alt39=4;
					}
					break;
				case 'u':
					{
					alt39=5;
					}
					break;
				default:
					{
						if (state.backtracking>0) {state.failed=true; return ;}
						NoViableAltException nvae = new NoViableAltException("", 39, 1, input);

						throw nvae;
					}
				}

			}
			else
			{
				if (state.backtracking>0) {state.failed=true; return ;}
				NoViableAltException nvae = new NoViableAltException("", 39, 0, input);

				throw nvae;
			}
			switch ( alt39 )
			{
			case 1:
				// Language\\Template.g3:469:4: '\\\\n'
				{
				Match("\\n"); if (state.failed) return ;

				if ( state.backtracking == 0 )
				{
					uc = '\n';
				}

				}
				break;
			case 2:
				// Language\\Template.g3:470:4: '\\\\r'
				{
				Match("\\r"); if (state.failed) return ;

				if ( state.backtracking == 0 )
				{
					uc = '\r';
				}

				}
				break;
			case 3:
				// Language\\Template.g3:471:4: '\\\\t'
				{
				Match("\\t"); if (state.failed) return ;

				if ( state.backtracking == 0 )
				{
					uc = '\t';
				}

				}
				break;
			case 4:
				// Language\\Template.g3:472:4: '\\\\ '
				{
				Match("\\ "); if (state.failed) return ;

				if ( state.backtracking == 0 )
				{
					uc = ' ';
				}

				}
				break;
			case 5:
				// Language\\Template.g3:473:4: '\\\\u' a= HEX b= HEX c= HEX d= HEX
				{
				Match("\\u"); if (state.failed) return ;

				int aStart1192 = GetCharIndex();
				mHEX(); if (state.failed) return ;
				a = new CommonToken(input, TokenConstants.INVALID_TOKEN_TYPE, TokenConstants.DEFAULT_CHANNEL, aStart1192, GetCharIndex()-1);
				int bStart1196 = GetCharIndex();
				mHEX(); if (state.failed) return ;
				b = new CommonToken(input, TokenConstants.INVALID_TOKEN_TYPE, TokenConstants.DEFAULT_CHANNEL, bStart1196, GetCharIndex()-1);
				int cStart1200 = GetCharIndex();
				mHEX(); if (state.failed) return ;
				c = new CommonToken(input, TokenConstants.INVALID_TOKEN_TYPE, TokenConstants.DEFAULT_CHANNEL, cStart1200, GetCharIndex()-1);
				int dStart1204 = GetCharIndex();
				mHEX(); if (state.failed) return ;
				d = new CommonToken(input, TokenConstants.INVALID_TOKEN_TYPE, TokenConstants.DEFAULT_CHANNEL, dStart1204, GetCharIndex()-1);
				if ( state.backtracking == 0 )
				{
					uc = (char)int.Parse((a!=null?a.Text:null)+(b!=null?b.Text:null)+(c!=null?c.Text:null)+(d!=null?d.Text:null), System.Globalization.NumberStyles.AllowHexSpecifier);
				}

				}
				break;

			}
		}
		finally
		{
		}
	}
	// $ANTLR end "ESC_CHAR"

	// $ANTLR start "ESC"
	private void mESC()
	{
		try
		{
			// Language\\Template.g3:479:4: ( '\\\\' . )
			// Language\\Template.g3:479:4: '\\\\' .
			{
			Match('\\'); if (state.failed) return ;
			MatchAny(); if (state.failed) return ;

			}

		}
		finally
		{
		}
	}
	// $ANTLR end "ESC"

	// $ANTLR start "HEX"
	private void mHEX()
	{
		try
		{
			// Language\\Template.g3:484:4: ( '0' .. '9' | 'A' .. 'F' | 'a' .. 'f' )
			// Language\\Template.g3:
			{
			if ( (input.LA(1)>='0' && input.LA(1)<='9')||(input.LA(1)>='A' && input.LA(1)<='F')||(input.LA(1)>='a' && input.LA(1)<='f') )
			{
				input.Consume();
			state.failed=false;
			}
			else
			{
				if (state.backtracking>0) {state.failed=true; return ;}
				MismatchedSetException mse = new MismatchedSetException(null,input);
				Recover(mse);
				throw mse;}


			}

		}
		finally
		{
		}
	}
	// $ANTLR end "HEX"

	// $ANTLR start "SUBTEMPLATE"
	private void mSUBTEMPLATE()
	{
		try
		{
			// Language\\Template.g3:489:4: ( '{' ( SUBTEMPLATE | ESC |~ ( '{' | '\\\\' | '}' ) )* '}' )
			// Language\\Template.g3:489:4: '{' ( SUBTEMPLATE | ESC |~ ( '{' | '\\\\' | '}' ) )* '}'
			{
			Match('{'); if (state.failed) return ;
			// Language\\Template.g3:490:3: ( SUBTEMPLATE | ESC |~ ( '{' | '\\\\' | '}' ) )*
			for ( ; ; )
			{
				int alt40=4;
				int LA40_0 = input.LA(1);

				if ( (LA40_0=='{') )
				{
					alt40=1;
				}
				else if ( (LA40_0=='\\') )
				{
					alt40=2;
				}
				else if ( ((LA40_0>='\u0000' && LA40_0<='[')||(LA40_0>=']' && LA40_0<='z')||LA40_0=='|'||(LA40_0>='~' && LA40_0<='\uFFFF')) )
				{
					alt40=3;
				}


				switch ( alt40 )
				{
				case 1:
					// Language\\Template.g3:490:5: SUBTEMPLATE
					{
					mSUBTEMPLATE(); if (state.failed) return ;

					}
					break;
				case 2:
					// Language\\Template.g3:491:5: ESC
					{
					mESC(); if (state.failed) return ;

					}
					break;
				case 3:
					// Language\\Template.g3:492:5: ~ ( '{' | '\\\\' | '}' )
					{
					input.Consume();
					state.failed=false;

					}
					break;

				default:
					goto loop40;
				}
			}

			loop40:
				;


			Match('}'); if (state.failed) return ;

			}

		}
		finally
		{
		}
	}
	// $ANTLR end "SUBTEMPLATE"

	// $ANTLR start "NESTED_PARENS"
	private void mNESTED_PARENS()
	{
		try
		{
			// Language\\Template.g3:499:4: ( '(' ( NESTED_PARENS | ESC |~ ( '(' | '\\\\' | ')' ) )+ ')' )
			// Language\\Template.g3:499:4: '(' ( NESTED_PARENS | ESC |~ ( '(' | '\\\\' | ')' ) )+ ')'
			{
			Match('('); if (state.failed) return ;
			// Language\\Template.g3:500:3: ( NESTED_PARENS | ESC |~ ( '(' | '\\\\' | ')' ) )+
			int cnt41=0;
			for ( ; ; )
			{
				int alt41=4;
				int LA41_0 = input.LA(1);

				if ( (LA41_0=='(') )
				{
					alt41=1;
				}
				else if ( (LA41_0=='\\') )
				{
					alt41=2;
				}
				else if ( ((LA41_0>='\u0000' && LA41_0<='\'')||(LA41_0>='*' && LA41_0<='[')||(LA41_0>=']' && LA41_0<='\uFFFF')) )
				{
					alt41=3;
				}


				switch ( alt41 )
				{
				case 1:
					// Language\\Template.g3:500:5: NESTED_PARENS
					{
					mNESTED_PARENS(); if (state.failed) return ;

					}
					break;
				case 2:
					// Language\\Template.g3:501:5: ESC
					{
					mESC(); if (state.failed) return ;

					}
					break;
				case 3:
					// Language\\Template.g3:502:5: ~ ( '(' | '\\\\' | ')' )
					{
					input.Consume();
					state.failed=false;

					}
					break;

				default:
					if ( cnt41 >= 1 )
						goto loop41;

					if (state.backtracking>0) {state.failed=true; return ;}
					EarlyExitException eee41 = new EarlyExitException( 41, input );
					throw eee41;
				}
				cnt41++;
			}
			loop41:
				;


			Match(')'); if (state.failed) return ;

			}

		}
		finally
		{
		}
	}
	// $ANTLR end "NESTED_PARENS"

	// $ANTLR start "INDENT"
	private void mINDENT()
	{
		try
		{
			// Language\\Template.g3:509:4: ( ( ' ' | '\\t' )+ )
			// Language\\Template.g3:509:4: ( ' ' | '\\t' )+
			{
			// Language\\Template.g3:509:4: ( ' ' | '\\t' )+
			int cnt42=0;
			for ( ; ; )
			{
				int alt42=2;
				int LA42_0 = input.LA(1);

				if ( (LA42_0=='\t'||LA42_0==' ') )
				{
					alt42=1;
				}


				switch ( alt42 )
				{
				case 1:
					// Language\\Template.g3:
					{
					input.Consume();
					state.failed=false;

					}
					break;

				default:
					if ( cnt42 >= 1 )
						goto loop42;

					if (state.backtracking>0) {state.failed=true; return ;}
					EarlyExitException eee42 = new EarlyExitException( 42, input );
					throw eee42;
				}
				cnt42++;
			}
			loop42:
				;



			}

		}
		finally
		{
		}
	}
	// $ANTLR end "INDENT"

	// $ANTLR start "COMMENT"
	private void mCOMMENT()
	{
		try
		{

				int startCol = CharPositionInLine;

			// Language\\Template.g3:518:4: ( '$!' ( . )* '!$' ({...}? => ( '\\r' )? '\\n' )? )
			// Language\\Template.g3:518:4: '$!' ( . )* '!$' ({...}? => ( '\\r' )? '\\n' )?
			{
			Match("$!"); if (state.failed) return ;

			// Language\\Template.g3:518:9: ( . )*
			for ( ; ; )
			{
				int alt43=2;
				int LA43_0 = input.LA(1);

				if ( (LA43_0=='!') )
				{
					int LA43_1 = input.LA(2);

					if ( (LA43_1=='$') )
					{
						alt43=2;
					}
					else if ( ((LA43_1>='\u0000' && LA43_1<='#')||(LA43_1>='%' && LA43_1<='\uFFFF')) )
					{
						alt43=1;
					}


				}
				else if ( ((LA43_0>='\u0000' && LA43_0<=' ')||(LA43_0>='\"' && LA43_0<='\uFFFF')) )
				{
					alt43=1;
				}


				switch ( alt43 )
				{
				case 1:
					// Language\\Template.g3:518:0: .
					{
					MatchAny(); if (state.failed) return ;

					}
					break;

				default:
					goto loop43;
				}
			}

			loop43:
				;


			Match("!$"); if (state.failed) return ;

			// Language\\Template.g3:518:17: ({...}? => ( '\\r' )? '\\n' )?
			int alt45=2;
			int LA45_0 = input.LA(1);

			if ( (LA45_0=='\n'||LA45_0=='\r') && ((startCol==0)))
			{
				alt45=1;
			}
			switch ( alt45 )
			{
			case 1:
				// Language\\Template.g3:518:19: {...}? => ( '\\r' )? '\\n'
				{
				if ( !((startCol==0)) )
				{
					if (state.backtracking>0) {state.failed=true; return ;}
					throw new FailedPredicateException(input, "COMMENT", "startCol==0");
				}
				// Language\\Template.g3:518:37: ( '\\r' )?
				int alt44=2;
				int LA44_0 = input.LA(1);

				if ( (LA44_0=='\r') )
				{
					alt44=1;
				}
				switch ( alt44 )
				{
				case 1:
					// Language\\Template.g3:518:38: '\\r'
					{
					Match('\r'); if (state.failed) return ;

					}
					break;

				}

				Match('\n'); if (state.failed) return ;

				}
				break;

			}


			}

		}
		finally
		{
		}
	}
	// $ANTLR end "COMMENT"

	public override void mTokens()
	{
		// Language\\Template.g3:1:10: ( NEWLINE | ACTION | LITERAL )
		int alt46=3;
		int LA46_0 = input.LA(1);

		if ( (LA46_0=='\n'||LA46_0=='\r') )
		{
			alt46=1;
		}
		else if ( (LA46_0=='$') )
		{
			alt46=2;
		}
		else if ( ((LA46_0>='\u0000' && LA46_0<='\t')||(LA46_0>='\u000B' && LA46_0<='\f')||(LA46_0>='\u000E' && LA46_0<='#')||(LA46_0>='%' && LA46_0<='\uFFFF')) )
		{
			alt46=3;
		}
		else
		{
			if (state.backtracking>0) {state.failed=true; return ;}
			NoViableAltException nvae = new NoViableAltException("", 46, 0, input);

			throw nvae;
		}
		switch ( alt46 )
		{
		case 1:
			// Language\\Template.g3:1:10: NEWLINE
			{
			mNEWLINE(); if (state.failed) return ;

			}
			break;
		case 2:
			// Language\\Template.g3:1:18: ACTION
			{
			mACTION(); if (state.failed) return ;

			}
			break;
		case 3:
			// Language\\Template.g3:1:25: LITERAL
			{
			mLITERAL(); if (state.failed) return ;

			}
			break;

		}

	}

	// $ANTLR start synpred1_Template
	public void synpred1_Template_fragment()
	{
		// Language\\Template.g3:280:4: ( '$\\\\' )
		// Language\\Template.g3:280:5: '$\\\\'
		{
		Match("$\\"); if (state.failed) return ;


		}
	}
	// $ANTLR end synpred1_Template

	// $ANTLR start synpred10_Template
	public void synpred10_Template_fragment()
	{
		// Language\\Template.g3:342:8: ( '\\r' | '\\n' )
		// Language\\Template.g3:
		{
		if ( input.LA(1)=='\n'||input.LA(1)=='\r' )
		{
			input.Consume();
		state.failed=false;
		}
		else
		{
			if (state.backtracking>0) {state.failed=true; return ;}
			MismatchedSetException mse = new MismatchedSetException(null,input);
			Recover(mse);
			throw mse;}


		}
	}
	// $ANTLR end synpred10_Template

	// $ANTLR start synpred11_Template
	public void synpred11_Template_fragment()
	{
		// Language\\Template.g3:343:8: ( '$@end$' )
		// Language\\Template.g3:343:9: '$@end$'
		{
		Match("$@end$"); if (state.failed) return ;


		}
	}
	// $ANTLR end synpred11_Template

	// $ANTLR start synpred12_Template
	public void synpred12_Template_fragment()
	{
		// Language\\Template.g3:442:6: ( '\\r' | '\\n' )
		// Language\\Template.g3:
		{
		if ( input.LA(1)=='\n'||input.LA(1)=='\r' )
		{
			input.Consume();
		state.failed=false;
		}
		else
		{
			if (state.backtracking>0) {state.failed=true; return ;}
			MismatchedSetException mse = new MismatchedSetException(null,input);
			Recover(mse);
			throw mse;}


		}
	}
	// $ANTLR end synpred12_Template

	// $ANTLR start synpred13_Template
	public void synpred13_Template_fragment()
	{
		// Language\\Template.g3:444:5: ( ( '\\r' )? '\\n>>' )
		// Language\\Template.g3:444:6: ( '\\r' )? '\\n>>'
		{
		// Language\\Template.g3:444:6: ( '\\r' )?
		int alt47=2;
		int LA47_0 = input.LA(1);

		if ( (LA47_0=='\r') )
		{
			alt47=1;
		}
		switch ( alt47 )
		{
		case 1:
			// Language\\Template.g3:444:0: '\\r'
			{
			Match('\r'); if (state.failed) return ;

			}
			break;

		}

		Match("\n>>"); if (state.failed) return ;


		}
	}
	// $ANTLR end synpred13_Template

	// $ANTLR start synpred2_Template
	public void synpred2_Template_fragment()
	{
		// Language\\Template.g3:287:5: ( '$!' )
		// Language\\Template.g3:287:6: '$!'
		{
		Match("$!"); if (state.failed) return ;


		}
	}
	// $ANTLR end synpred2_Template

	// $ANTLR start synpred3_Template
	public void synpred3_Template_fragment()
	{
		// Language\\Template.g3:289:7: ( '$if' ( ' ' | '(' ) )
		// Language\\Template.g3:289:8: '$if' ( ' ' | '(' )
		{
		Match("$if"); if (state.failed) return ;

		if ( input.LA(1)==' '||input.LA(1)=='(' )
		{
			input.Consume();
		state.failed=false;
		}
		else
		{
			if (state.backtracking>0) {state.failed=true; return ;}
			MismatchedSetException mse = new MismatchedSetException(null,input);
			Recover(mse);
			throw mse;}


		}
	}
	// $ANTLR end synpred3_Template

	// $ANTLR start synpred4_Template
	public void synpred4_Template_fragment()
	{
		// Language\\Template.g3:296:6: ( '$elseif' ( ' ' | '(' ) )
		// Language\\Template.g3:296:7: '$elseif' ( ' ' | '(' )
		{
		Match("$elseif"); if (state.failed) return ;

		if ( input.LA(1)==' '||input.LA(1)=='(' )
		{
			input.Consume();
		state.failed=false;
		}
		else
		{
			if (state.backtracking>0) {state.failed=true; return ;}
			MismatchedSetException mse = new MismatchedSetException(null,input);
			Recover(mse);
			throw mse;}


		}
	}
	// $ANTLR end synpred4_Template

	// $ANTLR start synpred5_Template
	public void synpred5_Template_fragment()
	{
		// Language\\Template.g3:303:6: ( '$else$' )
		// Language\\Template.g3:303:7: '$else$'
		{
		Match("$else$"); if (state.failed) return ;


		}
	}
	// $ANTLR end synpred5_Template

	// $ANTLR start synpred6_Template
	public void synpred6_Template_fragment()
	{
		// Language\\Template.g3:310:6: ( '$endif$' )
		// Language\\Template.g3:310:7: '$endif$'
		{
		Match("$endif$"); if (state.failed) return ;


		}
	}
	// $ANTLR end synpred6_Template

	// $ANTLR start synpred7_Template
	public void synpred7_Template_fragment()
	{
		// Language\\Template.g3:320:5: ( '$@' )
		// Language\\Template.g3:320:6: '$@'
		{
		Match("$@"); if (state.failed) return ;


		}
	}
	// $ANTLR end synpred7_Template

	// $ANTLR start synpred8_Template
	public void synpred8_Template_fragment()
	{
		// Language\\Template.g3:335:8: ( '\\r' | '\\n' )
		// Language\\Template.g3:
		{
		if ( input.LA(1)=='\n'||input.LA(1)=='\r' )
		{
			input.Consume();
		state.failed=false;
		}
		else
		{
			if (state.backtracking>0) {state.failed=true; return ;}
			MismatchedSetException mse = new MismatchedSetException(null,input);
			Recover(mse);
			throw mse;}


		}
	}
	// $ANTLR end synpred8_Template

	// $ANTLR start synpred9_Template
	public void synpred9_Template_fragment()
	{
		// Language\\Template.g3:338:9: ( '\\r' | '\\n' )
		// Language\\Template.g3:
		{
		if ( input.LA(1)=='\n'||input.LA(1)=='\r' )
		{
			input.Consume();
		state.failed=false;
		}
		else
		{
			if (state.backtracking>0) {state.failed=true; return ;}
			MismatchedSetException mse = new MismatchedSetException(null,input);
			Recover(mse);
			throw mse;}


		}
	}
	// $ANTLR end synpred9_Template

	public bool synpred8_Template()
	{
		state.backtracking++;
		int start = input.Mark();
		try
		{
			synpred8_Template_fragment(); // can never throw exception
		}
		catch ( RecognitionException re )
		{
			System.Console.Error.WriteLine("impossible: "+re);
		}
		bool success = !state.failed;
		input.Rewind(start);
		state.backtracking--;
		state.failed=false;
		return success;
	}
	public bool synpred9_Template()
	{
		state.backtracking++;
		int start = input.Mark();
		try
		{
			synpred9_Template_fragment(); // can never throw exception
		}
		catch ( RecognitionException re )
		{
			System.Console.Error.WriteLine("impossible: "+re);
		}
		bool success = !state.failed;
		input.Rewind(start);
		state.backtracking--;
		state.failed=false;
		return success;
	}
	public bool synpred10_Template()
	{
		state.backtracking++;
		int start = input.Mark();
		try
		{
			synpred10_Template_fragment(); // can never throw exception
		}
		catch ( RecognitionException re )
		{
			System.Console.Error.WriteLine("impossible: "+re);
		}
		bool success = !state.failed;
		input.Rewind(start);
		state.backtracking--;
		state.failed=false;
		return success;
	}
	public bool synpred11_Template()
	{
		state.backtracking++;
		int start = input.Mark();
		try
		{
			synpred11_Template_fragment(); // can never throw exception
		}
		catch ( RecognitionException re )
		{
			System.Console.Error.WriteLine("impossible: "+re);
		}
		bool success = !state.failed;
		input.Rewind(start);
		state.backtracking--;
		state.failed=false;
		return success;
	}
	public bool synpred3_Template()
	{
		state.backtracking++;
		int start = input.Mark();
		try
		{
			synpred3_Template_fragment(); // can never throw exception
		}
		catch ( RecognitionException re )
		{
			System.Console.Error.WriteLine("impossible: "+re);
		}
		bool success = !state.failed;
		input.Rewind(start);
		state.backtracking--;
		state.failed=false;
		return success;
	}
	public bool synpred4_Template()
	{
		state.backtracking++;
		int start = input.Mark();
		try
		{
			synpred4_Template_fragment(); // can never throw exception
		}
		catch ( RecognitionException re )
		{
			System.Console.Error.WriteLine("impossible: "+re);
		}
		bool success = !state.failed;
		input.Rewind(start);
		state.backtracking--;
		state.failed=false;
		return success;
	}
	public bool synpred5_Template()
	{
		state.backtracking++;
		int start = input.Mark();
		try
		{
			synpred5_Template_fragment(); // can never throw exception
		}
		catch ( RecognitionException re )
		{
			System.Console.Error.WriteLine("impossible: "+re);
		}
		bool success = !state.failed;
		input.Rewind(start);
		state.backtracking--;
		state.failed=false;
		return success;
	}
	public bool synpred6_Template()
	{
		state.backtracking++;
		int start = input.Mark();
		try
		{
			synpred6_Template_fragment(); // can never throw exception
		}
		catch ( RecognitionException re )
		{
			System.Console.Error.WriteLine("impossible: "+re);
		}
		bool success = !state.failed;
		input.Rewind(start);
		state.backtracking--;
		state.failed=false;
		return success;
	}
	public bool synpred7_Template()
	{
		state.backtracking++;
		int start = input.Mark();
		try
		{
			synpred7_Template_fragment(); // can never throw exception
		}
		catch ( RecognitionException re )
		{
			System.Console.Error.WriteLine("impossible: "+re);
		}
		bool success = !state.failed;
		input.Rewind(start);
		state.backtracking--;
		state.failed=false;
		return success;
	}
	public bool synpred1_Template()
	{
		state.backtracking++;
		int start = input.Mark();
		try
		{
			synpred1_Template_fragment(); // can never throw exception
		}
		catch ( RecognitionException re )
		{
			System.Console.Error.WriteLine("impossible: "+re);
		}
		bool success = !state.failed;
		input.Rewind(start);
		state.backtracking--;
		state.failed=false;
		return success;
	}
	public bool synpred2_Template()
	{
		state.backtracking++;
		int start = input.Mark();
		try
		{
			synpred2_Template_fragment(); // can never throw exception
		}
		catch ( RecognitionException re )
		{
			System.Console.Error.WriteLine("impossible: "+re);
		}
		bool success = !state.failed;
		input.Rewind(start);
		state.backtracking--;
		state.failed=false;
		return success;
	}
	public bool synpred12_Template()
	{
		state.backtracking++;
		int start = input.Mark();
		try
		{
			synpred12_Template_fragment(); // can never throw exception
		}
		catch ( RecognitionException re )
		{
			System.Console.Error.WriteLine("impossible: "+re);
		}
		bool success = !state.failed;
		input.Rewind(start);
		state.backtracking--;
		state.failed=false;
		return success;
	}
	public bool synpred13_Template()
	{
		state.backtracking++;
		int start = input.Mark();
		try
		{
			synpred13_Template_fragment(); // can never throw exception
		}
		catch ( RecognitionException re )
		{
			System.Console.Error.WriteLine("impossible: "+re);
		}
		bool success = !state.failed;
		input.Rewind(start);
		state.backtracking--;
		state.failed=false;
		return success;
	}


	#region DFA
	DFA18 dfa18;

	protected override void InitDFAs()
	{
		base.InitDFAs();
		dfa18 = new DFA18( this, new SpecialStateTransitionHandler( specialStateTransition18 ) );
	}

	class DFA18 : DFA
	{

		const string DFA18_eotS =
			"\x3\xFFFF\x2\x6\x2\xFFFF\x1\x5\x2\xFFFF\x3\x5\x1\xFFFF";
		const string DFA18_eofS =
			"\xE\xFFFF";
		const string DFA18_minS =
			"\x5\x0\x2\xFFFF\x1\x65\x2\x0\x1\x6E\x1\x64\x1\x24\x1\x0";
		const string DFA18_maxS =
			"\x1\xFFFF\x2\x0\x2\xFFFF\x2\xFFFF\x1\x65\x2\x0\x1\x6E\x1\x64\x1\x24\x1"+
			"\x0";
		const string DFA18_acceptS =
			"\x5\xFFFF\x1\x1\x1\x2\x7\xFFFF";
		const string DFA18_specialS =
			"\x1\x0\x1\x1\x1\x2\x1\x3\x1\x4\x2\xFFFF\x1\x5\x1\x6\x1\x7\x1\x8\x1\x9"+
			"\x1\xA\x1\xB}>";
		static readonly string[] DFA18_transitionS =
			{
				"\xA\x4\x1\x2\x2\x4\x1\x1\x16\x4\x1\x3\xFFDB\x4",
				"\x1\xFFFF",
				"\x1\xFFFF",
				"\xA\x5\x1\x9\x2\x5\x1\x8\x32\x5\x1\x7\xFFBF\x5",
				"\xA\x5\x1\x9\x2\x5\x1\x8\xFFF2\x5",
				"",
				"",
				"\x1\xA",
				"\x1\xFFFF",
				"\x1\xFFFF",
				"\x1\xB",
				"\x1\xC",
				"\x1\xD",
				"\x1\xFFFF"
			};

		static readonly short[] DFA18_eot = DFA.UnpackEncodedString(DFA18_eotS);
		static readonly short[] DFA18_eof = DFA.UnpackEncodedString(DFA18_eofS);
		static readonly char[] DFA18_min = DFA.UnpackEncodedStringToUnsignedChars(DFA18_minS);
		static readonly char[] DFA18_max = DFA.UnpackEncodedStringToUnsignedChars(DFA18_maxS);
		static readonly short[] DFA18_accept = DFA.UnpackEncodedString(DFA18_acceptS);
		static readonly short[] DFA18_special = DFA.UnpackEncodedString(DFA18_specialS);
		static readonly short[][] DFA18_transition;

		static DFA18()
		{
			int numStates = DFA18_transitionS.Length;
			DFA18_transition = new short[numStates][];
			for ( int i=0; i < numStates; i++ )
			{
				DFA18_transition[i] = DFA.UnpackEncodedString(DFA18_transitionS[i]);
			}
		}

		public DFA18( BaseRecognizer recognizer, SpecialStateTransitionHandler specialStateTransition )
			: base( specialStateTransition )	{
			this.recognizer = recognizer;
			this.decisionNumber = 18;
			this.eot = DFA18_eot;
			this.eof = DFA18_eof;
			this.min = DFA18_min;
			this.max = DFA18_max;
			this.accept = DFA18_accept;
			this.special = DFA18_special;
			this.transition = DFA18_transition;
		}
		public override string GetDescription()
		{
			return "()+ loopback of 337:6: ({...}? => (=> ( '\\r' )? '\\n' |ch= . ) )+";
		}
	}

	int specialStateTransition18( DFA dfa, int s, IIntStream _input )
	{
		IIntStream input = _input;
		int _s = s;
		switch ( s )
		{

			case 0:
				int LA18_0 = input.LA(1);

				s = -1;
				if ( (LA18_0=='\r') ) {s = 1;}

				else if ( (LA18_0=='\n') ) {s = 2;}

				else if ( (LA18_0=='$') ) {s = 3;}

				else if ( ((LA18_0>='\u0000' && LA18_0<='\t')||(LA18_0>='\u000B' && LA18_0<='\f')||(LA18_0>='\u000E' && LA18_0<='#')||(LA18_0>='%' && LA18_0<='\uFFFF')) ) {s = 4;}

				if ( s>=0 ) return s;
				break;

			case 1:
				int LA18_1 = input.LA(1);


				int index18_1 = input.Index;
				input.Rewind();
				s = -1;
				if ( ((!(upcomingAtEND(1) || ( input.LA(1) == '\n' && upcomingAtEND(2) ) || ( input.LA(1) == '\r' && input.LA(2) == '\n' && upcomingAtEND(3) )))) ) {s = 5;}

				else if ( (true) ) {s = 6;}


				input.Seek(index18_1);
				if ( s>=0 ) return s;
				break;

			case 2:
				int LA18_2 = input.LA(1);


				int index18_2 = input.Index;
				input.Rewind();
				s = -1;
				if ( ((!(upcomingAtEND(1) || ( input.LA(1) == '\n' && upcomingAtEND(2) ) || ( input.LA(1) == '\r' && input.LA(2) == '\n' && upcomingAtEND(3) )))) ) {s = 5;}

				else if ( (true) ) {s = 6;}


				input.Seek(index18_2);
				if ( s>=0 ) return s;
				break;

			case 3:
				int LA18_3 = input.LA(1);


				int index18_3 = input.Index;
				input.Rewind();
				s = -1;
				if ( (LA18_3=='@') ) {s = 7;}

				else if ( (LA18_3=='\r') ) {s = 8;}

				else if ( (LA18_3=='\n') ) {s = 9;}

				else if ( ((LA18_3>='\u0000' && LA18_3<='\t')||(LA18_3>='\u000B' && LA18_3<='\f')||(LA18_3>='\u000E' && LA18_3<='?')||(LA18_3>='A' && LA18_3<='\uFFFF')) && ((!(upcomingAtEND(1) || ( input.LA(1) == '\n' && upcomingAtEND(2) ) || ( input.LA(1) == '\r' && input.LA(2) == '\n' && upcomingAtEND(3) ))))) {s = 5;}

				else s = 6;


				input.Seek(index18_3);
				if ( s>=0 ) return s;
				break;

			case 4:
				int LA18_4 = input.LA(1);


				int index18_4 = input.Index;
				input.Rewind();
				s = -1;
				if ( (LA18_4=='\r') ) {s = 8;}

				else if ( (LA18_4=='\n') ) {s = 9;}

				else if ( ((LA18_4>='\u0000' && LA18_4<='\t')||(LA18_4>='\u000B' && LA18_4<='\f')||(LA18_4>='\u000E' && LA18_4<='\uFFFF')) && ((!(upcomingAtEND(1) || ( input.LA(1) == '\n' && upcomingAtEND(2) ) || ( input.LA(1) == '\r' && input.LA(2) == '\n' && upcomingAtEND(3) ))))) {s = 5;}

				else s = 6;


				input.Seek(index18_4);
				if ( s>=0 ) return s;
				break;

			case 5:
				int LA18_7 = input.LA(1);


				int index18_7 = input.Index;
				input.Rewind();
				s = -1;
				if ( (LA18_7=='e') ) {s = 10;}

				else s = 5;


				input.Seek(index18_7);
				if ( s>=0 ) return s;
				break;

			case 6:
				int LA18_8 = input.LA(1);


				int index18_8 = input.Index;
				input.Rewind();
				s = -1;
				if ( ((!(upcomingAtEND(1) || ( input.LA(1) == '\n' && upcomingAtEND(2) ) || ( input.LA(1) == '\r' && input.LA(2) == '\n' && upcomingAtEND(3) )))) ) {s = 5;}

				else if ( (true) ) {s = 6;}


				input.Seek(index18_8);
				if ( s>=0 ) return s;
				break;

			case 7:
				int LA18_9 = input.LA(1);


				int index18_9 = input.Index;
				input.Rewind();
				s = -1;
				if ( ((!(upcomingAtEND(1) || ( input.LA(1) == '\n' && upcomingAtEND(2) ) || ( input.LA(1) == '\r' && input.LA(2) == '\n' && upcomingAtEND(3) )))) ) {s = 5;}

				else if ( (true) ) {s = 6;}


				input.Seek(index18_9);
				if ( s>=0 ) return s;
				break;

			case 8:
				int LA18_10 = input.LA(1);


				int index18_10 = input.Index;
				input.Rewind();
				s = -1;
				if ( (LA18_10=='n') ) {s = 11;}

				else s = 5;


				input.Seek(index18_10);
				if ( s>=0 ) return s;
				break;

			case 9:
				int LA18_11 = input.LA(1);


				int index18_11 = input.Index;
				input.Rewind();
				s = -1;
				if ( (LA18_11=='d') ) {s = 12;}

				else s = 5;


				input.Seek(index18_11);
				if ( s>=0 ) return s;
				break;

			case 10:
				int LA18_12 = input.LA(1);


				int index18_12 = input.Index;
				input.Rewind();
				s = -1;
				if ( (LA18_12=='$') ) {s = 13;}

				else s = 5;


				input.Seek(index18_12);
				if ( s>=0 ) return s;
				break;

			case 11:
				int LA18_13 = input.LA(1);


				int index18_13 = input.Index;
				input.Rewind();
				s = -1;
				if ( ((!(upcomingAtEND(1) || ( input.LA(1) == '\n' && upcomingAtEND(2) ) || ( input.LA(1) == '\r' && input.LA(2) == '\n' && upcomingAtEND(3) )))) ) {s = 5;}

				else if ( (true) ) {s = 6;}


				input.Seek(index18_13);
				if ( s>=0 ) return s;
				break;
		}
		if (state.backtracking>0) {state.failed=true; return -1;}
		NoViableAltException nvae = new NoViableAltException(dfa.GetDescription(), 18, _s, input);
		dfa.Error(nvae);
		throw nvae;
	}
 
	#endregion

}

} // namespace Antlr3.ST.Language
