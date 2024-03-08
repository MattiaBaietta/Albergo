using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Albergo.Models
{
    public class Checkout
    {
        public int IdPrenotazione { get; set; }
        public DateTime Inizio { get; set; }
        public DateTime Fine { get; set; }
        public string TipoTariffa { get; set; }
        public int IdCamera { get; set; }
        public string TipoServizio { get; set; }
        public int Prezzo { get; set; }
        public int Quant { get; set; }

    }
}