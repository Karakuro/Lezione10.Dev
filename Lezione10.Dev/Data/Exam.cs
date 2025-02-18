using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lezione10.Dev.Data
{
    [PrimaryKey(nameof(StudentId), nameof(SubjectId))]
    public class Exam
    {
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public int Grade { get; set; }
        [ForeignKey(nameof(StudentId))]
        public Student? Student { get; set; }
        [ForeignKey(nameof(SubjectId))]
        public Subject? Subject { get; set; }
    }
}
