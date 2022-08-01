using Empresaurio.TeamOrganizer.Domain.CustomEntities;
using Empresaurio.TeamOrganizer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Empresaurio.TeamOrganizer.Infrastructure.Interfaces
{
    public interface IAccountRepository
    {
        Task<UserCredentials> AuthenticateAsync(Credentials credentials);
        Task RegisterAsync(User user);
    }
}
