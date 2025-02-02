using CourseManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagement.Domain.Entities
{
    public class Student : BaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int DepartmentId { get; set; }  // Foreign Key
        public Department Department { get; set; }  // Navigation Property
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>(); // Many-to-Many
    }
}
