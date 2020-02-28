using System.Collections.Generic;

namespace ZeroCredApp.Models
{
    public class SqlDatabaseViewModel
    {
        public List<SqlRowModel> EfResults { get; set; }
        public List<SqlRowModel> AdoNetResults { get; set; }
    }
}
