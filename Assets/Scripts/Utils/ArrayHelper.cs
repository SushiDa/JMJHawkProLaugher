using System;
using UnityEngine;

public static class ArrayHelper
{
    public static void AddToArray<t>(ref t[] array, t element)
    {
        int arrayLength = array.Length;
        Array.Resize(ref array, arrayLength + 1);
        array[arrayLength] = element;
    }

    public static void RemoveFromArray<t>(ref t[] array, t element)
    {
        bool foundedToRemove = false;
        int arrayLength = array.Length;
        for (int i = 0; i < arrayLength - 1; ++i) {
            if (!foundedToRemove) foundedToRemove = array[i].Equals(element);
            if (foundedToRemove) array[i] = array[i + 1];
        }
        Array.Resize(ref array, arrayLength - 1);
    }

    public static void InsertIntoArray<t>(ref t[] array, t element, int position)
    {
        int clampedPosition = Mathf.Clamp(position, 0, array.Length);
        Array.Resize(ref array, array.Length + 1);
        Array.Copy(array, clampedPosition, array, clampedPosition + 1, array.Length - clampedPosition - 1);
        array[clampedPosition] = element;
    }

    public static void CopyArray<t>(in t[] original, out t[] target) where t : ICloneable
    {
        target = new t[original.Length];
        for (int i = 0; i < original.Length; ++i) {
            target[i] = (t)original[i].Clone();
        }
    }

    public static t[] ArrayFilled<t>(int count, t element) where t : ICloneable
    {
        t[] array = new t[count];
        for (int i = 0; i < count; ++i) array[i] = (t)element.Clone();
        return array;
    }
}