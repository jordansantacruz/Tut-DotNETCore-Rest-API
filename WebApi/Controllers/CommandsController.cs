using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Dtos;
using WebApi.Models;

namespace WebApi.Controllers
{
    //This route is to the controller resource
    //The generic [controller] path dynamically takes the 'Commands' portion of CommandsController and adds it to the route
    //making the route dependent on the class name (api/commands until the class name changes)
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : Controller
    {
        //2 things for dependency injection to work:
        //1) Add the service to the services object in form of interface, implementation (Startup.cs)
        //2) Add a constructor in your controller that uses the interface (here)
        private readonly ICommanderRepo _repository;
        private readonly IMapper _mapper;

        public CommandsController(ICommanderRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //Both of these action results respond to requests thrown at the api/controller endpoint...
        //(GET api/commands)
        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands()
        {
            var commandItems = _repository.GetAllCommands();

            //Ok represents 200 success message. It comes bundled with an optional return param that gives
            //the IEnumerable back to the http request.
            //There are other messeges, like 
            //return NotFound() [404]
            //return BadRequest() [400]
            //etc
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
        }

        //...To differentiate the two and to add look up functionality to the method, 
        //we add the id parameter to the action's decorator. this makes the response uri:
        //GET api/commands/5
        [HttpGet("{id}", Name = "GetCommandById")]
        public ActionResult<CommandReadDto> GetCommandById(int id)
        {
            var commandItem = _repository.GetCommandById(id);
            if (commandItem == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CommandReadDto>(commandItem));
        }

        //post api/commands
        //Although we're gonna be creating a command and accepting a CommandCreateDto as a param, 
        //we are returning a representation of the newly created command
        //So the ActionResult we return is going to be of type CommandReadDto
        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto commandCreateDto)
        {
            var commandModel = _mapper.Map<Command>(commandCreateDto);
            _repository.CreateCommand(commandModel);
            _repository.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(commandModel);

            //The CreatedAtRoute (201 Created) response tells the user's browser that a resource was successfully created
            //in response to the request, along with the URI route necessary to fetch the new resource in the future
            return CreatedAtRoute(nameof(GetCommandById), new { Id = commandReadDto.Id }, commandReadDto);
        }

        //PUT api/commands/{id}
        //PUT requests are update requests that require the complete new version of the resource supplied in order to follow through
        //with the action. Generally considered to be obsolete and inefficient, PATCH is much more flexible and considered the industry standard for update http requests
        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id, CommandUpdateDto commandUpdateDto)
        {
            var commandModelFromRepo = _repository.GetCommandById(id);
            if(commandModelFromRepo == null)
            {
                return NotFound();
            }

            //Feature of Entity Framework: 
            //When you supply two parameters that already contain data to automapper,
            //it'll map data from one to the other in the form (source , target)
            //If the target was pulled from the database, changing the model will create a
            //pending change to the database data, tracked by DBContext
            _mapper.Map(commandUpdateDto, commandModelFromRepo);
            //The call to UpdateCommand is purely syntactic as that function has no code.           
            _repository.UpdateCommand(commandModelFromRepo);
            //Because the change is still pending, you still need to call SaveChanges to push it down to the db layer
            _repository.SaveChanges();

            return NoContent();
        }

        //PATCH api/commands/{id}
        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandUpdateDto> patchDoc)
        {
            //Get the commandModelFromRepo. Changes to this will be tracked by entity framework
            var commandModelFromRepo = _repository.GetCommandById(id);
            if(commandModelFromRepo == null)
            {
                return NotFound();
            }

            //To use the patch features, we need both the command and a mapped CommandUpdateDTO
            var commandToPatch = _mapper.Map<CommandUpdateDto>(commandModelFromRepo);
            //The patchdoc specifies which subset of properties require an update
            //The ModelState comes from the ControllerBase class but is required for the patch application
            patchDoc.ApplyTo(commandToPatch, ModelState);

            //Make sure the patch application went ok
            /*
             * TryValidateModel refuses to work even though the ModelState.IsValid property is true and the ModelState's error log is empty.
             * Also, no errors are passed to the ValidationProblem json response. Googling the problem comes up with nothing
            if (TryValidateModel(commandToPatch))
            {
                return ValidationProblem(ModelState);
            }
            */
            //Map the changes from the patched command to the unpatched commandModelFromRepo and save changes
            _mapper.Map(commandToPatch, commandModelFromRepo);
            _repository.UpdateCommand(commandModelFromRepo);
            _repository.SaveChanges();

            return NoContent();
        }


        //DELETE api/commands/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteCommand(int id)
        {
            var commandModelFromRepo = _repository.GetCommandById(id);
            if (commandModelFromRepo == null)
            {
                return NotFound();
            }

            _repository.DeleteCommand(commandModelFromRepo);
            _repository.SaveChanges();
            return NoContent();
        }
    }
}
