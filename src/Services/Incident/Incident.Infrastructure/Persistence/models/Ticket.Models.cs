namespace TicketsModel;

public enum Tickets
{
    Claim,   // Reporte de pérdida
    Found,   // Objeto encontrado
    Match,   // Coincidencia encontrada
    Pickup   // Entrega/retiro
}

public class Ticket
{
    public String IdTicket { get; } = Guid.NewGuid().ToString();
    public required String IdUsuario { get; init; }
    public String? IdStructure { get; init; }
    public required Tickets TypeTicket { get; set; }
    public required Boolean IsActive { get; set; }
    public required DateTime DateReport { get; init; }
    public (decimal Latitude, decimal Longitude)? Coordenadas { get; set; }
}
