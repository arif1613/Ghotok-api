using System;
using System.Linq;
using Ghotok.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using QQuery.Context;

namespace GhotokApi.Context
{
    public class GhotokDbContext : QqContext
    {
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<User> Users { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{

        //    //modelBuilder.Entity<UserShortInfo>(entity =>
        //    //{
        //    //    entity.HasNoKey();

        //    //    entity.ToView("UserShortInfo");

        //    //    entity.Property(e => e.Id).HasColumnName("Id");
        //    //    entity.Property(e => e.ContactNumber).HasColumnName("ContactNumber");
        //    //    entity.Property(e => e.Dob).HasColumnName("Dob");
        //    //    entity.Property(e => e.IsPictureUploaded).HasColumnName("IsPictureUploaded");
        //    //    entity.Property(e => e.LookingForBride).HasColumnName("LookingForBride");
        //    //    entity.Property(e => e.MaritalStatus).HasColumnName("MaritalStatus");
        //    //    entity.Property(e => e.Name).HasColumnName("Name");
        //    //    entity.Property(e => e.PictureName).HasColumnName("PictureName");
        //    //    entity.Property(e => e.email).HasColumnName("email");
        //    //});
        //}
        
    }
}
