using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace BuildProtobuf.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CS_Pattern()
        {
            Regex regex = new Regex(Program.MsgCSPattern);

            Assert.IsFalse(regex.Match("abc").Groups["cs"].Success);
            Assert.IsTrue(regex.Match("CS_abc").Groups["cs"].Success);
            Assert.IsTrue(regex.Match("CS_CS_abc").Groups["cs"].Success);
            Assert.IsFalse(regex.Match("CS1_CS_abc").Groups["cs"].Success);
            Assert.IsTrue(regex.Match("ns.CS_abc").Groups["cs"].Success);
            Assert.IsTrue(regex.Match("ns.ns2.CS_abc").Groups["cs"].Success);

        }

        [TestMethod]
        public void SC_Pattern()
        {
            Regex regex = new Regex(Program.MsgSCPattern);
            Assert.IsFalse(regex.Match("abc").Groups["sc"].Success);
            Assert.IsTrue(regex.Match("SC_abc").Groups["sc"].Success);
            Assert.IsTrue(regex.Match("SC_SC_abc").Groups["sc"].Success);
            Assert.IsFalse(regex.Match("SC1_SC_abc").Groups["sc"].Success);
            Assert.IsTrue(regex.Match("ns.SC_abc").Groups["sc"].Success);
            Assert.IsTrue(regex.Match("ns.ns2.SC_abc").Groups["sc"].Success);
        }

        [TestMethod]
        public void CS_Name_Pattern()
        {
            Regex regex = new Regex(Program.MsgCSPattern);

            Assert.AreEqual("abc", regex.Match("CS_abc").Groups["name"].Value);
            Assert.AreEqual("abc", regex.Match("ns.CS_abc").Groups["name"].Value);
            Assert.AreEqual("CS_abc", regex.Match("CS_CS_abc").Groups["name"].Value);
        }

        [TestMethod]
        public void SC_Name_Pattern()
        {
            Regex regex = new Regex(Program.MsgSCPattern);

            Assert.AreEqual("abc", regex.Match("ns.SC_abc").Groups["name"].Value);
            Assert.AreEqual("abc", regex.Match("ns.ns2.SC_abc").Groups["name"].Value);
            Assert.AreEqual("abc", regex.Match("SC_abc").Groups["name"].Value);
        }

        [TestMethod]
        public void Name_No_Namespace_Pattern()
        {
            Regex regex = new Regex("([^\\.]+\\.)?(?<name>.*)$");

            Assert.AreEqual("abc", regex.Match("abc").Groups["name"].Value);
            Assert.AreEqual("SC_abc", regex.Match("SC_abc").Groups["name"].Value);
            Assert.AreEqual("SC_abc", regex.Match("ns.SC_abc").Groups["name"].Value);
        }

        [TestMethod]
        public void Request_Pattern()
        {
            Regex regex = new Regex(Program.MsgCSPattern);
            Assert.AreEqual("abc", regex.Match("ns.abcRequest").Groups["name"].Value);
            Assert.IsTrue(regex.Match("ns.abcRequest").Groups["cs"].Success);
            Assert.IsFalse(regex.Match("ns.abcRequest").Groups["sc"].Success);
        }
        [TestMethod]
        public void Response_Pattern()
        {
            Regex regex = new Regex(Program.MsgSCPattern);

            Assert.AreEqual("abc", regex.Match("ns.abcResponse").Groups["name"].Value);
            Assert.IsFalse(regex.Match("ns.abcResponse").Groups["cs"].Success);
            Assert.IsTrue(regex.Match("ns.abcResponse").Groups["sc"].Success);
        }
    }
}
