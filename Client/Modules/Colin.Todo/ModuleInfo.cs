using Oqtane.Models;
using Oqtane.Modules;

namespace Colin.Todo
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "Todo",
            Description = "Todo",
            Version = "1.0.0",
            ServerManagerType = "Colin.Todo.Manager.TodoManager, Colin.Todo.Server.Oqtane",
            ReleaseVersions = "1.0.0",
            Dependencies = "Colin.Todo.Shared.Oqtane",
            PackageName = "Colin.Todo" 
        };
    }
}
