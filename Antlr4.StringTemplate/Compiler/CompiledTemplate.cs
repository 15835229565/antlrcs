/*
 * [The "BSD licence"]
 * Copyright (c) 2011 Terence Parr
 * All rights reserved.
 *
 * Conversion to C#:
 * Copyright (c) 2011 Sam Harwell, Tunnel Vision Laboratories, LLC
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

namespace Antlr4.StringTemplate.Compiler
{
    using System.Collections.Generic;
    using Antlr.Runtime.Tree;
    using Antlr.Runtime;
    using Antlr4.StringTemplate.Misc;
    using System.Linq;
    using StringWriter = System.IO.StringWriter;
    using Console = System.Console;
    using ArgumentNullException = System.ArgumentNullException;

    /** The result of compiling an ST.  Contains all the bytecode instructions,
     *  string table, bytecode address to source code map, and other bookkeeping
     *  info.  It's the implementation of an ST you might say.  All instances
     *  of the same template share a single implementation (impl field).
     */
    public class CompiledST
    {
        public string name;

        /** The original, immutable pattern (not really used again after
         *  initial "compilation"). Useful for debugging.  Even for
         *  subtemplates, this is entire overall template.
         */
        public string template;

        /** Overall token stream for template (debug only) */
        public ITokenStream tokens;

        /** How do we interpret syntax of template? (debug only) */
        public CommonTree ast;

        /** Must be non null map if !noFormalArgs */
        public List<FormalArgument> formalArguments;

        public bool hasFormalArgs;

        /** A list of all regions and subtemplates */
        public List<CompiledST> implicitlyDefinedTemplates;

        /** The group that physically defines this ST definition.  We use it to initiate
         *  interpretation via ST.toString().  From there, it becomes field 'group'
         *  in interpreter and is fixed until rendering completes.
         */
        public STGroup nativeGroup = STGroup.defaultGroup;

        /** Does this template come from a <@region>...<@end> embedded in
         *  another template?
         */
        public bool isRegion;

        /** If someone refs <@r()> in template t, an implicit
         *
         *   @t.r() ::= ""
         *
         *  is defined, but you can overwrite this def by defining your
         *  own.  We need to prevent more than one manual def though.  Between
         *  this var and isEmbeddedRegion we can determine these cases.
         */
        public ST.RegionType regionDefType;

        public bool isAnonSubtemplate; // {...}

        public string[] strings;     // string operands of instructions
        public byte[] instrs;        // byte-addressable code memory.
        public int codeSize;
        public Interval[] sourceMap; // maps IP to range in template pattern

        public CompiledST()
        {
            instrs = new byte[Compiler.TEMPLATE_INITIAL_CODE_SIZE];
            sourceMap = new Interval[Compiler.TEMPLATE_INITIAL_CODE_SIZE];
            template = "";
        }

        public virtual FormalArgument TryGetFormalArgument(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (formalArguments == null)
                return null;

            return formalArguments.FirstOrDefault(i => i.name == name);
        }

        public virtual void addImplicitlyDefinedTemplate(CompiledST sub)
        {
            if (implicitlyDefinedTemplates == null)
                implicitlyDefinedTemplates = new List<CompiledST>();

            implicitlyDefinedTemplates.Add(sub);
        }

        public virtual int getNumberOfArgsWithDefaultValues()
        {
            if (formalArguments == null)
                return 0;

            int n = formalArguments.Count(i => i.defaultValueToken != null);
            return n;
        }

        public virtual void defineArgDefaultValueTemplates(STGroup group)
        {
            if (formalArguments == null)
                return;

            foreach (FormalArgument fa in formalArguments)
            {
                if (fa.defaultValueToken != null)
                {
                    string argSTname = fa.name + "_default_value";
                    Compiler c2 = new Compiler(group.errMgr, group.delimiterStartChar, group.delimiterStopChar);
                    string defArgTemplate = Utility.strip(fa.defaultValueToken.Text, 1);
                    fa.compiledDefaultValue = c2.compile(nativeGroup.getFileName(), argSTname, null, defArgTemplate, fa.defaultValueToken);
                    fa.compiledDefaultValue.name = argSTname;
                }
            }
        }

        public virtual void defineFormalArgs(List<FormalArgument> args)
        {
            hasFormalArgs = true; // even if no args; it's formally defined
            if (args == null)
            {
                formalArguments = null;
            }
            else
            {
                foreach (FormalArgument a in args)
                    addArg(a);
            }
        }

        /** Used by ST.add() to add args one by one w/o turning on full formal args definition signal */
        public virtual void addArg(FormalArgument a)
        {
            if (formalArguments == null)
                formalArguments = new List<FormalArgument>();

            a.index = formalArguments.Count;
            formalArguments.Add(a);
        }

        public virtual void defineImplicitlyDefinedTemplates(STGroup group)
        {
            if (implicitlyDefinedTemplates != null)
            {
                foreach (CompiledST sub in implicitlyDefinedTemplates)
                {
                    group.rawDefineTemplate(sub.name, sub, null);
                    sub.defineImplicitlyDefinedTemplates(group);
                }
            }
        }

        public virtual string getTemplateSource()
        {
            Interval r = getTemplateRange();
            return template.Substring(r.A, r.B + 1 - r.A);
        }

        public virtual Interval getTemplateRange()
        {
            if (isAnonSubtemplate)
            {
                Interval start = sourceMap[0];
                Interval stop = null;
                for (int i = sourceMap.Length - 1; i > 0; i--)
                {
                    Interval I = sourceMap[i];
                    if (I != null)
                    {
                        stop = I;
                        break;
                    }
                }

                if (template != null)
                    return new Interval(start.A, stop.B);
            }
            return new Interval(0, template.Length - 1);
        }

        public virtual string Instrs()
        {
            BytecodeDisassembler dis = new BytecodeDisassembler(this);
            return dis.instrs();
        }

        public virtual void dump()
        {
            BytecodeDisassembler dis = new BytecodeDisassembler(this);
            Console.WriteLine(name + ":");
            Console.WriteLine(dis.disassemble());
            Console.WriteLine("Strings:");
            Console.WriteLine(dis.strings());
            Console.WriteLine("Bytecode to template map:");
            Console.WriteLine(dis.sourceMap());
        }

        public virtual string disasm()
        {
            BytecodeDisassembler dis = new BytecodeDisassembler(this);
            using (StringWriter sw = new StringWriter())
            {
                sw.WriteLine(dis.disassemble());
                sw.WriteLine("Strings:");
                sw.WriteLine(dis.strings());
                sw.WriteLine("Bytecode to template map:");
                sw.WriteLine(dis.sourceMap());
                return sw.ToString();
            }
        }
    }
}
