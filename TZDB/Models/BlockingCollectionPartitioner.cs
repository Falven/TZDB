﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUIClockUpdater.Models
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BlockingCollectionPartitioner<T> : Partitioner<T>
    {
        private BlockingCollection<T> _collection;

        internal BlockingCollectionPartitioner(BlockingCollection<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            _collection = collection;
        }

        public override bool SupportsDynamicPartitions
        {
            get { return true; }
        }

        public override IList<IEnumerator<T>> GetPartitions(int partitionCount)
        {
            if (partitionCount < 1)
            {
                throw new ArgumentOutOfRangeException("partitionCount");
            }

            var dynamicPartitioner = GetDynamicPartitions();
            return Enumerable.Range(0, partitionCount).Select(_ => dynamicPartitioner.GetEnumerator()).ToArray();
        }

        public override IEnumerable<T> GetDynamicPartitions()
        {
            return _collection.GetConsumingEnumerable();
        }
    }
}
