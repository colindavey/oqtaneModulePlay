using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Oqtane.Modules;
using Colin.Todo.Models;

namespace Colin.Todo.Repository
{
    public class TodoRepository : ITodoRepository, IService
    {
        private readonly TodoContext _db;

        public TodoRepository(TodoContext context)
        {
            _db = context;
        }

        public IEnumerable<Models.Todo> GetTodos(int ModuleId)
        {
            return _db.Todo.Where(item => item.ModuleId == ModuleId);
        }

        public Models.Todo GetTodo(int TodoId)
        {
            return GetTodo(TodoId, true);
        }

        public Models.Todo GetTodo(int TodoId, bool tracking)
        {
            if (tracking)
            {
                return _db.Todo.Find(TodoId);
            }
            else
            {
                return _db.Todo.AsNoTracking().FirstOrDefault(item => item.TodoId == TodoId);
            }
        }

        public Models.Todo AddTodo(Models.Todo Todo)
        {
            _db.Todo.Add(Todo);
            _db.SaveChanges();
            return Todo;
        }

        public Models.Todo UpdateTodo(Models.Todo Todo)
        {
            _db.Entry(Todo).State = EntityState.Modified;
            _db.SaveChanges();
            return Todo;
        }

        public void DeleteTodo(int TodoId)
        {
            Models.Todo Todo = _db.Todo.Find(TodoId);
            _db.Todo.Remove(Todo);
            _db.SaveChanges();
        }
    }
}
