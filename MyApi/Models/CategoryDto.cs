using Entities;
using System.Collections.Generic;
using WebFramework.Api;

namespace MyApi.Models
{
    public class CategoryDto : BaseDto<CategoryDto, Category>
    {
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
    }

    public class CategorySelectDto : BaseDto<CategorySelectDto, Category>
    {
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }

        public string ParentCategoryName { get; set; } //=> mapped from ParentCategory.Name
    }
}
