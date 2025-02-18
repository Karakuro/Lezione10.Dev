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
    }
}
