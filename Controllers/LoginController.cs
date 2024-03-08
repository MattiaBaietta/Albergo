using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Albergo.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        static string connectionString = ConfigurationManager.ConnectionStrings["Albergo"].ToString();
        SqlConnection conn = new SqlConnection(connectionString);
        // GET: Login
        public ActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("AlreadyLogged");
            }

            return View();
        }
        public ActionResult Login(string User, string Password)
        {

            conn.Open();
            var command = new SqlCommand($"SELECT * FROM Dipendenti WHERE [Password]='{Password}'and [Username]='{User}'", conn);
            var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                FormsAuthentication.SetAuthCookie(User, true);
            }
            else
            {
                TempData["login"] = false;

            }
            conn.Close();
            return RedirectToAction("Index");
        }
        public ActionResult AlreadyLogged()
        {
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}