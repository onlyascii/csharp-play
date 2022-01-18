﻿using System;
using System.Collections.Generic;

namespace Architecture
{
    public class RandomIntProvider : IDataProvider<int>
    {
        private readonly int _maxValue;
        private readonly int _count;
        private readonly Random _random;

        public RandomIntProvider(IConfig config)
        {
            _maxValue = config.MaxRandomInt;
            _count = config.RandomIntCount;
            _random = new Random();
        }

        public IEnumerable<int> GetData()
        {
            for(int i=0; i<_count; i++ )
            {
                yield return _random.Next(_maxValue);
            }
        }
    }
}