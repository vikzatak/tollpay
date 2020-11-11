//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DPTPWebAPI
{
    using System;
    using System.Collections.Generic;
    
    public partial class TollPlaza
    {
        public int SrNo { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public Nullable<double> lat { get; set; }
        public Nullable<double> lon { get; set; }
        public Nullable<double> cumulative_revenue { get; set; }
        public string design_capacity { get; set; }
        public Nullable<System.DateTime> fee_effective_date { get; set; }
        public Nullable<double> rates_car_single { get; set; }
        public Nullable<double> rates_car_multi { get; set; }
        public Nullable<double> rates_car_monthly { get; set; }
        public Nullable<double> rates_lcv_single { get; set; }
        public Nullable<double> rates_lcv_multi { get; set; }
        public Nullable<double> rates_lcv_monthly { get; set; }
        public Nullable<double> rates_bus_single { get; set; }
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
        public string location { get; set; }
        public Nullable<double> capital_cost { get; set; }
        public string project_type { get; set; }
        public Nullable<System.DateTime> date_fee_notification { get; set; }
        public Nullable<System.DateTime> date_commercial_operation { get; set; }
        public string fee_rule { get; set; }
        public string concessions_period { get; set; }
        public Nullable<double> traffic_per_day { get; set; }
        public Nullable<double> target_traffic_per_day { get; set; }
        public string contractor_name { get; set; }
        public string contact_details { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
