using System.Collections.Generic;
using Colin.Todo.Models;

namespace Colin.Todo.Repository
{
    public interface ITodoRepository
    {
        IEnumerable<Models.Todo> GetTodos(int ModuleId);
        Models.Todo GetTodo(int TodoId);
        Models.Todo GetTodo(int TodoId, bool tracking);
        Models.Todo AddTodo(Models.Todo Todo);
        Models.Todo UpdateTodo(Models.Todo Todo);
        void DeleteTodo(int TodoId);
    }
}
