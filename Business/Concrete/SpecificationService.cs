using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.SuccessResults;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.SpecificationDTOs;

namespace Business.Concrete
{
    public class SpecificationService : ISpecificationService
    {
        private readonly ISpecificationDAL _specificationDAL;
        private readonly IMapper _mapper;
        public SpecificationService(ISpecificationDAL specificationDAL, IMapper mapper)
        {
            _specificationDAL = specificationDAL;
            _mapper = mapper;
        }


        public IResult CreateSpecification(int productId, List<SpecificationAddDTO> specificationAddDTOs)
        {
            var map = _mapper.Map<List<Specification>>(specificationAddDTOs);
            _specificationDAL.AddSpecification(productId, map);
            return new SuccessResult();
        }
    }
}
