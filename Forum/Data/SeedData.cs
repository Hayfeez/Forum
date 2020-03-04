﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Models;
using Microsoft.AspNetCore.Identity;

namespace Forum.Data
{
    public class SeedData
    {
        private readonly ApplicationDbContext _context;
        public SeedData(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task Seed()
        {
            try
            {
                if (_context.Channels.Any())
                    return Task.CompletedTask;

                var subscriber = new Subscriber { Description = "Main Forum", DateCreated = DateTime.Now, Domain = "", HeaderImageUrl = "", Name = "" };

                var channel1 = new Channel { Subscriber = subscriber, DateCreated = DateTime.Now, Description = "Career", LogoUrl = "", Title = "Career" };
                var channel2 = new Channel { Subscriber = subscriber, DateCreated = DateTime.Now, Description = "Sports", LogoUrl = "", Title = "Sports" };
                var channel3 = new Channel { Subscriber = subscriber, DateCreated = DateTime.Now, Description = "General", LogoUrl = "", Title = "General" };


                var channels = new List<Channel> { channel1, channel2, channel3 };

                var category1 = new Category { Channel = channel1, DateCreated = DateTime.Now, Description = "For College Students", Title = "High School/College" };
                var category2 = new Category { Channel = channel1, DateCreated = DateTime.Now, Description = "For University Undergraduates", Title = "University" };
                var category3 = new Category { Channel = channel1, DateCreated = DateTime.Now, Description = "Post Graduates", Title = "PostGraduate" };
                var category4 = new Category { Channel = channel2, DateCreated = DateTime.Now, Description = "English Premier League", Title = "English Premier League" };
                var category5 = new Category { Channel = channel2, DateCreated = DateTime.Now, Description = "UEFA Champions League", Title = "UEFA Champions League" };
                var category6 = new Category { Channel = channel3, DateCreated = DateTime.Now, Description = "Fun Facts", Title = "Fun Facts" };

                var categories = new List<Category> { category1, category2, category3, category4, category5, category6 };

                var userId = Guid.NewGuid().ToString();
                var superUser = new ApplicationUser { Id = userId, Email = "admin@forum.com", NormalizedEmail = "admin@forum.com", EmailConfirmed = true, LockoutEnabled = false, UserName = "forumadmin", NormalizedUserName = "forumadmin" };
                var hasher = new PasswordHasher<ApplicationUser>();
                string password = hasher.HashPassword(superUser, "password");
                superUser.PasswordHash = password;

                var subscriberUser = new SubscriberUser { ApplicationUser = superUser, DateJoined = DateTime.Now, IsActive = true, Subscriber = subscriber, Rating = 0.0, ProfileImageUrl = "", HeaderImageUrl = "" };

                _context.Subscribers.Add(subscriber);
                _context.Channels.AddRange(channels);
                _context.Categories.AddRange(categories);
                _context.ApplicationUsers.Add(superUser);
                _context.SubscriberUsers.Add(subscriberUser);

                _context.SaveChanges();

            }
            catch (Exception e)
            {

            }
            return Task.CompletedTask;
        }

        public Task ClearDB()
        {
            try
            {
                var subscribers = _context.Subscribers.ToList();
                var channels = _context.Channels.ToList();
                var categories = _context.Categories.ToList();
                var appUsers = _context.ApplicationUsers.ToList();
                var subscriberUsers = _context.SubscriberUsers.ToList();

                categories.ForEach(a => _context.Categories.Remove(a));
                channels.ForEach(a => _context.Channels.Remove(a));
                subscriberUsers.ForEach(a => _context.SubscriberUsers.Remove(a));
                appUsers.ForEach(a => _context.ApplicationUsers.Remove(a));
                subscribers.ForEach(a => _context.Subscribers.Remove(a));

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
               
            }
            return Task.CompletedTask;
        }
    }
}