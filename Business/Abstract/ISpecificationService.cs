using Core.Utilities.Results.Abstract;
using Entities.DTOs.SpecificationDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ISpecificationService
    {
        IResult CreateSpecification(int productId,List<SpecificationAddDTO> specificationAddDTOs);

    }
}
