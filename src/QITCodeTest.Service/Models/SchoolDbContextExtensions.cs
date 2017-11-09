using System;
using System.Collections.Generic;
using System.Linq;

namespace QITCodeTest.Service.Models
{
    /// <summary>
    /// Workaround for EF core not having Seed support https://github.com/aspnet/EntityFramework/issues/629
    /// </summary>
    public static class SchoolDbContextExtensions
    {
        public static void EnsureSeeded(this SchoolDbContext context)
        {
            if (!context.Classes.Any())
            {
                context.Classes.AddRange(new List<Class>
                {
                    new Class
                    {
                        Name = "Biology",
                        Location = "Building 5 Room 201",
                        Teacher = "Mr. Robertson",
                        Students = new List<Student>
                        {
                            new Student
                            {
                                Name = "David",
                                Surname = "Jackson",
                                DOB = DateTime.Now.AddYears(-19),
                                GPA = 3.4
                            },
                            new Student
                            {
                                Name = "Peter",
                                Surname = "Parker",
                                DOB = DateTime.Now.AddYears(-19),
                                GPA = 2.9
                            },
                            new Student
                            {
                                Name = "Robert",
                                Surname = "Smith",
                                DOB = DateTime.Now.AddYears(-18),
                                GPA = 3.1
                            },
                            new Student
                            {
                                Name = "Rebecca",
                                Surname = "Black",
                                DOB = DateTime.Now.AddYears(-19),
                                GPA = 2.1
                            }
                        }
                    },
                    new Class
                    {
                        Name = "English",
                        Location = "Building 3 Room 134",
                        Teacher = "Ms. Sanderson",
                        Students = new List<Student>
                        {
                            new Student
                            {
                                Name = "David",
                                Surname = "Jason",
                                DOB = DateTime.Now.AddYears(-17),
                                GPA = 3.3
                            },
                            new Student{
                                Name = "Peter",
                                Surname = "Allen",
                                DOB = DateTime.Now.AddYears(-15),
                                GPA = 2.8
                            },
                            new Student
                            {
                                Name = "Robert",
                                Surname = "King",
                                DOB = DateTime.Now.AddYears(-16),
                                GPA = 3.0
                            },
                            new Student
                            {
                                Name = "Rebecca",
                                Surname = "White",
                                DOB = DateTime.Now.AddYears(-17),
                                GPA = 2.0
                            }
                        }
                    }
                });
            }
            context.SaveChanges();
        }
    }
}