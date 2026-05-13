using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Security.Principal;
using System.Web.Mvc;
using OfficeConnect_Web.Models;
using OfficeConnect_Web;
using OfficeConnect_Web.Controllers;

public class AuthAttribute : FilterAttribute, IAuthorizationFilter
{
    private readonly string key = "OfficeConnect";
    private readonly string key1 = "Visitors";

    DB_Offc_ConEntities DB = new DB_Offc_ConEntities();
    ClsAuthentication ObjAuth = new ClsAuthentication();
    ClsAuthorization Auth = new ClsAuthorization();

    public void OnAuthorization(AuthorizationContext filterContext)
    {
        try
        {
            string authenticationToken = filterContext.HttpContext.Request.Headers["Authorization"];
            string AuthKey = filterContext.HttpContext.Request.Headers["AuthKey"];

            if (string.IsNullOrEmpty(authenticationToken))
            {
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                return;
            }
            else
            {

                //string authenticationToken = actionContext.Request.Headers["Authorization"];
                //var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];

                if (authenticationToken == key)
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(key), null);
                }
                else if (authenticationToken == key1)
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(key1), null);
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
                        string KeyUsername = Auth.DeCodeAuthKey(AuthKey);

                        var KeyValidCredentials = KeyUsername.ToString().Split(',');
                        string Keyuname = "";
                        int KeyRole = 0;
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

                                var KeyValidCredentials2 = KeyValidCredentials[1].ToString().Split('\"');
                                if (KeyValidCredentials2 != null)
                                {
                                    if (KeyValidCredentials2.Count() >= 2)
                                    {
                                        try
                                        {

                                            KeyRole = Convert.ToInt32(KeyValidCredentials2[3].ToString());
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                }

                                KeyValidCredentials = KeyValidCredentials[2].ToString().Split('\"');
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
                                              where (suser.Username).ToUpper() == uname.ToUpper() && suser.RoleId == KeyRole && suser.TockenId == authenticationToken && suser.AuthKey == AuthKey
                                              && suser.Status == true && suser.Expired == false && suser.IsActive == true && suser.IsDeleted == false
                                              select suser).SingleOrDefault();

                            if (usertok != null)
                            {
                                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(key), null);
                            }
                            else
                            {
                                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.NotFound);
                            }
                        }

                    }
                    else
                    {
                        filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
                    }
                }
            }
        }
        catch(CustomApiException ex)
        {
            throw new CustomApiException(HttpStatusCode.NotFound, "EmpId is Missing");
        }
    }
}