using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace AppWeb.Filtros
{
    public class Miemb : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string rol = context.HttpContext.Session.GetString("rol");
            if(string.IsNullOrEmpty(rol))
            {
                context.Result = new RedirectResult("Usuario/Index");
            }
            if(rol != "miembro")
            {
                context.Result = new RedirectResult("/Usuario/Index");
            }
        }
    }
}
