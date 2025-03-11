using Lezione10.Dev.Data;
using Lezione10.Dev.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lezione10.Dev.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private readonly SchoolDbContext _ctx;
        private readonly Mapper _mapper;

        public ExamController(SchoolDbContext ctx, Mapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<ExamDTO> result = _ctx.Exams.ToList()
                .ConvertAll(_mapper.MapEntityToDto);
            return Ok(result);
        }
        [HttpGet]
        [Route("{studentId}/{subjectId}")]
        public IActionResult GetSingle(int studentId,int subjectId)
        {
            Exam? result = _ctx.Exams.Find(new { studentId, subjectId });
            if (result == null)
                return NotFound($"ID studente {studentId} e materia {subjectId} non trovato");
            return Ok(_mapper.MapEntityToDto(result));
        }

        [HttpPost]
        public IActionResult Create(ExamDTO Exam)
        {
            Exam entity = _mapper.MapDtoToEntity(Exam);

            _ctx.Exams.Add(entity);
            _ctx.SaveChanges();
            return Created("", _mapper.MapEntityToDto(entity));
        }

        [HttpPost]
        [Route("All")]
        public IActionResult CreateAll(List<ExamDTO> Exams)
        {
            List<Exam> entities = Exams.ConvertAll(_mapper.MapDtoToEntity);
            _ctx.Exams.Join(entities, e => new { e.StudentId, e.SubjectId }, e => new { e.StudentId, e.SubjectId }, )
            _ctx.SaveChanges();
            return Ok();
        }

        [HttpPut]
        public IActionResult Update(ExamDTO exam)
        {
            Exam? result = _ctx.Exams.Find(new { exam.StudentId, exam.SubjectId });
            if (result == null)
                return NotFound($"ID studente {exam.StudentId} e materia {exam.SubjectId} non trovato");
            result.Grade = exam.Grade;
            _ctx.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("{studentId}/{subjectId}")]
        public IActionResult Delete(int studentId, int subjectId)
        {
            Exam? result = _ctx.Exams.Find(new { studentId, subjectId });
            if (result == null)
                return NotFound($"ID studente {studentId} e materia {subjectId} non trovato");
            _ctx.Exams.Remove(result);
            _ctx.SaveChanges();
            return Ok();
        }
    }
}
