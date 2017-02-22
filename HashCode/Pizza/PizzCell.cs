namespace HashCode
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
    }
}