using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimchaApp.data;

namespace SimchaApp.web.Models
{
    public class SimchaListModal
    {
        public List<Simcha>AllSimchas { get; set; }
        public int TotalContributors { get; set; }

        public string TempDataMessage { get; set; }
    }
}