using FluentAssertions;
using Microsoft.Extensions.Logging;
using Model.Domain.Common;
using Model.Domain.Entities;
using Model.Domain.Interfaces;
using Model.Service.Exceptions;
using Model.Service.Services;
using Model.Service.Services.DTO;
using NSubstitute;
using System.Linq.Expressions;


namespace ServicesTests
{
    public class RefundServiceTests
    {
        private RefundService _sut;


        private IRepository<Refund> _repository;
        private IRuleService _ruleService;
        private ICategoryService _categoryService;
        private ILogger<RefundService> _logger;
        private CancellationToken ct;

        public RefundServiceTests()
        {
            ct = new CancellationToken();
            _repository = Substitute.For<IRepository<Refund>>();
            _ruleService = Substitute.For<IRuleService>();
            _categoryService = Substitute.For<ICategoryService>();
            _logger = Substitute.For<ILogger<RefundService>>();


            _sut = new RefundService(_repository, _ruleService, _categoryService, _logger);
        }

        [Fact]
        public async Task create_refund_must_throw_an_exception_when_category_is_invalid()
        {
            var newRefund = new Refund
            {
                Category = new Category { Id = 5}
            };

            _categoryService.GetById(Arg.Any<uint>(), ct).Returns(Task.FromResult<Category?>(null));

            await _sut.Invoking(x => x.CreateRefund(newRefund, ct))
                .Should().ThrowAsync<ResourceNotFoundException>();
        }

        [Fact]
        public async Task create_refund_should_succeed()
        {
            var category = new Category() { Id = 1, Name = "Food" };
            var newRefund = new Refund
            {
                Total = 100,
                Category = category,
            };


            _categoryService.GetById(Arg.Any<uint>(), ct).Returns(Task.FromResult<Category?>(category));

            _ruleService.GetRulesThatApplyToCategory(Arg.Any<uint>(), ct).Returns(Task.FromResult<IEnumerable<Rule?>>(RulesValidToCategoryId1()));


            await _sut.Invoking(x => x.CreateRefund(newRefund, ct))
                .Should().NotThrowAsync();
        }

        [Fact]
        public async Task get_all_should_return_all_refunds()
        {

            _repository.GetByParameter(ct).Returns(Task.FromResult<IEnumerable<Refund?>>(ListOfRefunds()));

            var result = await _sut.GetAll(ct);

            result.Should().BeEquivalentTo(ListOfRefunds());
        }

        [Fact]
        public async Task get_all_by_status_approved_should_return_all_approved_refunds()
        {
            _repository.GetByParameter(ct).
                Returns(Task.FromResult<IEnumerable<Refund?>>(ListOfRefunds().Where(x =>x.Status == EStatus.Approved)));
            
            var result = await _sut.GetAll(ct);

            result.Should().BeEquivalentTo(ListOfRefunds().Where(x => x.Status == EStatus.Approved));
        }

        [Fact]
        public async Task approve_refund_must_throw_an_exception_when_refund_could_not_be_found()
        {
            _repository.GetById(Arg.Any<uint>(), ct).Returns(Task.FromResult<Refund?>(null));

            await _sut.Invoking(x => x.ApproveRefund(3, "9", ct))
                .Should().ThrowAsync<ResourceNotFoundException>();
        }

        [Fact]
        public async Task approve_refund_must_throw_an_exception_when_refund_status_is_not_under_evaluation()
        {
            var refund = new Refund
            {
                Id = 3,
                Status = EStatus.Rejected,
                Total = 800,
                OwnerID = "9",
                Category = new Category() { Id = 3 }
            };

            _repository.GetById(Arg.Any<uint>(), ct).Returns(Task.FromResult<Refund?>(refund));

            await _sut.Invoking(x => x.ApproveRefund(3, "9", ct))
                .Should().ThrowAsync<InvalidRefundException > ();            
        }

        [Fact]
        public async Task approve_refund_should_succeed()
        {
            var approveRefund = new Refund
            {
                Id = 3,
                Status = EStatus.UnderEvaluation,
                Total = 100,
                OwnerID = "9",
                Category = new Category() { Id = 3 }
            };
            
            _repository.GetById(Arg.Any<uint>(), ct).Returns(Task.FromResult<Refund?>(approveRefund));

            await _sut.Invoking(x => x.ApproveRefund(3, "9", ct)).Should().NotThrowAsync();
        }

        [Fact]
        public async Task reject_refund_must_throw_an_exception_when_refund_could_not_be_found()
        {
            _repository.GetById(Arg.Any<uint>(), ct).Returns(Task.FromResult<Refund?>(null));

            await _sut.Invoking(x => x.RejectRefund(3, "9", ct))
                .Should().ThrowAsync<ResourceNotFoundException>();            
        }

        [Fact]
        public async Task reject_refund_must_throw_an_exception_when_refund_status_is_not_under_evaluation()
        {
            var refund = new Refund
            {
                Id = 3,
                Status = EStatus.Approved,
                Total = 100,
                OwnerID = "9",
                Category = new Category() { Id = 3 }
            };

            _repository.GetById(Arg.Any<uint>(), ct).Returns(Task.FromResult<Refund?>(refund));

            await _sut.Invoking(x => x.ApproveRefund(3, "9", ct))
                .Should().ThrowAsync<InvalidRefundException>();
        }

        [Fact]
        public async Task reject_refund_should_succeed()
        {
            var approveRefund = new Refund
            {
                Id = 3,
                Status = EStatus.UnderEvaluation,
                Total = 800,
                OwnerID = "9",
                Category = new Category() { Id = 3 }
            };

            _repository.GetById(Arg.Any<uint>(), ct).Returns(Task.FromResult<Refund?>(approveRefund));

            await _sut.Invoking(x => x.ApproveRefund(3, "9", ct)).Should().NotThrowAsync();
        }

        [Fact]
        public async Task change_status_refund_must_throw_an_exception_when_refund_could_not_be_found()
        {
            _repository.GetById(Arg.Any<uint>(), ct).Returns(Task.FromResult<Refund?>(null));

            await _sut.Invoking(x => x.ChangeRefundStatus(3, EStatus.Approved, "9", ct))
                .Should().ThrowAsync<ResourceNotFoundException>();
        }

        [Fact]
        public async Task change_status_refund_should_succeed()
        {
            var approveRefund = new Refund
            {
                Id = 3,
                Status = EStatus.Rejected,
                Total = 800,
                OwnerID = "9",
                Category = new Category() { Id = 3 }
            };

            _repository.GetById(Arg.Any<uint>(), ct).Returns(Task.FromResult<Refund?>(approveRefund));

            await _sut.Invoking(x => x.ChangeRefundStatus(3, EStatus.Approved, "9", ct))
                .Should().NotThrowAsync();
        }






        [Fact]
        public async Task get_all_by_status_paginated_should_return_paginated_result_with_correct_values()
        {
            var expectedTotalCount = ListOfRefunds().Where(x => x.Status == EStatus.Approved).Count();
            var expectedRefunds = ListOfRefunds().Where(x => x.Status == EStatus.Approved);

            var expectedPaginatedResult = new PaginatedResult<Refund> { TotalCount = expectedTotalCount, Items = expectedRefunds};


            _repository.GetPaginatedByParameter(ct, Arg.Any<int>(), Arg.Any<int>(), Arg.Any<Expression<Func<Refund, bool>>>())
                      .Returns(Task.FromResult(expectedPaginatedResult));

            var result = await _sut.GetAllByStatusPaginated(EStatus.Approved, ct, skip: 0, take: 10);

            result.TotalCount.Should().Be(expectedTotalCount);
            result.Items.Should().BeEquivalentTo(expectedRefunds);
        }




        [Fact]
        public async Task get_by_if_must_throw_an_exception_when_refund_could_not_be_found()
        {
            _repository.GetById(Arg.Any<uint>(), ct).Returns(Task.FromResult<Refund?>(null));

            await _sut.Invoking(x => x.GetById(3, ct))
                  .Should().ThrowAsync<ResourceNotFoundException>();
        }

         [Fact]
        public async Task get_by_if_should_return_a_refund_when_succeed()
        {
            var refund = new Refund
            {
                Id = 3,
                Status = EStatus.UnderEvaluation,
                Total = 100,
                OwnerID = "9",
                Category = new Category() { Id = 3 }
            };

            _repository.GetById(Arg.Any<uint>(), ct).Returns(Task.FromResult<Refund?>(refund));

            var result = await _sut.GetById(3, ct);

            result.Should().BeEquivalentTo(refund);
        }


        public static IEnumerable<Rule> RulesValidToCategoryId1()
        {
            var allCategories = new Category() { Id = 0, Name = "All" };
            var category1 = new Category() { Id = 1, Name = "Food" };

            return new List<Rule>{
                new Rule() {
                    Id = 1, MinValue = 0, MaxValue = 100, Action = true, Category = allCategories, IsActive = true
                },
                new Rule() {
                    Id = 3, MinValue = 100.01M, MaxValue = 500, Action = true, Category = category1, IsActive = true
                },
                new Rule() {
                    Id = 4, MinValue = 800, MaxValue = 999.99M, Action = false, Category = category1, IsActive = true
                },
                new Rule() {
                    Id = 10, MinValue = 1000, MaxValue = decimal.MaxValue, Action = false, Category = allCategories, IsActive = true
                }
            };
        }

        public static IEnumerable<Refund> ListOfRefunds()
        {
            var allCategories = new Category() { Id = 0, Name = "All" };
            var category1 = new Category() { Id = 1, Name = "Food" };

            return new List<Refund>{
                new Refund() {
                    Id = 1, Total = 80, Status = EStatus.Approved, Category = allCategories, Operations = new List<RefundOperation>()
                },
                new Refund() {
                    Id = 2, Total = 200, Status = EStatus.Approved, Category = category1, Operations = new List<RefundOperation>()
                },
                new Refund() {
                    Id = 3, Total = 500, Status = EStatus.UnderEvaluation, Category = allCategories, Operations = new List<RefundOperation>()
                },
                new Refund() {
                    Id = 4, Total = 1500, Status = EStatus.Rejected, Category = allCategories, Operations = new List<RefundOperation>()
                },
                new Refund() {
                    Id = 5, Total = 1200, Status = EStatus.Rejected, Category = category1, Operations = new List<RefundOperation>()
                },
            };
        }





    }
}
