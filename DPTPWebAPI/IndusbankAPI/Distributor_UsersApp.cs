using DPTPWebAPI.Controllers;
using IndusAPIMiddleWareAPI.Helperclasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Web;
using System.Web.Helpers;
using WebApplicationTP.DAL;

namespace DPTPWebAPI.IndusbankAPI
{
    public class Distributor_UsersApp
    {
        DP_TPEntities db = new DP_TPEntities();
        Notifications ns = new Notifications();
        public int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }
        public int createTollPayCustomer(IndusInd_CustomerRegistration icr)
        {

            User u = new User();
            u.password = Convert.ToString(GenerateRandomNo());
            u.FirstName = icr.FirstName;
            u.LastName = icr.LastName;
            u.userRollstatus = "C";
            u.mobno1 = icr.MobileNumber;
            u.userEmail = icr.EmailID;
            u.address = "FN=" + icr.FirstName + "|LN=" + icr.LastName + "|PAN=" + icr.PANNo + "|AadharNo=" + icr.Aadhaarno;
            u.status = "a";
            u.username = icr.MobileNumber;

            User ue = db.Users.Where(i => i.username == u.username).FirstOrDefault();
            if (ue == null)
            {
                db.Users.Add(u);
                db.SaveChanges();
            }
            else
            {
                ue.password = u.password;
                ue.userEmail = u.userEmail;
                ue.FirstName = icr.FirstName;
                ue.LastName = icr.LastName;
                ue.mobno1 = u.mobno1;
                ue.mobno2 = u.mobno2;
                ue.userRollstatus = "C";
                db.SaveChanges();

            }
         
            int custno = db.Users.Where(i => i.username == u.username).FirstOrDefault().srno;

            //er.ecom_CustomerId = custno;
            //db.ecom_RFID.Add(er);
            //db.SaveChanges();

            return custno;
        }


        public void distributorFasttag(ResposeTagRootobject obj, ClsTollPayTagRegistraion ObjTPTagReg)
        {

            //check if tagsrno alredy exist update this record
            //IndusInd_TagRegistration itr=db.IndusInd_TagRegistration.Where(t=>t.)



            db.IndusInd_TagRegistration.Add(new IndusInd_TagRegistration()
            {
                DistributorId = Convert.ToInt32(ObjTPTagReg.DistributorId),
                CustomerAccountType = ObjTPTagReg.AccountType,
                CustomerMobileNo = ObjTPTagReg.MobileNo,
                CustomerOrderNo = ObjTPTagReg.OrderNo,
                CustomerPaymentAmount = Convert.ToDecimal(ObjTPTagReg.TagTotalAmount),
                //CustomerPaymentDetails= ObjTPTagReg.
                CustomerPaymentType = ObjTPTagReg.PaymentType,
                CustomerShippingAddress = JsonConvert.SerializeObject(ObjTPTagReg.ShippingAddress),
                CustomerVehicleTagDetails = JsonConvert.SerializeObject(ObjTPTagReg.VehicleInfo),
                OrderStatus = obj.StatusCode,
                OrderDateTime = DateTime.Now,
                BankStatus = obj.Status,
                


            }) ;
            db.SaveChanges();

          
            foreach (var vehicle in obj.VehicleInfo)
            {
                if (vehicle.StatusCode == "000")
                {
                    ecom_RFID er = db.ecom_RFID.Where(o => o.Serial_Number == vehicle.SerialNo).FirstOrDefault();
                    er.ecom_CustomerVehicleNo = vehicle.VechileNo;
                    er.ecom_CustomerId = db.Users.Where(o => o.username == obj.MobileNo).FirstOrDefault().srno;
                    er.ecom_CustomerMobNo = obj.MobileNo;
                    er.ecom_axis_WalletID = ObjTPTagReg.MobileNo;
                    er.ecom_CustomerFName = obj.OrderNo;
                    er.ecom_CreatedDate = DateTime.Now;
                    er.BankStatus = obj.Status;
                    if (ObjTPTagReg.DistributorId != null)
                    {
                        er.ecom_DistributionID = Convert.ToInt32(ObjTPTagReg.DistributorId);
                    }
                    er.ecom_isAllocated = true;
                    er.ecom_RFIDStatus = "a";
                    db.SaveChanges();

                }
                else
                {
                    ecom_RFID er = db.ecom_RFID.Where(o => o.Serial_Number == vehicle.SerialNo).FirstOrDefault();
                    er.ecom_CustomerVehicleNo = vehicle.VechileNo;
                    er.ecom_CustomerId = db.Users.Where(o => o.username == obj.MobileNo).FirstOrDefault().srno;
                    er.ecom_CustomerMobNo = obj.MobileNo;
                    er.ecom_axis_WalletID = ObjTPTagReg.MobileNo;
                    er.ecom_CustomerLName = obj.OrderNo;
                    er.ecom_CreatedDate = DateTime.Now;
                    er.BankStatus = obj.Status;
                    if (ObjTPTagReg.DistributorId != null)
                    {
                        er.ecom_DistributionID = Convert.ToInt32(ObjTPTagReg.DistributorId);
                    }
                    er.ecom_isAllocated = true;
                    er.ecom_RFIDStatus = vehicle.StatusCode;
                    db.SaveChanges();
                }

            }



        }
    }
}