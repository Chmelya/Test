namespace AM.Domain.Enities;

public  class Geolocation : Entity
{
    //TODO: To enum
    public string Type { get; set; }

    public List<double> Coordinates { get; set; }
}
