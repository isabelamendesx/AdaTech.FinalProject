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
using System.Text;
using System.Threading.Tasks;

namespace ServicesTests
{
    public class RefundServiceTests
    {
        private RefundService _sut;


        private IRepository<Refund> repository;
        private IRepository<RefundOperation> operationRepository;
        private IRuleService ruleService;
        private CancellationToken ct;

        public RefundServiceTests()
        {
            ct = new CancellationToken();
            repository = Substitute.For<IRepository<Refund>>();
            operationRepository = Substitute.For<IRepository<RefundOperation>>();
            ruleService = Substitute.For<IRuleService>();

            _sut = new RefundService(repository, operationRepository, ruleService);
        }

        [Fact]
        public async Task approve_refund_must_throw_an_exception_when_refund_could_not_be_found()
        {
            repository.GetById(Arg.Any<uint>(), ct).Returns(Task.FromResult<Refund?>(null));

            await _sut.Invoking(x => x.ApproveRefund(3, 9, ct))
                .Should().ThrowAsync<ResourceNotFoundException>();            
        }
        
        [Fact]
        public async Task refuse_refund_must_throw_an_exception_when_refund_could_not_be_found()
        {
            repository.GetById(Arg.Any<uint>(), ct).Returns(Task.FromResult<Refund?>(null));

            await _sut.Invoking(x => x.RefuseRefund(3, 9, ct))
                .Should().ThrowAsync<ResourceNotFoundException>();            
        }
        [Theory]
        [MemberData(nameof(ReturnsFrom0To1000))]
        public async Task should_reject_all_refunds(Refund refund)
        {
            List<Rule?> rules = new List<Rule?>
            {
                new Rule() { 
                    MinValue = 0, MaxValue = 1000, Action = false,
                }
            };
            ruleService.GetRulesToReproveAny(ct).Returns(rules);

            await _sut.CreateRefund(refund, ct);

            refund.Status.Should().Be(EStatus.Rejected);
        }


        [Theory]
        [MemberData(nameof(ReturnsFrom0To1000))]
        public async Task should_approve_all_refunds(Refund refund)
        {
            List<Rule?> rules = new List<Rule?>
            {
                new Rule() {
                    MinValue = 0, MaxValue = 1000, Action = true,
                }
            };
            ruleService.GetRulesToApproveAny(ct).Returns(rules);

            await _sut.CreateRefund(refund, ct);

            refund.Status.Should().Be(EStatus.Approved);
        }

        [Theory]
        [MemberData(nameof(ReturnsFrom0To1000))]
        public async Task should_approve_only_the_refund_with_category_3(Refund refund)
        {
            List<Rule?> rules = new List<Rule?>
            {
                new Rule() {
                    MinValue = 0, MaxValue = 1000, Action = true,
                }
            };
            ruleService.GetRulesToApproveByCategoryId(3, ct).Returns(rules);

            await _sut.CreateRefund(refund, ct);

            if(refund.Category.Id == 3)
                refund.Status.Should().Be(EStatus.Approved);
            else
                refund.Status.Should().Be(EStatus.UnderEvaluation);

        }


        [Theory]
        [MemberData(nameof(AllRefundsAndExpectedStatus))]
        public async Task status_should_be_correct(Refund refund, EStatus expectedStatus)
        {
            ruleService.GetRulesToReproveAny(ct).Returns(ListOfRuleToRejectAny());

            ruleService.GetRulesToApproveAny(ct).Returns(ListOfRuleToApproveAny());

            ruleService.GetRulesToApproveByCategoryId(3, ct)
                .Returns(ListOfRulesToApproveCategory3());

            await _sut.CreateRefund(refund, ct);

            refund.Status.Should().Be(expectedStatus);
        }

        public static IEnumerable<object[]> ReturnsFrom0To1000()
        {
            return new[]{
                new object[]{ new Refund ()
                {Total = 100,
                    Category = new Category() { Id = 3 } } },
                new object[]{ new Refund ()
                {Total = 100,
                    Category = new Category() { Id = 4 } } },
                new object[]{ new Refund ()
                {Total = 500,
                    Category = new Category() { Id = 5 } } },
                new object[]{ new Refund ()
                {Total = 300,
                    Category = new Category() { Id = 3 } } },
                new object[]{ new Refund ()
                {Total = 600,
                    Category = new Category() { Id =4 } } },
                new object[]{ new Refund ()
                {Total = 700,
                    Category = new Category() { Id = 5 } } },
                new object[]{ new Refund ()
                {Total = 850,
                    Category = new Category() { Id = 3 } } },
                new object[]{ new Refund ()
                {Total = 1000,
                    Category = new Category() { Id = 4 } } },
                new object[]{ new Refund ()
                {Total = 50,
                    Category = new Category() { Id = 5 } } },
            };
        }

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
