using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WeightedList;

public class WeightedList<T> : IEnumerable<T>
{
    private List<T> list;
    private List<(float cur, float cum)> weights;

    private int? seed;
    private Random rng;

    public WeightedList(int? seed = null)
    {
        list = new List<T>();
        weights = new List<(float cur, float cum)>();

        this.seed = seed;

        if (seed.HasValue)
        {
            rng = new Random(seed.Value);
        }
        else
        {
            rng = new Random();
        }
    }

    public int Count => list.Count;

    public (T item, float weight) this[int i]
    {
        get => (list[i], weights[i].cur);
    }

    public void Add(T item, float weight)
    {
        var i = BinarySearch(weight, weights.Select(x => x.cur).ToList());
        list.Insert(i, item);
        weights.Insert(i, (weight, 0f));
        UpdateCumulatives(i);
    }

    public void Remove(T item)
    {
        var i = list.IndexOf(item);
        list.RemoveAt(i);
        weights.RemoveAt(i);
        UpdateCumulatives(i);
    }

    public void RemoveAt(int i)
    {
        list.RemoveAt(i);
        weights.RemoveAt(i);
        UpdateCumulatives(i);
    }

    public void Clear()
    {
        list.Clear();
        weights.Clear();

        if (seed.HasValue)
            rng = new Random(seed.Value);
        else
            rng = new Random();
    }

    public T GetRandom()
    {
        return list[BinarySearch((float)rng.NextDouble() * weights[^1].cum, weights.Select(x => x.cum).ToList())];
    }

    public IEnumerator<T> GetEnumerator()
    {
        return new Enumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return new Enumerator(this);
    }

    private static int BinarySearch(float value, List<float> list)
    {
        int min = 0;
        int max = list.Count - 1;

        while (min <= max)
        {
            int mid = (min + max) / 2;

            if (list[mid] < value)
            {
                min = mid + 1;
            }
            else if (list[mid] > value)
            {
                max = mid - 1;
            }
            else
            {
                return mid;
            }
        }

        return min;
    }

    private void UpdateCumulatives(int startIndex)
    {
        for (int i = startIndex; i < weights.Count; i++)
        {
            weights[i] = (weights[i].cur, weights[i].cur + (i > 0 ? weights[i - 1].cum : 0));
        }
    }

    public struct Enumerator : IEnumerator<T>
    {
        private WeightedList<T> list;
        private int index;

        private T current;
        public readonly T Current => current;
        readonly object IEnumerator.Current => current;

        internal Enumerator(WeightedList<T> list)
        {
            this.list = list;
            index = 0;
            current = default;
        }

        public void Dispose() { }

        public bool MoveNext()
        {
            if (list.list.Count > index)
            {
                current = list.list[index];
                index++;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Reset()
        {
            index = 0;
            current = default;
        }
    }
}
