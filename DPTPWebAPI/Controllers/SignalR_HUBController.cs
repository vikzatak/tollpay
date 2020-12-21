using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;

namespace DPTPWebAPI.Controllers
{
    public class SignalR_HUBController : Controller
    {
        // GET: SignalR_HUB
        public ActionResult Index()
        {
            return View();
        }
    }


    public class Global
    {
        public delegate void DelLogMessage(string data);
        public static DelLogMessage LogMessage;
    }

    public class Requestlog : Hub
    {
        public static void PostToClient(string data)
        {
            try
            {
                var chat = GlobalHost.ConnectionManager.GetHubContext("Requestlog");
                if (chat != null)
                    chat.Clients.All.postToClient(data);
            }
            catch
            {
            }
        }
    }




}