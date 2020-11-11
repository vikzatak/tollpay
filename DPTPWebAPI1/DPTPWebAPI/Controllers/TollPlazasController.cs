using DPTPWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DPTPWebAPI.Controllers
{
    public class Tollcal
    {
        public List<TollPlazaRateByVehicleTypes> tollplaza { get; set; }
        public double totalcost { get; set; }

    }
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TollPlazasController : ApiController
    {

        private DP_TPEntities db = new DP_TPEntities();
              
        [AllowAnonymous]
        public Tollcal GetTollPlaza(string source, string destination, string vehicletype, string journey)
        {
            bool flag = false;
            Tollcal response = new Tollcal();
            if (flag)
            {
                #region Sagar Code
                WebClient wb = new WebClient();
                string strurl = "http://tis.nhai.gov.in/UploadHandler.ashx?Up=3&Source=" + source + "&Destination=" + destination;

                string result = wb.DownloadString(strurl);
                //18.3313849,73.8525197$17.810312,73.971381$$113 km!2 hours 13 mins
                List<string> tollplazza = result.Split('$').ToList();
                DP_TPEntities db = new DP_TPEntities();
                List<TollPlaza> lsttollinrounte = new List<TollPlaza>();
                User_Trips ut = new User_Trips();
                ut.tripStartLocation = source;
                ut.tripEndLocation = destination;
                Random random = new Random();
                int numb = random.Next(1, 31) % 31;
                if (numb != 0)
                {
                    ut.tripDate = new DateTime(2019, 8, numb);
                    ut.tripStatus = "a";
                    ut.userid = 1;
                }
                foreach (var item in tollplazza)
                {
                    if (item.Contains(','))
                    {
                        List<string> latlng = item.Split(',').ToList();
                        double dlat = Convert.ToDouble(latlng[0]);
                        lsttollinrounte.Add(db.TollPlazas.Where(l => l.lat == dlat).FirstOrDefault());
                    }


                }
                dynamic orderTotals = 0.0;
                //let orderTotals;
                //var orderTotals;
                List<TollPlazaRateByVehicleTypes> LstTPRBV = new List<TollPlazaRateByVehicleTypes>();

                switch (vehicletype)
                {
                    case "Bus":
                        orderTotals = lsttollinrounte.GroupBy(i => 1).Select(g => g.Sum(item => item.rates_bus_multi)).FirstOrDefault();
                        LstTPRBV = lsttollinrounte.Select(x => new TollPlazaRateByVehicleTypes() { id = x.id, lat = x.lat, lon = x.lon, Rate = x.rates_bus_multi }).ToList();
                        break;
                    case "car":
                        if (journey == "multi")
                        {
                            orderTotals = lsttollinrounte.GroupBy(i => 1).Select(g => g.Sum(item => item.rates_car_multi)).FirstOrDefault();
                            LstTPRBV = lsttollinrounte.Select(x => new TollPlazaRateByVehicleTypes() { id = x.id, lat = x.lat, lon = x.lon, Rate = x.rates_car_multi }).ToList();
                        }
                        else
                        {
                            orderTotals = lsttollinrounte.GroupBy(i => 1).Select(g => g.Sum(item => Convert.ToDecimal(item.rates_car_single))).FirstOrDefault();
                            LstTPRBV = lsttollinrounte.Select(x => new TollPlazaRateByVehicleTypes() { id = x.id, lat = x.lat, lon = x.lon, Rate = x.rates_car_single }).ToList();
                        }
                        break;
                    case "sixaxle":
                        orderTotals = lsttollinrounte.GroupBy(i => 1).Select(g => g.Sum(item => item.rates_four_six_axle_multi)).FirstOrDefault();
                        LstTPRBV = lsttollinrounte.Select(x => new TollPlazaRateByVehicleTypes() { id = x.id, lat = x.lat, lon = x.lon, Rate = x.rates_four_six_axle_multi }).ToList();
                        break;
                    case "hcm":
                        orderTotals = lsttollinrounte.GroupBy(i => 1).Select(g => g.Sum(item => item.rates_hcm_multi)).FirstOrDefault();
                        LstTPRBV = lsttollinrounte.Select(x => new TollPlazaRateByVehicleTypes() { id = x.id, lat = x.lat, lon = x.lon, Rate = x.rates_hcm_multi }).ToList();
                        break;
                    case "lcv":
                        if (journey == "multi")
                        {
                            orderTotals = lsttollinrounte.GroupBy(i => 1).Select(g => g.Sum(item => item.rates_lcv_multi)).FirstOrDefault();
                            LstTPRBV = lsttollinrounte.Select(x => new TollPlazaRateByVehicleTypes() { id = x.id, lat = x.lat, lon = x.lon, Rate = x.rates_lcv_multi }).ToList();
                        }
                        else
                        {
                            orderTotals = lsttollinrounte.GroupBy(i => 1).Select(g => g.Sum(item => Convert.ToDecimal(item.rates_lcv_single))).FirstOrDefault();
                            LstTPRBV = lsttollinrounte.Select(x => new TollPlazaRateByVehicleTypes() { id = x.id, lat = x.lat, lon = x.lon, Rate = x.rates_lcv_single }).ToList();
                        }

                        break;
                    case "multaxle":
                        orderTotals = lsttollinrounte.GroupBy(i => 1).Select(g => g.Sum(item => item.rates_multiaxle_multi)).FirstOrDefault();
                        LstTPRBV = lsttollinrounte.Select(x => new TollPlazaRateByVehicleTypes() { id = x.id, lat = x.lat, lon = x.lon, Rate = x.rates_multiaxle_multi }).ToList();
                        break;
                    case "sevenaxle":
                        orderTotals = lsttollinrounte.GroupBy(i => 1).Select(g => g.Sum(item => item.rates_seven_plus_axle_multi)).FirstOrDefault();
                        LstTPRBV = lsttollinrounte.Select(x => new TollPlazaRateByVehicleTypes() { id = x.id, lat = x.lat, lon = x.lon, Rate = x.rates_seven_plus_axle_multi }).ToList();
                        break;
                    default:
                        Console.WriteLine("Default case");
                        break;
                }

                // Tollcal response = new Tollcal();

                response.tollplaza = LstTPRBV;
                response.totalcost = (double)orderTotals;
                return response;
                #endregion
            }
            else
            {
                string src = source;
                string dest = destination;
                string vt = vehicletype;
                string tt = journey;
                //TollPlazaServiceSoapClient proxy = new TollPlazaServiceSoapClient();
                //List<string> names= proxy.AutoCompleteLocation("pu").ToList();
                WebClient wb = new WebClient();
                string result = string.Empty;
                try
                {

                    result = db.RouteTollsQueries.Where(r => r.src.Contains(src) && r.dest.Contains(dest)).FirstOrDefault().routetolls;

                }
                catch (Exception ex)
                {
                    if (result == string.Empty)
                    {
                        string strurl = "http://tis.nhai.gov.in/UploadHandler.ashx?Up=3&Source=" + src + "&Destination=" + dest;
                        result = wb.DownloadString(strurl);
                        db.RouteTollsQueries.Add(new RouteTollsQuery() { src = src, dest = dest, routeactive = "a", routetolls = result });
                        db.SaveChanges();

                    }

                }


                List<getTripPrices_Result> lsttollplazz = db.getTripPrices(result).ToList();

                List<TollPlazaRateByVehicleTypes> LstTPRBV = new List<TollPlazaRateByVehicleTypes>();

                switch (vt)
                {
                    case "Bus":

                        LstTPRBV = lsttollplazz.Select(x => new TollPlazaRateByVehicleTypes() { id = x.id, name = x.name, lat = x.lat, lon = x.lon, Rate = x.rates_bus_multi }).ToList();
                        break;
                    case "car":
                        if (journey == "multi")
                        {

                            LstTPRBV = lsttollplazz.Select(x => new TollPlazaRateByVehicleTypes() { id = x.id, name = x.name, lat = x.lat, lon = x.lon, Rate = x.rates_car_multi }).ToList();
                        }
                        else
                        {

                            LstTPRBV = lsttollplazz.Select(x => new TollPlazaRateByVehicleTypes() { id = x.id, name = x.name, lat = x.lat, lon = x.lon, Rate = x.rates_car_single }).ToList();
                        }
                        break;
                    case "sixaxle":

                        LstTPRBV = lsttollplazz.Select(x => new TollPlazaRateByVehicleTypes() { id = x.id, name = x.name, lat = x.lat, lon = x.lon, Rate = x.rates_four_six_axle_multi }).ToList();
                        break;
                    case "hcm":

                        LstTPRBV = lsttollplazz.Select(x => new TollPlazaRateByVehicleTypes() { id = x.id, name = x.name, lat = x.lat, lon = x.lon, Rate = x.rates_hcm_multi }).ToList();
                        break;
                    case "lcv":
                        if (journey == "multi")
                        {

                            LstTPRBV = lsttollplazz.Select(x => new TollPlazaRateByVehicleTypes() { id = x.id, name = x.name, lat = x.lat, lon = x.lon, Rate = x.rates_lcv_multi }).ToList();
                        }
                        else
                        {

                            LstTPRBV = lsttollplazz.Select(x => new TollPlazaRateByVehicleTypes() { id = x.id, name = x.name, lat = x.lat, lon = x.lon, Rate = x.rates_lcv_single }).ToList();
                        }

                        break;
                    case "multaxle":

                        LstTPRBV = lsttollplazz.Select(x => new TollPlazaRateByVehicleTypes() { id = x.id, name = x.name, lat = x.lat, lon = x.lon, Rate = x.rates_multiaxle_multi }).ToList();
                        break;
                    case "sevenaxle":

                        LstTPRBV = lsttollplazz.Select(x => new TollPlazaRateByVehicleTypes() { id = x.id, name = x.name, lat = x.lat, lon = x.lon, Rate = x.rates_seven_plus_axle_multi }).ToList();
                        break;
                    default:
                        Console.WriteLine("Default case");
                        break;
                }

                List<TollRateCard> orderTotals = lsttollplazz
                       .GroupBy(i => 1)
                       .Select(g => new TollRateCard
                       {
                           cnt = g.Count(),
                           rates_car_single = g.Sum(item => item.rates_car_single),
                           rates_car_multi = g.Sum(item => item.rates_car_multi),
                           rates_car_monthly = g.Sum(item => item.rates_car_monthly),
                           rates_bus_multi = g.Sum(item => item.rates_bus_multi),
                           rates_bus_monthly = g.Sum(item => item.rates_bus_monthly),
                           rates_four_six_axle_single = g.Sum(item => item.rates_four_six_axle_single),
                           rates_four_six_axle_multi = g.Sum(item => item.rates_four_six_axle_multi),
                           rates_four_six_axle_monthly = g.Sum(item => item.rates_four_six_axle_monthly),
                           rates_seven_plus_axle_single = g.Sum(item => item.rates_seven_plus_axle_single),
                           rates_seven_plus_axle_multi = g.Sum(item => item.rates_seven_plus_axle_multi),
                           rates_seven_plus_axle_monthly = g.Sum(item => item.rates_seven_plus_axle_monthly),
                           rates_hcm_single = g.Sum(item => item.rates_hcm_single),
                           rates_hcm_multi = g.Sum(item => item.rates_hcm_multi),
                           rates_hcm_monthly = g.Sum(item => item.rates_hcm_monthly),
                           rates_lcv_single = g.Sum(item => item.rates_lcv_single),
                           rates_lcv_multi = g.Sum(item => item.rates_lcv_multi),
                           rates_lcv_monthly = g.Sum(item => item.rates_lcv_monthly),
                           rates_multiaxle_single = g.Sum(item => item.rates_multiaxle_single),
                           rates_multiaxle_multi = g.Sum(item => item.rates_multiaxle_multi),
                           rates_multiaxle_monthly = g.Sum(item => item.rates_multiaxle_monthly)



                       }).ToList();


                double? tollamt = 0;
                switch (vt)
                {

                    case "car":
                        if (tt.ToLower() == "single")
                        {
                            tollamt = orderTotals[0].rates_car_single;
                        }
                        else if (tt.ToLower() == "multi")
                        { tollamt = orderTotals[0].rates_car_multi; }
                        else
                        { tollamt = orderTotals[0].rates_car_monthly; }
                        break;

                    case "lcv":
                        if (tt.ToLower() == "single")
                        { tollamt = orderTotals[0].rates_lcv_single; }
                        else if (tt.ToLower() == "multi")
                        { tollamt = orderTotals[0].rates_lcv_multi; }
                        else
                        { tollamt = orderTotals[0].rates_lcv_monthly; }
                        break;
                    case "bus":
                        if (tt.ToLower() == "single")
                        { //tollamt = orderTotals[0].rates_bus_single;
                        }
                        else if (tt.ToLower() == "multi")
                        { tollamt = orderTotals[0].rates_bus_multi; }
                        else
                        { tollamt = orderTotals[0].rates_bus_monthly; }
                        break;
                    case "multiaxle":
                        if (tt.ToLower() == "single")
                        { tollamt = orderTotals[0].rates_multiaxle_single; }
                        else if (tt.ToLower() == "multi")
                        { tollamt = orderTotals[0].rates_multiaxle_multi; }
                        else
                        { tollamt = orderTotals[0].rates_multiaxle_monthly; }
                        break;
                    case "hcm":
                        if (tt.ToLower() == "single")
                        { tollamt = orderTotals[0].rates_hcm_single; }
                        else if (tt.ToLower() == "multi")
                        { tollamt = orderTotals[0].rates_hcm_multi; }
                        else
                        { tollamt = orderTotals[0].rates_hcm_monthly; }
                        break;
                    case "four_six_axle":
                        if (tt.ToLower() == "single")
                        { tollamt = orderTotals[0].rates_four_six_axle_single; }
                        else if (tt.ToLower() == "multi")
                        { tollamt = orderTotals[0].rates_four_six_axle_multi; }
                        else
                        { tollamt = orderTotals[0].rates_four_six_axle_monthly; }
                        break;
                    case "seven_plus_axle":
                        if (tt.ToLower() == "single")
                        { tollamt = orderTotals[0].rates_seven_plus_axle_single; }
                        else if (tt.ToLower() == "multi")
                        { tollamt = orderTotals[0].rates_seven_plus_axle_multi; }
                        else
                        { tollamt = orderTotals[0].rates_seven_plus_axle_monthly; }
                        break;
                    default: break;
                }


                List<TollPlaza> lsttp = new List<TollPlaza>();
                foreach (var item in lsttollplazz)
                {
                    TollPlaza tp = new TollPlaza();
                    tp.id = item.id;
                    tp.name = item.name;
                    tp.lat = item.lat;
                    tp.lon = item.lon;
                    lsttp.Add(tp);
                    //response.tollplaza.Add(tp);

                    #region mapcode
                    //public string name { get; set; }
                    //public Nullable<double> lat { get; set; }
                    //public Nullable<double> lon { get; set; }
                    //public Nullable<System.DateTime> fee_effective_date { get; set; }
                    //public Nullable<double> rates_car_single { get; set; }
                    //public Nullable<double> rates_car_multi { get; set; }
                    //public Nullable<double> rates_car_monthly { get; set; }
                    //public Nullable<double> rates_lcv_single { get; set; }
                    //public Nullable<double> rates_lcv_multi { get; set; }
                    //public Nullable<double> rates_lcv_monthly { get; set; }
                    //public Nullable<double> rates_bus_multi { get; set; }
                    //public Nullable<double> rates_bus_monthly { get; set; }
                    //public Nullable<double> rates_multiaxle_single { get; set; }
                    //public Nullable<double> rates_multiaxle_multi { get; set; }
                    //public Nullable<double> rates_multiaxle_monthly { get; set; }
                    //public Nullable<double> rates_hcm_single { get; set; }
                    //public Nullable<double> rates_hcm_multi { get; set; }
                    //public Nullable<double> rates_hcm_monthly { get; set; }
                    //public Nullable<double> rates_four_six_axle_single { get; set; }
                    //public Nullable<double> rates_four_six_axle_multi { get; set; }
                    //public Nullable<double> rates_four_six_axle_monthly { get; set; }
                    //public Nullable<double> rates_seven_plus_axle_single { get; set; }
                    //public Nullable<double> rates_seven_plus_axle_multi { get; set; }
                    //public Nullable<double> rates_seven_plus_axle_monthly { get; set; }

                    #endregion



                }
                response.tollplaza = LstTPRBV;
                response.totalcost = (double)tollamt;
                return response;

            }

        }


        [JwtAuthentication]
        [ResponseType(typeof(TollPlaza))]
        [Route("api\\GetTollPlazaByID")]
        public IHttpActionResult GetTollPlaza(int id)
        {
            TollPlaza tollPlaza = db.TollPlazas.Find(id);
            if (tollPlaza == null)
            {
                return NotFound();
            }

            return Ok(tollPlaza);
        }

        [JwtAuthentication]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTollPlaza(int id, TollPlaza tollPlaza)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tollPlaza.id)
            {
                return BadRequest();
            }

            db.Entry(tollPlaza).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TollPlazaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
       
        [JwtAuthentication]
        // POST: api/TollPlazas
        [ResponseType(typeof(TollPlaza))]
        public IHttpActionResult PostTollPlaza(TollPlaza tollPlaza)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TollPlazas.Add(tollPlaza);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tollPlaza.id }, tollPlaza);
        }
     
        [JwtAuthentication]
        // DELETE: api/TollPlazas/5
        [ResponseType(typeof(TollPlaza))]
        public IHttpActionResult DeleteTollPlaza(int id)
        {
            TollPlaza tollPlaza = db.TollPlazas.Find(id);
            if (tollPlaza == null)
            {
                return NotFound();
            }

            db.TollPlazas.Remove(tollPlaza);
            db.SaveChanges();

            return Ok(tollPlaza);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TollPlazaExists(int id)
        {
            return db.TollPlazas.Count(e => e.id == id) > 0;
        }
    }
}