﻿using AutoMapper;
using RhzLearnRest.Domains.Models;
using RhzLearnRest.Domains.Models.Dtos;

namespace RhzLearnRest.Profiles
{
    public class CoursesProfile : Profile
    {
        public CoursesProfile()
        {
            CreateMap<Course, CourseDto>();
            CreateMap<NewCourseDto, Course>();
            CreateMap<UpdateCourseDto, Course>();
            CreateMap<Course, UpdateCourseDto>();
        }
    }
}
