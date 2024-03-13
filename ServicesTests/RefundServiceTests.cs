﻿using FluentAssertions;
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

        public RefundServiceTests()
        {
            repository = Substitute.For<IRepository<Refund>>();
            operationRepository = Substitute.For<IRepository<RefundOperation>>();
            ruleService = Substitute.For<IRuleService>();

            _sut = new RefundService(repository, operationRepository, ruleService);
        }

        [Fact]
        public async Task approve_refund_must_throw_an_exception_when_refund_could_not_be_found()
        {
            repository.GetById(Arg.Any<uint>()).Returns(Task.FromResult<Refund?>(null));

            await _sut.Invoking(x => x.ApproveRefund(3, 9))
                .Should().ThrowAsync<RefundNotFoundException>();            
        }
        
        [Fact]
        public async Task refuse_refund_must_throw_an_exception_when_refund_could_not_be_found()
        {
            repository.GetById(Arg.Any<uint>()).Returns(Task.FromResult<Refund?>(null));

            await _sut.Invoking(x => x.RefuseRefund(3, 9))
                .Should().ThrowAsync<RefundNotFoundException>();            
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
            ruleService.GetRulesToReproveAny().Returns(rules);

            await _sut.CreateRefund(refund);

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
            ruleService.GetRulesToApproveAny().Returns(rules);

            await _sut.CreateRefund(refund);

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
            ruleService.GetRulesToApproveByCategoryId(3).Returns(rules);

            await _sut.CreateRefund(refund);

            if(refund.Category.Id == 3)
                refund.Status.Should().Be(EStatus.Approved);
            else
                refund.Status.Should().Be(EStatus.UnderEvaluation);

        }


        [Theory]
        [MemberData(nameof(AllRefundsAndExpectedStatus))]
        public async Task status_should_be_correct(Refund refund, EStatus expectedStatus)
        {
            ruleService.GetRulesToReproveAny().Returns(ListOfRuleToRejectAny());

            ruleService.GetRulesToApproveAny().Returns(ListOfRuleToApproveAny());

            ruleService.GetRulesToApproveByCategoryId(3)
                .Returns(ListOfRulesToApproveCategory3());

            await _sut.CreateRefund(refund);

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
            };
        }

        public static List<Rule> ListOfRuleToRejectAny()
        {
            return new List<Rule>(){
                     new Rule() {
                        MinValue = 1000, Action = false,
                    }
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
                        MinValue = 101, MaxValue = 500, Action = true,
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
                {Total = 200,
                    Category = new Category() { Id = 4 } }, EStatus.UnderEvaluation },
                new object[]{ new Refund ()
                {
                    Total = 100,
                    Category = new Category() { Id = 2 } }, EStatus.Approved },
                new object[]{ new Refund ()
                {
                    Total = 1005,
                    Category = new Category() { Id = 3 } }, EStatus.Rejected },
                new object[]{ new Refund ()
                {
                    Total = 200,
                    Category = new Category() { Id = 5 } }, EStatus.UnderEvaluation},
            };
        }
    }
}