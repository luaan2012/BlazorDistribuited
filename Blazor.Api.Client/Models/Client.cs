namespace Blazor.Api.Client.Models
{
    public class Client
    {
        public Guid Id { get; set; } = new Guid();
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public int? Number { get; set; }
        public bool? Active { get; set; } = true;
        public DateTime? DateCreated { get; set; }
        public DateTime? DateDelete { get; set; }
        public DateTime? DateModificated { get; set; }
    }
}
