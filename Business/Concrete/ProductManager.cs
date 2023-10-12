using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.SuccessResults;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {

        private readonly IProductDAL _productDAL;
        private readonly IMapper _mapper;

        public ProductManager(IProductDAL productDAL, IMapper mapper)
        {
            _productDAL = productDAL;
            _mapper = mapper;
        }

        public IResult ProductCreate(ProductCreateDTO productCreateDTO)
        {
            var map=_mapper.Map<Product>(productCreateDTO);
            map.CreatedDate = DateTime.Now;
            _productDAL.Add(map);
            return new SuccessResult("Product Added");
        }

        public IDataResult<ProductDetailDTO> ProductDetail(int productId)
        {
            var product=_productDAL.GetProduct(productId);
            var map=_mapper.Map<ProductDetailDTO>(product);
            return new SuccessDataResult<ProductDetailDTO>(map);
        }

        public IResult ProductUpdate(ProductUpdateDTO productUpdateDTO)
        {
           var product=_productDAL.Get(x=>x.Id==productUpdateDTO.Id);
            var map = _mapper.Map<Product>(productUpdateDTO);
            product.Status = map.Status;
            product.ProductName = map.ProductName;
            product.Price = map.Price;
            product.Description = map.Description;
            product.Quantity = map.Quantity;
            product.CategoryId = map.CategoryId;
            product.IsFeatured = map.IsFeatured;
            product.Discount = map.Discount;
            product.PhotoUrl = map.PhotoUrl;
            _productDAL.Update(map);
            return new SuccessResult("Product Updated");
        }
    }
}
