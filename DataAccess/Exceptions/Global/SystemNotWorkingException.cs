using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Exceptions.Global
{
    public class SystemNotWorkingException : Exception
    {
        public SystemNotWorkingException() : base(message: "Something Was Wrong...")
        {

        }
    }
}