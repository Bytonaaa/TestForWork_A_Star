using System;
using System.Collections.Generic;

public class MinHeap<TKey, TValue> where TKey : IComparable<TKey>
{
    
    public class HeapObjectContainer
    {
        public readonly TKey m_key;
        public readonly TValue m_value;

        public HeapObjectContainer(TKey key, TValue value)
        {
            m_key = key;
            m_value = value;
        }
    }
    
    private readonly List<HeapObjectContainer> _elements;
    private int _size;
    
    public MinHeap()
    {
        _elements = new List<HeapObjectContainer>();
    }
    
    private int GetLeftChildIndex(int elementIndex) => 2 * elementIndex + 1;
    private int GetRightChildIndex(int elementIndex) => 2 * elementIndex + 2;
    private int GetParentIndex(int elementIndex) => (elementIndex - 1) / 2;

    private bool HasLeftChild(int elementIndex) => GetLeftChildIndex(elementIndex) < _size;
    private bool HasRightChild(int elementIndex) => GetRightChildIndex(elementIndex) < _size;
    private bool IsRoot(int elementIndex) => elementIndex == 0;

    private HeapObjectContainer GetLeftChild(int elementIndex) => _elements[GetLeftChildIndex(elementIndex)];
    private HeapObjectContainer GetRightChild(int elementIndex) => _elements[GetRightChildIndex(elementIndex)];
    private HeapObjectContainer GetParent(int elementIndex) => _elements[GetParentIndex(elementIndex)];

    private void Swap(int firstIndex, int secondIndex)
    {
        var temp = _elements[firstIndex];
        _elements[firstIndex] = _elements[secondIndex];
        _elements[secondIndex] = temp;
    }

    public bool IsEmpty()
    {
        return _size == 0;
    }

    public HeapObjectContainer Peek()
    {
        if (_size == 0)
            throw new IndexOutOfRangeException();

        return _elements[0];
    }

    public HeapObjectContainer Pop()
    {
        if (_size == 0)
            throw new IndexOutOfRangeException();

        var result = _elements[0];
        _elements[0] = _elements[_size - 1];
        _size--;

        ReCalculateDown();
        
        _elements.RemoveAt(_size);
        return result;
    }

    public void Add(TKey key, TValue value)
    {
        HeapObjectContainer element = new HeapObjectContainer(key, value);
        if (_size == _elements.Count)
        {
            _elements.Add(element);
        }
        else
        {
            _elements[_size] = element;
        }
        
        _size++;
        
        ReCalculateUp();
    }

    private void ReCalculateDown()
    {
        int index = 0;
        while (HasLeftChild(index))
        {
            var smallerIndex = GetLeftChildIndex(index);
            if (HasRightChild(index) && GetRightChild(index).m_key.CompareTo(GetLeftChild(index).m_key) < 0)
            {
                smallerIndex = GetRightChildIndex(index);
            }

            if (_elements[smallerIndex].m_key.CompareTo(_elements[index].m_key) >= 0)
            {
                break;
            }

            Swap(smallerIndex, index);
            index = smallerIndex;
        }
    }

    private void ReCalculateUp()
    {
        var index = _size - 1;
        while (!IsRoot(index) && _elements[index].m_key.CompareTo(GetParent(index).m_key) < 0)
        {
            var parentIndex = GetParentIndex(index);
            Swap(parentIndex, index);
            index = parentIndex;
        }
    }
}