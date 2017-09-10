using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimchaApp.data
{
  public  class ContributorsForSimcha
    {
        public  int Id { get;set;}
        public string Name { get; set; }
        public bool AlwaysInclude { get; set; }

        public bool PledgedForSimcha { get; set; }
        public int Balance { get; set; }
        public int Amount { get; set; }
        public DateTime? Date { get; set; }
        }

    }

