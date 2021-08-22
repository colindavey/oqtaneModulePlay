using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using Oqtane.Migrations.EntityBuilders;

namespace Colin.Todo.Migrations.EntityBuilders
{
    public class TodoEntityBuilder : AuditableBaseEntityBuilder<TodoEntityBuilder>
    {
        private const string _entityTableName = "ColinTodo";
        private readonly PrimaryKey<TodoEntityBuilder> _primaryKey = new("PK_ColinTodo", x => x.TodoId);
        private readonly ForeignKey<TodoEntityBuilder> _moduleForeignKey = new("FK_ColinTodo_Module", x => x.ModuleId, "Module", "ModuleId", ReferentialAction.Cascade);

        public TodoEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_moduleForeignKey);
        }

        protected override TodoEntityBuilder BuildTable(ColumnsBuilder table)
        {
            TodoId = AddAutoIncrementColumn(table,"TodoId");
            ModuleId = AddIntegerColumn(table,"ModuleId");
            Name = AddMaxStringColumn(table,"Name");
            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> TodoId { get; set; }
        public OperationBuilder<AddColumnOperation> ModuleId { get; set; }
        public OperationBuilder<AddColumnOperation> Name { get; set; }
    }
}
