﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KnightFrank.Antares.Api.Controllers
{
    using System.Security.Claims;
    using System.Web.Http;

    using KnightFrank.Antares.Domain.User;

    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        [Route("data")]
        public UserDataResult GetUserData()
        {
            var identity = (ClaimsIdentity)this.User.Identity;

            var user = new UserDataResult()
            {
                Name = identity.Name,
                Email = identity.Claims.First(c => c.Type == ClaimTypes.Email).Value,
                Roles = identity.FindAll(ClaimTypes.Role).Select(claim => claim.Value)
            };

            return user;
        }
    }
}