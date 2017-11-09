using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QITCodeTest.Service.Models;

namespace QITCodeTest.Service.Controllers
{
    [Produces("application/json")]
    [Route("api/classes")]
    [AllowAnonymous]
    public class ClassesController : Controller
    {
        private readonly SchoolDbContext _dbContext;

        public ClassesController(SchoolDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        /// <summary>
        /// Get all сlasses
        /// </summary>
        /// <returns>
        /// 200 [OK] – collection of Classes;
        /// </returns>
        /// <example>GET: api/classes</example>
        [HttpGet]
        public IActionResult GetClasses()
        {
            var classes = _dbContext.Classes;
            return Ok(classes);
        }

        /// <summary>
        /// Get class
        /// </summary>
        /// <param name="id">Class Id</param>
        /// <returns>
        /// 404 [NotFound] – Class not found by Id;
        /// <para> </para>
        /// 200 [OK] – Class;
        /// </returns>
        /// <example>GET: api/classes/15bb9dbe-2df2-4c62-94b7-c523e1cacfe0</example>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClass([FromRoute] Guid id)
        {
            Class @class = await _dbContext.Classes.FindAsync(id);
            if (@class == null)
            {
                return NotFound();
            }

            return Ok(@class);
        }

        /// <summary>
        /// Update сlass
        /// </summary>
        /// <param name="id">Class Id to update</param>
        /// <param name="class">target Class</param>
        /// <returns>
        /// 500 [Internal Server Error] – model is not valid;
        /// <para> </para>
        /// 400 [Bad Request] – supplied Class Id is not equal to target Class Id;
        /// <para> </para>
        /// 404 [NotFound] – Class not found by Id;
        /// <para> </para>
        /// 200 [OK] – Class updated successfully;
        /// </returns>
        /// <example>PUT: api/classes/15bb9dbe-2df2-4c62-94b7-c523e1cacfe0</example>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClass([FromRoute] Guid id, [FromBody] Class @class)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != @class.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(@class).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _dbContext.Classes.FindAsync(id) == null)
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        /// <summary>
        /// Add new сlass
        /// </summary>
        /// <param name="class">target Class</param>
        /// <returns>
        /// 500 [Internal Server Error] – model is not valid;
        /// <para> </para>
        /// 201 [Created] – new Class created successfully;
        /// </returns>
        /// <example>POST: api/classes</example>
        [HttpPost]
        public async Task<IActionResult> PostClass([FromBody] Class @class)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dbContext.Classes.Add(@class);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction("GetClass", new { id = @class.Id }, @class);
        }

        /// <summary>
        /// Remove existing сlass
        /// </summary>
        /// <param name="id">Class Id to remove</param>
        /// <returns>
        /// 404 [NotFound] – Class not found by Id;
        /// <para> </para>
        /// 200 [OK] – Class deleted successfully;
        /// </returns>
        /// <example>DELETE: api/classes/15bb9dbe-2df2-4c62-94b7-c523e1cacfe0</example>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClass([FromRoute] Guid id)
        {
            Class studentClass = await _dbContext.Classes.FindAsync(id);
            if (studentClass == null)
            {
                return NotFound();
            }

            _dbContext.Classes.Remove(studentClass);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}