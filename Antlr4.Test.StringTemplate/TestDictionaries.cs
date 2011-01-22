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

namespace Antlr4.Test.StringTemplate
{
    using Antlr4.StringTemplate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;
    using Antlr4.StringTemplate.Misc;
    using Antlr4.Test.StringTemplate.Extensions;

    [TestClass]
    public class TestDictionaries : BaseTest
    {
        [TestMethod]
        public void TestDict()
        {
            string templates =
                    "typeInit ::= [\"int\":\"0\", \"float\":\"0.0\"] " + newline +
                    "var(type,name) ::= \"<type> <name> = <typeInit.(type)>;\"" + newline
                    ;
            writeFile(tmpdir, "test.stg", templates);
            STGroup group = new STGroupFile(tmpdir + "/" + "test.stg");
            ST st = group.getInstanceOf("var");
            st.add("type", "int");
            st.add("name", "x");
            string expecting = "int x = 0;";
            string result = st.render();
            Assert.AreEqual(expecting, result);
        }

        [TestMethod]
        public void TestDictValuesAreTemplates()
        {
            string templates =
                    "typeInit ::= [\"int\":{0<w>}, \"float\":{0.0<w>}] " + newline +
                    "var(type,w,name) ::= \"<type> <name> = <typeInit.(type)>;\"" + newline
                    ;
            writeFile(tmpdir, "test.stg", templates);
            STGroup group = new STGroupFile(tmpdir + "/" + "test.stg");
            ST st = group.getInstanceOf("var");
            st.impl.dump();
            st.add("w", "L");
            st.add("type", "int");
            st.add("name", "x");
            string expecting = "int x = 0L;";
            string result = st.render();
            Assert.AreEqual(expecting, result);
        }

        [TestMethod]
        public void TestDictKeyLookupViaTemplate()
        {
            // Make sure we try rendering stuff to string if not found as regular object
            string templates =
                    "typeInit ::= [\"int\":{0<w>}, \"float\":{0.0<w>}] " + newline +
                    "var(type,w,name) ::= \"<type> <name> = <typeInit.(type)>;\"" + newline
                    ;
            writeFile(tmpdir, "test.stg", templates);
            STGroup group = new STGroupFile(tmpdir + "/" + "test.stg");
            ST st = group.getInstanceOf("var");
            st.add("w", "L");
            st.add("type", new ST("int"));
            st.add("name", "x");
            string expecting = "int x = 0L;";
            string result = st.render();
            Assert.AreEqual(expecting, result);
        }

        [TestMethod]
        public void TestDictKeyLookupAsNonToStringableObject()
        {
            // Make sure we try rendering stuff to string if not found as regular object
            string templates =
                    "foo(m,k) ::= \"<m.(k)>\"" + newline
                    ;
            writeFile(tmpdir, "test.stg", templates);
            STGroup group = new STGroupFile(tmpdir + "/" + "test.stg");
            ST st = group.getInstanceOf("foo");
            IDictionary<HashableUser, string> m = new Dictionary<HashableUser, string>();
            m[new HashableUser(99, "parrt")] = "first";
            m[new HashableUser(172036, "tombu")] = "second";
            m[new HashableUser(391, "sriram")] = "third";
            st.add("m", m);
            st.add("k", new HashableUser(172036, "tombu"));
            string expecting = "second";
            string result = st.render();
            Assert.AreEqual(expecting, result);
        }

        [TestMethod]
        public void TestDictMissingDefaultValueIsEmpty()
        {
            string templates =
                    "typeInit ::= [\"int\":\"0\", \"float\":\"0.0\"] " + newline +
                    "var(type,w,name) ::= \"<type> <name> = <typeInit.(type)>;\"" + newline
                    ;
            writeFile(tmpdir, "test.stg", templates);
            STGroup group = new STGroupFile(tmpdir + "/" + "test.stg");
            ST st = group.getInstanceOf("var");
            st.add("w", "L");
            st.add("type", "double"); // double not in typeInit map
            st.add("name", "x");
            string expecting = "double x = ;";
            string result = st.render();
            Assert.AreEqual(expecting, result);
        }

        [TestMethod]
        public void TestDictMissingDefaultValueIsEmptyForNullKey()
        {
            string templates =
                    "typeInit ::= [\"int\":\"0\", \"float\":\"0.0\"] " + newline +
                    "var(type,w,name) ::= \"<type> <name> = <typeInit.(type)>;\"" + newline
                    ;
            writeFile(tmpdir, "test.stg", templates);
            STGroup group = new STGroupFile(tmpdir + "/" + "test.stg");
            ST st = group.getInstanceOf("var");
            st.add("w", "L");
            st.add("type", null); // double not in typeInit map
            st.add("name", "x");
            string expecting = " x = ;";
            string result = st.render();
            Assert.AreEqual(expecting, result);
        }

        [TestMethod]
        public void TestDictHiddenByFormalArg()
        {
            string templates =
                    "typeInit ::= [\"int\":\"0\", \"float\":\"0.0\"] " + newline +
                    "var(typeInit,type,name) ::= \"<type> <name> = <typeInit.(type)>;\"" + newline
                    ;
            writeFile(tmpdir, "test.stg", templates);
            STGroup group = new STGroupFile(tmpdir + "/" + "test.stg");
            ST st = group.getInstanceOf("var");
            st.add("type", "int");
            st.add("name", "x");
            string expecting = "int x = ;";
            string result = st.render();
            Assert.AreEqual(expecting, result);
        }

        [TestMethod]
        public void TestDictEmptyValueAndAngleBracketStrings()
        {
            string templates =
                    "typeInit ::= [\"int\":\"0\", \"float\":, \"double\":<<0.0L>>] " + newline +
                    "var(type,name) ::= \"<type> <name> = <typeInit.(type)>;\"" + newline
                    ;
            writeFile(tmpdir, "test.stg", templates);
            STGroup group = new STGroupFile(tmpdir + "/" + "test.stg");
            ST st = group.getInstanceOf("var");
            st.add("type", "float");
            st.add("name", "x");
            string expecting = "float x = ;";
            string result = st.render();
            Assert.AreEqual(expecting, result);
        }

        [TestMethod]
        public void TestDictDefaultValue()
        {
            string templates =
                    "typeInit ::= [\"int\":\"0\", default:\"null\"] " + newline +
                    "var(type,name) ::= \"<type> <name> = <typeInit.(type)>;\"" + newline
                    ;
            writeFile(tmpdir, "test.stg", templates);
            STGroup group = new STGroupFile(tmpdir + "/" + "test.stg");
            ST st = group.getInstanceOf("var");
            st.add("type", "UserRecord");
            st.add("name", "x");
            string expecting = "UserRecord x = null;";
            string result = st.render();
            Assert.AreEqual(expecting, result);
        }

        [TestMethod]
        public void TestDictNullKeyGetsDefaultValue()
        {
            string templates =
                    "typeInit ::= [\"int\":\"0\", default:\"null\"] " + newline +
                    "var(type,name) ::= \"<type> <name> = <typeInit.(type)>;\"" + newline
                    ;
            writeFile(tmpdir, "test.stg", templates);
            STGroup group = new STGroupFile(tmpdir + "/" + "test.stg");
            ST st = group.getInstanceOf("var");
            // missing or set to null: st.add("type", null);
            st.add("name", "x");
            string expecting = " x = null;";
            string result = st.render();
            Assert.AreEqual(expecting, result);
        }

        [TestMethod]
        public void TestDictEmptyDefaultValue()
        {
            string templates =
                    "typeInit ::= [\"int\":\"0\", default:] " + newline +
                    "var(type,name) ::= \"<type> <name> = <typeInit.(type)>;\"" + newline
                    ;
            writeFile(tmpdir, "test.stg", templates);
            ErrorBuffer errors = new ErrorBuffer();
            STGroupFile group = new STGroupFile(tmpdir + "/" + "test.stg");
            group.setListener(errors);
            group.load();
            string expected = "[test.stg 1:33: missing value for key at ']']";
            string result = errors.Errors.ToListString();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestDictDefaultValueIsKey()
        {
            string templates =
                    "typeInit ::= [\"int\":\"0\", default:key] " + newline +
                    "var(type,name) ::= \"<type> <name> = <typeInit.(type)>;\"" + newline
                    ;
            writeFile(tmpdir, "test.stg", templates);
            STGroup group = new STGroupFile(tmpdir + "/" + "test.stg");
            ST st = group.getInstanceOf("var");
            st.add("type", "UserRecord");
            st.add("name", "x");
            string expecting = "UserRecord x = UserRecord;";
            string result = st.render();
            Assert.AreEqual(expecting, result);
        }

        /**
         * Test that a map can have only the default entry.
         */
        [TestMethod]
        public void TestDictDefaultStringAsKey()
        {
            string templates =
                    "typeInit ::= [\"default\":\"foo\"] " + newline +
                    "var(type,name) ::= \"<type> <name> = <typeInit.(type)>;\"" + newline
                    ;
            writeFile(tmpdir, "test.stg", templates);
            STGroup group = new STGroupFile(tmpdir + "/" + "test.stg");
            ST st = group.getInstanceOf("var");
            st.add("type", "default");
            st.add("name", "x");
            string expecting = "default x = foo;";
            string result = st.render();
            Assert.AreEqual(expecting, result);
        }

        /**
         * Test that a map can return a <b>string</b> with the word: default.
         */
        [TestMethod]
        public void TestDictDefaultIsDefaultString()
        {
            string templates =
                    "map ::= [default: \"default\"] " + newline +
                    "t() ::= << <map.(\"1\")> >>" + newline
                    ;
            writeFile(tmpdir, "test.stg", templates);
            STGroup group = new STGroupFile(tmpdir + "/" + "test.stg");
            ST st = group.getInstanceOf("t");
            string expecting = " default ";
            string result = st.render();
            Assert.AreEqual(expecting, result);
        }

        [TestMethod]
        public void TestDictViaEnclosingTemplates()
        {
            string templates =
                    "typeInit ::= [\"int\":\"0\", \"float\":\"0.0\"] " + newline +
                    "intermediate(type,name) ::= \"<var(type,name)>\"" + newline +
                    "var(type,name) ::= \"<type> <name> = <typeInit.(type)>;\"" + newline
                    ;
            writeFile(tmpdir, "test.stg", templates);
            STGroup group = new STGroupFile(tmpdir + "/" + "test.stg");
            ST st = group.getInstanceOf("intermediate");
            st.add("type", "int");
            st.add("name", "x");
            string expecting = "int x = 0;";
            string result = st.render();
            Assert.AreEqual(expecting, result);
        }

        [TestMethod]
        public void TestDictViaEnclosingTemplates2()
        {
            string templates =
                    "typeInit ::= [\"int\":\"0\", \"float\":\"0.0\"] " + newline +
                    "intermediate(stuff) ::= \"<stuff>\"" + newline +
                    "var(type,name) ::= \"<type> <name> = <typeInit.(type)>;\"" + newline
                    ;
            writeFile(tmpdir, "test.stg", templates);
            STGroup group = new STGroupFile(tmpdir + "/" + "test.stg");
            ST interm = group.getInstanceOf("intermediate");
            ST var = group.getInstanceOf("var");
            var.add("type", "int");
            var.add("name", "x");
            interm.add("stuff", var);
            string expecting = "int x = 0;";
            string result = interm.render();
            Assert.AreEqual(expecting, result);
        }
    }
}
