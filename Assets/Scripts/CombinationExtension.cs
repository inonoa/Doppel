using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CombinationExtension
{
    public static void ForEachCombi<T>(this IEnumerable<T> collection, Action<T, T> action)
        where T: class
    {
        Dictionary<T, List<T>> others = new Dictionary<T, List<T>>();
        
        foreach(T elm1 in collection)
        {
            foreach(T elm2 in collection)
            {
                if(others.ContainsKey(elm1) && others[elm1].Contains(elm2)) continue;

                if(! others.ContainsKey(elm1)) others.Add(elm1, new List<T>());
                if(! others.ContainsKey(elm2)) others.Add(elm2, new List<T>());
                others[elm1].Add(elm2);
                others[elm2].Add(elm1);

                action.Invoke(elm1, elm2);
            }
        }
    }
}
