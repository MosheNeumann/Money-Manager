using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimchaApp.data;
namespace SimchaApp.web.Models
{
    public class ListOfContributorsForSimchaModel
    {
        public  Simcha Simcha {get; set;}
        public List<string> Names { get; set; }
    }
}