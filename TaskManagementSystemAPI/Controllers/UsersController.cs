using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TaskManagementSystemAPI.Controllers.Base;
using TaskManagementSystemAPI.Models;
using TaskManagementSystemAPI.Models.Context;
using TaskManagementSystemAPI.Models.DTO;
using TaskManagementSystemAPI.Models.Entities;
using TaskManagementSystemAPI.ServicesLayers.Interfaces;

namespace TaskManagementSystemAPI.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IUserService userService;

        public UsersController(IHttpContextAccessor httpContextAccessor, IUserService userService): base(httpContextAccessor)
        {
            this.userService = userService;
        }

        // GET: api/Users
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<UserResponse>), 200)]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserResponse>?>>> GetUsers()
        {
            try
            {
                AddCallingServiceMessage();

                if (!await IsAdminRole())
                    return UnAuthorizedAccessResponse();

                var result = await userService.GetAll();
                return HandleResponse(userService.PrepareUserResponse(result));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UserResponse?>>> GetUser(int id)
        {
            try
            {
                AddCallingServiceMessage();

                var result = await userService.Get(id);

                if (!await IsAdminRole())
                {
                    if (currentUserId != result.Data?.ID)
                        return UnAuthorizedAccessResponse();
                }

                return HandleResponse(userService.PrepareUserResponse(result));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<UserResponse>>> PutUser(int id, UserRequest userRequest)
        {
            try
            {
                AddCallingServiceMessage();

                if (currentUserId != id)
                    return UnAuthorizedAccessResponse();

                var result = await userService.UpdateUser(id, userRequest, (int)currentUserId);
                return HandleResponse(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<ApiResponse<UserResponse>>> PostUser(UserRequest userRequest)
        {
            try
            {
                AddCallingServiceMessage();

                var result = await userService.AddNewUser(userRequest);
                return HandleResponse(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteUser(int id)
        {
            try
            {
                AddCallingServiceMessage();

                var result = await userService.Delete(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
