using Albergo.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Albergo.Controllers
{
    public class ChiamateAsyncController : Controller
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["Albergo"].ToString();
        SqlConnection conn = new SqlConnection(connectionString);
        List<Clienti> clients = new List<Clienti>();

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult RicercaCF(string CFiscale)
        {
            conn.Open();
            var cmd = new SqlCommand($"Select * From Clienti where CFiscale='{CFiscale}'", conn);
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                var c = new Clienti()
                {
                    Nome = (string)reader["Nome"],
                    Cognome = (string)reader["Cognome"],
                    Provincia = (string)reader["Provincia"],
                    Mail = (string)reader["Mail"],
                    Telefono = (int)reader["Telefono"],
                    Cel = (int)reader["Cel"]
                };
                clients.Add(c);

            }
            conn.Close();
            return Json(clients, JsonRequestBehavior.AllowGet);
        }
        public JsonResult PensioneCompleta()
        {
            int count = 0;
            conn.Open();
            var cmd = new SqlCommand($"SELECT COUNT(*) AS NumeroPensioneCompleta FROM Prenotazioni WHERE TipoTariffa = 'Pensione Completa'", conn);
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                count = (int)reader["NumeroPensioneCompleta"];
            }
            conn.Close();
            return Json(count, JsonRequestBehavior.AllowGet);

        }
    }
}