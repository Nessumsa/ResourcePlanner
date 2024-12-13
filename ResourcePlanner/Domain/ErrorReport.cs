using System.Text.Json.Serialization;

namespace ResourcePlanner.Domain
{
    public class ErrorReport
    {
        public string? Id { get; set; }
        public string? InstitutionId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ResourceId { get; set; }
        public string? Description { get; set; }
        public string? UserId { get; set; }
        public bool Resolved { get; set; }

        [JsonIgnore]
        public Resource? AssociatedResource { get; set; }
        [JsonIgnore]
        public User? AssociatedUser { get; set; }
    }
}
