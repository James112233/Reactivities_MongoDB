using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Models;
using Application.ViewModels;

namespace API.Domain.Contracts
{
    public interface IActivityService
    {
        Task<bool> CreateActivity(Activities newActivity);
        Task<ActivitiesViewModel> GetActivityById(string activityId);
        List<ActivitiesViewModel> GetActivities(int limit, int skip);
        Task<bool> DeleteActivity(string activityId);
        Task<bool> UpdateActivity(ActivitiesViewModel activity);
    }
}