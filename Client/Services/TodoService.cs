using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Oqtane.Modules;
using Oqtane.Services;
using Oqtane.Shared;
using Colin.Todo.Models;

namespace Colin.Todo.Services
{
    public class TodoService : ServiceBase, ITodoService, IService
    {
        public TodoService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string Apiurl => CreateApiUrl("Todo");

        public async Task<List<Models.Todo>> GetTodosAsync(int ModuleId)
        {
            List<Models.Todo> Todos = await GetJsonAsync<List<Models.Todo>>(CreateAuthorizationPolicyUrl($"{Apiurl}?moduleid={ModuleId}", EntityNames.Module, ModuleId));
            return Todos.OrderBy(item => item.Name).ToList();
        }

        public async Task<Models.Todo> GetTodoAsync(int TodoId, int ModuleId)
        {
            return await GetJsonAsync<Models.Todo>(CreateAuthorizationPolicyUrl($"{Apiurl}/{TodoId}", EntityNames.Module, ModuleId));
        }

        public async Task<Models.Todo> AddTodoAsync(Models.Todo Todo)
        {
            return await PostJsonAsync<Models.Todo>(CreateAuthorizationPolicyUrl($"{Apiurl}", EntityNames.Module, Todo.ModuleId), Todo);
        }

        public async Task<Models.Todo> UpdateTodoAsync(Models.Todo Todo)
        {
            return await PutJsonAsync<Models.Todo>(CreateAuthorizationPolicyUrl($"{Apiurl}/{Todo.TodoId}", EntityNames.Module, Todo.ModuleId), Todo);
        }

        public async Task DeleteTodoAsync(int TodoId, int ModuleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{Apiurl}/{TodoId}", EntityNames.Module, ModuleId));
        }
    }
}
