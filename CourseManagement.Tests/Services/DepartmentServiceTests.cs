using Xunit;
using Moq;
using System.Collections.Generic;
using FluentAssertions;
using CourseManagement.Domain.Entities;
using CourseManagement.Application.Services;
using CourseManagement.Application.Interfaces;
using CourseManagement.Application.DTOs;

namespace CourseManagement.Tests.Services
{
    public class DepartmentServiceTests
    {
        private readonly Mock<IDepartment> _departmentRepositoryMock;
        private readonly DepartmentService _departmentService;

        public DepartmentServiceTests()
        {
            _departmentRepositoryMock = new Mock<IDepartment>();

            // If DepartmentService doesn't use AutoMapper, remove _mapperMock
            _departmentService = new DepartmentService(_departmentRepositoryMock.Object);
        }

        [Fact]
        public void GetAllDepartments_ShouldReturnListOfDepartments()
        {
            // Arrange
            var departments = new List<Department>
            {
                new Department { Id = 1, Name = "Computer Science" },
                new Department { Id = 2, Name = "Mathematics" }
            };

            _departmentRepositoryMock
                .Setup(repo => repo.GetDepartments())
                .Returns(departments);  // No Task needed (sync method)

            // Act
            var result = _departmentService.GetDepartments();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().Contain(d => d.Name == "Computer Science");
        }

        [Fact]
        public void InsertDepartment_ShouldAddDepartment()
        {
            // Arrange
            var department = new Department { Id = 1, Name = "Physics" };

            _departmentRepositoryMock
                .Setup(repo => repo.AddDepartment(department))
                .Verifiable();  // Ensures method is called

            // Act
            _departmentService.AddDepartment(department);

            // Assert
            _departmentRepositoryMock.Verify(repo => repo.AddDepartment(department), Times.Once);
        }

        [Fact]
        public void RemoveDepartment_ShouldRemoveDepartment()
        {
            // Arrange
            var departmentId = 1;

            _departmentRepositoryMock
                .Setup(repo => repo.RemoveDepartment(departmentId))
                .Verifiable();  // Ensures method is called

            // Act
            _departmentService.RemoveDepartment(departmentId);

            // Assert
            _departmentRepositoryMock.Verify(repo => repo.RemoveDepartment(departmentId), Times.Once);
        }

        [Fact]
        public void GetDepartment_ShouldReturnNull_WhenDepartmentDoesNotExist()
        {
            // Arrange
            var departmentId = 99;  // A non-existing ID

            _departmentRepositoryMock
                .Setup(repo => repo.GetDepartment(departmentId))
                .Returns((Department)null);  // Simulating no record found

            // Act
            var result = _departmentService.GetDepartment(departmentId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetDepartment_ShouldReturnDepartment_WhenDepartmentExists()
        {
            // Arrange
            var departmentId = 1;
            var department = new Department { Id = departmentId, Name = "Computer Science" };

            _departmentRepositoryMock
                .Setup(repo => repo.GetDepartment(departmentId))
                .Returns(department);  // Mocked return

            // Act
            var result = _departmentService.GetDepartment(departmentId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(departmentId);
            result.Name.Should().Be("Computer Science");
        }

    }
}
