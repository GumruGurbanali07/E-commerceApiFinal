using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.SuccessResults;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.SpecificationDTOs;
using Serilog;

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

            Log.Logger = new LoggerConfiguration()
                     .MinimumLevel.Debug()
                     .WriteTo.Console()
                     .WriteTo.File("logs/mySpecificationLogs-.txt", rollingInterval: RollingInterval.Day)
                     .CreateLogger();

            Log.Information("SpecificationService instance created.");
        }

        public IResult CreateSpecification(int productId, List<SpecificationAddDTO> specificationAddDTOs)
        {
            try
            {
                Log.Information($"Creating specifications for product ID: {productId}");

                var map = _mapper.Map<List<Specification>>(specificationAddDTOs);
                _specificationDAL.AddSpecification(productId, map);

                Log.Information("Specifications created successfully.");
                return new SuccessResult();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while creating specifications.");
                throw;
            }
        }
    }
}
