using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DietAppClient.Exceptions
{
    public class HealthCheckException : Exception
    {
        public HealthCheckException(string message) : base(message) 
        { }
    }
}
