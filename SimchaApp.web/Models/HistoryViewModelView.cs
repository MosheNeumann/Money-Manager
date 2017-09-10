using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimchaApp.data;


namespace SimchaApp.web.Models
{
    public class HistoryViewModelView
    {
        public Contributor Contributor { get; set; }
        public List<Transaction> Transactions { get; set; }
      
    }
}