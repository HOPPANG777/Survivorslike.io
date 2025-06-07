using System;
using System.Collections.Generic;
using UnityEngine;

public static class ShuffleUtils
{
    /// <summary>
    /// IList<T> (List<T> 또는 T[] 등) 를 Fisher–Yates 로 섞습니다.
    /// for 문만 쓰고, foreach 전혀 사용하지 않습니다.
    /// </summary>
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            // swap list[i] & list[j]
            T tmp = list[i];
            list[i] = list[j];
            list[j] = tmp;
        }
    }

    /// <summary>
    /// 배열 전용 오버로드 (사실 T[] 도 IList<T> 이므로 위 메서드로도 동작하지만, 
    /// 편의상 이름만 따로 두고 싶을 때)
    /// </summary>
    public static void Shuffle<T>(this T[] array)
    {
        int n = array.Length;
        for (int i = n - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            T tmp = array[i];
            array[i] = array[j];
            array[j] = tmp;
        }
    }
}