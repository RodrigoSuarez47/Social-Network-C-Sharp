using Dominio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace AppWeb.Filtros
{
    //Solo lo arme, lo dejo todo a tu criterio pablo jajaja, no le encuentro demasiada utilidad a esto
    public class Admin : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string rol = context.HttpContext.Session.GetString("rol");
            if (string.IsNullOrEmpty(rol))
            {
                context.Result = new RedirectResult("Usuario/Index");
            }
            else if (rol != "administrador")
            {
                context.Result = new RedirectResult("/Usuario/Index");
            }
        }
    }
}
