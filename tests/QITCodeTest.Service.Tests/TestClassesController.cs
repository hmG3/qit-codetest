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
    public class TestClassesController
    {
        private ClassesController _testController;
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

            _testController = new ClassesController(_dbContext);
        }

        [Test]
        public void GetClasses_ShouldReturnAllClasses()
        {
            var testItemsCount = _dbContext.Classes.Count();

            var result = _testController.GetClasses() as OkObjectResult;
            Assert.IsNotNull(result);

            var resultValue = result.Value as IEnumerable<Class>;
            Assert.IsNotNull(resultValue);
            Assert.AreEqual(testItemsCount, resultValue.Count());
        }

        [Test]
        public void GetClass_ShouldReturnClassWithSameId()
        {
            Class testItem = _dbContext.Classes.First();

            var result = _testController.GetClass(testItem.Id).GetAwaiter().GetResult() as OkObjectResult;
            Assert.IsNotNull(result);

            var resultValue = result.Value as Class;
            Assert.IsNotNull(resultValue);

            Assert.AreEqual(testItem.Id, resultValue.Id);
        }

        [Test]
        public void GetClass_ShouldReturnNotFoundStatusCode_ForMissingClass()
        {
            Guid testItemId = Guid.NewGuid();

            var result = _testController.GetClass(testItemId).GetAwaiter().GetResult() as StatusCodeResult;
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
        }

        [Test]
        public void PutClass_ShouldReturnNoContentStatusCode()
        {
            Class testItem = _dbContext.Classes.First();

            var result = _testController.PutClass(testItem.Id, testItem).GetAwaiter().GetResult() as StatusCodeResult;
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
        }

        [Test]
        public void PutClass_ShouldFail_WhenDifferentId()
        {
            Class testItem = _dbContext.Classes.First();
            Guid testItemId = Guid.NewGuid();

            var result = _testController.PutClass(testItemId, testItem).GetAwaiter().GetResult() as StatusCodeResult;
            Assert.IsNotNull(result);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Test]
        public void PostClass_ShouldReturnSameClass()
        {
            var testItem = new Class { Name = "testname", Location = "testlocation", Teacher = "testteacher" };
            var result = _testController.PostClass(testItem).GetAwaiter().GetResult() as CreatedAtActionResult;

            Assert.IsNotNull(result);

            var resultValue = result.Value as Class;
            Assert.IsNotNull(resultValue);

            Assert.AreEqual(result.RouteValues["id"], resultValue.Id);
            Assert.IsTrue(resultValue.Equals(testItem));
        }

        [Test]
        public void DeleteClass_ShouldReturnNoContentStatusCode()
        {
            Class testItem = _dbContext.Classes.First();

            var result = _testController.DeleteClass(testItem.Id).GetAwaiter().GetResult() as StatusCodeResult;

            Assert.IsNotNull(result);

            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
        }

        [Test]
        public void DeleteClass_ShouldReturnNotFoundStatusCode_ForMissingClass()
        {
            Guid testItemId = Guid.NewGuid();

            var result = _testController.DeleteClass(testItemId).GetAwaiter().GetResult() as StatusCodeResult;
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
