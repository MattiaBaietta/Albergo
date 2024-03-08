using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Albergo.Models
{
    public class Servizi
    {
        public int IdCamera { get; set; }
        public int IdServizio { get; set; }
        public int Quant { get; set; }
        public DateTime Data { get; set; }
        public string TipoServizio { get; set; }
        public int Prezzo { get; set; }

    }
}