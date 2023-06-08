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
    /// <summary>
    /// 課程管理 API
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ContosoUniversityContext _context;

        public CoursesController(ContosoUniversityContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 取得課程清單
        /// </summary>
        /// <returns>CourseDto</returns>
        [HttpGet(Name = nameof(GetCourseAll))]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourseAll()
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

        /// <summary>
        /// 取得單筆課程資料
        /// </summary>
        /// <param name="id">CourseId</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = nameof(GetCourseById))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<CourseDto>> GetCourseById(int id)
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

        [HttpGet("{*dt:datetime}", Name = nameof(GetCourseByDate))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourseByDate(DateTime dt)
        {
            return await _context.Course.Include(p => p.Department)
                .Where(p => p.DateCreated >= dt)
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

        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}", Name = nameof(UpdateCourse))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> UpdateCourse(int id, CourseUpdateDto course)
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

        /// <summary>
        /// 新增課程
        /// </summary>
        /// <param name="courseDto"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///       "title": "111",
        ///       "credits": "2",
        ///       "departmentId": 1
        ///     }
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        [HttpPost(Name = nameof(CreateCourse))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<CourseDto>> CreateCourse(CourseCreateDto courseDto)
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

            return CreatedAtAction(nameof(GetCourseById), new { id = c2.CourseId }, c2);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}", Name = nameof(DeleteCourse))]
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
