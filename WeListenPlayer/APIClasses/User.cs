using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeListenPlayer.APIClasses
{
    class User
    {
        public int UserID { set; get; }
        public string Username { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string EmailAddress { set; get; }
        public string Password { set; get; }
    }
}
