namespace HashCode.Test
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Pizza;

    [TestClass]
    public class PizzaTests
    {
        [TestMethod]
        [Timeout(2000)]
        public void PizzaBigInObject()
        {
            int rows = 1000;
            int columns = 1000;
            int lowLimit = 6;
            int highLimit = 14;

            int MCount = 499824;
            int TCount = 500176;

            string expected = $"rows:{rows} columns{columns} low{lowLimit} high{highLimit} TC{TCount} MC{MCount}";

            PizzGrid pizza = PizzGrid.PizzGridFromFile(PizzInputs.PizzInputBig);

            string actual = $"rows:{pizza.Rows} columns{pizza.Columns} low{pizza.MinIngredientPerSlice} high{pizza.MaxCellsPerSlice} TC{pizza.Tomatoes.Count()} MC{pizza.Mushroom.Count()}";

            Assert.AreEqual(expected, actual);
        }
    }
}