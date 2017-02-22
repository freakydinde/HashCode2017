namespace HashCode
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    public class Pizza : Grid
    {
        public Pizza(int rows, int columns) : base(rows, columns, false)
        {
            for (int i = 0; i < this.Columns; i++)
            {
                for (int j = 0; j < this.Rows; j++)
                {
                    this.Cells.Add(new PizzCell(i, j));
                }
            }
        }

        public int MaxCellsPerSlice { get; set; }

        public int MinIngredientPerSlice { get; set; }

        public static Pizza PizzaFromFile(string filePath)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Trace.WriteLine("A: " + sw.ElapsedMilliseconds);

            List<string> lines = File.ReadAllLines(filePath).ToList();

            sw.Restart();
            Trace.WriteLine("B: " + sw.ElapsedMilliseconds);

            int[] firstLine = (from i in lines.Take(1) select (from j in i.Split(' ') select Convert.ToInt32(j, CultureInfo.InvariantCulture)).ToArray()).FirstOrDefault();

            sw.Restart();
            Trace.WriteLine("C: " + sw.ElapsedMilliseconds);

            Pizza pizza = new Pizza(firstLine[0], firstLine[1]);
            pizza.MinIngredientPerSlice = firstLine[2];
            pizza.MaxCellsPerSlice = firstLine[3];

            sw.Restart();
            Trace.WriteLine("D: " + sw.ElapsedMilliseconds);

            foreach (string line in lines.Skip(1))
            {
                pizza.SetLine(lines.IndexOf(line), line);
            }

            sw.Restart();
            Trace.WriteLine("E: " + sw.ElapsedMilliseconds);

            return pizza;
        }

        public IEnumerable<ICell> Tomatoes
        {
            get
            {
                return from i in this.Cells where (i as PizzCell).Type == PizzCellType.Tomato select i;
            }
        }

        public IEnumerable<ICell> Mushroom
        {
            get
            {
                return from i in this.Cells where (i as PizzCell).Type == PizzCellType.Mushroom select i;
            }
        }

        public PizzCellType? CellType(int row, int column)
        {
            ICell cell = this.GetCell(row, column);

            if (cell != null)
            {
                if (cell is PizzCell)
                {
                    return (cell as PizzCell).Type;
                }
                else
                {
                    return PizzCellType.Free;
                }
            }
            else
            {
                return null;
            }
        }

        public void SetCellType(int row, int column, PizzCellType type)
        {
            (this.GetCell(row, column) as PizzCell).Type = type;
        }

        public void SetLine(int lineNumber, string lineString)
        {
            int characterNumber = 0;

            foreach (char character in lineString)
            {
                this.SetCellType(characterNumber, lineNumber, PizzCell.TypeFromChar(character));
                characterNumber++;
            }
        }
    }
}