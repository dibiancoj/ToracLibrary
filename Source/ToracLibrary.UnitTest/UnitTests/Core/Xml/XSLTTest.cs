﻿using System;
using System.Linq;
using System.Xml.Linq;
using ToracLibrary.Core.Xml.XSLT;
using Xunit;

namespace ToracLibrary.UnitTest.Core
{

    /// <summary>
    /// Unit test for xml. Schema is tested in country, state unit tests
    /// </summary>
    public class XSLTTest
    {

        /// <summary>
        /// Test the xslt functionality
        /// </summary>
        [Fact]
        public void XSLTTest1()
        {
            //create the xslt file
            var XSLTToTransform = XElement.Parse(Properties.Resources.XmlTransform);

            //let's grab the xml file
            var XmlFileToTransform = XElement.Parse(Properties.Resources.XmlFileForTransformTest);

            //go transform the data
            var TransformResult = XSLTTransformation.TransformToXML(XSLTToTransform, XmlFileToTransform);

            //should be 
            /*
            <Customers>
                    <Customer>First Name 1</Customer>
                    <Customer>First Name 2</Customer>
            </Customers>
            */

            //let's grab the customer elements and cache it
            var CustomerElementsFromResult = TransformResult.Elements("Customer").ToArray();

            //make sure we have 2 customer elements
            Assert.Equal(2, CustomerElementsFromResult.Length);

            //index we are up to
            int Index = 1;

            //loop through the elements
            foreach (var CustomerToTest in CustomerElementsFromResult)
            {
                //check the first name
                Assert.Equal("First Name " + Index, CustomerToTest.Value);

                //increase the count
                Index++;
            }
        }

    }

}