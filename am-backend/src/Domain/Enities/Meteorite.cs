namespace AM.Domain.Enities;

public class Meteorite : Entity
{
    public string Name { get; set; }

    //TODO: To enum
    public string NameType { get; set; }

    public int RecclassId { get; set; }

    //TODO: To enum
    public Recclass Recclass { get; set; }

    public double Mass { get; set; }

    //TODO: To bool
    public string Fall { get; set; }

    public int Year { get; set; }

    public double Reclat { get; set; }

    public double Reclong { get; set; }

    public Geolocation? Geolocation { get; set; }
}
