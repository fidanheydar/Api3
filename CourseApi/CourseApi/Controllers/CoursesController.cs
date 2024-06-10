using CourseApi.Data;
using CourseApi.Data.Entities;
using CourseApi.Dtos.CourseDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CoursesController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet("")]
        public ActionResult<List<CourseGetAllDto>> GetAll(int page = 1, int pageSize = 2)
        {
            List<CourseGetAllDto> data = _context.Courses.Skip((page - 1) * pageSize).Take(pageSize).Select(x => new CourseGetAllDto
            {
               Name=x.Name,
                Limit = x.Limit,
                StudentCount = x.Students.Count
            }).ToList();

            return StatusCode(200, data);
        }

        [HttpGet("{id}")]
        public ActionResult<CourseGetByIdDto> GetById(int id)
        {
            var data = _context.Courses.Find(id);

            if (data == null)
            {
                return StatusCode(404, data);
            }
            CourseGetByIdDto courseGetByIdDto = new CourseGetByIdDto()
            {
               
                Name = data.Name,
                Limit = data.Limit
            };

            return StatusCode(200, courseGetByIdDto);
        }

        [HttpPost("")]
        public ActionResult Create(CourseCreateDto courseCreateDto)
        {
            if (_context.Courses.Any(x => x.Name == courseCreateDto.Name && !x.IsDeleted))
                return StatusCode(409);
            Course course = new Course
            {
                Name = courseCreateDto.Name,
                Limit = courseCreateDto.Limit
            };

            _context.Courses.Add(course);
            _context.SaveChanges();

            return StatusCode(201);
        }


        [HttpPut("{id}")]
        public ActionResult Update(CourseUpdateDto courseUpdateDto)
        {
            if (_context.Courses.Any(x => x.Name == courseUpdateDto.Name && !x.IsDeleted))
                return StatusCode(409);
            var existCourse = _context.Courses.Find(courseUpdateDto.Id);
            if (existCourse == null)
            {
                return StatusCode(404, courseUpdateDto);
            }

            existCourse.Name = courseUpdateDto.Name;
            existCourse.Limit = courseUpdateDto.Limit;
            existCourse.ModifiedAt = DateTime.Now;
            _context.Courses.Update(existCourse);
            _context.SaveChanges();

            return StatusCode(200);
        }


        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var  existCourse = _context.Courses.FirstOrDefault(x => x.Id == id);
            if (existCourse == null)
            {
                return NotFound();
            }
            existCourse.IsDeleted = true;
            _context.Courses.Remove(existCourse);
            _context.SaveChanges();
            return StatusCode(204);
        }

    }
}
