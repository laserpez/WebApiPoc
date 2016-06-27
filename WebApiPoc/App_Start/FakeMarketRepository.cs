using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace WebApiPoc
{
    internal class FakeMarketRepository: IMarketRepository
    {
        private static readonly ConcurrentBag<Market> _markets = new ConcurrentBag<Market>();

        public DateTime LastUpdateTime { get; private set; }

        public FakeMarketRepository()
        {
            LastUpdateTime = DateTime.Now;
        }

        public IEnumerable<Market> GetAll()
        {
            return _markets;
        }

        public Market Get(int id)
        {
            var found =
                from m in _markets
                where m.Id == id
                select m;

            var result = found.FirstOrDefault();
            return result;
        }

        public bool Store(Market market)
        {
            var first = _markets.FirstOrDefault(m => m.Id == market.Id);
            if (first == null)
            {
                _markets.Add(market);
                LastUpdateTime = DateTime.Now;
            }
            return first == null;
        }

        public bool Delete(int id)
        {
            var market = Get(id);
            return market != null;
        }
    }
}