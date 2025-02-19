using Xunit;
using Moq;
using System.Collections.Generic;
using FluentAssertions;
using CourseManagement.Domain.Entities;
using CourseManagement.Application.Services;
using CourseManagement.Application.Interfaces;
using CourseManagement.Application.DTOs;
using CourseManagement.Application;

namespace CourseManagement.Tests.Services
{
    public class DepartmentServiceTests
    {
        private readonly Mock<IDepartment> _departmentRepositoryMock;
        private readonly DepartmentService _departmentService;
        private readonly Mock<IRedisCacheService> _cacheServiceMock;
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(10);

        public DepartmentServiceTests()
        {
            _departmentRepositoryMock = new Mock<IDepartment>();
            _cacheServiceMock = new Mock<IRedisCacheService>();

            // If DepartmentService doesn't use AutoMapper, remove _mapperMock
            _departmentService = new DepartmentService(_departmentRepositoryMock.Object, _cacheServiceMock.Object);
        }

        [Fact]
        public async Task GetDepartments_ShouldReturnListOfDepartments_FromCacheIfAvailable()
        {
            // Arrange
            var cachedDepartments = new List<Department>
            {
                new Department { Id = 1, Name = "Computer Science" },
                new Department { Id = 2, Name = "Mathematics" }
            };

            _cacheServiceMock
                .Setup(cache => cache.GetCacheAsync<IList<Department>>("departments"))
                .ReturnsAsync(cachedDepartments);

            // Act
            var result = await _departmentService.GetDepartmentsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            _departmentRepositoryMock.Verify(repo => repo.GetDepartmentsAsync(), Times.Never); // Repository should NOT be called if cache exists
        }

        [Fact]
        public async void GetDepartments_ShouldFetchFromDatabase_IfCacheIsEmpty()
        {
            var departments = new List<Department>
            {
                new Department { Id = 1, Name = "Computer Science" },
                new Department { Id = 2, Name = "Mathematics" }
            };

            _cacheServiceMock
                .Setup(cache => cache.GetCacheAsync<IList<Department>>("departments"))
                .ReturnsAsync((List<Department>)null);

            _departmentRepositoryMock
                .Setup(repo => repo.GetDepartmentsAsync())
                .ReturnsAsync(departments);

            var result = await _departmentService.GetDepartmentsAsync();

            result.Should().NotBeNull();
            result.Should().HaveCount(2);

            _departmentRepositoryMock.Verify(repo => repo.GetDepartmentsAsync(), Times.Once);
            //_cacheServiceMock.Verify(cache => cache.SetCacheAsync("departments", departments, _cacheExpiration), Times.Once); // Data must be cached
        }

        [Fact]
        public void RemoveDepartment_ShouldRemoveDepartment_AndClearCache()
        {
            // Arrange
            var departmentId = 1;

            _departmentRepositoryMock
                .Setup(repo => repo.RemoveDepartment(departmentId))
                .Verifiable();  // Ensures method is called

            _cacheServiceMock
                .Setup(cache => cache.RemoveCache("departments"))
                .Verifiable(); // Ensures this cache removal is called

            _cacheServiceMock
                .Setup(cache => cache.RemoveCache($"department_{departmentId}"))
                .Verifiable(); // Ensures this cache removal is called

            // Act
            _departmentService.RemoveDepartment(departmentId);

            // Assert
            _departmentRepositoryMock.Verify(repo => repo.RemoveDepartment(departmentId), Times.Once);
            _cacheServiceMock.Verify(cache => cache.RemoveCache("departments"), Times.Once);
            _cacheServiceMock.Verify(cache => cache.RemoveCache($"department_{departmentId}"), Times.Once);
        }


        [Fact]
        public void GetDepartment_ShouldReturnNull_WhenDepartmentDoesNotExist()
        {
            var departmentId = 99;

            _departmentRepositoryMock
                .Setup(repo => repo.GetDepartment(departmentId))
                .Returns((Department)null);

            var result = _departmentService.GetDepartment(departmentId);

            result.Should().BeNull();
        }

        [Fact]
        public void GetDepartment_ShouldReturnDepartment_WhenDepartmentExists()
        {
            var departmentId = 1;
            var department = new Department { Id = departmentId, Name = "Computer Science" };

            _departmentRepositoryMock
                .Setup(repo => repo.GetDepartment(departmentId))
                .Returns(department);

            var result = _departmentService.GetDepartment(departmentId);

            result.Should().NotBeNull();
            result.Id.Should().Be(departmentId);
            result.Name.Should().Be("Computer Science");
        }

        [Fact]
        public void CreateDepartment_ShouldCreateDepartment_AndClearCache()
        {
            // Arrange
            var newDepartment = new Department
            {
                Id = 1,
                Name = "Biology"
            };

            _departmentRepositoryMock
                .Setup(repo => repo.AddDepartment(It.IsAny<Department>()))
                .Verifiable(); // Ensures method is called

            _cacheServiceMock
                .Setup(cache => cache.RemoveCache("departments"))
                .Verifiable(); // Ensures this cache removal is called

            _cacheServiceMock
                .Setup(cache => cache.RemoveCache($"department_{newDepartment.Id}"))
                .Verifiable(); // Ensures this cache removal is called

            // Act
            _departmentService.AddDepartment(newDepartment);

            // Assert
            _departmentRepositoryMock.Verify(repo => repo.AddDepartment(It.IsAny<Department>()), Times.Once);
            _cacheServiceMock.Verify(cache => cache.RemoveCache("departments"), Times.Once);
            _cacheServiceMock.Verify(cache => cache.RemoveCache($"department_{newDepartment.Id}"), Times.Once);
        }


    }
}
