using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderMonitor.Server
{
    public class Client
    {

        public string UserName { get; set; }
        public Guid CID { get; set; }

        public Client()
        {
            CID = Guid.NewGuid();
        }
    }
}
