using System;
using UnityEngine;
using Random = System.Random;

namespace Extensions
{
    public static class EnumExtensions
    {
        public static T RandomEnumValue<T> ()
        {
            Random random = new Random ();
            var v = Enum.GetValues (typeof (T));
            return (T) v.GetValue (random.Next(v.Length));
        }
        
        public static T NextEnumElement<T>(this T src, int next) where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

            T[] Arr = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(Arr, src) + next;
            return (Arr.Length == j || Arr.Length <= j || j < 0) ? Arr[0] : Arr[j];            
        }
    }
}
