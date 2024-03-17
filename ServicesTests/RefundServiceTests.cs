using FluentAssertions;
using Microsoft.Extensions.Logging;
using Model.Domain.Entities;
using Model.Domain.Interfaces;
using Model.Service.Exceptions;
using Model.Service.Services;
using Model.Service.Services.DTO;
using NSubstitute;

using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public async Task approve_refund_must_throw_an_exception_when_refund_could_not_be_found()
        {
            _repository.GetById(Arg.Any<uint>(), ct).Returns(Task.FromResult<Refund?>(null));

            await _sut.Invoking(x => x.ApproveRefund(3, "9", ct))
                .Should().ThrowAsync<ResourceNotFoundException>();            
        }

        [Fact]
        public async Task approve_refund_should_return_a_refund_when_seccessed()
        {
            var approveRefund = new Refund { Id = 3, Status = EStatus.Submitted, Total = 100, OwnerID = "9",
                                Category = new Category() { Id = 3 } };
            
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
        public async Task reject_refund_should_return_a_refund_when_successed()
        {
            var approveRefund = new Refund
            {
                Id = 3,
                Status = EStatus.Submitted,
                Total = 100,
                OwnerID = "9",
                Category = new Category() { Id = 3 }
            };

            _repository.GetById(Arg.Any<uint>(), ct).Returns(Task.FromResult<Refund?>(approveRefund));

            await _sut.Invoking(x => x.ApproveRefund(3, "9", ct)).Should().NotThrowAsync();
        }


        //[Theory]
        //[MemberData(nameof(ReturnsFrom0To1000))]
        //public async Task should_reject_all_refunds(Refund refund)
        //{
        //    List<Rule?> rules = new List<Rule?>
        //    {
        //        new Rule() { 
        //            MinValue = 0, MaxValue = 1000, Action = false,
        //        }
        //    };
        //    //ruleService.GetRulesToRejectAny(ct).Returns(rules);

        //    await _sut.CreateRefund(refund, ct);

        //    refund.Status.Should().Be(EStatus.Rejected);
        //}


        //[Theory]
        //[MemberData(nameof(ReturnsFrom0To1000))]
        //public async Task should_approve_all_refunds(Refund refund)
        //{
        //    List<Rule?> rules = new List<Rule?>
        //    {
        //        new Rule() {
        //            MinValue = 0, MaxValue = 1000, Action = true,
        //        }
        //    };
        //    //ruleService.GetRulesToApproveAny(ct).Returns(rules);

        //    await _sut.CreateRefund(refund, ct);

        //    refund.Status.Should().Be(EStatus.Approved);
        //}

        //[Theory]
        //[MemberData(nameof(ReturnsFrom0To1000))]
        //public async Task should_approve_only_the_refund_with_category_3(Refund refund)
        //{
        //    List<Rule?> rules = new List<Rule?>
        //    {
        //        new Rule() {
        //            MinValue = 0, MaxValue = 1000, Action = true,
        //        }
        //    };
        //    //ruleService.GetRulesToApproveByCategoryId(3, ct).Returns(rules);

        //    await _sut.CreateRefund(refund, ct);

        //    if(refund.Category.Id == 3)
        //        refund.Status.Should().Be(EStatus.Approved);
        //    else
        //        refund.Status.Should().Be(EStatus.UnderEvaluation);

        //}


        //[Theory]
        //[MemberData(nameof(AllRefundsAndExpectedStatus))]
        //public async Task status_should_be_correct(Refund refund, EStatus expectedStatus)
        //{
        //    //ruleService.GetRulesToRejectAny(ct).Returns(ListOfRuleToRejectAny());

        //    //ruleService.GetRulesToApproveAny(ct).Returns(ListOfRuleToApproveAny());

        //    //ruleService.GetRulesToApproveByCategoryId(3, ct)
        //    //    .Returns(ListOfRulesToApproveCategory3());

        //    await _sut.CreateRefund(refund, ct);

        //    refund.Status.Should().Be(expectedStatus);
        //}

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






        //public static IEnumerable<object[]> ReturnsFrom0To1000()
        //{
        //    return new[]{
        //        new object[]{ new Refund ()
        //        {Total = 100,
        //            Category = new Category() { Id = 3 } } },
        //        new object[]{ new Refund ()
        //        {Total = 100,
        //            Category = new Category() { Id = 4 } } },
        //        new object[]{ new Refund ()
        //        {Total = 500,
        //            Category = new Category() { Id = 5 } } },
        //        new object[]{ new Refund ()
        //        {Total = 300,
        //            Category = new Category() { Id = 3 } } },
        //        new object[]{ new Refund ()
        //        {Total = 600,
        //            Category = new Category() { Id =4 } } },
        //        new object[]{ new Refund ()
        //        {Total = 700,
        //            Category = new Category() { Id = 5 } } },
        //        new object[]{ new Refund ()
        //        {Total = 850,
        //            Category = new Category() { Id = 3 } } },
        //        new object[]{ new Refund ()
        //        {Total = 1000,
        //            Category = new Category() { Id = 4 } } },
        //        new object[]{ new Refund ()
        //        {Total = 50,
        //            Category = new Category() { Id = 5 } } },
        //    };
        //}

        public static List<Rule> ListOfRuleToRejectAny()
        {
            return new List<Rule>(){
                     new Rule() {
                        MinValue = 1000, Action = false,
                    },
            };
        }
        
        public static List<Rule> ListOfRuleToApproveAny()
        {
            return new List<Rule>{
                     new Rule() {
                        MinValue = 0, MaxValue = 100, Action = true,
                    }
            };
        }

        public static List<Rule> ListOfRulesToApproveCategory3()
        {
            return new List<Rule>{
                     new Rule() {
                        MinValue = 100.01M, MaxValue = 500, Action = true,
                    }
            };
        }


        public static IEnumerable<object[]> AllRefundsAndExpectedStatus()
        {
            return new[]
            {
                new object[]{ new Refund ()
                {
                    Total = 100,
                    Category = new Category() { Id = 3 } }, EStatus.Approved },
                new object[]{ new Refund ()
                {
                    Total = 300,
                    Category = new Category() { Id = 3 } }, EStatus.Approved },
                new object[]{ new Refund ()
                {
                    Total = 1001,
                    Category = new Category() { Id = 3 } }, EStatus.Rejected },
                new object[]{ new Refund ()
                {
                    Total = 1005,
                    Category = new Category() { Id = 4 } }, EStatus.Rejected },
                new object[]{ new Refund ()
                {
                    Total = 800,
                    Category = new Category() { Id = 4 } }, EStatus.UnderEvaluation},
                new object[]{ new Refund ()
                {
                    Total = 80,
                    Category = new Category() { Id = 4 } }, EStatus.Approved },
                new object[]{ new Refund ()
                {Total = 300,
                    Category = new Category() { Id = 5 } }, EStatus.UnderEvaluation },
                new object[]{ new Refund ()
                {
                    Total = 50,
                    Category = new Category() { Id = 5 } }, EStatus.Approved },
                new object[]{ new Refund ()
                {
                    Total = 1000,
                    Category = new Category() { Id = 5 } }, EStatus.Rejected },
            };
        }
    }
}
