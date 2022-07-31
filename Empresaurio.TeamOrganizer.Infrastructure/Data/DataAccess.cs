using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Empresaurio.TeamOrganizer.Infrastructure.Data
{
    public class DataAccess
    {
        private string _ConnectionSQL;
        public string ConnectionSQL { get => _ConnectionSQL; }

        public DataAccess(string connectionSQL)
        {
            _ConnectionSQL = connectionSQL;
        }
    }
}
