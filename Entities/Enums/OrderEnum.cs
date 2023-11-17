using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Enums
{
    //user defined value type used to represent list of named integer constant
    public enum OrderEnum
    {
        OnPending,
        Shipped,
        Arrived,
        Complited,
        Returned,
        Canceled
    }
}
