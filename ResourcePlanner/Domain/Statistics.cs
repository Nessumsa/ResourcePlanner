using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcePlanner.Domain
{
    class Statistics
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndTime { get; set; }
        public string? InstitutionId { get; set; }

        public Statistics(DateTime startDate, DateTime endTime, string institutionId)
        {
            
            this.StartDate = startDate;
            this.EndTime = endTime;
            this.InstitutionId = institutionId;
        }
    }
}
