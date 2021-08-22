using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Oqtane.Modules;
using Oqtane.Repository;
using Oqtane.Infrastructure;
using Oqtane.Repository.Databases.Interfaces;

namespace Colin.Todo.Repository
{
    public class TodoContext : DBContextBase, IService, IMultiDatabase
    {
        public virtual DbSet<Models.Todo> Todo { get; set; }

        public TodoContext(ITenantManager tenantManager, IHttpContextAccessor accessor) : base(tenantManager, accessor)
        {
            // ContextBase handles multi-tenant database connections
        }
    }
}
