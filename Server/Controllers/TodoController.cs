using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Colin.Todo.Repository;
using Oqtane.Controllers;
using System.Net;

namespace Colin.Todo.Controllers
{
    [Route(ControllerRoutes.ApiRoute)]
    public class TodoController : ModuleControllerBase
    {
        private readonly ITodoRepository _TodoRepository;

        public TodoController(ITodoRepository TodoRepository, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _TodoRepository = TodoRepository;
        }

        // GET: api/<controller>?moduleid=x
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public IEnumerable<Models.Todo> Get(string moduleid)
        {
            int ModuleId;
            if (int.TryParse(moduleid, out ModuleId) && ModuleId == AuthEntityId(EntityNames.Module))
            {
                return _TodoRepository.GetTodos(ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Todo Get Attempt {ModuleId}", moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public Models.Todo Get(int id)
        {
            Models.Todo Todo = _TodoRepository.GetTodo(id);
            if (Todo != null && Todo.ModuleId == AuthEntityId(EntityNames.Module))
            {
                return Todo;
            }
            else
            { 
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Todo Get Attempt {TodoId}", id);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        // POST api/<controller>
        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public Models.Todo Post([FromBody] Models.Todo Todo)
        {
            if (ModelState.IsValid && Todo.ModuleId == AuthEntityId(EntityNames.Module))
            {
                Todo = _TodoRepository.AddTodo(Todo);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "Todo Added {Todo}", Todo);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Todo Post Attempt {Todo}", Todo);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                Todo = null;
            }
            return Todo;
        }

        // PUT api/<controller>/5
        [ValidateAntiForgeryToken]
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public Models.Todo Put(int id, [FromBody] Models.Todo Todo)
        {
            if (ModelState.IsValid && Todo.ModuleId == AuthEntityId(EntityNames.Module) && _TodoRepository.GetTodo(Todo.TodoId, false) != null)
            {
                Todo = _TodoRepository.UpdateTodo(Todo);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "Todo Updated {Todo}", Todo);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Todo Put Attempt {Todo}", Todo);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                Todo = null;
            }
            return Todo;
        }

        // DELETE api/<controller>/5
        [ValidateAntiForgeryToken]
        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public void Delete(int id)
        {
            Models.Todo Todo = _TodoRepository.GetTodo(id);
            if (Todo != null && Todo.ModuleId == AuthEntityId(EntityNames.Module))
            {
                _TodoRepository.DeleteTodo(id);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Todo Deleted {TodoId}", id);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Todo Delete Attempt {TodoId}", id);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
        }
    }
}
