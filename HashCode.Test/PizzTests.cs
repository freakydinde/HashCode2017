namespace HashCode.Test
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Pizza;
    using Client;

    [TestClass]
    public class PizzTests
    {
        private static PizzGrid pizza;

        /// <summary>Inialize a new static instance of delivery</summary>
        /// <param name="context">test context</param>
        [ClassInitialize]
        public static void InitializeDelivery(TestContext context)
        {
            PizzTests.pizza = PizzGrid.PizzGridFromFile(PizzInputs.PizzInputBig);
        }

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

            string actual = $"rows:{PizzTests.pizza.Rows} columns{PizzTests.pizza.Columns} low{PizzTests.pizza.MinIngredientPerSlice} high{PizzTests.pizza.MaxCellsPerSlice} TC{PizzTests.pizza.Tomatoes.Count()} MC{PizzTests.pizza.Mushroom.Count()}";

            Assert.AreEqual(expected, actual);
        }
    }
}