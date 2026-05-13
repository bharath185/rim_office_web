using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeConnect_Web.Models
{
    public class ClsDatabase
    {
        public static string connecttodb()
        {
            string dbconnection = "Data Source=.;Initial Catalog='DB_Offc_Con';User ID=sa;Password=sql@123;Min Pool Size=5;Max Pool Size=300;Pooling=true;Connection Timeout=60;";

            return dbconnection;
        }
    }
}