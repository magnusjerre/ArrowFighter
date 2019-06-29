using System.Collections.Generic;

namespace Jerre.Utils
{
    // This is not thread safe, only one consumer at a time
    public class QueueRerunnable<T>
    {
        public const int DEFAULT_QUEUE_CAPACITY = 20;
        private List<T> internalQueue;

        private int nextIndex = 0;

        public QueueRerunnable(int initialCapacity)
        {
            internalQueue = new List<T>(initialCapacity);
        }

        public QueueRerunnable() : this(DEFAULT_QUEUE_CAPACITY)
        {
        }

        public void Add(T element)
        {
            internalQueue.Add(element);
        }

        public void Add(List<T> elements)
        {
            internalQueue.AddRange(elements);
        }

        public bool HasNext()
        {
            return internalQueue.Count > 0 && nextIndex < internalQueue.Count;
        }

        public T Next()
        {
            return internalQueue[nextIndex++];
        }

        public void ResetQueueIteration()
        {
            nextIndex = 0;
        }

        public void Clear()
        {
            internalQueue.Clear();
            nextIndex = 0;
        }
    }
}
