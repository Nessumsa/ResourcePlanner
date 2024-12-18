namespace ResourcePlanner.Domain
{
    public class Resource
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? InstitutionId { get; set; }
        public Resource (string name, string description, string imgPath, string institutionId)
        {
            this.Name = name;
            this.Description = description;
            this.ImageUrl = imgPath;
            this.InstitutionId = institutionId;
        }
    }
}
