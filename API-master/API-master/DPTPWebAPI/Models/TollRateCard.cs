using System;

namespace DPTPWebAPI.Models
{
    public class TollRateCard
    {
        public int cnt { get; set; }
        public Nullable<double> rates_car_single { get; set; }
        public Nullable<double> rates_car_multi { get; set; }
        public Nullable<double> rates_car_monthly { get; set; }
        public Nullable<double> rates_lcv_single { get; set; }
        public Nullable<double> rates_lcv_multi { get; set; }
        public Nullable<double> rates_lcv_monthly { get; set; }
        public Nullable<double> rates_bus_multi { get; set; }
        public Nullable<double> rates_bus_monthly { get; set; }
        public Nullable<double> rates_multiaxle_single { get; set; }
        public Nullable<double> rates_multiaxle_multi { get; set; }
        public Nullable<double> rates_multiaxle_monthly { get; set; }
        public Nullable<double> rates_hcm_single { get; set; }
        public Nullable<double> rates_hcm_multi { get; set; }
        public Nullable<double> rates_hcm_monthly { get; set; }
        public Nullable<double> rates_four_six_axle_single { get; set; }
        public Nullable<double> rates_four_six_axle_multi { get; set; }
        public Nullable<double> rates_four_six_axle_monthly { get; set; }
        public Nullable<double> rates_seven_plus_axle_single { get; set; }
        public Nullable<double> rates_seven_plus_axle_multi { get; set; }
        public Nullable<double> rates_seven_plus_axle_monthly { get; set; }
    }
}