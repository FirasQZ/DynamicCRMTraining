using log4net;
using RibbonAccountMVC.DAL;
using RibbonAccountMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RibbonAccountMVC.Controllers
{
    public class RibbonAccountController : Controller
    {
        private static readonly ILog log = LogHelper.GetLogger();


        // GET: RibbonAccount
        public ActionResult Index()
        {
            return View();
        }

        // GET: RibbonAccount/Details/5
        public ActionResult Details(string id)
        {
            try
            {
                DAL_AccountEntity accountEntity = new DAL_AccountEntity();
                List<AccountEntityModels> accountInfo = accountEntity.RetriveRecords(id);
                AccountEntityModels obj = null;
                if (accountInfo != null)
                {
                    obj = accountInfo.FirstOrDefault();
                    ViewBag.AccountStatus = obj.AccountStatus;
                    log.Info("success retrive View");
                    return View("RibbonAccountView", obj);
                }
                log.Info("exception error when retrive View");
                return View("Error");
            }
            catch (Exception ex)
            {
                log.Info("exception error when retrive View");
                return View("Error");
            }
        }

        // GET: RibbonAccount/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RibbonAccount/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: RibbonAccount/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RibbonAccount/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: RibbonAccount/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RibbonAccount/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
