using AutoMapper;
using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Authorization;
using MyApi.Models;
using WebFramework.Api;

namespace MyApi.Controllers.v1
{
    [AllowAnonymous]
    public class CategoriesController : CrudController<CategoryDto, CategorySelectDto, Category>
    {
        public CategoriesController(IRepository<Category> repository, IMapper mapper)
            : base(repository, mapper)
        {
        }
    }
}
