using System;
using System.Collections.Generic;
public static class ExtensionMethods
{
    //dependant on Unity being set to use .Net4.x
    ///<summary>
    ///Returns the 2 dimensional index of a 2D array as a tuple of 2 ints
    ///</summary>
    public static MyTuple<int, int> IndexOf2D<T>(this T[, ] matrix, T value)
    {
        int w = matrix.GetLength(0); // width
        int h = matrix.GetLength(1); // height

        for (int x = 0; x < w; ++x)
        {
            for (int y = 0; y < h; ++y)
            {
                if (matrix[x, y].Equals(value))
                    return MyTuple.Create(x, y);
            }
        }

        return MyTuple.Create(-1, -1);
    }
}

public class MyTuple<T1, T2>
{
    public MyTuple(T1 item1, T2 item2)
    {
        this.Item1 = item1;
        this.Item2 = item2;
    }

    public T1 Item1 { get; private set; }

    public T2 Item2 { get; private set; }
}

public class MyTuple<T1, T2, T3>
{
    public MyTuple(T1 item1, T2 item2, T3 item3)
    {
        this.Item1 = item1;
        this.Item2 = item2;
        this.Item3 = item3;
    }

    public T1 Item1 { get; private set; }

    public T2 Item2 { get; private set; }

    public T3 Item3 { get; private set; }
}

public static class MyTuple
{
    public static MyTuple<T1, T2> Create<T1, T2>(
        T1 item1, T2 item2)
    {
        return new MyTuple<T1, T2>(item1, item2);
    }

    public static MyTuple<T1, T2, T3> Create<T1, T2, T3>(
        T1 item1, T2 item2, T3 item3)
    {
        return new MyTuple<T1, T2, T3>(item1, item2, item3);
    }
}