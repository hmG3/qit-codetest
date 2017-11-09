using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using QITCodeTest.Service.Controllers;
using QITCodeTest.Service.Models;

namespace QITCodeTest.Service.Tests
{
    [TestFixture]
    public class TestStudentsController
    {
        private StudentsController _testController;
        private SchoolDbContext _dbContext;

        [SetUp]
        public void TestSetup()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);

            var options = new DbContextOptionsBuilder<SchoolDbContext>()
                .UseSqlite(connection)
                .Options;

            _dbContext = new SchoolDbContext(options);
            _dbContext.Database.OpenConnection();
            _dbContext.Database.EnsureCreated();
            _dbContext.EnsureSeeded();

            _testController = new StudentsController(_dbContext);
        }

        [Test]
        public void GetProducts_ShouldReturnAllStudents()
        {
            Class testStudentClass = _dbContext.Classes.First();

            var result = _testController.GetStudentsOfClass(testStudentClass.Id) as OkObjectResult;
            Assert.IsNotNull(result);

            var resultValue = result.Value as IEnumerable<Student>;
            Assert.IsNotNull(resultValue);
            Assert.IsTrue(resultValue.All(s => s.ClassId == testStudentClass.Id));
        }

        [Test]
        public void GetProduct_ShouldReturnStudentWithSameId()
        {
            Student testItem = _dbContext.Students.First();

            var result = _testController.GetStudent(testItem.Id).GetAwaiter().GetResult() as OkObjectResult;
            Assert.IsNotNull(result);

            var resultValue = result.Value as Student;
            Assert.IsNotNull(resultValue);

            Assert.AreEqual(testItem.Id, resultValue.Id);
        }

        [Test]
        public void GetProduct_ShouldReturnNotFoundStatusCode_ForMissingStudent()
        {
            Guid testItemId = Guid.NewGuid();

            var result = _testController.GetStudent(testItemId).GetAwaiter().GetResult() as StatusCodeResult;
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
        }

        [Test]
        public void PutProduct_ShouldReturnNoContentStatusCode()
        {
            Student testItem = _dbContext.Students.First();

            var result = _testController.PutStudent(testItem.Id, testItem).GetAwaiter().GetResult() as StatusCodeResult;
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
        }

        [Test]
        public void PutProduct_ShouldFail_WhenDifferentId()
        {
            Student testItem = _dbContext.Students.First();
            Guid testItemId = Guid.NewGuid();

            var result = _testController.PutStudent(testItemId, testItem).GetAwaiter().GetResult() as StatusCodeResult;
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Test]
        public void PutProduct_ShouldFail_WhenDifferentClassId()
        {
            Student testItem = _dbContext.Students.First();
            testItem.ClassId = Guid.NewGuid();

            var result = _testController.PutStudent(testItem.Id, testItem).GetAwaiter().GetResult() as ObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual("Cannot find parent Class by id.", result.Value);
        }

        [Test]
        public void PutProduct_ShouldReturnConflictStatusCode_ForDuplicateStudent()
        {
            Class testStudentClass = _dbContext.Classes.First();
            var testItem = new Student { Name = "testname", Surname = "testuniquesurname", DOB = DateTime.Now.AddYears(-20), GPA = 1.0, ClassId = testStudentClass.Id };
            Student testDuplicateItem = _dbContext.Students.First();
            testDuplicateItem.Surname = "testuniquesurname";

            _testController.PostStudent(testItem).GetAwaiter().GetResult();
            var result = _testController.PutStudent(testDuplicateItem.Id, testDuplicateItem).GetAwaiter().GetResult() as ContentResult;
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.Conflict, result.StatusCode);
            Assert.AreEqual("Student with the same surname already exists.", result.Content);
        }

        [Test]
        public void PostProduct_ShouldReturnSameProduct()
        {
            Class testStudentClass = _dbContext.Classes.First();
            var testItem = new Student { Name = "testname", Surname = "testsurname", DOB = DateTime.Now.AddYears(-20), GPA = 1.0, ClassId = testStudentClass.Id };
            var result = _testController.PostStudent(testItem).GetAwaiter().GetResult() as CreatedAtActionResult;

            Assert.IsNotNull(result);

            var resultValue = result.Value as Student;
            Assert.IsNotNull(resultValue);

            Assert.AreEqual(result.RouteValues["id"], resultValue.Id);
            Assert.IsTrue(resultValue.Equals(testItem));
        }

        [Test]
        public void PostProduct_ShouldFail_WhenDifferentClassId()
        {
            Student testItem = _dbContext.Students.First();
            testItem.ClassId = Guid.NewGuid();

            var result = _testController.PostStudent(testItem).GetAwaiter().GetResult() as ObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual("Cannot find parent Class by id.", result.Value);
        }

        [Test]
        public void PostProduct_ShouldReturnConflictStatusCode_ForDuplicateStudent()
        {
            Class testStudentClass = _dbContext.Classes.First();
            var testItem = new Student { Name = "testname", Surname = "testuniquesurname", DOB = DateTime.Now.AddYears(-20), GPA = 1.0, ClassId = testStudentClass.Id };
            var testDuplicateItem = new Student { Name = "testname2", Surname = "testuniquesurname", DOB = DateTime.Now.AddYears(-22), GPA = 2.0, ClassId = testStudentClass.Id };

            _testController.PostStudent(testItem).GetAwaiter().GetResult();
            var result = _testController.PostStudent(testDuplicateItem).GetAwaiter().GetResult() as ContentResult;
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.Conflict, result.StatusCode);
            Assert.AreEqual("Student with the same surname already exists.", result.Content);
        }

        [Test]
        public void DeleteProduct_ShouldReturnNoContentStatusCode()
        {
            Student testItem = _dbContext.Students.First();

            var result = _testController.DeleteStudent(testItem.Id).GetAwaiter().GetResult() as StatusCodeResult;

            Assert.IsNotNull(result);

            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
        }

        [Test]
        public void DeleteProduct_ShouldReturnNotFoundStatusCode_ForMissingStudent()
        {
            Guid testItemId = Guid.NewGuid();

            var result = _testController.DeleteStudent(testItemId).GetAwaiter().GetResult() as StatusCodeResult;
            Assert.IsNotNull(result);

            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
        }

        [TearDown]
        public void Dispose()
        {
            _testController.Dispose();
            _dbContext.Database.CloseConnection();
            _dbContext.Dispose();
        }
    }
}
