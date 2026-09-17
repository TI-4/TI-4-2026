namespace ObjectModel;

public enum Statusenum {
    Pendiente,
    EnProceso,
    Resuelto,
    Cancelado
}

public class ObjectoReportado
{
    public String IdObject { get; } = Guid.NewGuid().ToString();
    public required String IdtTicket { get; init; }
    public required String Title { get; set; }
    public required String Description { get; set; }
    public required Statusenum Status { get; set; }
    public String? PictureUri { get; set; }
}
