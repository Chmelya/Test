using AM.Domain.Enities;

namespace AM.Application.Common.Responses
{
    public class MeteoritesGropedResponse
    {
        public int Year { get; set; }

        public double TotalMass { get; set; }

        public double MeteoritesCount { get; set; }

        public List<Meteorite> Meteorites { get; set; }
    }
}
