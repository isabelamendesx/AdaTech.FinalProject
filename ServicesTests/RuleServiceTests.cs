﻿using FluentAssertions;
using Model.Domain.Entities;
using Model.Domain.Interfaces;
using Model.Service.Exceptions;
using Model.Service.Services;
using NSubstitute;
using Rule = Model.Domain.Entities.Rule;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Model.Service.Services.Util;
using System.Data;
using Microsoft.AspNetCore.Rewrite;
using System.Threading;
using Model.Infra.Data.Repositories;
using Serilog;

namespace ServicesTests
{
    public class RuleServiceTests
    {
        private RuleService _sut;

        private IRepository<Rule> repository;
        private CancellationToken ct;

        public RuleServiceTests()
        {
            ct = new CancellationToken();
            repository = Substitute.For<IRepository<Rule>>();

            _sut = new RuleService(repository);
        }

        [Fact]
        public async Task get_by_id_must_throw_an_exception_when_rule_could_not_be_found()
        {
            repository.GetById(Arg.Any<uint>(), ct).Returns(Task.FromResult<Rule?>(null));

            await _sut.Invoking(x => x.GetById(3, ct))
                .Should().ThrowAsync<ResourceNotFoundException>();
        }

        [Fact]
        public async Task get_by_id_sould_return_rule_when_rule_exists()
        {
            var expectedRule = new Rule { Id = 3, MinValue = 0, MaxValue = 100, Action = true, Category = new Category(), IsActive = true };
            repository.GetById(Arg.Any<uint>(), ct).Returns(Task.FromResult<Rule?>(expectedRule));

            var result = await _sut.GetById(3, ct);

            result.Should().BeEquivalentTo(expectedRule);
        }

        [Fact]
        public async Task get_all_should_return_all_rules()
        {
            repository.GetByParameter(ct).Returns(Task.FromResult<IEnumerable<Rule?>>(ListOfRules()));

            var result = await _sut.GetAll(ct);

            result.Should().BeEquivalentTo(ListOfRules());
        }


        //[Theory]
        //[InlineData(50, 100, false)]
        //[InlineData(50, 200, false)]
        //[InlineData(1500, 2000, true)]
        //[InlineData(500, 2000, true)]
        //public async Task create_rule_with_category_0_must_throw_an_exception_when_a_conflict_occurs
        //                  (decimal minValue, decimal maxValue, bool action)
        //{
        //    var existingRules = repository.GetByParameter(ct, Arg.Any<Expression<Func<Rule, bool>>>())
        //       .Returns(RulesValidToCategoryId0());

        //    var category = new Category() { Id = 0, Name = "All" };

        //    var newRule = new Rule
        //    {
        //        Id = 20,
        //        MinValue = minValue,
        //        MaxValue = maxValue,
        //        Action = action,
        //        Category = category,
        //        IsActive = true
        //    };

        //    await _sut.Invoking(x => x.CreateRule(newRule, ct))
        //           .Should().ThrowAsync<RuleConflictException>();
        //}

        //[Theory]
        //[InlineData(50, 300, true)]  //Overlap
        //[InlineData(20, 100, true)] //Overlap
        //[InlineData(800, 5000, false)] //Overlap
        //[InlineData(1000, 5000, false)] //Overlap
        //public async Task create_rule_with_category_0_must_throw_an_exception_when_a_overlap_occurs
        //                  (decimal minValue, decimal maxValue, bool action)
        //{
        //    var existingRules = repository.GetByParameter(ct, Arg.Any<Expression<Func<Rule, bool>>>())
        //       .Returns(RulesValidToCategoryId0());

        //    var category = new Category() { Id = 0, Name = "All" };

        //    var newRule = new Rule
        //    {
        //        Id = 20,
        //        MinValue = minValue,
        //        MaxValue = maxValue,
        //        Action = action,
        //        Category = category,
        //        IsActive = true
        //    };

        //    await _sut.Invoking(x => x.CreateRule(newRule, ct))
        //           .Should().ThrowAsync<RuleOverlapException>();
        //}

        [Theory]
        [InlineData(50, 100, false)]
        [InlineData(150, 600, false)]
        [InlineData(800, 1000, true)]
        [InlineData(1000, 2000, true)]
        public async Task create_rule_with_category_id_1_must_throw_an_exception_when_a_conflict_occurs
                          (decimal minValue, decimal maxValue, bool action)
        {
            var existingRules = repository.GetByParameter(ct, Arg.Any<Expression<Func<Rule, bool>>>())
               .Returns(RulesValidToCategoryId1());

            var category = new Category() { Id = 1, Name = "Food" };

            var newRule = new Rule
            {
                Id = 20,
                MinValue = minValue,
                MaxValue = maxValue,
                Action = action,
                Category = category,
                IsActive = true
            };

            await _sut.Invoking(x => x.CreateRule(newRule, ct))
                   .Should().ThrowAsync<RuleConflictException>();
        }

        [Theory]
        [InlineData(50, 300, true)]
        [InlineData(20, 100, true)]
        [InlineData(600, 1000, false)]
        [InlineData(1000, 5000, false)]
        public async Task create_rule_with_category_id_1_must_throw_an_exception_when_a_overlap_occurs
                          (decimal minValue, decimal maxValue, bool action)
        {
            var existingRules = repository.GetByParameter(ct, Arg.Any<Expression<Func<Rule, bool>>>())
               .Returns(RulesValidToCategoryId1());

            var category = new Category() { Id = 1, Name = "Food" };

            var newRule = new Rule
            {
                Id = 20,
                MinValue = minValue,
                MaxValue = maxValue,
                Action = action,
                Category = category,
                IsActive = true
            };

            await _sut.Invoking(x => x.CreateRule(newRule, ct)).Should().ThrowAsync<RuleOverlapException>();
        }

        [Theory]
        [InlineData(600, 700, true)]
        [InlineData(500.01, 799.99, true)]
        [InlineData(600, 799.99, false)]
        [InlineData(500.01, 700, false)]
        public async Task create_rule_with_category_id_1_should_succeed_when_there_is_no_conflict_or_overlap
                          (decimal minValue, decimal maxValue, bool action)
        {
            var existingRules = repository.GetByParameter(ct, Arg.Any<Expression<Func<Rule, bool>>>())
               .Returns(RulesValidToCategoryId1());

            var category = new Category() { Id = 1, Name = "Food" };

            var newRule = new Rule
            {
                Id = 20,
                MinValue = minValue,
                MaxValue = maxValue,
                Action = action,
                Category = category,
                IsActive = true
            };

            await FluentActions.Awaiting(() => _sut.CreateRule(newRule, ct))
                  .Should().NotThrowAsync();
        }


        [Fact]
        public async Task deactivte_a_category_rules_must_throw_an_exception_when_there_is_no_rule_found_for_the_category()
        {
            uint categoryId = 2;

            repository.GetByParameter(ct, Arg.Any<Expression<Func<Rule, bool>>>())
                .Returns(Task.FromResult<IEnumerable<Rule?>>(null));

            await _sut.Invoking(x => x.DeactivateACategorysRules(categoryId, ct))
                  .Should().ThrowAsync<ResourceNotFoundException>();
        }

        [Fact]
        public async Task deactivte_a_category_rules_should_succeed_to_category_id_2()
        {
            uint categoryId = 2;

            repository.GetByParameter(ct, Arg.Any<Expression<Func<Rule, bool>>>())
                .Returns(Task.FromResult<IEnumerable<Rule?>>(ActiveRulesToCategoryId2()));

            var result = await _sut.DeactivateACategorysRules(categoryId, ct);

            result.Should().BeTrue();
        }

        [Theory]
        [InlineData(6)]
        [InlineData(5)]
        public async Task deactivte_rule_must_throw_an_exception_when_there_is_no_rule_found(uint ruleId)
        {

            repository.GetById(Arg.Any<uint>(), ct).Returns(Task.FromResult<Rule?>(null));

            await _sut.Invoking(x => x.GetById(ruleId, ct)).Should().ThrowAsync<ResourceNotFoundException>();
        }

        [Fact]
        public async Task deactivte_rule_with_id_4_should_succeed()
        {
            var category1 = new Category() { Id = 1, Name = "Food" };
            var expectedRule = new Rule() {
                    Id = 4, MinValue = 800, MaxValue = 999.99M, Action = false, Category = category1, IsActive = true
                };

            repository.GetById(Arg.Any<uint>(), ct).Returns(Task.FromResult<Rule?>(expectedRule));

            uint ruleId = 4;

            var result = await _sut.DeactivateACategorysRules(ruleId, ct);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task get_rules_to_approve_any_should_return_correct_rules()
        {
            repository.GetByParameter(ct, Arg.Any<Expression<Func<Rule, bool>>>())
                .Returns(ListOfRules().Where(x => x.Category.Id == 0 && x.Action && x.IsActive));

            var result = await _sut.GetRulesToApproveAny(ct);

            result.Should().BeEquivalentTo(RulesToApproveAny());
        }

        [Fact]
        public async Task get_rules_to_reprove_by_category_id_3_should_return_correct_rules()
        {
            repository.GetByParameter(ct, Arg.Any<Expression<Func<Rule, bool>>>())
                .Returns(ListOfRules().Where(x => x.Category.Id == 3 && !x.Action && x.IsActive));

            var result = await _sut.GetRulesToRejectByCategoryId(3, ct);

            result.Should().BeEquivalentTo(RulesToReproveByCategoryId3());
        }

        [Fact]
        public async Task get_rules_to_approve_by_category_id_2_should_return_correct_rules()
        {
            repository.GetByParameter(ct, Arg.Any<Expression<Func<Rule, bool>>>())
                .Returns(ListOfRules().Where(x => x.Category.Id == 2 && x.Action && x.IsActive));

            var result = await _sut.GetRulesToApproveByCategoryId(2, ct);

            result.Should().BeEquivalentTo(RulesToApproveByCategoryId2());
        }

        [Fact]
        public async Task get_rules_to_reprove_any_should_return_correct_rules()
        {
            repository.GetByParameter(ct, Arg.Any<Expression<Func<Rule, bool>>>())
                .Returns(ListOfRules().Where(x => x.Category.Id == 0 && !x.Action && x.IsActive));

            var result = await _sut.GetRulesToRejectAny(ct);

            result.Should().BeEquivalentTo(RulesToReproveAny());
        }






        public static IEnumerable<Rule> ListOfRules()
        {
            var allCategories = new Category() { Id = 0, Name = "All" };
            var category1 = new Category() { Id = 1, Name = "Food" };
            var category2 = new Category() { Id = 2, Name = "Accomodation" };
            var category3 = new Category() { Id = 3, Name = "Transportation" };
            var category4 = new Category() { Id = 4, Name = "Stationery" };


            return new List<Rule>{
                new Rule() {
                    Id = 1, MinValue = 0, MaxValue = 100, Action = true, Category = allCategories, IsActive = true
                },
                new Rule() {
                    Id = 2, MinValue = 100.01M, MaxValue = 300, Action = true, Category = category1, IsActive = false
                },
                new Rule() {
                    Id = 3, MinValue = 100.01M, MaxValue = 500, Action = true, Category = category1, IsActive = true
                },
                new Rule() {
                    Id = 4, MinValue = 800, MaxValue = 999.99M, Action = false, Category = category1, IsActive = true
                },
                new Rule() {
                    Id = 5, MinValue = 100.01M, MaxValue = 200, Action = true, Category = category2, IsActive = true
                },
                new Rule() {
                    Id = 6, MinValue = 500, MaxValue = 999.99M, Action = false, Category = category2, IsActive = true
                },
                new Rule() {
                    Id = 7,MinValue = 100.01M, MaxValue = 500, Action = true, Category = category3, IsActive = true
                },
                new Rule() {
                    Id = 8,MinValue = 700, MaxValue = 999.99M, Action = false, Category = category3, IsActive = true
                },
                new Rule() {
                    Id = 9, MinValue = 500.01M, MaxValue = 999.99M, Action = true, Category = category3, IsActive = false
                },
                new Rule() {
                    Id = 10, MinValue = 1000, MaxValue = decimal.MaxValue, Action = false, Category = allCategories, IsActive = true
                },
                new Rule() {
                    Id = 11, MinValue = 0, MaxValue = 50, Action = true, Category = category4, IsActive = false
                },
            };
        }

        //public static IEnumerable<Rule> RulesValidToCategoryId0()
        //{
        //    var allCategories = new Category() { Id = 0, Name = "All" };

        //    return new List<Rule>{
        //        new Rule() {
        //            Id = 1, MinValue = 0, MaxValue = 100, Action = true, Category = allCategories, IsActive = true
        //        },
        //        new Rule() {
        //            Id = 10, MinValue = 1000, MaxValue = decimal.MaxValue, Action = false, Category = allCategories, IsActive = true
        //        }
        //    };
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

        public static IEnumerable<Rule> ActiveRulesToCategoryId2()
        {
            var category2 = new Category() { Id = 2, Name = "Accomodation" };

            return new List<Rule>{
                new Rule() {
                    Id = 5, MinValue = 100.01M, MaxValue = 200, Action = true, Category = category2, IsActive = true
                },
                new Rule() {
                    Id = 6, MinValue = 500, MaxValue = 999.99M, Action = false, Category = category2, IsActive = true
                },
            };

        }

            public static IEnumerable<Rule> RulesToApproveAny()
        {
            var allCategories = new Category() { Id = 0, Name = "All" };
            
            return new List<Rule>{
                new Rule() {
                    Id = 1, MinValue = 0, MaxValue = 100, Action = true, Category = allCategories, IsActive = true
                },
            };
        }

        public static IEnumerable<Rule> RulesToReproveByCategoryId3()
        {
            var category3 = new Category() { Id = 3, Name = "Transportation" };

            return new List<Rule>
                    {
                        new Rule() {
                            Id = 8,MinValue = 700, MaxValue = 999.99M, Action = false, Category = category3, IsActive = true
                        },
                    };
        }

        public static IEnumerable<Rule> RulesToApproveByCategoryId2()
        {
            var category2 = new Category() { Id = 2, Name = "Accomodation" };

            return new List<Rule>
            {
                new Rule() {
                        Id = 5, MinValue = 100.01M, MaxValue = 200, Action = true, Category = category2, IsActive = true
                    },
            };
        }

        public static IEnumerable<Rule> RulesToReproveAny()
        {
            var allCategories = new Category() { Id = 0, Name = "All" };

            return new List<Rule>{
                 new Rule() {
                    Id = 10, MinValue = 1000, MaxValue = decimal.MaxValue, Action = false, Category = allCategories, IsActive = true
                },
            };

        }

    }
}