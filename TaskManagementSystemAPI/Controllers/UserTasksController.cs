using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystemAPI.Controllers.Base;
using TaskManagementSystemAPI.Models;
using TaskManagementSystemAPI.Models.Context;
using TaskManagementSystemAPI.Models.DTO;
using TaskManagementSystemAPI.Models.Entities;
using TaskManagementSystemAPI.ServicesLayers.Implementations;
using TaskManagementSystemAPI.ServicesLayers.Interfaces;

namespace TaskManagementSystemAPI.Controllers
{
    public class UserTasksController : BaseApiController
    {
        private readonly AppDBContext _context;
        private readonly IUserTaskService userTaskService;

        public UserTasksController(IHttpContextAccessor httpContextAccessor, AppDBContext context, IUserTaskService userTaskService) : base(httpContextAccessor)
        {
            _context = context;
            this.userTaskService = userTaskService;
        }

        // GET: api/UserTasks
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserTaskResponse>>>> GetUserTasks()
        {
            try
            {
                AddCallingServiceMessage();

                if (!await IsAdminRole())
                    return UnAuthorizedAccessResponse();

                var result = await userTaskService.GetAll();
                return HandleResponse(userTaskService.PrepareResponse(result));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        [HttpGet("getTaskByUserId/{userId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserTaskResponse>>>> GetTaskByUserId(int userId)
        {
            try
            {
                AddCallingServiceMessage();

                var result = await userTaskService.FindAll(x=>x.UserId == userId);
                return HandleResponse(userTaskService.PrepareResponse(result));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        // GET: api/UserTasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UserTaskResponse>>> GetUserTask(int id)
        {
            try
            {
                AddCallingServiceMessage();

                if (!await IsAdminRole())
                    return UnAuthorizedAccessResponse();

                var result = await userTaskService.Get(id);
                return HandleResponse(userTaskService.PrepareResponse(result));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        // PUT: api/UserTasks/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<UserTaskResponse>>> PutUserTask(int id, UpdateUserTaskRequest userTaskRequest)
        {
            try
            {
                AddCallingServiceMessage();

                var result = await userTaskService.UpdateTask(id, userTaskRequest, (int)currentUserId);
                return HandleResponse(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        // POST: api/UserTasks
        [HttpPost]
        public async Task<ActionResult<ApiResponse<UserTaskResponse>>> PostUserTask(UserTaskRequest userTaskRequest)
        {
            try
            {
                AddCallingServiceMessage();

                if (!await IsAdminRole())
                    return UnAuthorizedAccessResponse();

                var result = await userTaskService.AssignedTaskToUser(userTaskRequest);
                return HandleResponse(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        // DELETE: api/UserTasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserTask(int id)
        {
            try
            {
                AddCallingServiceMessage();

                if (!await IsAdminRole())
                    return UnAuthorizedAccessResponse();

                var result = await userTaskService.Delete(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
