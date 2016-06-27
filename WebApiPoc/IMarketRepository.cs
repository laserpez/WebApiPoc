using System;
using System.Collections.Generic;

namespace WebApiPoc
{
    public interface IMarketRepository
    {
        IEnumerable<Market> GetAll();
        Market Get(int id);
        bool Store(Market market);
        bool Delete(int id);

        DateTime LastUpdateTime { get; }
    }
}