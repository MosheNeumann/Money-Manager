using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimchaApp.data
{
  public  class Deposit
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; } 
        public int ContributorsId { get; set; }
    }
}
