namespace QuejaModel;

public class Queja

{
    public String IdQueja { get; } = Guid.NewGuid().ToString();
    public required String IdTicket { get; init; }
    public required String Title { get; set; }
    public required String Description { get; set; }
}
