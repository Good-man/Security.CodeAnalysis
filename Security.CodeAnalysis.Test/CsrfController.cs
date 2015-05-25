using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SecurityWeaknesses.Web.Controllers
{
    public class CsrfController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        // POST: Default
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(FormCollection formCollection)
        {
            return View();
        }

        // POST: Default
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult PostWithoutAntiForgery(FormCollection formCollection)
        {
            return View();
        }

        internal void SomeOtherMethod()
        {
            // do something
        }
    }
}