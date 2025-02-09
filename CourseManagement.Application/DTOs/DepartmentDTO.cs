using CourseManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagement.Application.DTOs
{
    public class DepartmentDTO : BaseEntity
    {
        public string Name { get; set; }
    }
}
