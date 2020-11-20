﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace budoco.Pages
{
    public class AdminModel : PageModel
    {
        public void OnGet()
        {

            bd_util.redirect_if_not_logged_in(HttpContext);

            var is_admin = HttpContext.Session.GetInt32("us_is_admin");

        }
    }
}
