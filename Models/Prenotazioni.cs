using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Albergo.Models
{
    public class Prenotazioni
    {
        public DateTime DataPrenotazione { get; set; }
        public DateTime Inizio { get; set; }
        public DateTime Fine { get; set; }
        public int Caparra { get; set; }
        public string TipoTariffa { get; set; }
        public int IdCamera { get; set; }
        public int IdPrenotazione { get; set; }
    }
}