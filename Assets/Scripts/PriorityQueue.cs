using System;
using System.Collections.Generic;

public class PriorityQueue<T>
{
    private List<Tuple<T, int>> _elements = new List<Tuple<T, int>>();

    public int GetElementCount()
    {
        return _elements.Count;
    }

    public void Enqueue(T item, int priority)
    {
        _elements.Add(Tuple.Create(item, priority));
    }

    public T Dequeue()
    {
        int bestIndex = 0;

        for (int i = 0; i < _elements.Count; i++)
        {
            if (_elements[i].Item2 < _elements[bestIndex].Item2)
            {
                bestIndex = i;
            }
        }

        T bestItem = _elements[bestIndex].Item1;
        _elements.RemoveAt(bestIndex);
        return bestItem;
    }
}
