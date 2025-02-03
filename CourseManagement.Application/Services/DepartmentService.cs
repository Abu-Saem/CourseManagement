using CourseManagement.Application.Interfaces;
using CourseManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagement.Application.Services
{
    public class DepartmentService : IDepartment
    {
        private readonly IDepartment _departmentRepository;

        public DepartmentService(IDepartment departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public void AddDepartment(Department department)
        {
            _departmentRepository.AddDepartment(department);
        }

        public Department GetDepartment(int id)
        {
            return _departmentRepository.GetDepartment(id);
        }

        public IEnumerable<Department> GetDepartments()
        {
            return _departmentRepository.GetDepartments();
        }

        public void RemoveDepartment(int id)
        {
            _departmentRepository.RemoveDepartment(id);
        }
    }
}
