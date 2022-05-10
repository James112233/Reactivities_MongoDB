using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Domain.Contracts;
using Application.Contracts;
using Application.Models;
using Application.ViewModels;
using MongoDB.Bson;

namespace API.Domain.Services
{
    public class ActivityService : IActivityService
    {
        public IRepository<Activities> _activitiesRepository;
        public ActivityService(IRepository<Activities> activitiesRepository)
        {
            _activitiesRepository = activitiesRepository;
        }

        public async Task<bool> CreateActivity(Activities newActivity)
        {
            try
            {
                if (newActivity == null) throw new ApplicationException("Activity is not completed.");
                var objectId = ObjectId.GenerateNewId().ToString();
                var newActivityObj = new Activities
                {
                    Id = objectId,
                    Title = newActivity.Title,
                    Date = newActivity.Date,
                    Description = newActivity.Description,
                    Category = newActivity.Category,
                    City = newActivity.City,
                    Venue = newActivity.Venue,
                    IsCancelled = false,
                    IsDelete = false
                };
                await _activitiesRepository.Add(newActivityObj);
                return true;
            }
            catch (Exception e)
            {
                throw new ApplicationException("Create activity error" + e.Message);
            }
        }

        public async Task<bool> DeleteActivity(string activityId)
        {
            try
            {
                var activity = await _activitiesRepository.GetByIdAsync(activityId);
                if (activity == null)
                    throw new ApplicationException("Activity Id not found or activtiy has already deleted.");
                await _activitiesRepository.Delete(activity);
                return true;
            }
            catch (Exception e)
            {
                throw new ApplicationException("Delete activity error " + e.Message);
            }
        }

        public List<ActivitiesViewModel> GetActivities(int limit, int skip)
        {
            try
            {
                int getLimit = limit == 0 ? 10 : limit;
                var activities = _activitiesRepository.GetQueryable().Select(x => new ActivitiesViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Date = x.Date,
                    Description = x.Description,
                    Category = x.Category,
                    City = x.City,
                    Venue = x.Venue
                }).OrderByDescending(x => x.Id).Skip(skip).Take(getLimit).ToList();
                return activities;
            }
            catch (Exception e)
            {

                throw new ApplicationException(e.Message);
            }
        }

        public async Task<ActivitiesViewModel> GetActivityById(string activityId)
        {
            try
            {
                var activity = await _activitiesRepository.GetByIdAsync(activityId);
                if (activity == null) throw new ApplicationException("This activity is not available.");

                var viewActivity = new ActivitiesViewModel
                {
                    Id = activity.Id,
                    Title = activity.Title,
                    Date = activity.Date,
                    Description = activity.Description,
                    Category = activity.Category,
                    City = activity.City,
                    Venue = activity.Venue,
                    IsCancelled = activity.IsCancelled
                };
                return viewActivity;
            }
            catch (Exception e)
            {
                throw new ApplicationException("Can not get the Activity with error " + e.Message);
            }
        }

        public async Task<bool> UpdateActivity(ActivitiesViewModel activity)
        {
            try
            {
                var existActivity = _activitiesRepository.GetByIdAsync(activity.Id);
                if (existActivity == null)
                    throw new ApplicationException("This activity is not available to update.");

                var updActivity = new Activities
                {
                    Id = activity.Id,
                    Title = activity.Title,
                    Date = activity.Date,
                    Description = activity.Description,
                    Category = activity.Category,
                    City = activity.City,
                    Venue = activity.Venue,
                    IsCancelled = activity.IsCancelled
                };
                await _activitiesRepository.Update(updActivity);
                return true;
            }
            catch (Exception e)
            {
                throw new ApplicationException("Can not update activity with error " + e.Message);
            }
        }
    }
}
