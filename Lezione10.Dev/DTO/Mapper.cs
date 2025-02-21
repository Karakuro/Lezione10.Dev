using Lezione10.Dev.Data;

namespace Lezione10.Dev.DTO
{
    public class Mapper
    {
        public StudentDTO MapEntityToDto(Student entity)
        {
            return new StudentDTO()
            {
                Name = entity.Name,
                Surname = entity.Surname,
                Id = entity.Id
            };
        }

        public Student MapDtoToEntity(StudentDTO student)
        {
            return new Student()
            {
                Name = student.Name,
                Surname = student.Surname,
                Id = student.Id
            };
        }

        public SubjectDTO MapEntityToDto(Subject entity)
        {
            return new SubjectDTO()
            {
                Title = entity.Title,
                Credits = entity.Credits,
                Id = entity.Id
            };
        }
        public Subject MapDtoToEntity(SubjectDTO dto)
        {
            return new Subject()
            {
                Title = dto.Title,
                Credits = dto.Credits,
                Id = dto.Id
            };
        }

        public ExamDTO MapEntityToDto(Exam entity)
        {
            return new ExamDTO()
            {
                StudentId = entity.StudentId,
                SubjectId = entity.SubjectId,
                Grade = entity.Grade
            };
        }
        public Exam MapDtoToEntity(ExamDTO dto)
        {
            return new Exam()
            {
                StudentId = dto.StudentId,
                SubjectId = dto.SubjectId,
                Grade = dto.Grade
            };
        }
    }
}
