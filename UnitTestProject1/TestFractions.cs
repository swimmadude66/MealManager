using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestHarness
{
    [TestClass]
    public class TestFractions
    {
        [TestMethod]
        public void TestConvertFractionToDouble()
        {
            String[] fractions = {"1/2", "1/4", "1 1/2", "5/8", "10", "115  1/4", "0/63", "1/0" };
            double[] desired = {0.5, 0.25, 1.5, 0.625, 10, 115.25, 0, -1};
            List<double> results = new List<double>();
            foreach(string frac in fractions)
                results.Add(Inventory.Tools.ToolBox.FractionToDecimal(frac));
            int i = 0;
            foreach (double result in results)
            {
                Assert.AreEqual(desired[i], result);
                i++;
            }
        }

        [TestMethod]
        public void TestConvertDoubleToFraction()
        {
            String[] fractions = { "1/2", "1/4", "1 1/2", "5/8", "10", "115 1/4", "0"};
            double[] inputs = { 0.5, 0.25, 1.5, 0.625, 10, 115.25, 0};
            List<string> results = new List<string>();
            foreach (double input in inputs)
                results.Add(Inventory.Tools.ToolBox.DecimalToFraction(input));
            int i = 0;
            foreach(string result in results)
            {
                Assert.AreEqual(fractions[i], result);
                i++;
            }
        }

    }
}
