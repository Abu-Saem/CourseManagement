using CourseManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagement.Domain.Entities
{
    public class Enrollment : BaseEntity
    {
        public int StudentId { get; set; }
        public Student Student { get; set; } // Navigation Property

        public int CourseId { get; set; }
        public Course Course { get; set; } // Navigation Property

    }
}
