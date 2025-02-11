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
        private readonly IRedisCacheService _cacheService;
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(10);

        public DepartmentService(IDepartment departmentRepository, IRedisCacheService cacheService)
        {
            _departmentRepository = departmentRepository;
            _cacheService = cacheService;
        }

        public void AddDepartment(Department department)
        {
            _departmentRepository.AddDepartment(department);
        }

        public Department GetDepartment(int id)
        {
            return _departmentRepository.GetDepartment(id);
        }

        public async Task<IList<Department>> GetDepartmentsAsync()
        {
            string cacheKey = "departments";

            // Check if data is in Redis
            var cachedDepartments = await _cacheService.GetCacheAsync<IList<Department>>(cacheKey);
            if (cachedDepartments != null)
            {
                return cachedDepartments;
            }

            // If not, fetch from database
            var departments = await _departmentRepository.GetDepartmentsAsync();

            // Store in Redis for future requests
            await _cacheService.SetCacheAsync(cacheKey, departments, _cacheExpiration);

            return departments;
        }

        public void RemoveDepartment(int id)
        {
            _departmentRepository.RemoveDepartment(id);
        }
    }
}
