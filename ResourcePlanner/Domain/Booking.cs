using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcePlanner.Domain
{
    internal class Booking
    {
        public string Id { get; set; }
        public string InstitutionId { get; set; }
        public string UserId { get; set; }
        public string ResourceId { get; set; }
        public DateTime Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
