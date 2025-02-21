using Lezione10.Dev.Data;
using Lezione10.Dev.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lezione10.Dev.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly SchoolDbContext _ctx;
        private readonly Mapper _mapper;

        public SubjectController(SchoolDbContext ctx, Mapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<SubjectDTO> result = _ctx.Subjects.ToList()
                .ConvertAll(_mapper.MapEntityToDto);
            return Ok(result);
        }

        [HttpGet]
        [Route("Worst")]
        public IActionResult GetWorstSubjects()
        {
            var query = _ctx.Subjects.Select(s => new
            {
                s.Id,
                s.Title,
                s.Credits,
                Average = s.Exams.Average(e => e.Grade)
            }).OrderBy(s => s.Average).Take(3);

            return Ok(query);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetSingle(int id)
        {
            Subject? result = _ctx.Subjects.SingleOrDefault(s => s.Id == id);
            if (result == null)
                return NotFound($"ID {id} non trovato");
            return Ok(_mapper.MapEntityToDto(result));
        }

        [HttpPost]
        public IActionResult Create(SubjectDTO subject)
        {
            Subject entity = _mapper.MapDtoToEntity(subject);

            _ctx.Subjects.Add(entity);
            _ctx.SaveChanges();
            return Created("", _mapper.MapEntityToDto(entity));
        }

        [HttpPut]
        public IActionResult Update(SubjectDTO subject)
        {
            Subject? result = _ctx.Subjects.Find(subject.Id);
            if (result == null)
                return NotFound($"ID {subject.Id} non trovato");
            result.Title = subject.Title;
            result.Credits = subject.Credits;
            _ctx.SaveChanges();
            return Ok();
        }

        // https://localhost:7447/api/Student/1

        // https://localhost:7447/1
        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            Subject? result = _ctx.Subjects.Find(id);
            if (result == null)
                return NotFound($"ID {id} non trovato");
            _ctx.Subjects.Remove(result);
            _ctx.SaveChanges();
            return Ok();
        }
    }
}
