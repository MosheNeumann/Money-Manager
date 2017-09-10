using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimchaApp.data;

namespace SimchaApp.web.Models
{
    public class ContributorViewModel
    {
        public List<Contributor> AllContributors { get; set; }
        public int TotalInFund { get; set; }
        public string TempDataMessage { get; set; }

    }
}