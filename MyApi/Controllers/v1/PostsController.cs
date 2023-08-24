using AutoMapper;
using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Authorization;
using MyApi.Models;
using System;
using WebFramework.Api;

namespace MyApi.Controllers.v1
{
    [AllowAnonymous]
    public class PostsController : CrudController<PostDto, PostSelectDto, Post, Guid>
    {
        public PostsController(IRepository<Post> repository, IMapper mapper)
            : base(repository, mapper)
        {
        }
    }
}
