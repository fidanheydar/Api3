using CourseApi.Data;
using CourseApi.Data.Entities;
using CourseApi.Dtos.CourseDtos;
using CourseApi.Dtos.StudentDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet("")]
        public ActionResult<List<StudentGetAllDto>> GetAll(int page = 1, int pageSize = 2)
        {
            List<StudentGetAllDto> data = _context.Students.Skip((page - 1) * pageSize).Take(pageSize).Select(x => new StudentGetAllDto
            {
                FullName = x.FullName,
                Email = x.Email,
                CourseName=x.Course.Name,
                Birthdate = x.BirthDate
            }).ToList();

            return StatusCode(200, data);
        }


        [HttpGet("{id}")]
        public ActionResult<StudentGetByIdDto> GetById(int id)
        {
            var data = _context.Students.Find(id);
            if (data == null)
            {
                return StatusCode(404, data);
            }
            StudentGetByIdDto courseGetByIdDto = new StudentGetByIdDto()
            {
                FullName = data.FullName,
                Email = data.Email,
                Birthdate = data.BirthDate,
                CourseName = data.Course.Name
            };
            return StatusCode(200, courseGetByIdDto);
        }



        [HttpPost("")]
        public ActionResult Create(StudentCreateDto studentCreateDto)
        {
            Course course = _context.Courses.FirstOrDefault(x => x.Id == studentCreateDto.CourseId);
            if (course == null)
            {
                return StatusCode(404, studentCreateDto);
            }

            if (course.Limit <= _context.Students.Count(s => s.CourseId == studentCreateDto.CourseId))
            {
                return StatusCode(400, "Course limit is reached!");
            }

            Student student = new Student
            {
                FullName = studentCreateDto.FullName,
                Email = studentCreateDto.Email,
                BirthDate = studentCreateDto.Birthdate,
                CourseId = studentCreateDto.CourseId,
                CreatedAt = DateTime.Now
            };

            _context.Students.Add(student);
            _context.SaveChanges();
            return StatusCode(201);
        }


        [HttpPut("{id}")]
        public ActionResult Update(int id, StudentUpdateDto studentUpdateDto)
        {
            var existStudent = _context.Students.Find(id);
            if (existStudent == null)
            {
                return StatusCode(404, studentUpdateDto);
            }

            Course course = _context.Courses.FirstOrDefault(x => x.Id == studentUpdateDto.CourseId);
            if (course == null)
            {
                return StatusCode(404, "Course not found.");
            }


            if (course.Limit <= _context.Students.Count(s => s.CourseId == studentUpdateDto.CourseId && s.Id != id))
            {
                return StatusCode(400, "Course limit is reached!");
            }

            existStudent.FullName = studentUpdateDto.FullName;
            existStudent.Email = studentUpdateDto.Email;
            existStudent.BirthDate = studentUpdateDto.Birthdate;
            existStudent.CourseId = studentUpdateDto.CourseId;
            existStudent.ModifiedAt = DateTime.Now;

            _context.Students.Update(existStudent);
            _context.SaveChanges();

            return StatusCode(204);
        }




        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var student = _context.Students.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (student == null)
            {
                return NotFound();
            }
            student.IsDeleted = true;
            _context.SaveChanges();
            return StatusCode(204);
        }
    }
}
