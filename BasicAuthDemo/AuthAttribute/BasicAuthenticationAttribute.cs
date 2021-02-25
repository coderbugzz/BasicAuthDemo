using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace BasicAuthDemo.AuthAttribute
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {

            var identity = DecodeAuthHeader(actionContext);

            if (identity == null)   
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized request");
            }
            else
            {

                if (OnAuthorizeUser(identity.Name,identity.Password,actionContext))
                {
                    var principal = new GenericPrincipal(identity, null);
                    Thread.CurrentPrincipal = principal;
                }
                else
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized request");
                }
            }
        }

        protected virtual bool OnAuthorizeUser(string username, string password, HttpActionContext actionContext)
        {
            if (username == "freecode" && password == "freecodespot")
                return true;

            return false;
        }

        protected virtual AuthIdentity DecodeAuthHeader(HttpActionContext actionContext)
        {
            string authHeader = null;
            var auth = actionContext.Request.Headers.Authorization;
            if (auth != null && auth.Scheme == "Basic")
                authHeader = auth.Parameter;

            if (string.IsNullOrEmpty(authHeader))
                return null;

            authHeader = Encoding.Default.GetString(Convert.FromBase64String(authHeader));

            var tokens = authHeader.Split(':');
            if (tokens.Length < 2)
                return null;

            return new AuthIdentity(tokens[0], tokens[1]);
        }
    }
}