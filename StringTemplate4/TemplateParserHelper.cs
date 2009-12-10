﻿/*
 * [The "BSD licence"]
 * Copyright (c) 2005-2008 Terence Parr
 * All rights reserved.
 *
 * Conversion to C#:
 * Copyright (c) 2008-2009 Sam Harwell
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

namespace StringTemplate
{
    using System.Collections.Generic;
    using Antlr.Runtime;
    using Console = System.Console;
    using IList = System.Collections.IList;

    partial class TemplateParser
    {
        List<string> IFindents = new List<string>();

        ICodeGenerator gen = new CodeGenerator();

        public TemplateParser(ITokenStream input, ICodeGenerator gen)
            : this(input, new RecognizerSharedState(), gen)
        {
        }

        public TemplateParser(ITokenStream input, RecognizerSharedState state, ICodeGenerator gen)
            : base(null, null) // overcome bug in ANTLR 3.2
        {
            this.input = input;
            this.state = state;
            if (gen != null)
                this.gen = gen;
        }
        protected override object RecoverFromMismatchedToken(IIntStream input, int ttype, BitSet follow)
        {
            throw new MismatchedTokenException(ttype, input);
        }

        public static string Strip(string s, int n)
        {
            return s.Substring(n, s.Length - 2 * n);
        }

        public void RefAttr(IToken id)
        {
            string name = id.Text;
            if (Interpreter.predefinedAttributes.Contains(name))
            {
                gen.Emit(Bytecode.INSTR_LOAD_LOCAL, name);
            }
            else
            {
                gen.Emit(Bytecode.INSTR_LOAD_ATTR, name);
            }
        }

        public void SetOption(IToken id)
        {
            int i;
            if (!Compiler.supportedOptions.TryGetValue(id.Text, out i))
            {
                Console.Error.WriteLine("no such option: " + id.Text);
                return;
            }

            gen.Emit(Bytecode.INSTR_STORE_OPTION, i);
        }

        public void DefaultOption(IToken id)
        {
            string v;
            if (!Compiler.defaultOptionValues.TryGetValue(id.Text, out v))
            {
                Console.Error.WriteLine("no def value for " + id.Text);
                return;
            }

            gen.Emit(Bytecode.INSTR_LOAD_STR, v);
        }

        public void Func(IToken id)
        {
            short funcBytecode;
            if (!Compiler.funcs.TryGetValue(id.Text, out funcBytecode))
            {
                Console.Error.WriteLine("no such fun: " + id);
                gen.Emit(Bytecode.INSTR_NOOP);
                return;
            }

            gen.Emit(funcBytecode);
        }

        public void PushIFIndentation(string indent)
        {
            IFindents.Add(indent);
        }

        public string PopIFIndentation()
        {
            var result = IFindents[IFindents.Count - 1];
            IFindents.RemoveAt(IFindents.Count - 1);
            return result;
        }

        public void Indent(string indent)
        {
            /*
                    if ( IFindents.size()>0 ) {
                        String ifIndent = IFindents.get(IFindents.size()-1);
                        if ( indent.startsWith(ifIndent) ) indent = indent.substring(ifIndent.length());
                    }
                    */
            gen.Emit(Bytecode.INSTR_INDENT, indent);
        }

        private sealed class CodeGenerator : ICodeGenerator
        {
            public void Emit(short opcode)
            {
            }

            public void Emit(short opcode, int arg)
            {
            }

            public void Emit(short opcode, string s)
            {
            }

            public void Write(int addr, short value)
            {
            }

            public int Address()
            {
                return 0;
            }

            public string CompileAnonTemplate(ITokenStream input, IList ids, RecognizerSharedState state)
            {
                Compiler c = new Compiler();
                c.Compile(input, state);
                return null;
            }
        }
    }
}
