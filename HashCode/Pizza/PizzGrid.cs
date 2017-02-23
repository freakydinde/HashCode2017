namespace HashCode.Pizza
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    public class PizzGrid : Grid
    {
        public PizzGrid(int rows, int columns) : base(rows, columns, false)
        { }

        public IEnumerable<PizzCell> FreeCells
        {
            get
            {
                return from i in this.PizzCells where i.IsSliced == false select i;
            }
        }

        public int MaxCellsPerSlice { get; set; }

        public int MinIngredientPerSlice { get; set; }

        public IEnumerable<ICell> Mushroom
        {
            get
            {
                return from i in this.PizzCells where i.Type == PizzCellType.Mushroom select i;
            }
        }

        public IEnumerable<PizzCell> PizzCells
        {
            get
            {
                return from i in this.Cells select i as PizzCell;
            }
        }

        public IEnumerable<ICell> Tomatoes
        {
            get
            {
                return from i in this.PizzCells where i.Type == PizzCellType.Tomato select i;
            }
        }

        public static PizzGrid PizzGridFromFile(string filePath)
        {
            List<string> lines = File.ReadAllLines(filePath).ToList();

            int[] firstLine = (from i in lines.Take(1) select (from j in i.Split(' ') select Convert.ToInt32(j, CultureInfo.InvariantCulture)).ToArray()).FirstOrDefault();

            PizzGrid pizza = new PizzGrid(firstLine[0], firstLine[1]);
            pizza.MinIngredientPerSlice = firstLine[2];
            pizza.MaxCellsPerSlice = firstLine[3];

            int index = 0;

            foreach (string line in lines.Skip(1))
            {
                for (int j = 0; j < pizza.Rows; j++)
                {
                    pizza.Cells.Add(new PizzCell(index, j, line[j]));
                }

                index++;
            }

            return pizza;
        }

        public bool IsSliced(ICell cell)
        {
            return PizzCell(cell).IsSliced;
        }

        public PizzCell PizzCell(ICell cell)
        {
            return this.GetCell(cell) as PizzCell;
        }

        public PizzCell PizzCell(int row, int column)
        {
            return this.GetCell(row, column) as PizzCell;
        }
    }
}