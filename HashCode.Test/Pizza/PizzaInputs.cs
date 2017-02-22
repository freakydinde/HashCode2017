namespace HashCode.Test
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PizzaInputs
    {
        [TestMethod]
        [Timeout(120000)]
        public void PizzaBigInRObject()
        {
            int rows = 1000;
            int columns = 1000;
            int lowLimit = 6;
            int highLimit = 14;

            int TCount = 499824;
            int CCount = 500176;

            string expected = $"rows:{rows} columns{columns} low{lowLimit} high{highLimit} TC{TCount} CC{CCount}";

            Pizza pizza = Pizza.PizzaFromFile(Inputs.ResourcesPizzaInputBig);

            string actual = $"rows:{pizza.Rows} columns{pizza.Columns} low{pizza.MinIngredientPerSlice} high{pizza.MaxCellsPerSlice} TC{pizza.Tomatoes.Count()} CC{pizza.Mushroom.Count()}";

            Assert.AreEqual(expected, actual);
        }
    }
}