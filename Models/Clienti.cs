using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace Albergo.Models
{
    public class Clienti
    {
        public string CFiscale { get; set; }
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public string Provincia { get; set; }
        public string Mail { get; set; }
        public int Telefono { get; set; }
        public int Cel { get; set; }

    }
}