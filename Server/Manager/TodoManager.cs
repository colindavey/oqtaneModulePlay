using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Oqtane.Modules;
using Oqtane.Models;
using Oqtane.Infrastructure;
using Oqtane.Enums;
using Colin.Todo.Repository;

namespace Colin.Todo.Manager
{
    public class TodoManager : MigratableModuleBase, IInstallable, IPortable
    {
        private ITodoRepository _TodoRepository;
        private readonly ITenantManager _tenantManager;
        private readonly IHttpContextAccessor _accessor;

        public TodoManager(ITodoRepository TodoRepository, ITenantManager tenantManager, IHttpContextAccessor accessor)
        {
            _TodoRepository = TodoRepository;
            _tenantManager = tenantManager;
            _accessor = accessor;
        }

        public bool Install(Tenant tenant, string version)
        {
            return Migrate(new TodoContext(_tenantManager, _accessor), tenant, MigrationType.Up);
        }

        public bool Uninstall(Tenant tenant)
        {
            return Migrate(new TodoContext(_tenantManager, _accessor), tenant, MigrationType.Down);
        }

        public string ExportModule(Module module)
        {
            string content = "";
            List<Models.Todo> Todos = _TodoRepository.GetTodos(module.ModuleId).ToList();
            if (Todos != null)
            {
                content = JsonSerializer.Serialize(Todos);
            }
            return content;
        }

        public void ImportModule(Module module, string content, string version)
        {
            List<Models.Todo> Todos = null;
            if (!string.IsNullOrEmpty(content))
            {
                Todos = JsonSerializer.Deserialize<List<Models.Todo>>(content);
            }
            if (Todos != null)
            {
                foreach(var Todo in Todos)
                {
                    _TodoRepository.AddTodo(new Models.Todo { ModuleId = module.ModuleId, Name = Todo.Name });
                }
            }
        }
    }
}