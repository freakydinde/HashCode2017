namespace HashCode.Test
{
    using System.Text;
    using HashCode.Pizza;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SliceTest
    {
        private static PizzGrid pizza;

        /// <summary>Inialize a new static instance of delivery</summary>
        /// <param name="context">test context</param>
        [ClassInitialize]
        public static void InitializeDelivery(TestContext context)
        {
            SliceTest.pizza = PizzGrid.PizzGridFromFile(PizzInputs.PizzInputSmall);
        }

        [TestMethod]
        public void SliceIsRectangle()
        {
            PizzSlice slice = new PizzSlice(new Cell(0, 0), SliceTest.pizza);

            Write.Trace(slice.ToString());

            slice.AddCell(new PizzCell(0, 1));
            slice.AddCell(new PizzCell(0, 2));

            Write.Trace(slice.ToString());

            StringBuilder actual = new StringBuilder();

            actual.Append(slice.IsRectancle.ToString());

            Write.Trace(slice.ToString());

            slice.AddCell(new PizzCell(1, 1));

            actual.Append(slice.IsRectancle.ToString());

            Write.Trace(slice.ToString());

            slice.AddCell(new PizzCell(1, 2));

            actual.Append(slice.IsRectancle.ToString());

            string expected = "TrueFalseTrue";

            Assert.AreEqual(expected, actual.ToString());
        }
    }
}