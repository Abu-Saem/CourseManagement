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
        private readonly Mock<IRedisCacheService> _cacheServiceMock;

        public DepartmentServiceTests()
        {
            _departmentRepositoryMock = new Mock<IDepartment>();
            _cacheServiceMock = new Mock<IRedisCacheService>();

            // If DepartmentService doesn't use AutoMapper, remove _mapperMock
            _departmentService = new DepartmentService(_departmentRepositoryMock.Object, _cacheServiceMock.Object);
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
        public async Task GetDepartments_ShouldReturnFromCache_WhenDataExists()
        {
            // Arrange
            var cachedDepartments = new List<Department>
        {
            new Department { Id = 1, Name = "Computer Science" },
            new Department { Id = 2, Name = "Mathematics" }
        };

            _cacheServiceMock
                .Setup(cache => cache.GetCacheAsync<List<Department>>("departments"))
                .ReturnsAsync(cachedDepartments);

            // Act
            var result = await _departmentService.GetDepartmentsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            _departmentRepositoryMock.Verify(repo => repo.GetDepartmentsAsync(), Times.Never);
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
