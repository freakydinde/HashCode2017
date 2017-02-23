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

            this.Cells = (from i in pizza.Cells where (i.Row == this.StartCell.Row && i.Column == this.StartCell.Column) select i as PizzCell).ToList();
        }

        public PizzSlice(ICell startCell, ICell endCell, PizzGrid pizza)
        {
            this.StartCell = startCell;
            this.EndCell = endCell;

            this.Cells = (from i in pizza.Cells where (i.Row >= this.StartCell.Row && i.Column >= this.StartCell.Column && i.Row <= this.EndCell.Row && i.Column <= this.EndCell.Column) select i as PizzCell).ToList();
        }

        public ICell EndCell { get; private set; }

        public void EditSlice(ICell endCell, PizzGrid pizza)
        {
            this.EndCell = endCell;

            this.Cells = (from i in pizza.Cells where (i.Row >= this.StartCell.Row && i.Column >= this.StartCell.Column && i.Row <= this.EndCell.Row && i.Column <= this.EndCell.Column) select i as PizzCell).ToList();
        }

        public int MushroomCount
        {
            get
            {
                return (from i in Cells where i.Type == PizzCellType.Mushroom select i).Count();
            }
        }

        public List<PizzCell> Cells { get; private set; }

        public ICell StartCell { get; private set; }

        public int TomatoCount
        {
            get
            {
                return (from i in Cells where i.Type == PizzCellType.Tomato select i).Count();
            }
        }

        public static PizzSlice GetSlice(ICell startFrom, PizzGrid pizza)
        {
            PizzSlice slice = new PizzSlice(startFrom, pizza);
        }

        public override string ToString()
        {
            return Write.Invariant($"{this.StartCell} {this.EndCell}");
        }
    }
}