namespace NZWalks.Models.DTO
{
    public class UpdateRegionRequest
    {
        /*Depending on how much you want the client to update create the properties here*/

        public string Code { get; set; }

        public string Name { get; set; }

        public double Area { get; set; }

        public double Lat { get; set; }

        public double Long { get; set; }

        public long Population { get; set; }
    }
}
