using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimchaApp.data;
using SimchaApp.web.Models;

namespace SimchaApp.web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            DB db = new DB(Properties.Settings.Default.ConnectionString);
            SimchaListModal SLM = new SimchaListModal();
            SLM.AllSimchas = db.GetAllSimchas();

            if (TempData["NewSimchadded"] != null)
            {
                SLM.TempDataMessage = (string)TempData["NewSimchadded"];
            }
           
            return View(SLM);
        }

        public ActionResult NewSimcha(Simcha simcha)
        {
            DB db = new DB(Properties.Settings.Default.ConnectionString);
            db.AddNewSimcha(simcha);

            TempData["NewSimchadded"] = $"{simcha.Name} Simcha was added";

            return (Redirect("/"));
        }
        public ActionResult contributors()
        {
            ContributorViewModel CVM = new ContributorViewModel();
            DB db = new DB(Properties.Settings.Default.ConnectionString);
            CVM.AllContributors = db.GetAllContributors();
            CVM.TotalInFund = db.GetTotalCurrentlyInFund();
            if (TempData["Deposit"] != null)
            {
                CVM.TempDataMessage = (string)TempData["Deposit"];
            }
       else     if (TempData["DepositAndContributor"] != null)
            {
                CVM.TempDataMessage = (string)TempData["DepositAndContributor"];
            }
            else if (TempData["Contributor"] != null)
            {
                CVM.TempDataMessage = (string)TempData["Contributor"];
            }
            return View(CVM);
        }
        public ActionResult Deposit(Deposit deposit)
        {
            DB db = new DB(Properties.Settings.Default.ConnectionString);
            db.AddDeposit(deposit);
            TempData["Deposit"] = "Deposit has been added";
            return (Redirect("/Home/contributors"));
        }

        public ActionResult AddContributor(Contributor C, Deposit D)
        {
            C.Name = C.Name + " " + C.LastName;
            DB db = new DB(Properties.Settings.Default.ConnectionString);
            db.AddContributor(C);
            if(D.Amount != 0)
            {
                D.ContributorsId = C.Id;
                db.AddDeposit(D);
                TempData["DepositAndContributor"] = "New Contributor and Deposit has been added";
                return (Redirect("/Home/contributors"));
            }

            TempData["Contributor"] = "New Contributor has been added";

            return (Redirect("/Home/contributors"));
        }
        public ActionResult History (int id)
        {
            DB db = new DB(Properties.Settings.Default.ConnectionString);
            HistoryViewModelView HVM = new HistoryViewModelView();
            HVM.Contributor = db.GetContributorById(id);
            HVM.Transactions = db.ShowHistoryByContributorID(id);

            return View(HVM);       
        }

        public ActionResult Contributions(int id)
        {
            DB db = new DB(Properties.Settings.Default.ConnectionString);
            ListOfContributorsForSimchaModel SimchaList = new ListOfContributorsForSimchaModel();
            SimchaList.Simcha = db.GetSimchaByID(id);
            SimchaList.Names = db.GetContributorsForSimchaList(id);
            return View(SimchaList);

        }
        public ActionResult Simcha(int id)
        {
            DB db = new DB(Properties.Settings.Default.ConnectionString);
            ContributorsForSimchas ContributorsList = new ContributorsForSimchas();
            ContributorsList.Simcha = db.GetSimchaByID(id);
            ContributorsList.List = db.ContributorsListForSImcha(id);

            return View(ContributorsList);
               
        }
        public ActionResult PledgesForSimcha(List<Contribution> ContributorList)
        {
            int x = ContributorList[0].SimchaId;
            List<Contribution> list = new List<Contribution>();
            
            foreach (Contribution C in ContributorList)
            {
                if (C.Contribute == true)
                {
                    list.Add(C);
                }
            }
            DB db = new DB(Properties.Settings.Default.ConnectionString);
            db.PostcontributionsForSimcha(list);
            return (Redirect($"/Home/Simcha?id={list[0].SimchaId}"));
        }

    }
}