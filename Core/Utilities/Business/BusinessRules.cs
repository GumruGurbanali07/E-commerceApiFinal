using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Business
{
    //multiple conditions in a clean and organized way,
    //making it easy to see if everything is in order or if there's an issue.
    public static  class BusinessRules
    {
        //allows you to pass multiple IResult objects as arguments.
        public static IResult Check(params IResult[] logics)
        {
            foreach(var logic in logics)
            {
                if (!logic.Success) 
                    return new ErrorResult();
                }
            return new SuccessResult();
            }
        
        }
    }

