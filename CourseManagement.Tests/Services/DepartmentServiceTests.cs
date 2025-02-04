using Xunit;
using Moq;
using System.Collections.Generic;
using FluentAssertions;
using CourseManagement.Domain.Entities;
using CourseManagement.Application.Services;
using AutoMapper;
using CourseManagement.Application.Interfaces;

namespace CourseManagement.Tests.Services
{
    public class DepartmentServiceTests
    {
        private readonly Mock<IDepartment> _departmentRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly DepartmentService _departmentService;

        public DepartmentServiceTests()
        {
            _departmentRepositoryMock = new Mock<IDepartment>();
            _mapperMock = new Mock<IMapper>();

            _departmentService = new DepartmentService(_departmentRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllDepartmentsAsync_ShouldReturnListOfDepartments()
        {
            // Arrange
            var departments = new List<Department>
            {
                new Department { Id = 1, Name = "Computer Science" },
                new Department { Id = 2, Name = "Mathematics" }
            };

            _departmentRepositoryMock
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(departments);

            _mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<DepartmentDTO>>(departments))
                .Returns(new List<DepartmentDTO>
                {
                    new DepartmentDTO { Id = 1, Name = "Computer Science" },
                    new DepartmentDTO { Id = 2, Name = "Mathematics" }
                });

            // Act
            var result = await _departmentService.GetAllDepartmentsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().Contain(d => d.Name == "Computer Science");
        }
    }
}
