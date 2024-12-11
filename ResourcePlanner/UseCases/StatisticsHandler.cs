using ResourcePlanner.Domain;
using ResourcePlanner.Interfaces.Adapters.CRUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcePlanner.UseCases
{
    class StatisticsHandler
    {
        private readonly ICreateAdapter<Statistics> _statisticsAdapter;
        public StatisticsHandler(ICreateAdapter<Statistics> statisticsAdapter)
        {
            this._statisticsAdapter = statisticsAdapter;
        }

        public async Task<bool> CreateStatistics(Statistics statistics)
        {
            return await _statisticsAdapter.CreateAsync(statistics);
        }
    }
}
