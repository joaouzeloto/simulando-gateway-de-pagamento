using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace XXXXX
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private string PolicyName { get; set; }

        public AuthorizeAttribute(string policyName) :base()
        {
            PolicyName = policyName;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {

	    var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
   	    if (allowAnonymous)
	       return;

            if (PolicyName != "APIAuth")
                return;


            bool autenticado = false;

            var authorizationAux = context.HttpContext.Request.Headers["Authorization"];

            string authorizationToken = "";
            if (string.IsNullOrEmpty(authorizationAux))
            {
                context.Result = new JsonResult(new { message = "Token não fornecido." }) { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }
            else
            {
                authorizationToken = authorizationAux.ToString().Replace("Bearer ", "");

                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    tokenHandler.ValidateToken(authorizationToken, new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("minha-chave-secreta-minha-chave-secreta-super")),
                        ValidAudience = "Usuários da API",
                        ValidIssuer = "Joao Uzeloto",
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                    }, out var validatedToken);

                   
                        autenticado = true;
                        //segue o fluxo...
                    

                }
                catch { }


         
            }
            
            if (!autenticado)
            {
                context.Result = new JsonResult(new { message = "Não autorizado."}) { StatusCode = StatusCodes.Status401Unauthorized };
            }
            
        }

    }
}
