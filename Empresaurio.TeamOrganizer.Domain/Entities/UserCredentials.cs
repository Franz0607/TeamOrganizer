using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Empresaurio.TeamOrganizer.Domain.Entities
{
    public class UserCredentials
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhotoUrl { get; set; }
        public string Token { get; set; }
    }
}
