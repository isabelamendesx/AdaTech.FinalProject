using FluentAssertions;
using Model.Domain.Entities;
using Model.Domain.Interfaces;
using Model.Service.Exceptions;
using Model.Service.Services;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ServicesTests
{
    public class CategoryServiceTests
    {
        private CategoryService _sut;

        private IRepository<Category> repository;
        private CancellationToken ct;
        
        public CategoryServiceTests()
        {
            ct = CancellationToken.None;
            repository = Substitute.For<IRepository<Category>>();
           
            _sut = new CategoryService(repository);
        }

        [Fact]
        public async Task create_category_must_throw_an_exception_when_name_already_exists()
        {
            var newCategory = new Category { Id = 5, Name = "Food" };
            repository.GetByParameter(ct, Arg.Any<Expression<Func<Category, bool>>>())
                .Returns(IEnumerableOfCategory().Where(x => x.Name.ToLower().Equals(newCategory.Name.ToLower())));

            await _sut.Invoking(x => x.CreateCategory(newCategory, ct))
                   .Should().ThrowAsync<CategoryAlreadyRegisteredException>();
        }

        [Fact]
        public async Task create_category_should_succeed_when_name_does_not_already_exist()
        {
            var newCategory = new Category { Id = 5, Name = "Eletronics" };
            repository.GetByParameter(ct, Arg.Any<Expression<Func<Category, bool>>>())
                .Returns(IEnumerableOfCategory().Where(x => x.Name.ToLower().Equals(newCategory.Name.ToLower())));

            await _sut.Invoking(x => x.CreateCategory(newCategory, ct))
                   .Should().NotThrowAsync<CategoryAlreadyRegisteredException>();
        }


        [Fact]
        public async Task get_all_should_return_all_categories()
        {
            
            repository.GetByParameter(ct).Returns(Task.FromResult<IEnumerable<Category?>>(IEnumerableOfCategory()));

            var result = await _sut.GetAll(ct);

            result.Should().BeEquivalentTo(IEnumerableOfCategory());
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


        public static IEnumerable<Category> IEnumerableOfCategory()
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
