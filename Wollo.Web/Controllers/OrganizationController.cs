using Wollo.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wollo.Web.Helper;
using System.Threading.Tasks;

namespace Wollo.Web.Controllers
{
    public class OrganizationController : Controller
    {
        /// <summary>
        /// Change Language
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> ChangeCurrentCulture(int id)
        {
            await Task.Yield();
            //  
            // Change the current culture for this user.  
            //  
            CultureHelper.CurrentCulture = id;
            //  
            // Cache the new current culture into the user HTTP session.   
            //  
            Session["CurrentCulture"] = id;
            //  
            // Redirect to the same page from where the request was made!   
            //  
            return Redirect(Request.UrlReferrer.ToString());
        }

        //
        // GET: /Organization/
        [AuthorizeRole(Module = "Organization", Permission = "View")]
        public ActionResult Index()
        {
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    List<organization> org = new List<organization>();
                    if (User.IsInRole("Super Admin 1") || User.IsInRole("Super Admin 2"))
                    {
                        org = db.Organization.ToList();
                    }
                    else
                    {
                        long orgId = Convert.ToInt64(User.Identity.GetOrganizationId());
                        org = db.Organization.Where(x => x.Id == orgId).ToList();
                    }
                    return View(org);
                }
            }
            catch (Exception e)
            {

            }
            return View();
        }

        [HttpGet]
        [AuthorizeRole(Module = "Organization", Permission = "Update")]
        public ActionResult Edit(long Id)
        {
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    organization org = db.Organization.Find(Id);
                    return View(org);
                }
            }
            catch (Exception e)
            {

            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole(Module = "Organization", Permission = "Update")]
        public ActionResult Edit(organization orgModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        db.Entry(orgModel).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception e)
                {

                }
            }
            return View(orgModel);
        }

        [AuthorizeRole(Module = "Organization", Permission = "View")]
        public ActionResult Details(long Id)
        {
            try
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    organization org = db.Organization.Find(Id);
                    return View(org);
                }
            }
            catch (Exception e)
            {

            }
            return RedirectToAction("Index");
        }
    }
}