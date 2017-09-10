using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimchaApp.data;

namespace SimchaApp.web.Models
{
    public class ContributorsForSimchas 
    {
        public Simcha Simcha { get; set; }
        public List<ContributorsForSimcha> List { get; set; }
        public int index =0;
    }
}