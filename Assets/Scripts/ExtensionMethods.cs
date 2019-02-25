using System;
using System.Collections.Generic;
public static class ExtensionMethods
{
    //dependant on Unity being set to use .Net4.x
    ///<summary>
    ///Returns the 2 dimensional index of a 2D array as a tuple of 2 ints
    ///</summary>
    public static Tuple<int, int> IndexOf2D<T>(this T[, ] matrix, T value)
    {
        int w = matrix.GetLength(0); // width
        int h = matrix.GetLength(1); // height

        for (int x = 0; x < w; ++x)
        {
            for (int y = 0; y < h; ++y)
            {
                if (matrix[x, y].Equals(value))
                    return Tuple.Create(x, y);
            }
        }

        return Tuple.Create(-1, -1);
    }
}