using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcePlanner.Domain
{
    class Resource
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImgPath { get; set; }
        public string? InstitutionId { get; set; }
        public Resource (string name, string description, string imgPath, string institutionId)
        {
            this.Name = name;
            this.Description = description;
            this.ImgPath = imgPath;
            this.InstitutionId = institutionId;
        }
    }
}
