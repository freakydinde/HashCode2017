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

            RowLoopDown = false;
            ColumnLoopRight = false;
            RowLoopUp = false;
            ColumnLoopLeft = false;
            DiagonalMove = false;

            this.StartCell = new Cell(0, 0);
        }

        /// <summary>Gets or sets grid cells</summary>
        public List<ICell> Cells { get; set; }

        /// <summary>Gets or sets a boolean indicating if grid can loop when column left limit is reached</summary>
        public bool ColumnLoopLeft { get; set; }

        /// <summary>Gets or sets a boolean indicating if grid can loop when column right limit is reached</summary>
        public bool ColumnLoopRight { get; set; }

        /// <summary>Gets or sets grid's maximum columns</summary>
        public int Columns { get; set; }

        /// <summary>Gets or sets a boolean indicating if diagonal move are allowed</summary>
        public bool DiagonalMove { get; set; }

        /// <summary>Gets or sets a boolean indicating if grid can loop when row down limit is reached</summary>
        public bool RowLoopDown { get; set; }

        /// <summary>Gets or sets a boolean indicating if grid can loop when row up limit is reached</summary>
        public bool RowLoopUp { get; set; }

        /// <summary>Gets grid's maximum rows</summary>
        public int Rows { get; set; }

        /// <summary>Gets or sets grid start cell</summary>
        public ICell StartCell { get; set; }

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

        /// <summary>Get available neigbors for a given cell</summary>
        /// <param name="cell">source cell</param>
        /// <returns>available neighbors</returns>
        public IEnumerable<ICell> Neighbors(ICell cell)
        {
            List<ICell> availableNeighbors = new List<ICell>();

            availableNeighbors.Add(new Cell(cell.Row - 1, cell.Column));
            availableNeighbors.Add(new Cell(cell.Row + 1, cell.Column));
            availableNeighbors.Add(new Cell(cell.Row, cell.Column - 1));
            availableNeighbors.Add(new Cell(cell.Row, cell.Column + 1));

            if (this.DiagonalMove)
            {
                availableNeighbors.Add(new Cell(cell.Row - 1, cell.Column - 1));
                availableNeighbors.Add(new Cell(cell.Row - 1, cell.Column + 1));
                availableNeighbors.Add(new Cell(cell.Row + 1, cell.Column - 1));
                availableNeighbors.Add(new Cell(cell.Row + 1, cell.Column + 1));
            }

            List<ICell> neighbors = new List<ICell>();

            foreach (ICell neighbor in availableNeighbors)
            {
                if (neighbor.Row > this.Rows && this.RowLoopDown)
                {
                    neighbor.Row = 0;
                }

                if (neighbor.Row < 0 && this.RowLoopUp)
                {
                    neighbor.Row = this.Rows;
                }

                if (neighbor.Column > this.Columns && this.ColumnLoopRight)
                {
                    neighbor.Column = 0;
                }

                if (neighbor.Column < 0 && this.ColumnLoopLeft)
                {
                    neighbor.Column = this.Columns;
                }

                if (neighbor.Row >= 0 && neighbor.Row <= this.Rows && neighbor.Column >= 0 && neighbor.Column <= this.Columns)
                {
                    neighbors.Add(neighbor);
                }
            }

            return neighbors;
        }

        /// <summary>Get available neigbors for a given cell</summary>
        /// <param name="cell">source cell</param>
        /// <param name="exclusion">cell excluded from available neighbors</param>
        /// <returns>available neighbors</returns>
        public IEnumerable<ICell> NeighborsOut(ICell cell, IEnumerable<ICell> exclusion)
        {
            IEnumerable<ICell> availableNeighbors = this.Neighbors(cell);
            List<ICell> neighbors = new List<ICell>();

            foreach (ICell neighbor in availableNeighbors)
            {
                if (exclusion == null || exclusion.Where(x => x.Row == neighbor.Row && x.Column == neighbor.Column).FirstOrDefault() == null)
                {
                    neighbors.Add(neighbor);
                }
            }

            return neighbors;
        }

        /// <summary>Get available neigbors for a given cell</summary>
        /// <param name="cell">source cell</param>
        /// <param name="inclusion">exclusive available neighbors</param>
        /// <returns>available neighbors</returns>
        public IEnumerable<ICell> NeighborsIn(ICell cell, IEnumerable<ICell> inclusion)
        {
            IEnumerable<ICell> availableNeighbors = this.Neighbors(cell);
            List<ICell> neighbors = new List<ICell>();

            foreach (ICell neighbor in availableNeighbors)
            {
                if (inclusion != null && inclusion.Where(x => x.Row == neighbor.Row && x.Column == neighbor.Column).FirstOrDefault() != null)
                {
                    neighbors.Add(neighbor);
                }
            }

            return neighbors;
        }

        /// <summary>Gets grid cell corresponding to cell input</summary>
        /// <param name="cell">input cell</param>
        /// <returns>cell or null if cell is not a grid cell</returns>
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

        /// <summary>Gets a string representation of the current object</summary>
        /// <returns>this as <see cref="string"/></returns>
        public override string ToString()
        {
            return Write.Invariant($"Rows:{this.Rows} Columns:{this.Columns} CellsNumber:{this.Cells.Count()}");
        }
    }
}