using CourseManagement.Application.Interfaces;
using CourseManagement.Domain.Entities;
using CourseManagement.Infrastructure.DbModel;


namespace CourseManagement.Infrastructure.Repository
{
    public class DepartmentRepository : BaseDataRepository, IDepartment
    {
        public DepartmentRepository(CourseManagementDbContext model) : base(model)
        {
        }
        public async Task<IList<Department>> GetDepartmentsAsync()
        {
            return await GetListDataAsync<Department>();
        }

        public Department GetDepartment(int id)
        {
            return FindEntity<Department>(id);
        }

        public void AddDepartment(Department department)
        {
            AddUpdateEntity<Department>(department);
        }

        public void RemoveDepartment(int id)
        {
            RemoveEntity<Department>(id);
        }
    }
}
