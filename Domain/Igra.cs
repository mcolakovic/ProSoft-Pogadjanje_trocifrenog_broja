using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    [Serializable]
    public class Igra
    {
        public int Pozicija { get; set; }
        public int Vrijednost { get; set; }
        public List<int?> Polja { get; set; }
    }
}
