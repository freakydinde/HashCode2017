namespace HashCode.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class InputsPizza
    {
        [TestMethod]
        public void PizzaSmallInputReturnedObject()
        {
            int rows = 6;
            int columns = 7;
            int lowLimit = 1;
            int highLimit = 5;

            int TCount = 24;
            int CCount = 18;

            string expected = @"C:\Tfs\Git\HashCode2017\Resources\Pizza";
            string actual = Inputs.ResourcesPizzaFolder;

            Assert.AreEqual(expected, actual);
        }
    }
}