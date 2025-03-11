using Lezione10.Dev.Data;
using Lezione10.Dev.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;

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

        [HttpPost]
        public IActionResult GetMany(List<int> ids)
        {
            //List<Student> resultLinq = (from s in _ctx.Students
            //                  join id in ids
            //                  on s.Id equals id
            //                  select s).ToList();


            var newResult = _ctx.Students.Join(ids,
                s => s.Id,
                id => id,
                (s, id) => s).ToList();
            return Ok(newResult);
        }

        /// <summary>
        /// API che restituisca i migliori 3 studenti per media ponderata
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        [Route("Top")]
        public IActionResult GetTopStudents()
        {
            var query = _ctx.Students.Select(s => new
            {
                s.Id,
                s.Name,
                s.Surname,
                Average = (double)s.Exams.Sum(e => e.Grade * e.Subject.Credits) / s.Exams.Sum(e => e.Subject.Credits)
            });

            query = query.OrderByDescending(s => s.Average).Take(3);

            //query = (from s in _ctx.Students
            //         select new
            //         {
            //             s.Id,
            //             s.Name,
            //             s.Surname,
            //             Average = (double)s.Exams.Sum(e => e.Grade * e.Subject.Credits) / s.Exams.Sum(e => e.Subject.Credits)
            //         } into q
            //         orderby q.Average descending
            //         select q
            //         ).Take(3);

            return Ok(query);
        }

        /// <summary>
        /// API che dichiari, dato uno studente, quale media futura dovrà tenere 
        /// per arrivare alla media del 28
        /// PRESUPPOSTI: inserire abbastanza esami in database da raggiungere
        /// ESATTAMENTE 180 crediti totali, massimo 15 crediti per esame
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        [Route("FutureAvg/{id}")]
        public IActionResult GetFutureAvg(int id)
        {
            _ctx.Subjects.Where(s => !s.Exams.Any(e => e.StudentId == id)).Sum(e => e.Credits);

            _ctx.Subjects.Except(_ctx.Exams.Where(e => e.StudentId == id).Select(e => e.Subject));

            Student? student = _ctx.Students
                .Include(s => s.Exams)
                .ThenInclude(e => e.Subject)
                .SingleOrDefault(s => s.Id == id);

            if (student == null)
                return BadRequest();
            int totCredits = _ctx.Subjects.Sum(s => s.Credits);
            int actualCredits = student.Exams.Sum(e => e.Subject.Credits);
            int weightedGrades = student.Exams.Sum(e => e.Grade * e.Subject.Credits);

            double result = Math.Max((28 * totCredits - weightedGrades) / (totCredits - actualCredits),18);
            if (result > 30)
                return Ok("SEEEEEEEEE VOLEVI!!!!!!");
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
