using Lezione10.Dev.Data;
using Lezione10.Dev.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Lezione10.Dev.Controllers
{
    // api/Student
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly SchoolDbContext _ctx;
        private readonly Mapper _mapper;

        public StudentController(SchoolDbContext ctx, Mapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        // https://localhost:7447/api/Student
        [HttpGet]
        public IActionResult GetAll()
        {
            List<StudentDTO> result = _ctx.Students.ToList()
                .ConvertAll(_mapper.MapEntityToDto);
            return Ok(result);
        }

        // https://localhost:7447/api/Student/1

        // https://localhost:7447/1
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetSingle(int id)
        {
            Student? result = _ctx.Students.SingleOrDefault(s => s.Id == id);
            if (result == null)
                return NotFound($"ID {id} non trovato");
            return Ok(_mapper.MapEntityToDto(result));
        }

        [HttpPost]
        public IActionResult Create(StudentDTO student)
        {
            Student entity = _mapper.MapDtoToEntity(student);

            _ctx.Students.Add(entity);
            _ctx.SaveChanges();
            return Created("", _mapper.MapEntityToDto(entity));
        }

        [HttpPut]
        public IActionResult Update(StudentDTO student)
        {
            Student? result = _ctx.Students.Find(student.Id);
            if (result == null)
                return NotFound($"ID {student.Id} non trovato");
            result.Name = student.Name;
            result.Surname = student.Surname;
            _ctx.SaveChanges();
            return Ok();
        }

        // https://localhost:7447/api/Student/1

        // https://localhost:7447/1
        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            Student? result = _ctx.Students.Find(id);
            if (result == null)
                return NotFound($"ID {id} non trovato");
            _ctx.Students.Remove(result);
            _ctx.SaveChanges();
            return Ok();
        }
    }
}
