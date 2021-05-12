using Microsoft.VisualStudio.TestTools.UnitTesting;
using KPIMicroservice.Utils.Calculator;
using System;

namespace KPIMicroserviceTest.Utils
{
    [TestClass]
    public class CalculatorContextTest
    {
        [TestMethod]
        [DataRow(CalculationType.Simple)]
        [DataRow(CalculationType.Advanced)]
        public void SetContext_InputCorrectCalculationType_ReturnVoid(CalculationType calculationType)
        {
            var context = new CalculatorContext();
            context.SetCalculator(calculationType);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetContext_InputWrongCalculationType_ThrowException()
        {
            var context = new CalculatorContext();
            context.SetCalculator((CalculationType)3);
        }
    }
}
