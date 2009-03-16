/*
 * [The "BSD licence"]
 * Copyright (c) 2005-2008 Terence Parr
 * All rights reserved.
 *
 * Grammar conversion to ANTLR v3 and C#:
 * Copyright (c) 2008 Sam Harwell, Pixel Mine, Inc.
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 1. Redistributions of source code must retain the above copyright
 *	notice, this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright
 *	notice, this list of conditions and the following disclaimer in the
 *	documentation and/or other materials provided with the distribution.
 * 3. The name of the author may not be used to endorse or promote products
 *	derived from this software without specific prior written permission.
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

namespace Antlr3.ST.Language
{
    using ANTLRReaderStream = Antlr.Runtime.ANTLRReaderStream;
    using RecognitionException = Antlr.Runtime.RecognitionException;
    using TextReader = System.IO.TextReader;

    partial class TemplateLexer
    {
        protected string currentIndent = null;
        protected StringTemplate self;

        public TemplateLexer( StringTemplate self, TextReader r )
            : this( new ANTLRReaderStream( r ) )
        {
            this.self = self;
        }

        public override void ReportError( RecognitionException e )
        {
            self.error( "$...$ chunk lexer error", e );
        }

        //protected boolean upcomingELSE(int i) throws CharStreamException {
        //    return LA(i)=='$'&&LA(i+1)=='e'&&LA(i+2)=='l'&&LA(i+3)=='s'&&LA(i+4)=='e'&&
        //           LA(i+5)=='$';
        //}

        //protected boolean upcomingENDIF(int i) throws CharStreamException {
        //    return LA(i)=='$'&&LA(i+1)=='e'&&LA(i+2)=='n'&&LA(i+3)=='d'&&LA(i+4)=='i'&&
        //           LA(i+5)=='f'&&LA(i+6)=='$';
        //}

        protected bool upcomingAtEND(int i)
        {
            return input.LA(i)=='$'&&input.LA(i+1)=='@'&&input.LA(i+2)=='e'&&input.LA(i+3)=='n'&&input.LA(i+4)=='d'&&input.LA(i+5)=='$';
        }

        //protected boolean upcomingNewline(int i) throws CharStreamException {
        //    return (LA(i)=='\r'&&LA(i+1)=='\n')||LA(i)=='\n';
        //}
    }
}
