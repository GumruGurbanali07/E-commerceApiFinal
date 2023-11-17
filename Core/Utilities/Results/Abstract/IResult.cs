using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results.Abstract
{
    public interface IResult
    {
        //A string that contains a message about the result of the operation.
        string Message { get; }
        //A boolean value that indicates whether the operation was successful.
        bool Success { get; }
    }
}
