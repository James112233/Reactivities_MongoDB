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

        public async Task SeedData()
        {
            var activitiesInDB = _activitiesRepository.GetQueryable().ToList();

            if (activitiesInDB != null) return;

            var activities = new List<Activities>
            {
                new Activities
                {
                    Title = "Past Activity 1",
                    Date = DateTime.Now.AddMonths(-2),
                    Description = "Activity 2 months ago",
                    Category = "drinks",
                    City = "London",
                    Venue = "Pub",
                },
                new Activities
                {
                    Title = "Past Activity 2",
                    Date = DateTime.Now.AddMonths(-1),
                    Description = "Activity 1 month ago",
                    Category = "culture",
                    City = "Paris",
                    Venue = "Louvre",
                },
                new Activities
                {
                    Title = "Future Activity 1",
                    Date = DateTime.Now.AddMonths(1),
                    Description = "Activity 1 month in future",
                    Category = "culture",
                    City = "London",
                    Venue = "Natural History Museum",
                },
                new Activities
                {
                    Title = "Future Activity 2",
                    Date = DateTime.Now.AddMonths(2),
                    Description = "Activity 2 months in future",
                    Category = "music",
                    City = "London",
                    Venue = "O2 Arena",
                },
                new Activities
                {
                    Title = "Future Activity 3",
                    Date = DateTime.Now.AddMonths(3),
                    Description = "Activity 3 months in future",
                    Category = "drinks",
                    City = "London",
                    Venue = "Another pub",
                },
                new Activities
                {
                    Title = "Future Activity 4",
                    Date = DateTime.Now.AddMonths(4),
                    Description = "Activity 4 months in future",
                    Category = "drinks",
                    City = "London",
                    Venue = "Yet another pub",
                },
                new Activities
                {
                    Title = "Future Activity 5",
                    Date = DateTime.Now.AddMonths(5),
                    Description = "Activity 5 months in future",
                    Category = "drinks",
                    City = "London",
                    Venue = "Just another pub",
                },
                new Activities
                {
                    Title = "Future Activity 6",
                    Date = DateTime.Now.AddMonths(6),
                    Description = "Activity 6 months in future",
                    Category = "music",
                    City = "London",
                    Venue = "Roundhouse Camden",
                },
                new Activities
                {
                    Title = "Future Activity 7",
                    Date = DateTime.Now.AddMonths(7),
                    Description = "Activity 2 months ago",
                    Category = "travel",
                    City = "London",
                    Venue = "Somewhere on the Thames",
                },
                new Activities
                {
                    Title = "Future Activity 8",
                    Date = DateTime.Now.AddMonths(8),
                    Description = "Activity 8 months in future",
                    Category = "film",
                    City = "London",
                    Venue = "Cinema",
                }
            };
            foreach (var activity in activities)
            {
                await _activitiesRepository.Add(activity);
            }
        }
    }
}
