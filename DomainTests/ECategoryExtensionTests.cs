using FluentAssertions;
using Model.Domain.Entities;

namespace DomainTests
{
    public class ECategoryExtensionTests
    {
        [Theory]
        [MemberData(nameof(ShouldRejectAny))]
        public void should_reject_any_above_1000(ECategory category, decimal value)
        {
            var sut = category.CheckStatusByRules(value);

            sut.Should().Be(EStatus.Rejected);
        }
        
        [Theory]
        [MemberData(nameof(ShouldApproveAny))]
        public void should_aprove_any_up_to_100(ECategory category, decimal value)
        {
            var sut = category.CheckStatusByRules(value);

            sut.Should().Be(EStatus.Approved);
        }
        
        [Theory]
        [MemberData(nameof(SpecificConditions))]
        public void answer_should_be_correct(ECategory category, decimal value, EStatus expectedStatus)
        {
            var sut = category.CheckStatusByRules(value);

            sut.Should().Be(expectedStatus);
        }


        public static IEnumerable<object[]> ShouldRejectAny()
        {
            return new [] {
                new object[]{ ECategory.Food, 1005},
                new object[]{ ECategory.Travel, 1001},
                new object[]{ ECategory.Transportation, 2000},
                new object[]{ ECategory.Acomodation, 1558},
                new object[]{ ECategory.Others, 1050},
            };
        }
        
        public static IEnumerable<object[]> ShouldApproveAny()
        {
            return new [] {
                new object[]{ ECategory.Food, 5},
                new object[]{ ECategory.Travel, 90},
                new object[]{ ECategory.Transportation, 100},
                new object[]{ ECategory.Acomodation, 12},
                new object[]{ ECategory.Others, 97},
            };
        }
        
        public static IEnumerable<object[]> SpecificConditions()
        {
            return new [] {
                new object[]{ ECategory.Food, 5, EStatus.Approved},
                new object[]{ ECategory.Food, 102, EStatus.Approved},
                new object[]{ ECategory.Food, 501, EStatus.UnderEvaluation},
                new object[]{ ECategory.Food, 5122, EStatus.Rejected},
                new object[]{ ECategory.Transportation, 5, EStatus.Approved},
                new object[]{ ECategory.Transportation, 102, EStatus.Approved},
                new object[]{ ECategory.Transportation, 501, EStatus.UnderEvaluation},
                new object[]{ ECategory.Transportation, 2000, EStatus.Rejected},
                new object[]{ ECategory.Acomodation, 506, EStatus.UnderEvaluation},
                new object[]{ ECategory.Acomodation, 1001, EStatus.Rejected},
                new object[]{ ECategory.Acomodation, 100, EStatus.Approved},
                new object[]{ ECategory.Others, 506, EStatus.UnderEvaluation},
                new object[]{ ECategory.Others, 1001, EStatus.Rejected},
                new object[]{ ECategory.Others, 100, EStatus.Approved},
                new object[]{ ECategory.Travel, 506, EStatus.UnderEvaluation},
                new object[]{ ECategory.Travel, 1001, EStatus.Rejected},
                new object[]{ ECategory.Travel, 100, EStatus.Approved},
            };
        }
    }
}