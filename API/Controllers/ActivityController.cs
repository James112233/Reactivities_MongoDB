using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.Domain.Contracts;
using Application.Models;
using Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Route("[controller]")]
    public class ActivityController : Controller
    {
        private readonly IActivityService _activityService;
        public ActivityController(IActivityService activityService)
        {
            _activityService = activityService;
        }
        [HttpPost("createActivity")]
        public async Task<IActionResult> CreateActivity([FromBody] Activities newActivity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { IsSuccess = false, Message = "Activity Not Found" });
                }
                else
                {
                    newActivity.Date = new DateTime(newActivity.Date.Ticks);
                    var activityCreate = await _activityService.CreateActivity(newActivity);
                    return Json(new { Issuccess = activityCreate });
                }
            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, Message = e.Message });
            }

        }

        [HttpGet("getActivities")]
        public IActionResult GetActivities()
        {
            try
            {
                var activities = _activityService.GetActivities(4, 0);
                return Json(new { IsSuccess = true, Activities = activities });
            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, Message = e.Message });
            }
        }

        [HttpGet("getActivityById")]
        public async Task<IActionResult> GetActivityById(string activityId)
        {
            try
            {
                if (string.IsNullOrEmpty(activityId))
                {
                    return Json(new { IsSuccess = false, Message = "Activity Id is null or empty." });
                }
                else
                {
                    var activityDetail = await _activityService.GetActivityById(activityId);

                    return Json(new { IsSuccess = activityDetail is object, activity = activityDetail });
                }
            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, Message = e.Message });
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteActivity(string activityId)
        {
            try
            {
                if (string.IsNullOrEmpty(activityId))
                {
                    return Json(new { IsSuccess = false, Message = "Activity Id is empty." });
                }
                else
                {
                    var activityDeleted = await _activityService.DeleteActivity(activityId);
                    return Json(new { IsSuccess = activityDeleted });
                }
            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, Message = e.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateActivity([FromBody] ActivitiesViewModel activity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { IsSuceess = false, Message = "Someting went wrong." });
                }
                else
                {
                    var isUpdated = await _activityService.UpdateActivity(activity);
                    return Json(new { IsSuccess = isUpdated });
                }
            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, Message = e.Message });
            }
        }
    }
}