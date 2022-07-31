using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Empresaurio.TeamOrganizer.Domain.DTOs
{
    public class UserDTO
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string PhotoUrl { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Telephone { get; set; }
    }
}
