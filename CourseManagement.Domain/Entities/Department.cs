using CourseManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagement.Domain.Entities
{
    public class Department : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<Student> Students { get; set; } = new List<Student>(); // One-to-Many
        public ICollection<Course> Courses { get; set; } = new List<Course>(); // One-to-Many    
    }
}
