using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CovaldysPilot.Application.DTOs.Category.Request;
using CovaldysPilot.Application.DTOs.Category.Response;
using CovaldysPilot.Application.Interfaces.Repositories;
using CovaldysPilot.Application.Services;
using CovaldysPilot.Domain.Entities;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace CovaldysPilot.Tests;

public class CategoryServiceTests
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IEventRepository _eventRepository;
    private readonly ILogger<CategoryService> _logger;
    private readonly CategoryService _categoryService;

    public CategoryServiceTests()
    {
        _categoryRepository = Substitute.For<ICategoryRepository>();
        _eventRepository = Substitute.For<IEventRepository>();
        _logger = Substitute.For<ILogger<CategoryService>>();

        _categoryService = new CategoryService(
            _categoryRepository,
            _eventRepository,
            _logger
        );
    }

    #region GetAllAsync Tests

    [Fact]
    public async Task GetAllAsync_ReturnsAllMappedCategories()
    {
        // Arrange
        var categories = new List<Category>
        {
            new Category { Id = Guid.NewGuid(), Name = "Category 1", CreatedAt = DateTime.UtcNow },
            new Category { Id = Guid.NewGuid(), Name = "Category 2", CreatedAt = DateTime.UtcNow }
        };

        _categoryRepository.GetAllAsync().Returns(categories);

        // Act
        var result = await _categoryService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        var resultList = result.ToList();
        Assert.Equal(categories.Count, resultList.Count);
        Assert.Equal(categories[0].Id, resultList[0].Id);
        Assert.Equal(categories[0].Name, resultList[0].Name);
        Assert.Equal(categories[1].Id, resultList[1].Id);
        Assert.Equal(categories[1].Name, resultList[1].Name);

        await _categoryRepository.Received(1).GetAllAsync();
    }

    #endregion

    #region CreateAsync Tests

    [Fact]
    public async Task CreateAsync_WhenNameAlreadyExists_ThrowsInvalidOperationException()
    {
        // Arrange
        var request = new CreateCategoryRequestDto
        {
            Name = "Existing Category"
        };

        _categoryRepository.NameExistsAsync(request.Name).Returns(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _categoryService.CreateAsync(request));
        Assert.Equal($"La catégorie '{request.Name}' existe déjà", exception.Message);

        await _categoryRepository.DidNotReceive().AddAsync(Arg.Any<Category>());
        await _categoryRepository.DidNotReceive().SaveChangesAsync();
    }

    [Fact]
    public async Task CreateAsync_WhenRequestIsValid_AddsCategoryAndReturnsResponse()
    {
        // Arrange
        var request = new CreateCategoryRequestDto
        {
            Name = "New Category"
        };

        _categoryRepository.NameExistsAsync(request.Name).Returns(false);

        // Act
        var result = await _categoryService.CreateAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(request.Name, result.Name);

        await _categoryRepository.Received(1).NameExistsAsync(request.Name);
        await _categoryRepository.Received(1).AddAsync(Arg.Is<Category>(c => c.Name == request.Name));
        await _categoryRepository.Received(1).SaveChangesAsync();
    }

    #endregion

    #region DeleteAsync Tests

    [Fact]
    public async Task DeleteAsync_WhenCategoryIsLinkedToEvents_ThrowsInvalidOperationException()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        _eventRepository.AnyByCategoryIdAsync(categoryId).Returns(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _categoryService.DeleteAsync(categoryId));
        Assert.Equal("Impossible de supprimer cette catégorie car elle est liée à un ou plusieurs événements.", exception.Message);

        await _categoryRepository.DidNotReceive().DeleteAsync(Arg.Any<Guid>());
        await _categoryRepository.DidNotReceive().SaveChangesAsync();
    }

    [Fact]
    public async Task DeleteAsync_WhenCategoryIsNotLinkedToEvents_DeletesCategoryAndSaves()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        _eventRepository.AnyByCategoryIdAsync(categoryId).Returns(false);

        // Act
        await _categoryService.DeleteAsync(categoryId);

        // Assert
        await _eventRepository.Received(1).AnyByCategoryIdAsync(categoryId);
        await _categoryRepository.Received(1).DeleteAsync(categoryId);
        await _categoryRepository.Received(1).SaveChangesAsync();
    }

    #endregion
}
