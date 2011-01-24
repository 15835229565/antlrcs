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

    [TestClass]
    public class TestFunctions : BaseTest
    {
        [TestMethod]
        public void TestFirst()
        {
            string template = "<first(names)>";
            Template st = new Template(template);
            List<string> names = new List<string>() { "Ter", "Tom" };
            st.add("names", names);
            string expected = "Ter";
            string result = st.render();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestLength()
        {
            string template = "<length(names)>";
            Template st = new Template(template);
            List<string> names = new List<string>() { "Ter", "Tom" };
            st.add("names", names);
            string expected = "2";
            string result = st.render();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestLengthWithNullValues()
        {
            string template = "<length(names)>";
            Template st = new Template(template);
            List<string> names = new List<string>() { "Ter", null, "Tom", null };
            st.add("names", names);
            string expected = "4";
            string result = st.render();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestFirstOp()
        {
            Template e = new Template(
                    "<first(names)>"
                );
            e.add("names", "Ter");
            e.add("names", "Tom");
            e.add("names", "Sriram");
            string expecting = "Ter";
            Assert.AreEqual(expecting, e.render());
        }

        [TestMethod]
        public void TestTruncOp()
        {
            Template e = new Template(
                    "<trunc(names); separator=\", \">"
                );
            e.add("names", "Ter");
            e.add("names", "Tom");
            e.add("names", "Sriram");
            string expecting = "Ter, Tom";
            Assert.AreEqual(expecting, e.render());
        }

        [TestMethod]
        public void TestRestOp()
        {
            Template e = new Template(
                    "<rest(names); separator=\", \">"
                );
            e.add("names", "Ter");
            e.add("names", "Tom");
            e.add("names", "Sriram");
            string expecting = "Tom, Sriram";
            Assert.AreEqual(expecting, e.render());
        }

        [TestMethod]
        public void TestRestOpEmptyList()
        {
            Template e = new Template(
                    "<rest(names); separator=\", \">"
                );
            e.add("names", new List<string>());
            string expecting = "";
            Assert.AreEqual(expecting, e.render());
        }

        [TestMethod]
        public void TestReUseOfRestResult()
        {
            string templates =
                "a(names) ::= \"<b(rest(names))>\"" + newline +
                "b(x) ::= \"<x>, <x>\"" + newline
                ;
            writeFile(tmpdir, "t.stg", templates);
            TemplateGroup group = new TemplateGroupFile(tmpdir + "/" + "t.stg");
            Template e = group.getInstanceOf("a");
            List<string> names = new List<string>();
            names.Add("Ter");
            names.Add("Tom");
            e.add("names", names);
            string expecting = "Tom, Tom";
            Assert.AreEqual(expecting, e.render());
        }

        [TestMethod]
        public void TestLastOp()
        {
            Template e = new Template(
                    "<last(names)>"
                );
            e.add("names", "Ter");
            e.add("names", "Tom");
            e.add("names", "Sriram");
            string expecting = "Sriram";
            Assert.AreEqual(expecting, e.render());
        }

        [TestMethod]
        public void TestStripOp()
        {
            Template e = new Template(
                    "<strip(names); null=\"n/a\">"
                );
            e.add("names", null);
            e.add("names", "Tom");
            e.add("names", null);
            e.add("names", null);
            e.add("names", "Sriram");
            e.add("names", null);
            string expecting = "TomSriram";
            Assert.AreEqual(expecting, e.render());
        }

        [TestMethod]
        public void TestLengthStrip()
        {
            Template e = new Template(
                    "<length(strip(names))>"
                );
            e.add("names", null);
            e.add("names", "Tom");
            e.add("names", null);
            e.add("names", null);
            e.add("names", "Sriram");
            e.add("names", null);
            string expecting = "2";
            Assert.AreEqual(expecting, e.render());
        }

        [TestMethod]
        public void TestCombinedOp()
        {
            // replace first of yours with first of mine
            Template e = new Template(
                    "<[first(mine),rest(yours)]; separator=\", \">"
                );
            e.add("mine", "1");
            e.add("mine", "2");
            e.add("mine", "3");
            e.add("yours", "a");
            e.add("yours", "b");
            string expecting = "1, b";
            Assert.AreEqual(expecting, e.render());
        }

        [TestMethod]
        public void TestCatListAndSingleAttribute()
        {
            // replace first of yours with first of mine
            Template e = new Template(
                    "<[mine,yours]; separator=\", \">"
                );
            e.add("mine", "1");
            e.add("mine", "2");
            e.add("mine", "3");
            e.add("yours", "a");
            string expecting = "1, 2, 3, a";
            Assert.AreEqual(expecting, e.render());
        }

        [TestMethod]
        public void TestReUseOfCat()
        {
            string templates =
                "a(mine,yours) ::= \"<b([mine,yours])>\"" + newline +
                "b(x) ::= \"<x>, <x>\"" + newline
                ;
            writeFile(tmpdir, "t.stg", templates);
            TemplateGroup group = new TemplateGroupFile(tmpdir + "/" + "t.stg");
            Template e = group.getInstanceOf("a");
            List<string> mine = new List<string>();
            mine.Add("Ter");
            mine.Add("Tom");
            e.add("mine", mine);
            List<string> yours = new List<string>();
            yours.Add("Foo");
            e.add("yours", yours);
            string expecting = "TerTomFoo, TerTomFoo";
            Assert.AreEqual(expecting, e.render());
        }

        [TestMethod]
        public void TestCatListAndEmptyAttributes()
        {
            // + is overloaded to be cat strings and cat lists so the
            // two operands (from left to right) determine which way it
            // goes.  In this case, x+mine is a list so everything from their
            // to the right becomes list cat.
            Template e = new Template(
                    "<[x,mine,y,yours,z]; separator=\", \">"
                );
            e.add("mine", "1");
            e.add("mine", "2");
            e.add("mine", "3");
            e.add("yours", "a");
            string expecting = "1, 2, 3, a";
            Assert.AreEqual(expecting, e.render());
        }

        [TestMethod]
        public void TestNestedOp()
        {
            Template e = new Template(
                    "<first(rest(names))>" // gets 2nd element
                );
            e.add("names", "Ter");
            e.add("names", "Tom");
            e.add("names", "Sriram");
            string expecting = "Tom";
            Assert.AreEqual(expecting, e.render());
        }

        [TestMethod]
        public void TestFirstWithOneAttributeOp()
        {
            Template e = new Template(
                    "<first(names)>"
                );
            e.add("names", "Ter");
            string expecting = "Ter";
            Assert.AreEqual(expecting, e.render());
        }

        [TestMethod]
        public void TestLastWithOneAttributeOp()
        {
            Template e = new Template(
                    "<last(names)>"
                );
            e.add("names", "Ter");
            string expecting = "Ter";
            Assert.AreEqual(expecting, e.render());
        }

        [TestMethod]
        public void TestLastWithLengthOneListAttributeOp()
        {
            Template e = new Template(
                    "<last(names)>"
                );
            e.add("names", new List<string>() { "Ter" });
            string expecting = "Ter";
            Assert.AreEqual(expecting, e.render());
        }

        [TestMethod]
        public void TestRestWithOneAttributeOp()
        {
            Template e = new Template(
                    "<rest(names)>"
                );
            e.add("names", "Ter");
            string expecting = "";
            Assert.AreEqual(expecting, e.render());
        }

        [TestMethod]
        public void TestRestWithLengthOneListAttributeOp()
        {
            Template e = new Template(
                    "<rest(names)>"
                );
            e.add("names", new List<string>() { "Ter" });
            string expecting = "";
            Assert.AreEqual(expecting, e.render());
        }

        [TestMethod]
        public void TestRepeatedRestOp()
        {
            Template e = new Template(
                    "<rest(names)>, <rest(names)>" // gets 2nd element
                );
            e.add("names", "Ter");
            e.add("names", "Tom");
            string expecting = "Tom, Tom";
            Assert.AreEqual(expecting, e.render());
        }

        [TestMethod]
        public void TestIncomingLists()
        {
            Template e = new Template(
                    "<rest(names)>, <rest(names)>" // gets 2nd element
                );
            e.add("names", "Ter");
            e.add("names", "Tom");
            string expecting = "Tom, Tom";
            Assert.AreEqual(expecting, e.render());
        }

        [TestMethod]
        public void TestFirstWithCatAttribute()
        {
            Template e = new Template(
                    "<first([names,phones])>"
                );
            e.add("names", "Ter");
            e.add("names", "Tom");
            e.add("phones", "1");
            e.add("phones", "2");
            string expecting = "Ter";
            Assert.AreEqual(expecting, e.render());
        }

        [TestMethod]
        public void TestFirstWithListOfMaps()
        {
            Template e = new Template(
                    "<first(maps).Ter>"
                );
            IDictionary<string, string> m1 = new Dictionary<string, string>();
            IDictionary<string, string> m2 = new Dictionary<string, string>();
            m1["Ter"] = "x5707";
            e.add("maps", m1);
            m2["Tom"] = "x5332";
            e.add("maps", m2);
            string expecting = "x5707";
            Assert.AreEqual(expecting, e.render());

            List<IDictionary<string, string>> list = new List<IDictionary<string, string>>() { m1, m2 };
            e.add("maps", list);
            expecting = "x5707";
            Assert.AreEqual(expecting, e.render());
        }

        [TestMethod]
        public void TestFirstWithListOfMaps2()
        {
            Template e = new Template(
                    "<first(maps):{ m | <m>!}>"
                );
            IDictionary<string, string> m1 = new Dictionary<string, string>();
            IDictionary<string, string> m2 = new Dictionary<string, string>();
            m1["Ter"] = "x5707";
            e.add("maps", m1);
            m2["Tom"] = "x5332";
            e.add("maps", m2);
            string expecting = "Ter!";
            Assert.AreEqual(expecting, e.render());
            List<IDictionary<string, string>> list = new List<IDictionary<string, string>>() { m1, m2 };
            e.add("maps", list);
            expecting = "Ter!";
            Assert.AreEqual(expecting, e.render());
        }

        [TestMethod]
        public void TestTrim()
        {
            Template e = new Template(
                    "<trim(name)>"
                );
            e.add("name", " Ter  \n");
            string expecting = "Ter";
            Assert.AreEqual(expecting, e.render());
        }

        [TestMethod]
        public void TestStrlen()
        {
            Template e = new Template(
                    "<strlen(name)>"
                );
            e.add("name", "012345");
            string expecting = "6";
            Assert.AreEqual(expecting, e.render());
        }

        [TestMethod]
        public void TestReverse()
        {
            Template e = new Template(
                    "<reverse(names); separator=\", \">"
                );
            e.add("names", "Ter");
            e.add("names", "Tom");
            e.add("names", "Sriram");
            string expecting = "Sriram, Tom, Ter";
            Assert.AreEqual(expecting, e.render());
        }
    }
}
