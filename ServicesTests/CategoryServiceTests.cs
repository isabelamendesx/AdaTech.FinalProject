using FluentAssertions;
using Microsoft.Extensions.Logging;
using Model.Domain.Common;
using Model.Domain.Entities;
using Model.Domain.Interfaces;
using Model.Service.Exceptions;
using Model.Service.Services;
using NSubstitute;
using System.Linq.Expressions;

namespace ServicesTests
{
    public class CategoryServiceTests
    {
        private CategoryService _sut;

        private IRepository<Category> repository;
        private ILogger<CategoryService> logger;
        private CancellationToken ct;

        public CategoryServiceTests()
        {

            ct = new CancellationToken();
            repository = Substitute.For<IRepository<Category>>();
            logger = Substitute.For<ILogger<CategoryService>>();

            _sut = new CategoryService(repository, logger);
        }

        [Fact]
        public async Task create_category_must_throw_an_exception_when_name_already_exists()
        {
            var newCategory = new Category { Id = 5, Name = "Food" };
            repository.GetByParameter(ct, Arg.Any<Expression<Func<Category, bool>>>())
                .Returns(ListOfCategory().Where(x => x.Name.ToLower().Equals(newCategory.Name.ToLower())));

            await _sut.Invoking(x => x.CreateCategory(newCategory, ct))
                   .Should().ThrowAsync<CategoryAlreadyRegisteredException>();
        }

        [Fact]
        public async Task create_category_should_succeed_when_name_does_not_already_exists()
        {
            var newCategory = new Category { Id = 5, Name = "Eletronics" };
            repository.GetByParameter(ct, Arg.Any<Expression<Func<Category, bool>>>())
                .Returns(ListOfCategory().Where(x => x.Name.ToLower().Equals(newCategory.Name.ToLower())));

            await _sut.Invoking(x => x.CreateCategory(newCategory, ct))
                   .Should().NotThrowAsync<CategoryAlreadyRegisteredException>();
        }


        [Fact]
        public async Task get_all_should_return_all_categories()
        {

            repository.GetByParameter(ct).Returns(Task.FromResult<IEnumerable<Category?>>(ListOfCategory()));

            var result = await _sut.GetAll(ct);

            result.Should().BeEquivalentTo(ListOfCategory());
        }

        [Fact]
        public async Task get_all_paginated_should_return_paginated_result_with_correct_values()
        {
            var expectedTotalCount = 3;
            var expectedCategories = ListOfCategory();

            var expextedPaginatedResult = new PaginatedResult<Category> { TotalCount = expectedTotalCount, Items = ListOfCategory() };


            repository.GetPaginatedByParameter(ct, Arg.Any<int>(), Arg.Any<int>(), Arg.Any<Expression<Func<Category, bool>>>())
                      .Returns(Task.FromResult<PaginatedResult<Category>>(expextedPaginatedResult));

            var result = await _sut.GetAllPaginated(ct, skip: 0, take: 10);

            result.TotalCount.Should().Be(expectedTotalCount);
            result.Items.Should().BeEquivalentTo(expectedCategories);
        }


        [Fact]
        public async Task get_by_id_must_throw_an_exception_when_category_could_not_be_found()
        {
            repository.GetById(Arg.Any<uint>(), ct).Returns(Task.FromResult<Category?>(null));

            await _sut.Invoking(x => x.GetById(3, ct))
                .Should().ThrowAsync<ResourceNotFoundException>();
        }

        [Fact]
        public async Task get_by_id_sould_return_category_when_category_exists()
        {
            var expectedCategory = new Category { Id = 3, Name = "Transportation" };
            repository.GetById(Arg.Any<uint>(), ct).Returns(Task.FromResult<Category?>(expectedCategory));

            var result = await _sut.GetById(3, ct);

            result.Should().BeEquivalentTo(expectedCategory);
        }


        public static IEnumerable<Category> ListOfCategory()
        {
            return new List<Category>{
                     new Category() {
                        Name = "Food"
                    },
                     new Category() {
                        Id = 2, Name = "Accomodation"
                    },
                      new Category() {
                        Id = 3, Name = "Transportation"
                    },
            };
        }
    }
}