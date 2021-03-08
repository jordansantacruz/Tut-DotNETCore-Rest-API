using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Dtos
{
    //Data Transfer Objects (Dtos) are used to decouple internal models with external requests.
    //DTOs supply data to external requests by mapping to internal models with 'cleaned up' data
    //An important usecase of DTOs is privacy: an internal model might have the birth date of a client,
        //the DTO might only supply that client's age
    //As an example, this DTO is missing the platform property of the internal model
    public class CommandReadDto
    {
        public int Id { get; set; }
        public string HowTo { get; set; }
        public string Line { get; set; }
    }
}
