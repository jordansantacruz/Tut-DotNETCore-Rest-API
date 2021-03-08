using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Data
{
    public class CommanderContext : DbContext
    {
        //Make sure you get the correct type of options to pass down to the base class
        public CommanderContext(DbContextOptions<CommanderContext> opt) : base(opt)
        {

        }

        //We want to represent our Command object down to the database
        //Entity Framework will create the database and map its tables to whatever DbSets are within the context
        public DbSet<Command> Commands { get; set; }
    }
}
