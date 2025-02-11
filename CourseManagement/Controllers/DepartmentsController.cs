using CourseManagement.Application.Interfaces;
using CourseManagement.Application.Services;
using CourseManagement.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly DepartmentService _departmentService;

        public DepartmentsController(DepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDepartments()
        {
            var departments = await _departmentService.GetDepartmentsAsync();
            return Ok(departments);
        }


        [HttpGet("{id}")]
        public IActionResult GetDepartment(int id)
        {
            return Ok(_departmentService.GetDepartment(id));
        }

        [HttpPost]
        public IActionResult AddDepartment(Department department)
        {
            _departmentService.AddDepartment(department);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpDelete("{id}")]
        public IActionResult RemoveDepartment(int id)
        {
            _departmentService.RemoveDepartment(id);
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
