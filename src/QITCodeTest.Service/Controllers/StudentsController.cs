using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QITCodeTest.Service.Models;

namespace QITCodeTest.Service.Controllers
{
    [Produces("application/json")]
    [Route("api/students")]
    [AllowAnonymous]
    public class StudentsController : Controller
    {
        private readonly SchoolDbContext _dbContext;

        public StudentsController(SchoolDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// Get all students of specific сlass
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// 200 [OK] – collection of Students;
        /// </returns>
        /// <example>GET: api/students</example>
        [HttpGet("class/{id}")]
        public IActionResult GetStudentsOfClass([FromRoute] Guid id)
        {
            var students = _dbContext.Students.Where(s => s.ClassId == id);
            return Ok(students);
        }

        /// <summary>
        /// Get student
        /// </summary>
        /// <param name="id">current Student Id</param>
        /// <returns>
        /// 404 [NotFound] – Student not found by Id;
        /// <para> </para>
        /// 200 [OK] – Student;
        /// </returns>
        /// <example>GET: api/students/38af8159-3f8c-4914-a77e-785e5fdcc3ec</example>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent([FromRoute] Guid id)
        {
            Student student = await _dbContext.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        /// <summary>
        /// Update student
        /// </summary>
        /// <param name="id">Student Id to update</param>
        /// <param name="student">target Student</param>
        /// <returns>
        /// 500 [Internal Server Error] – model is not valid;
        /// <para> </para>
        /// 400 [Bad Request] – supplied Student Id is not equal to target Student Id;
        /// <para> </para>
        /// 404 [NotFound] – Student not found by Id;
        /// <para> </para>
        /// 200 [OK] – Student updated successfully;
        /// </returns>
        /// <example>PUT: api/students/38af8159-3f8c-4914-a77e-785e5fdcc3ec</example>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent([FromRoute] Guid id, [FromBody] Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != student.Id)
            {
                return BadRequest();
            }

            if (_dbContext.Classes.Find(student.ClassId) == null)
            {
                return BadRequest("Cannot find parent Class by id.");
            }

            _dbContext.Entry(student).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // assume that exception occurred when unique constraint failed with codes: 2601 for MSSQL and 19 for SQLite
                if (ex.InnerException.Message.ToLowerInvariant().Contains("unique"))
                {
                    return new ContentResult
                    {
                        StatusCode = StatusCodes.Status409Conflict,
                        Content = "Student with the same surname already exists."
                    };
                }
                throw;
            }

            return NoContent();
        }

        /// <summary>
        /// Add new student
        /// </summary>
        /// <param name="student">Student Name</param>
        /// <returns>
        /// 500 [Internal Server Error] – model is not valid;
        /// <para> </para>
        /// 400 [Bad Request] – Student properties are not filled properly;
        /// <para> </para>
        /// 409 [Conflict] – Student already exists with the same Surname;
        /// <para> </para>
        /// 201 [Created] – new Student created successfully;
        /// </returns>
        /// <example>POST: api/students</example>
        [HttpPost]
        public async Task<IActionResult> PostStudent([FromBody] Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_dbContext.Classes.Find(student.ClassId) == null)
            {
                return BadRequest("Cannot find parent Class by id.");
            }

            _dbContext.Students.Add(student);
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // assume that exception occurred when unique constraint failed with codes: 2601 for MSSQL and 19 for SQLite
                if (ex.InnerException.Message.ToLowerInvariant().Contains("unique"))
                {
                    return new ContentResult
                    {
                        StatusCode = StatusCodes.Status409Conflict,
                        Content = "Student with the same surname already exists."
                    };
                }
                throw;
            }

            return CreatedAtAction("GetStudent", new { id = student.Id }, student);
        }

        /// <summary>
        /// Remove existing student
        /// </summary>
        /// <param name="id">Student Id to remove</param>
        /// <returns>
        /// 404 [NotFound] – Student not found by Id;
        /// <para> </para>
        /// 200 [OK] – Student deleted successfully;
        /// </returns>
        /// <example>DELETE: api/students/38af8159-3f8c-4914-a77e-785e5fdcc3ec</example>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent([FromRoute] Guid id)
        {
            Student student = await _dbContext.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _dbContext.Students.Remove(student);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}