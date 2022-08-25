using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArrTools
{
	public static T[] Copy<T>(T[] arr)
    {
        T[] copy = new T[arr.Length];

        for(int i = 0; i < arr.Length; i++)
        {
            copy[i] = arr[i];
        }

        return copy;
    }

	public static void Swap<T>(T[] arr, int i, int j)
    {
        T temp = arr[i];
        arr[i] = arr[j];
        arr[j] = temp;
    }

	public static T[] Shuffle<T>(T[] arr)
	{
		T[] copy = Copy(arr);

		for(int i = copy.Length-1; i >= 1; i--)
		{
			int j = Random.Range(0, i);
			Swap(copy, i, j);
		}

        return copy;
	}

	public static T PickRandom<T>(T[] array)
    {
        int index = Random.Range(0, array.Length);
        return array[index];
    }
	
	public static T[] Repeat<T>(T val, int l)
	{
		T[] arr = new T[l];
		for(int i = 0; i < l; i++)
		{ arr[i] = val; }
		return arr;
	}
}
