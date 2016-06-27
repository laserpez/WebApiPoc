using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace WebApiPoc
{
    internal class FakeMarketRepository: IMarketRepository
    {
        private readonly ConcurrentDictionary<int, Market> _markets = new ConcurrentDictionary<int, Market>();

        public DateTime LastUpdateTime { get; private set; }

        public FakeMarketRepository()
        {
            LastUpdateTime = DateTime.Now;
        }

        public IEnumerable<Market> GetAll()
        {
            return _markets.Values;
        }

        public Market Get(int id)
        {
            Market market;
            _markets.TryGetValue(id, out market);
            return market;
        }

        public bool Store(Market market)
        {
            var r = _markets.TryAdd(market.Id, market);
            if (!r) return false;
            
            LastUpdateTime = DateTime.Now;
            return true;
        }

        public bool Delete(int id)
        {
            Market market;
            var r = _markets.TryRemove(id, out market);
            if (!r) return false;

            LastUpdateTime = DateTime.Now;
            return true;
        }
    }
}