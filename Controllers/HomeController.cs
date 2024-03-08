using Albergo.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace Albergo.Controllers
{
    public class HomeController : Controller
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["Albergo"].ToString();
        SqlConnection conn = new SqlConnection(connectionString);

        List<Servizi> services = new List<Servizi>();
        List<Clienti> clients = new List<Clienti>();
        List<Prenotazioni> reserve = new List<Prenotazioni>();
        List<Camere> camera = new List<Camere>();
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Reservation(bool? isok)
        {

            conn.Open();
            var cmd = new SqlCommand("Select * From Camere", conn);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Camere camere = new Camere()
                {
                    IdCamera = (int)reader["IdCamera"],
                    Descrizione = (string)reader["Descrizione"],
                    Tipo = (string)reader["Tipo"],
                    Prezzo = (int)reader["Prezzo"]
                };
                camera.Add(camere);

            }
            ViewBag.Camere = camera;

            ViewBag.PrenotazioneEffettuata = isok;
            return View();
        }
        public ActionResult AddReservation(Clienti c, Prenotazioni p)
        {
            int totalepagamento = 0;
            DateTime now = DateTime.Now;
            conn.Open();
            var cmd3 = new SqlCommand("Select * From Camere", conn);
            var reader = cmd3.ExecuteReader();
            while (reader.Read())
            {
                if ((int)reader["IdCamera"] == p.IdCamera)
                {
                    totalepagamento += (int)reader["Prezzo"];
                }
            }
            conn.Close();


            switch (p.TipoTariffa)
            {
                case "Mezza Pensione":
                    totalepagamento += 50;
                    break;
                case "Pensione Completa":
                    totalepagamento += 100;
                    break;
                case "Pernottamento Colazione":
                    totalepagamento += 150;
                    break;
            }



            conn.Open();
            totalepagamento -= p.Caparra;
            var cmd = new SqlCommand("Insert Into Clienti " +
                "(CFiscale,Cognome,Nome,Provincia,Mail,Telefono,Cel)" +
                $"Values ('{c.CFiscale}','{c.Cognome}','{c.Nome}','{c.Provincia}','{c.Mail}',@Tel,@Cel) ; SELECT SCOPE_IDENTITY();", conn);
            cmd.Parameters.AddWithValue("@Tel", (object)c.Telefono ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Cel", (object)c.Cel ?? DBNull.Value);
            int nuovoId = Convert.ToInt32(cmd.ExecuteScalar());
            conn.Close();
            conn.Open();
            var cmd2 = new SqlCommand("Insert INTO PRENOTAZIONI" +
                " (DataPrenotazione,Anno,Inizio,Fine,Caparra,TipoTariffa,IdCamera,IdCliente,TotPagamento)" +
                $"Values (@DataAgg,'{now.Year}',@DataInizio,@DataFine,{p.Caparra},'{p.TipoTariffa}',{p.IdCamera},{nuovoId},{totalepagamento})", conn);
            cmd2.Parameters.AddWithValue("@DataInizio", p.Inizio.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd2.Parameters.AddWithValue("@DataFine", p.Fine.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd2.Parameters.AddWithValue("@DataAgg", now.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd2.ExecuteNonQuery();

            return RedirectToAction("Reservation", new { isok = true });
        }

        public ActionResult Services(bool? isOk)
        {
            conn.Open();
            var cmd = new SqlCommand("Select * from Servizi", conn);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var s = new Servizi()
                {
                    IdServizio = (int)reader["IdServizio"],
                    TipoServizio = (string)reader["TipoServizio"],
                    Prezzo = (int)reader["Prezzo"]
                };
                services.Add(s);
            }
            conn.Close();
            conn.Open();
            var cmd2 = new SqlCommand("Select * From Prenotazioni", conn);
            var reader2 = cmd2.ExecuteReader();
            while (reader2.Read())
            {
                var c = new Prenotazioni()
                {
                    IdPrenotazione = (int)reader2["IdPrenotazione"]
                };
                reserve.Add(c);

            }
            conn.Close();
            ViewBag.ListaCamere = reserve;
            ViewBag.ListaServizi = services;
            ViewBag.PrenotazioneEffettuata = isOk;
            return View();
        }
        public ActionResult AddServices(Servizi s)
        {
            DateTime now = DateTime.Now;
            int tempadd = 0;
            conn.Open();
            var cmd2 = new SqlCommand($@"SELECT  * FROM Servizi WHERE IdServizio = {s.IdServizio}", conn);
            var reader = cmd2.ExecuteReader();
            while (reader.Read())
            {
                tempadd = (int)reader["Prezzo"];
            }
            conn.Close();
            conn.Open();
            var cmd3 = new SqlCommand($"UPDATE Prenotazioni SET TotPagamento=TotPagamento+{tempadd} WHERE IdPrenotazione={s.IdCamera}", conn);
            cmd3.ExecuteNonQuery();
            conn.Close();
            conn.Open();
            var cmd = new SqlCommand("Insert into Servizi_Prenotazioni" +
                "(IdServizio,IdPrenotazione,Quant,Data)" +
                $"Values ({s.IdServizio},{s.IdCamera},{s.Quant},@DataAgg)", conn);
            cmd.Parameters.AddWithValue("@DataAgg", now.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("Services", new { isOk = true });

        }
    }
}