using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using RazorPartialToString.Services;

namespace budoco.Pages
{
    public class RunSqlModel : PageModel
    {
        private readonly IRazorPartialToStringRenderer _renderer;

        public RunSqlModel(IRazorPartialToStringRenderer renderer)
        {
            _renderer = renderer;
        }

        [BindProperty]
        public string sql { get; set; }

        public DataTable dt;

        public string error;

        public void OnGet()
        {

        }

        public async Task<ContentResult> OnPostRunAsync()
        {
            if (!bd_util.check_user_permissions(HttpContext, bd_util.MUST_BE_ADMIN))
                return Content("<div>Must be admin</div>");

            if (bd_config.get("DebugEnableRunSql") == 0)
            {
                return Content("<div>DebugEnableRunSql: 0</div>");
            }
            dt = null;
            error = null;

            try
            {
                dt = bd_db.get_datatable(sql);
            }
            catch (Exception e)
            {

                if (e.Message == "Cannot find table 0.")
                {
                    // suppres this - this is just what happens when we run a query that does't SELECT 
                    // like an update   
                }
                else
                {
                    error = e.Message;
                }
            }

            String html = await _renderer.RenderPartialToStringAsync("_PlainDataTablePartial", this);
            return Content(html);
        }
    }
}
