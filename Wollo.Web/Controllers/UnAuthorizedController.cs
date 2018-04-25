using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Wollo.Web.Helper;
using Wollo.Web.Controllers.Helper;


namespace Wollo.Web.Controllers
{
    public class UnAuthorizedController : BaseController
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
        // GET: /UnAuthorized/
        public ActionResult Index()
        {
            return View();
        }
	}
}