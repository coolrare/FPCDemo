using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFCoreDemo.Models;
using EFCoreDemo.Models.Dto;
using Omu.ValueInjecter;

namespace EFCoreDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ContosoUniversityContext _context;

        public CoursesController(ContosoUniversityContext context)
        {
            _context = context;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourse()
        {
            return await _context.Course.Include(p => p.Department)
                .Select(p => new CourseDto()
                {
                    DepartmentName = p.Department.Name,
                    CourseId = p.CourseId,
                    Credits = p.Credits,
                    DepartmentId = p.DepartmentId,
                    Title = p.Title,
                    DateCreated = p.DateCreated,
                    DateModified = p.DateModified
                }).ToListAsync();
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<CourseDto>> GetCourse(int id)
        {
            var course = await _context.Course.Include(p => p.Department).FirstOrDefaultAsync(p => p.CourseId == id);

            if (course == null)
            {
                return NotFound();
            }

            var courseDto = new CourseDto();

            courseDto.InjectFrom(course);

            courseDto.DepartmentName = course.Department.Name;

            return courseDto;
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PutCourse(int id, CourseUpdateDto course)
        {
            var c = await _context.Course.FindAsync(id);

            if (c == null)
            {
                return NotFound();
            }

            c.InjectFrom(course);

            //c.DateModified = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<CourseDto>> PostCourse(CourseCreateDto courseDto)
        {
            if (_context.Course == null)
            {
                return Problem("Entity set 'ContosoUniversityContext.Course'  is null.");
            }

            var c = new Course();
            c.InjectFrom(courseDto);

            //c.DateCreated = DateTime.Now;

            _context.Course.Add(c);
            await _context.SaveChangesAsync();

            var c1 = await _context.Course.Include(p => p.Department).FirstOrDefaultAsync(p => p.CourseId == c.CourseId);
            var c2 = new CourseDto();

            c2.InjectFrom(c1);
            c2.DepartmentName = c1?.Department.Name;

            return CreatedAtAction("GetCourse", new { id = c2.CourseId }, c2);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Course.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CourseExists(int id)
        {
            return (_context.Course?.Any(e => e.CourseId == id)).GetValueOrDefault();
        }
    }
}
