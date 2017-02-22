namespace HashCode.Pizza
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class PizzRound
    {
        public List<PizzSlice> PizzaSlices { get; set; }

        public void WriteResult()
        {
            int slicesCount = PizzaSlices.Count();

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(PizzaSlices.Count().ToString());

            foreach(PizzSlice slice in PizzaSlices)
            {
                sb.AppendLine(slice.ToString());
            }

            File.WriteAllText(PizzInputs.PizzResult, sb.ToString());
        }
    }
}
