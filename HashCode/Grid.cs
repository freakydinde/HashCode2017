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
        /// <param name="gridPackets">Cells containing a packets</param>
        /// <param name="startCell">start position on the grid</param>
        public Grid(int rows, int columns, List<ICell> cells)
        {
            this.Columns = columns;
            this.Rows = rows;

            this.Cells = cells;
        }

        /// <summary>Gets or sets grid cells</summary>
        public List<ICell> Cells { get; set; }

        /// <summary>Gets grid's maximum columns</summary>
        public int Columns { get; set; }

        /// <summary>Gets grid's maximum rows</summary>
        public int Rows { get; set; }

        /// <summary>Gets a string representation of the current object</summary>
        /// <returns>this as <see cref="string"/></returns>
        public override string ToString()
        {
            return Write.Invariant($"Rows:{this.Rows} Columns:{this.Columns} CellsNumber:{this.Cells.Count()}");
        }
    }
}