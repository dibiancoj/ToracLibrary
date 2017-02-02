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

        #region Query With A Namespace

        #region Static Properties

        private static readonly XNamespace NamespaceToUse = "http://example.com/name";
        private static readonly XNamespace SchemaNamespaceToUse = "http://www.w3.org/2001/XMLSchema-instance";
        private const string IdAttributeName = "Id";

        #endregion

        [Fact]
        public void QueryElementWithNamespaceTest1()
        {
            //row to test 
            var RowToTest = BuildXElementWithNamespace(1);

            //make sure this returns null because we are querying without a namespace
            Assert.True(RowToTest.Element("Example") == null);

            //now query our extension method which uses namespace
            var Result = RowToTest.Element(NamespaceToUse, "Example");

            //make sure its not null
            Assert.False(Result == null);

            //make sure the id is 0
            Assert.Equal("0", Result.Attribute(IdAttributeName).Value);
        }

        [Fact]
        public void QueryElementsWithNamespaceTest1()
        {
            //how many to build
            const int HowManyToBuild = 5;

            //row to test 
            var RowToTest = BuildXElementWithNamespace(HowManyToBuild);

            //make sure this returns null because we are querying without a namespace
            Assert.True(RowToTest.Element("Example") == null);

            //now query our extension method which uses namespace
            var Result = RowToTest.Elements(NamespaceToUse, "Example").ToArray();

            //make sure its not null
            Assert.False(Result == null);

            //loop through the records
            for (int i = 0; i < HowManyToBuild; i++)
            {
                //make sure the id the corresponding i
                Assert.Equal(i.ToString(), Result[i].Attribute(IdAttributeName).Value);
            }
        }

        #region Framework

        /// <summary>
        /// Build a set of xelements with a namespace
        /// </summary>
        /// <param name="HowManyRecordsToBuild">How many records to build</param>
        /// <returns>Root Element with child elements in it</returns>
        private static XElement BuildXElementWithNamespace(int HowManyRecordsToBuild)
        {
            //<name:Example xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:name="http://example.com/name" Id="#"></name:Example>

            //root element
            var RootElement = new XElement("root");

            //loop through the rows
            for (int i = 0; i < HowManyRecordsToBuild; i++)
            {
                RootElement.Add(new XElement(NamespaceToUse + "Example",
                            new XAttribute(IdAttributeName, i),
                            new XAttribute(XNamespace.Xmlns + "name", NamespaceToUse),
                            new XAttribute(XNamespace.Xmlns + "xsi", SchemaNamespaceToUse)));
            }

            //return the root
            return RootElement;
        }

        #endregion

        #endregion

        #region Remove Blank Elements

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

        #endregion

    }

}