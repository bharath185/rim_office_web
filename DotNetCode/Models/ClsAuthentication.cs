using Jose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeConnect_Web.Models
{
    public class ClsAuthentication
    {
        private byte[] Base64UrlDecode(string arg) // This function is for decoding string to   
        {
            string s = arg;
            s = s.Replace('-', '+'); // 62nd char of encoding  
            s = s.Replace('_', '/'); // 63rd char of encoding  
            switch (s.Length % 4) // Pad with trailing '='s  
            {
                case 0: break; // No pad chars in this case  
                case 2: s += "=="; break; // Two pad chars  
                case 3: s += "="; break; // One pad char  
                default:
                    throw new System.Exception(
                "Illegal base64url string!");
            }
            return Convert.FromBase64String(s); // Standard base64 decoder  
        }
        private long ToUnixTime(DateTime dateTime)
        {
            //return (int)(dateTime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return dateTime.Hour;
        }
        public string GetJwt(string user) //function for JWT Token  
        {
            byte[] secretKey = Base64UrlDecode("RimIndia");//pass key to secure and decode it  
            DateTime issued = DateTime.Now;
            var User = new Dictionary<string, object>()
                    {
                        {"user", user},
                         {"iat", (issued).ToString()}
                    };

            string token = JWT.Encode(User, secretKey, JwsAlgorithm.HS256);

            return token;
        }

        public string DeCodeToken(string TokenId) //function for JWT Token  
        {
            byte[] secretKey = Base64UrlDecode("RimIndia");//pass key to secure and decode it  
            DateTime issued = DateTime.Now;

            string token = JWT.Decode(TokenId, secretKey, JwsAlgorithm.HS256);

            return token;
        }
    }
}