using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Data
{
    public class MockCommanderRepo : ICommanderRepo
    {
        public void CreateCommand(Command cmd)
        {
            throw new NotImplementedException();
        }

        public void DeleteCommand(Command cmd)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Command> GetAllCommands()
        {
            var commands = new List<Command>
            {
                new Command { Id=0, HowTo="Boil water", Line="Add water, add heat", Platform="Stove top"},
                new Command { Id = 1, HowTo = "Boil water", Line = "Add water, add heat", Platform = "Stove top" },
                new Command { Id = 2, HowTo = "Boil water", Line = "Add water, add heat", Platform = "Stove top" }

            };

            return commands;
        }

        public Command GetCommandById(int id)
        {
            return new Command { Id=0, HowTo="Boil water", Line="Add water, add heat", Platform="Stove top"};
        }

        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void UpdateCommand(Command cmd)
        {
            throw new NotImplementedException();
        }
    }
}
