using AutoMapper;
using Entities.Concrete;
using Entities.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.AutoMapper
{
    public class MapProfile :Profile
    {
        public MapProfile()
        {
             CreateMap<CategoryCreateDTO, Category>().ReverseMap();
        CreateMap<CategoryUpdateDTO, Category>().ReverseMap();
        }
       
    }
}
