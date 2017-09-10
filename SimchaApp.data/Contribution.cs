using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimchaApp.data
{
    public class Contribution
    {
       
        public string Name { get; set; }
        public int   ContributersId { get; set; }
        public int SimchaId { get; set; }
        public int Amount { get; set; }
        public bool Contribute { get; set; }
        public DateTime? date { get; set; }
        
    }
}
