using System;
using System.Collections.Generic;

namespace GameCore.Utility.DataStructure
{
    public class PriorityQueue<T>
    {
        private List<T> heap = new();
        private Comparison<T> compare;
        public int Count => heap.Count;

        public PriorityQueue(Comparison<T> comparison)
        {
            compare = comparison;
        }

        public void Enqueue(T item)
        {
            heap.Add(item);
            HeapifyUp(heap.Count - 1);
        }

        public T Dequeue()
        {
            var root = heap[0];
            heap[0] = heap[^1];
            heap.RemoveAt(heap.Count - 1);
            HeapifyDown(0);
            return root;
        }

        private void HeapifyUp(int i)
        {
            while (i > 0)
            {
                int parent = (i - 1) / 2;

                if (compare(heap[i], heap[parent]) >= 0)
                    break;
                Swap(i, parent);
                i = parent;
            }
        }
        private void HeapifyDown(int i)
        {
            int last = heap.Count - 1;

            while (true)
            {
                int left = i * 2 + 1;
                int right = i * 2 + 2;
                int smallest = i;

                if (left <= last && compare(heap[left], heap[smallest]) < 0)
                    smallest = left;

                if (right <= last && compare(heap[right], heap[smallest]) < 0)
                    smallest = right;

                if (smallest == i) break;

                Swap(i, smallest);
                i = smallest;
            }
        }

        private void Swap(int a, int b)
        {
            (heap[a], heap[b]) = (heap[b], heap[a]);
        }

    }
}