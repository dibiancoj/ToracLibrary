using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ToracLibrary.Core.ExtensionMethods.XElementExtensions;
using Xunit;

namespace ToracLibrary.UnitTest.ExtensionMethods.Core
{

    /// <summary>
    /// Unit test to test XElement Extension Methods
    /// </summary>
    public class XElementExtensionTest
    {

        /// <summary>
        /// Unit test to remove blank elements from XElement. 
        /// </summary>
        [Fact]
        public void RemoveBlankElementsTest1()
        {
            //declare the xml so we can test it later
            const string TestXml = "<root> " +
                                   "<jason id=\"2\">Jason</jason> " +
                                   "</root>";

            //throw the xml into an xelement
            var XElementToTest = XElement.Parse(TestXml);

            //let's remove the blank (which there shouldn't be any of them)
            XElementToTest.RemoveBlankElements();

            //now make sure nothing has changed
            Assert.Equal(XElement.Parse(TestXml).ToString(), XElementToTest.ToString());
        }

        /// <summary>
        /// Unit test to remove blank elements from XElement. 
        /// </summary>
        [Fact]
        public void RemoveBlankElementsTest2()
        {
            //declare the xml so we can test it later
            const string TestXml = "<root> " +
                                   "<jason id=\"2\"></jason> " +
                                   "</root>";

            //throw the xml into an xelement
            var XElementToTest = XElement.Parse(TestXml);

            //let's remove the blank
            XElementToTest.RemoveBlankElements();

            //now make sure the jason node is gone
            Assert.Equal(XElement.Parse("<root />").ToString(), XElementToTest.ToString());
        }

        /// <summary>
        /// Unit test to remove blank elements from XElement. 
        /// </summary>
        [Fact]
        public void RemoveBlankElementsTest3()
        {
            //declare the xml so we can test it later
            const string TestXml = "<root> " +
                                   "<jason id=\"1\">jason1</jason> " +
                                   "<jason id=\"2\"></jason> " +
                                   "<jason id=\"3\">jason3</jason> " +
                                   "</root>";

            //throw the xml into an xelement
            var XElementToTest = XElement.Parse(TestXml);

            //let's remove the blank (which there shouldn't be any of them)
            XElementToTest.RemoveBlankElements();

            //now make sure the jason node is gone
            Assert.Equal(XElement.Parse("<root>" +
                                           "<jason id=\"1\">jason1</jason>" +
                                           "<jason id=\"3\">jason3</jason>" +
                                           "</root>").ToString(), XElementToTest.ToString());
        }

        /// <summary>
        /// Unit test to remove blank elements from XElement. 
        /// </summary>
        [Fact]
        public void RemoveBlankElementsTest4()
        {
            //declare the xml so we can test it later
            const string TestXml = "<root> " +
                                   "<jason id=\"1\"></jason> " +
                                   "<jason id=\"2\"></jason> " +
                                   "<jason id=\"3\"></jason> " +
                                   "<subNode></subNode> " +
                                   "</root>";

            //throw the xml into an xelement
            var XElementToTest = XElement.Parse(TestXml);

            //let's remove the blank (which there shouldn't be any of them)
            XElementToTest.RemoveBlankElements();

            //now make sure the all the nodes are gone
            Assert.Equal(XElement.Parse("<root />").ToString(), XElementToTest.ToString());
        }

        /// <summary>
        /// Unit test to remove blank elements from XElement. 
        /// </summary>
        [Fact]
        public void RemoveBlankElementsTest5()
        {
            //declare the xml so we can test it later
            const string TestXml = "<root> " +
                                   "<jason id=\"1\">jason1</jason> " +
                                   "<jason id=\"2\"></jason> " +
                                   "<jason id=\"3\"></jason> " +
                                   "<subNode>" +
                                   "<SubNodeItem>s1</SubNodeItem>" +
                                   "<SubNodeItem></SubNodeItem>" +
                                   "</subNode> " +
                                   "</root>";

            //throw the xml into an xelement
            var XElementToTest = XElement.Parse(TestXml);

            //let's remove the blank (which there shouldn't be any of them)
            XElementToTest.RemoveBlankElements();

            //now make sure the jason node is gone
            Assert.Equal(XElement.Parse("<root>" +
                                           "<jason id=\"1\">jason1</jason> " +
                                            "<subNode>" +
                                              "<SubNodeItem>s1</SubNodeItem>" +
                                            "</subNode> " +
                                           "</root>").ToString(), XElementToTest.ToString());
        }


    }

}