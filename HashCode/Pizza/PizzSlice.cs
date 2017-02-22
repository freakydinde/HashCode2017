namespace HashCode.Pizza
{
    using System.Collections.Generic;
    using System.Linq;

    public class PizzSlice
    {
        public PizzSlice(ICell startCell, PizzGrid pizza)
        {
            this.StartCell = startCell;
            this.EndCell = startCell;

            this.SliceCells = (from i in pizza.Cells where (i.Row == startCell.Row && i.Column == startCell.Column) select i as PizzCell).ToList();
        }

        public PizzSlice(ICell startCell, ICell endCell, PizzGrid pizza)
        {
            this.StartCell = startCell;
            this.EndCell = endCell;

            this.SliceCells = (from i in pizza.Cells where (i.Row >= startCell.Row && i.Column >= startCell.Column && i.Row <= endCell.Row && i.Column <= endCell.Column) select i as PizzCell).ToList();
        }

        public ICell EndCell { get; set; }

        public bool IsRectancle
        {
            get
            {
                ICell UpLeft = (from i in SliceCells orderby i.Row, i.Column select i).Take(1).FirstOrDefault();

                ICell UpRight = (from i in SliceCells orderby i.Row, i.Column descending select i).Take(1).FirstOrDefault();

                ICell DownRight = (from i in SliceCells orderby i.Row descending, i.Column select i).Take(1).FirstOrDefault();

                ICell DownLeft = (from i in SliceCells orderby i.Row descending, i.Column descending select i).Take(1).FirstOrDefault();

                if (UpLeft.Column == DownLeft.Column && UpRight.Column == DownRight.Column && UpLeft.Row == UpRight.Row && DownLeft.Row == DownRight.Row)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public int MushroomCount
        {
            get
            {
                return (from i in SliceCells where i.Type == PizzCellType.Mushroom select i).Count();
            }
        }

        public List<PizzCell> SliceCells { get; private set; }

        public ICell StartCell { get; set; }

        public int TomatoCount
        {
            get
            {
                return (from i in SliceCells where i.Type == PizzCellType.Tomato select i).Count();
            }
        }

        public void AddCell(PizzCell cell)
        {
            this.SliceCells.Add(cell);
            this.EndCell = cell;
        }

        /*
        public static PizzSlice GetSlice(ICell startFrom, PizzGrid pizza)
        {
            PizzSlice slice = new PizzSlice(startFrom, pizza);

            SliceCells.Add(pizza.GetCell(startFrom) as PizzCell);

            while ()
                IEnumerable<PizzCell> pizzFreeCells = pizza.FreeCells;
        }*/

        public override string ToString()
        {
            return Write.Invariant($"{this.StartCell} {this.EndCell}");
        }
    }
}