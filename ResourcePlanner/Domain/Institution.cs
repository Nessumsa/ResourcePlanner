namespace ResourcePlanner.Domain
{
    public class Institution
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? InstitutionImage { get; set; }
        public string? OpenTime { get; set; }
        public string? CloseTime { get; set; }
        public int BookingInterval { get; set; }
    }
}
