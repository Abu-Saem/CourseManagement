using CourseManagement.Domain.Entities;

namespace CourseManagement.Application.Interfaces
{
    public interface IDepartment
    {
        Task<IList<Department>> GetDepartmentsAsync();
        Department GetDepartment(int id);
        void AddDepartment(Department department);
        void RemoveDepartment(int id);

    }
}
