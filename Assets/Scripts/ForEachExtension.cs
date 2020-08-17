using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ForEachExtension
{
    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach(T elem in enumerable)
        {
            action.Invoke(elem);
        }
    }

}
