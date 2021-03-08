using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.Models;

namespace WebApi.Profiles
{
    //Profiles handle the mapping between internal models and DTOs.
    //This profile uses automapper to simply match up model and DTO properties by name
    //More complex mappings would likely require manual application of mapping rules to a parameter object.
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            //To/from CommandReadDto
            CreateMap<Command, CommandReadDto>();
            CreateMap<CommandCreateDto, Command>();
            //To/From CommandUpdateDto 
            CreateMap<Command, CommandUpdateDto>();
            CreateMap<CommandUpdateDto, Command>();
        }
    }
}
