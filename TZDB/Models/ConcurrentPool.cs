/* (c) Copyright Francisco Aguilera (Falven)
 * You are free to edit and distribute this
 * source so long as this statement remains
 * in place, here and in all other such files.
 */

using System;
using System.Collections.Concurrent;

namespace NUIClockUpdater.Models
{
    /// <summary>
    /// Represents a thread safe pool of T items.
    /// </summary>
    /// <typeparam name="T">The entity type to store in this pool.</typeparam>
    class ConcurrentPool<T>
    {
        private ConcurrentStack<T> _pool;

        /// <summary>
        /// Initializes this T pool with the provided capacity.
        /// </summary>
        /// <param name="capacity"></param>
        public ConcurrentPool(int capacity)
        {
            _pool = new ConcurrentStack<T>();
        }

        /// <summary>
        /// Push a T instance into the pool.
        /// </summary>
        /// <param name="item">The item to add to this pool.</param>
        public void Push(T item)
        {
            if (null == item)
            {
                throw new ArgumentNullException("item");
            }
            _pool.Push(item);
        }

        /// <summary>
        /// Remove and return a T instance from the pool.
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            T popped = default(T);
            _pool.TryPop(out popped);
            return popped;
        }

        /// <summary>
        /// The number of T instances in the pool.
        /// </summary>
        public int Count
        {
            get
            {
                return _pool.Count;
            }
        }
    }
}
