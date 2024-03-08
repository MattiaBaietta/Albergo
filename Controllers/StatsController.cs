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
    public class StatsController : Controller
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["Albergo"].ToString();
        SqlConnection conn = new SqlConnection(connectionString);

        List<string> checkouts = new List<string>();
        List<int> prenotazioni = new List<int>();
        // GET: Stats
        public ActionResult Index()
        {
            conn.Open();
            var cmd = new SqlCommand("Select * From Prenotazioni", conn);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                prenotazioni.Add(reader.GetInt32(0));
            }
            ViewBag.prenotazioni = prenotazioni;
            conn.Close();
            return View();
        }

        public ActionResult SelStat(int IdPrenotazione)
        {
            conn.Open();
            var cmd = new SqlCommand("SELECT * FROM Prenotazioni " +
                                     $"INNER JOIN Servizi_Prenotazioni " +
                                        $"ON Prenotazioni.IdPrenotazione=Servizi_Prenotazioni.IdPrenotazione " +
                                        $"INNER JOIN Servizi " +
                                        $"ON Servizi_Prenotazioni.IdServizio=Servizi.IdServizio " +
                                        $"WHERE Prenotazioni.IdPrenotazione={IdPrenotazione}", conn);
            var reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    checkouts.Add((string)reader["TipoServizio"]);
                    ViewBag.idprenotazione = (int)reader["IdPrenotazione"];
                    ViewBag.Inizio = (DateTime)reader["Inizio"];
                    ViewBag.Fine = (DateTime)reader["Fine"];
                    ViewBag.Tipo = (string)reader["TipoTariffa"];
                    ViewBag.TotPagamento = (int)reader["TotPagamento"];
                }
                ViewBag.servizi = checkouts;
                conn.Close();

            }
            else
            {
                conn.Close();
                conn.Open();
                var cmd2 = new SqlCommand($"Select * from Prenotazioni where IdPrenotazione={IdPrenotazione}", conn);
                var reader2 = cmd2.ExecuteReader();
                while (reader2.Read())
                {
                    ViewBag.idprenotazione = (int)reader2["IdPrenotazione"];
                    ViewBag.Inizio = (DateTime)reader2["Inizio"];
                    ViewBag.Fine = (DateTime)reader2["Fine"];
                    ViewBag.Tipo = (string)reader2["TipoTariffa"];
                    ViewBag.TotPagamento = (int)reader2["TotPagamento"];
                }
                conn.Close();
            }
            //idCamera inizio fine TipoTariffa Prenotazioni totpagamento
            //lista servizi aggiuntivi
            //importo da saldare


            var cmdPrenotazioni = new SqlCommand("Select * From Prenotazioni", conn);
            conn.Close();
            conn.Open();
            var readerPrenotazioni = cmdPrenotazioni.ExecuteReader();

            while (readerPrenotazioni.Read())
            {
                prenotazioni.Add(readerPrenotazioni.GetInt32(0));
            }
            ViewBag.prenotazioni = prenotazioni;
            conn.Close();

            return View("Index");

        }

    }
}