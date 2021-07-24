using System;
using UnityEngine;
using Random = System.Random;

namespace Extensions
{
    public class EnumExtensions : MonoBehaviour
    {
        public static T RandomEnumValue<T> ()
        {
            Random random = new Random ();
            var v = Enum.GetValues (typeof (T));
            return (T) v.GetValue (random.Next(v.Length));
        }
    }
}
