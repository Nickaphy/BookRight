using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRight
{
    public class ConnectionString
    {
        public readonly string conn =
            "Server=MSIErikLaptop;Database=FitHubDB;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True;";
    }


}

// Erik have had a look on making a way to make connectionstrings eaiser, by only typing in, the local DB connectionstring once,
// for the workmember in the group.