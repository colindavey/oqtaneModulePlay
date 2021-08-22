using System.Collections.Generic;
using System.Threading.Tasks;
using Colin.Todo.Models;

namespace Colin.Todo.Services
{
    public interface ITodoService 
    {
        Task<List<Models.Todo>> GetTodosAsync(int ModuleId);

        Task<Models.Todo> GetTodoAsync(int TodoId, int ModuleId);

        Task<Models.Todo> AddTodoAsync(Models.Todo Todo);

        Task<Models.Todo> UpdateTodoAsync(Models.Todo Todo);

        Task DeleteTodoAsync(int TodoId, int ModuleId);
    }
}
