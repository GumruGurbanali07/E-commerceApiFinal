using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.SuccessResults;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private readonly ICategoryDAL _categoryDAL;
            private readonly IMapper _mapper;

        public CategoryManager(ICategoryDAL categoryDAL, IMapper mapper)
        {
            _categoryDAL = categoryDAL;
            _mapper = mapper;
        }

        public IResult AddCategory(CategoryCreateDTO categoryCreateDTO)
        {
            var map = _mapper.Map<Category>(categoryCreateDTO);
            map.Status = true;
            map.CreatedDate=DateTime.Now;
            _categoryDAL.Add(map);
            return new SuccessResult();
                
        }

        public IResult DeleteCategory(int categoryId)
        {
            throw new NotImplementedException();
        }

        public IResult UpdateCategory(CategoryUpdateDTO categoryUpdateDTO)
        {
            throw new NotImplementedException();
        }
    }
}
