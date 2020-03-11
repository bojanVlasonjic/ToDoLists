using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDo.Api.Exceptions
{
    public class EntityNotFoundException: Exception
    {

        public EntityNotFoundException()
        {

        }

        public EntityNotFoundException(string message): base(message)
        {

        }
    }
}
