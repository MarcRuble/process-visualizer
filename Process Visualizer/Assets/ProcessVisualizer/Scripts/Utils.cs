using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ProcessVisualizer
{
    public class Utils
    {
        public static Dictionary<T1, List<T2>> CreateDictListsFromArray<T1, T2>(T2[] input, Func<T2, T1> keyFunc)
        {
            Dictionary<T1, List<T2>> dict = new Dictionary<T1, List<T2>>();

            foreach (T2 t in input)
            {
                T1 key = keyFunc(t);

                if (!dict.ContainsKey(key))
                    dict.Add(key, new List<T2>());

                dict[key].Add(t);
            }

            return dict;
        }

        public static Dictionary<int, T> CreateDictFromChildren<T>(MonoBehaviour mono, Func<T, int> keyFunc)
        {
            T[] raw = mono.GetComponentsInChildren<T>();
            Dictionary<int, T> dict = new Dictionary<int, T>();

            foreach (T t in raw)
                dict.Add(keyFunc(t), t);

            return dict;
        }

        public static Dictionary<T1, T2> CreateDictFromChildren<T1, T2>(MonoBehaviour mono, Func<T2, T1> keyFunc)
        {
            T2[] raw = mono.GetComponentsInChildren<T2>();
            Dictionary<T1, T2> dict = new Dictionary<T1, T2>();

            foreach (T2 t in raw)
                dict.Add(keyFunc(t), t);

            return dict;
        }

        public static Dictionary<(T1, T2), T3> CreateDictFromChildren<T1, T2, T3>(MonoBehaviour mono, Func<T3, (T1, T2)> keyFunc)
        {
            T3[] raw = mono.GetComponentsInChildren<T3>();
            Dictionary<(T1, T2), T3> dict = new Dictionary<(T1, T2), T3>();

            foreach (T3 t in raw)
                dict.Add(keyFunc(t), t);

            return dict;
        }

        public static Dictionary<(T1, T2), T4> CreateDictFromChildren<T1, T2, T3, T4>
            (MonoBehaviour mono, Func<T3, (T1, T2)> keyFunc, Func<T3, T4> valueFunc)
        {
            T3[] raw = mono.GetComponentsInChildren<T3>();
            Dictionary<(T1, T2), T4> dict = new Dictionary<(T1, T2), T4>();

            foreach (T3 t in raw)
                dict.Add(keyFunc(t), valueFunc(t));

            return dict;
        }

        public static string FloatToString(float f)
        {
            f = (float)Math.Round(f, 4);
            return f.ToString().Replace(",", ".");
        }

        public static float StringToFloat(string s)
        {
            return float.Parse(s.Replace('.', ','));
        }
    }
}
