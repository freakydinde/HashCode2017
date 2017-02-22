namespace HashCode
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>Type representing a world grid</summary>
    public class Grid : IGrid
    {
        /// <summary>Initializes a new instance of the <see cref="Grid"/> class.</summary>
        /// <param name="rows">how many grid rows</param>
        /// <param name="columns">how many grid columns</param>
        /// <param name="cells">grid cells collection</param>
        public Grid(int rows, int columns, List<ICell> cells) : this(rows, columns)
        {
            this.Cells = cells.Distinct().OrderBy(x => x.Row).ThenBy(x => x.Column).ToList();
        }

        /// <summary>Initializes a new instance of the <see cref="Grid"/> class.</summary>
        /// <param name="rows">how many grid rows</param>
        /// <param name="columns">how many grid columns</param>
        /// <param name="buildCells">should build cells</param>
        public Grid(int rows, int columns, bool buildCells) : this(rows, columns)
        {
            if (buildCells)
            {
                List<ICell> cells = new List<ICell>();

                for (int i = 0; i < this.Columns; i++)
                {
                    for (int j = 0; j < this.Rows; j++)
                    {
                        cells.Add(new Cell(i, j));
                    }
                }

                this.Cells = cells.Distinct().OrderBy(x => x.Row).ThenBy(x => x.Column).ToList();
            }
            else
            {
                this.Cells = new List<ICell>();
            }
        }

        /// <summary>Initializes a new instance of the <see cref="Grid"/> class.</summary>
        /// <param name="rows">how many grid rows</param>
        /// <param name="columns">how many grid columns</param>
        public Grid(int rows, int columns)
        {
            this.Columns = columns;
            this.Rows = rows;
        }

        /// <summary>Gets or sets grid cells</summary>
        public List<ICell> Cells { get; set; }

        /// <summary>Gets grid's maximum columns</summary>
        public int Columns { get; set; }

        /// <summary>Gets grid's maximum rows</summary>
        public int Rows { get; set; }

        /// <summary>Gets or sets grid start cell</summary>
        public ICell startCell { get; set; }

        /// <summary>Gets grid cell corresponding to cell input</summary>
        /// <param name="cell">input cell</param>
        /// <returns>null if cell is not a cell</returns>
        public ICell GetCell(ICell cell)
        {
            return this.GetCell(cell.Row, cell.Column);
        }

        /// <summary>Gets grid cell corresponding to cell input</summary>
        /// <param name="cell">input cell</param>
        /// <returns>null if cell is not a cell</returns>
        public ICell SetCell(ICell cell)
        {
            IEnumerable<ICell> gridCells = this.Cells.Where(x => x.Row == cell.Row && x.Column == cell.Column);

            if (gridCells.Any())
            {
                int index = this.Cells.IndexOf(gridCells.FirstOrDefault());

                this.Cells[index] = cell;

                return this.Cells[index];
            }
            else
            {
                return null;
            }
        }

        /// <summary>Gets grid cell corresponding to cell input</summary>
        /// <param name="cell">input cell</param>
        /// <returns>null if cell is not a cell</returns>
        public ICell GetCell(int row, int column)
        {
            IEnumerable<ICell> gridCells = this.Cells.Where(x => x.Row == row && x.Column == column);

            if (gridCells.Any())
            {
                return gridCells.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        /// <summary>Gets a string representation of the current object</summary>
        /// <returns>this as <see cref="string"/></returns>
        public override string ToString()
        {
            return Write.Invariant($"Rows:{this.Rows} Columns:{this.Columns} CellsNumber:{this.Cells.Count()}");
        }
    }
}