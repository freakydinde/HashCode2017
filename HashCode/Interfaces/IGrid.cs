namespace HashCode
{
    using System.Collections.Generic;

    /// <summary>Type representing a world grid</summary>
    public interface IGrid
    {
        /// <summary>Gets or sets grid cells</summary>
        List<ICell> Cells { get; set; }

        /// <summary>Gets grid's maximum columns</summary>
        int Columns { get; set; }

        /// <summary>Gets grid's maximum rows</summary>
        int Rows { get; set; }

        /// <summary>Gets a string representation of the current object</summary>
        /// <returns>this as <see cref="string"/></returns>
        string ToString();
    }
}