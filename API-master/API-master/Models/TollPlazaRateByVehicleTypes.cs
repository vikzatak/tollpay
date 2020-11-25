using System;

namespace DPTPWebAPI.Models
{
    public class TollPlazaRateByVehicleTypes
    {
        public int id { get; set; }
        public string name { get; set; }
        public Nullable<double> lat { get; set; }
        public Nullable<double> lon { get; set; }
        public double? Rate { get; set; }

    }
}