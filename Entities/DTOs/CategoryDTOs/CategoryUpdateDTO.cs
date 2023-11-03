using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.CategoryDTOs
{
    public class CategoryUpdateDTO
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("CategoryName")]
        public string CategoryName { get; set; }

        [JsonProperty("PhotoUrl")]
        public string PhotoUrl { get; set; }
    }
}
