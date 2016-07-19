﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LimitedSizeStack<T> : LinkedList<T>
{
    private readonly int _maxSize;
    public LimitedSizeStack(int maxSize)
    {
        _maxSize = maxSize;
    }

    public void Push(T item)
    {
        this.AddFirst(item);

        if (this.Count > _maxSize)
            this.RemoveLast();
    }

    public T Pop()
    {
        var item = this.First.Value;
        this.RemoveFirst();
        return item;
    }

    public T Peek()
    {
        if (this.Count == 0)
            return default(T);

        return this.First.Value;
    }
}
