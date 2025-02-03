using CourseManagement.Domain.Entities;

namespace CourseManagement.Application.Interfaces
{
    public interface IDepartment
    {
        IEnumerable<Department> GetDepartments();
        Department GetDepartment(int id);
        void AddDepartment(Department department);
        void RemoveDepartment(int id);

    }
}
