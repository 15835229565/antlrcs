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
using Antlr.Runtime.JavaExtensions;


using System.Collections.Generic;
using Antlr.Runtime;
using Stack = System.Collections.Generic.Stack<object>;
using List = System.Collections.IList;
using ArrayList = System.Collections.Generic.List<object>;

namespace Antlr3.ST.Language
{
public partial class TemplateParser : Parser
{
	public static readonly string[] tokenNames = new string[] {
		"<invalid>", "<EOR>", "<DOWN>", "<UP>", "ACTION", "COMMENT", "ELSE", "ELSEIF", "ENDIF", "ESC", "ESC_CHAR", "EXPR", "HEX", "IF", "IF_EXPR", "INDENT", "LITERAL", "NESTED_PARENS", "NEWLINE", "REGION_DEF", "REGION_REF", "SUBTEMPLATE", "TEMPLATE"
	};
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

	public TemplateParser( ITokenStream input )
		: this( input, new RecognizerSharedState() )
	{
	}
	public TemplateParser( ITokenStream input, RecognizerSharedState state )
		: base( input, state )
	{
	}
		

	public override string[] GetTokenNames() { return TemplateParser.tokenNames; }
	public override string GrammarFileName { get { return "Language\\Template.g3"; } }


	#region Rules

	// $ANTLR start "template"
	// Language\\Template.g3:114:0: public template[StringTemplate self] : (s= LITERAL |nl= NEWLINE | action[self] )* ( EOF )? ;
	public void template( StringTemplate self )
	{
		IToken s=null;
		IToken nl=null;


			this.self = self;

		try
		{
			// Language\\Template.g3:119:4: ( (s= LITERAL |nl= NEWLINE | action[self] )* ( EOF )? )
			// Language\\Template.g3:119:4: (s= LITERAL |nl= NEWLINE | action[self] )* ( EOF )?
			{
			// Language\\Template.g3:119:4: (s= LITERAL |nl= NEWLINE | action[self] )*
			for ( ; ; )
			{
				int alt1=4;
				switch ( input.LA(1) )
				{
				case LITERAL:
					{
					alt1=1;
					}
					break;
				case NEWLINE:
					{
					alt1=2;
					}
					break;
				case ACTION:
				case IF:
				case REGION_DEF:
				case REGION_REF:
					{
					alt1=3;
					}
					break;

				}

				switch ( alt1 )
				{
				case 1:
					// Language\\Template.g3:119:6: s= LITERAL
					{
					s=(IToken)Match(input,LITERAL,Follow._LITERAL_in_template71); 
					self.addChunk(new StringRef(self,(s!=null?s.Text:null)));

					}
					break;
				case 2:
					// Language\\Template.g3:120:5: nl= NEWLINE
					{
					nl=(IToken)Match(input,NEWLINE,Follow._NEWLINE_in_template82); 

									if ( input.LA(1)!=ELSE && input.LA(1)!=ENDIF )
									{
										self.addChunk(new NewlineRef(self,(nl!=null?nl.Text:null)));
									}
								

					}
					break;
				case 3:
					// Language\\Template.g3:127:5: action[self]
					{
					PushFollow(Follow._action_in_template93);
					action(self);

					state._fsp--;


					}
					break;

				default:
					goto loop1;
				}
			}

			loop1:
				;


			// Language\\Template.g3:129:3: ( EOF )?
			int alt2=2;
			int LA2_0 = input.LA(1);

			if ( (LA2_0==EOF) )
			{
				alt2=1;
			}
			switch ( alt2 )
			{
			case 1:
				// Language\\Template.g3:129:0: EOF
				{
				Match(input,EOF,Follow._EOF_in_template103); 

				}
				break;

			}


			}

		}
		catch ( RecognitionException re )
		{
			ReportError(re);
			Recover(input,re);
		}
		finally
		{
		}
		return ;
	}
	// $ANTLR end "template"


	// $ANTLR start "action"
	// Language\\Template.g3:132:0: action[StringTemplate self] : (a= ACTION |i= IF template[subtemplate] (ei= ELSEIF template[elseIfSubtemplate] )* ( ELSE template[elseSubtemplate] )? ENDIF |rr= REGION_REF |rd= REGION_DEF );
	private void action( StringTemplate self )
	{
		IToken a=null;
		IToken i=null;
		IToken ei=null;
		IToken rr=null;
		IToken rd=null;

		try
		{
			// Language\\Template.g3:133:4: (a= ACTION |i= IF template[subtemplate] (ei= ELSEIF template[elseIfSubtemplate] )* ( ELSE template[elseSubtemplate] )? ENDIF |rr= REGION_REF |rd= REGION_DEF )
			int alt5=4;
			switch ( input.LA(1) )
			{
			case ACTION:
				{
				alt5=1;
				}
				break;
			case IF:
				{
				alt5=2;
				}
				break;
			case REGION_REF:
				{
				alt5=3;
				}
				break;
			case REGION_DEF:
				{
				alt5=4;
				}
				break;
			default:
				{
					NoViableAltException nvae = new NoViableAltException("", 5, 0, input);

					throw nvae;
				}
			}

			switch ( alt5 )
			{
			case 1:
				// Language\\Template.g3:133:4: a= ACTION
				{
				a=(IToken)Match(input,ACTION,Follow._ACTION_in_action118); 

							string indent = ((ChunkToken)a).Indentation;
							ASTExpr c = self.parseAction((a!=null?a.Text:null));
							c.setIndentation(indent);
							self.addChunk(c);
						

				}
				break;
			case 2:
				// Language\\Template.g3:141:4: i= IF template[subtemplate] (ei= ELSEIF template[elseIfSubtemplate] )* ( ELSE template[elseSubtemplate] )? ENDIF
				{
				i=(IToken)Match(input,IF,Follow._IF_in_action130); 

							ConditionalExpr c = (ConditionalExpr)self.parseAction((i!=null?i.Text:null));
							// create and precompile the subtemplate
							StringTemplate subtemplate = new StringTemplate(self.getGroup(), null);
							subtemplate.setEnclosingInstance(self);
							subtemplate.setName((i!=null?i.Text:null)+"_subtemplate");
							self.addChunk(c);
						
				PushFollow(Follow._template_in_action139);
				template(subtemplate);

				state._fsp--;

				if ( c!=null ) c.setSubtemplate(subtemplate);
				// Language\\Template.g3:153:3: (ei= ELSEIF template[elseIfSubtemplate] )*
				for ( ; ; )
				{
					int alt3=2;
					int LA3_0 = input.LA(1);

					if ( (LA3_0==ELSEIF) )
					{
						alt3=1;
					}


					switch ( alt3 )
					{
					case 1:
						// Language\\Template.g3:153:5: ei= ELSEIF template[elseIfSubtemplate]
						{
						ei=(IToken)Match(input,ELSEIF,Follow._ELSEIF_in_action151); 

										ASTExpr ec = self.parseAction((ei!=null?ei.Text:null));
										// create and precompile the subtemplate
										StringTemplate elseIfSubtemplate = new StringTemplate(self.getGroup(), null);
										elseIfSubtemplate.setEnclosingInstance(self);
										elseIfSubtemplate.setName((ei!=null?ei.Text:null)+"_subtemplate");
									
						PushFollow(Follow._template_in_action162);
						template(elseIfSubtemplate);

						state._fsp--;

						if ( c!=null ) c.addElseIfSubtemplate(ec, elseIfSubtemplate);

						}
						break;

					default:
						goto loop3;
					}
				}

				loop3:
					;


				// Language\\Template.g3:167:3: ( ELSE template[elseSubtemplate] )?
				int alt4=2;
				int LA4_0 = input.LA(1);

				if ( (LA4_0==ELSE) )
				{
					alt4=1;
				}
				switch ( alt4 )
				{
				case 1:
					// Language\\Template.g3:167:5: ELSE template[elseSubtemplate]
					{
					Match(input,ELSE,Follow._ELSE_in_action181); 

									// create and precompile the subtemplate
									StringTemplate elseSubtemplate = new StringTemplate(self.getGroup(), null);
									elseSubtemplate.setEnclosingInstance(self);
									elseSubtemplate.setName("else_subtemplate");
								
					PushFollow(Follow._template_in_action192);
					template(elseSubtemplate);

					state._fsp--;

					if ( c!=null ) c.setElseSubtemplate(elseSubtemplate);

					}
					break;

				}

				Match(input,ENDIF,Follow._ENDIF_in_action208); 

				}
				break;
			case 3:
				// Language\\Template.g3:181:4: rr= REGION_REF
				{
				rr=(IToken)Match(input,REGION_REF,Follow._REGION_REF_in_action216); 

							// define implicit template and
							// convert <@r()> to <region__enclosingTemplate__r()>
							string regionName = (rr!=null?rr.Text:null);
							string mangledRef = null;
							bool err = false;
							// watch out for <@super.r()>; that does NOT def implicit region
							// convert to <super.region__enclosingTemplate__r()>
							if ( regionName.StartsWith("super.") )
							{
								//System.out.println("super region ref "+regionName);
								string regionRef = regionName.substring("super.".Length,regionName.Length);
								string templateScope = self.getGroup().getUnMangledTemplateName(self.getName());
								StringTemplate scopeST = self.getGroup().lookupTemplate(templateScope);
								if ( scopeST==null )
								{
									self.getGroup().error("reference to region within undefined template: "+templateScope);
									err=true;
								}
								if ( !scopeST.containsRegionName(regionRef) )
								{
									self.getGroup().error("template "+templateScope+" has no region called "+regionRef);
									err=true;
								}
								else
								{
									mangledRef = self.getGroup().getMangledRegionName(templateScope,regionRef);
									mangledRef = "super."+mangledRef;
								}
							}
							else
							{
								//System.out.println("region ref "+regionName);
								StringTemplate regionST = self.getGroup().defineImplicitRegionTemplate(self,regionName);
								mangledRef = regionST.getName();
							}

							if ( !err )
							{
								// treat as regular action: mangled template include
								string indent = ((ChunkToken)rr).Indentation;
								ASTExpr c = self.parseAction(mangledRef+"()");
								c.setIndentation(indent);
								self.addChunk(c);
							}
						

				}
				break;
			case 4:
				// Language\\Template.g3:229:4: rd= REGION_DEF
				{
				rd=(IToken)Match(input,REGION_DEF,Follow._REGION_DEF_in_action228); 

							string combinedNameTemplateStr = (rd!=null?rd.Text:null);
							int indexOfDefSymbol = combinedNameTemplateStr.IndexOf("::=");
							if ( indexOfDefSymbol>=1 )
							{
								string regionName = combinedNameTemplateStr.substring(0,indexOfDefSymbol);
								string template = combinedNameTemplateStr.substring(indexOfDefSymbol+3, combinedNameTemplateStr.Length);
								StringTemplate regionST = self.getGroup().defineRegionTemplate(self,regionName,template,StringTemplate.REGION_EMBEDDED);
								// treat as regular action: mangled template include
								string indent = ((ChunkToken)rd).Indentation;
								ASTExpr c = self.parseAction(regionST.getName()+"()");
								c.setIndentation(indent);
								self.addChunk(c);
							}
							else
							{
								self.error("embedded region definition screwed up");
							}
						

				}
				break;

			}
		}
		catch ( RecognitionException re )
		{
			ReportError(re);
			Recover(input,re);
		}
		finally
		{
		}
		return ;
	}
	// $ANTLR end "action"
	#endregion

	// Delegated rules

	#region Synpreds
	#endregion

	#region DFA

	protected override void InitDFAs()
	{
		base.InitDFAs();
	}

	#endregion

	#region Follow Sets
	public static class Follow
	{
		public static readonly BitSet _LITERAL_in_template71 = new BitSet(new ulong[]{0x1D2012UL});
		public static readonly BitSet _NEWLINE_in_template82 = new BitSet(new ulong[]{0x1D2012UL});
		public static readonly BitSet _action_in_template93 = new BitSet(new ulong[]{0x1D2012UL});
		public static readonly BitSet _EOF_in_template103 = new BitSet(new ulong[]{0x2UL});
		public static readonly BitSet _ACTION_in_action118 = new BitSet(new ulong[]{0x2UL});
		public static readonly BitSet _IF_in_action130 = new BitSet(new ulong[]{0x1D2010UL});
		public static readonly BitSet _template_in_action139 = new BitSet(new ulong[]{0x1C0UL});
		public static readonly BitSet _ELSEIF_in_action151 = new BitSet(new ulong[]{0x1D2010UL});
		public static readonly BitSet _template_in_action162 = new BitSet(new ulong[]{0x1C0UL});
		public static readonly BitSet _ELSE_in_action181 = new BitSet(new ulong[]{0x1D2010UL});
		public static readonly BitSet _template_in_action192 = new BitSet(new ulong[]{0x100UL});
		public static readonly BitSet _ENDIF_in_action208 = new BitSet(new ulong[]{0x2UL});
		public static readonly BitSet _REGION_REF_in_action216 = new BitSet(new ulong[]{0x2UL});
		public static readonly BitSet _REGION_DEF_in_action228 = new BitSet(new ulong[]{0x2UL});

	}
	#endregion
}

} // namespace Antlr3.ST.Language
