using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Threading;
using System.Security.Principal;

namespace OfficeConnect_Web.Models
{
    public class CustomAuthFilter : AuthorizationFilterAttribute
    {
        private string key = "OfficeConnect";

        DB_Offc_ConEntities DB = new DB_Offc_ConEntities();
        ClsAuthentication ObjAuth = new ClsAuthentication();

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
            }
            else
            {
                string authenticationToken = actionContext.Request.Headers.Authorization.Scheme;
                string AuthKey = actionContext.Request.Headers.GetValues("AuthKey").FirstOrDefault();
                //string authenticationToken = actionContext.Request.Headers["Authorization"];
                //var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];

                if (authenticationToken == key)
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(key), null);
                }
                else
                {
                    string Username = ObjAuth.DeCodeToken(authenticationToken);

                    var ValidCredentials = Username.ToString().Split(',');
                    string uname = "";
                    DateTime Timeval = new DateTime();
                    double TotHours = 0;
                    if (ValidCredentials != null)
                    {
                        if (ValidCredentials.Count() >= 1)
                        {
                            var ValidCredentialstmp = ValidCredentials[0].ToString().Split(':');
                            if (ValidCredentialstmp != null)
                            {
                                if (ValidCredentialstmp.Count() >= 1)
                                {
                                    var ValidCredentials1 = ValidCredentialstmp[1].ToString().Split('\"');
                                    if (ValidCredentials1 != null)
                                    {
                                        if (ValidCredentials1.Count() >= 1)
                                        {
                                            uname = ValidCredentials1[1].ToString();
                                        }
                                    }
                                }
                            }

                            ValidCredentials = ValidCredentials[1].ToString().Split('\"');
                            if (ValidCredentials != null)
                            {
                                if (ValidCredentials.Count() >= 3)
                                {
                                    try
                                    {

                                        Timeval = Convert.ToDateTime(ValidCredentials[3].ToString());
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                        }
                        try
                        {
                            if (Timeval != null)
                            {
                                TotHours = (DateTime.Now - Timeval).TotalHours;
                                if (TotHours > 1)
                                {

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    var usertok = (from suser in DB.SessionMasters
                                   where (suser.Username).ToUpper() == uname.ToUpper() && suser.TockenId == authenticationToken
                                   && suser.Status == true && suser.Expired == false && suser.IsActive == true && suser.IsDeleted == false
                                   select suser).SingleOrDefault();

                    if (usertok != null)
                    {
                        string KeyUsername = ObjAuth.DeCodeToken(authenticationToken);

                        var KeyValidCredentials = Username.ToString().Split(',');
                        string Keyuname = "";
                        DateTime KeyTimeval = new DateTime();
                        double KeyTotHours = 0;
                        if (KeyValidCredentials != null)
                        {
                            if (KeyValidCredentials.Count() >= 1)
                            {
                                var KeyValidCredentialstmp = KeyValidCredentials[0].ToString().Split(':');
                                if (KeyValidCredentialstmp != null)
                                {
                                    if (KeyValidCredentialstmp.Count() >= 1)
                                    {
                                        var KeyValidCredentials1 = KeyValidCredentialstmp[1].ToString().Split('\"');
                                        if (KeyValidCredentials1 != null)
                                        {
                                            if (KeyValidCredentials1.Count() >= 1)
                                            {
                                                Keyuname = KeyValidCredentials1[1].ToString();
                                            }
                                        }
                                    }
                                }

                                KeyValidCredentials = KeyValidCredentials[1].ToString().Split('\"');
                                if (KeyValidCredentials != null)
                                {
                                    if (KeyValidCredentials.Count() >= 3)
                                    {
                                        try
                                        {

                                            KeyTimeval = Convert.ToDateTime(KeyValidCredentials[3].ToString());
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                }
                            }
                            try
                            {
                                if (Timeval != null)
                                {
                                    TotHours = (DateTime.Now - Timeval).TotalHours;
                                    if (TotHours > 1)
                                    {

                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }

                            var Keyusertok = (from suser in DB.SessionMasters
                                              where (suser.Username).ToUpper() == uname.ToUpper() && suser.TockenId == authenticationToken
                                              && suser.Status == true && suser.Expired == false && suser.IsActive == true && suser.IsDeleted == false
                                              select suser).SingleOrDefault();

                            if (usertok != null)
                            {
                                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(key), null);
                            }
                            else
                            {
                                actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.NotFound);
                            }
                        }

                    }
                    else
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
                    }
                }
            }
        }
    }
}