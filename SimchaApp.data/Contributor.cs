using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimchaApp.data
{
   public class Contributor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Cell { get; set; }
        public bool AlwaysInclude { get; set; }
        public int? Balance { get; set; }
        public decimal LastContributionAmount { get; set; }

    }
}
