namespace HashCode.Pizza
{
    public enum PizzCellType
    {
        Free = 2,
        Tomato = 4,
        Mushroom = 8,
    }

    public class PizzCell : Cell
    {
        public PizzCell(int row, int column) : base(row, column)
        {
            this.Type = PizzCellType.Free;
        }

        public PizzCell(int row, int column, PizzCellType type) : base(row, column)
        {
            this.Type = type;
        }

        public PizzCell(int row, int column, char type) : base(row, column)
        {
            this.Type = PizzCell.TypeFromChar(type);
        }

        public PizzCellType Type { get; set; }

        public static PizzCellType TypeFromChar(char input)
        {
            switch (input)
            {
                case 'T':
                    return PizzCellType.Tomato;

                case 'M':
                    return PizzCellType.Mushroom;

                default:
                    return PizzCellType.Free;
            }
        }

        public bool IsSliced { get; set; }

        public override string ToString()
        {
            return Write.Invariant($"{this.Row} {this.Column}");
        }
    }
}