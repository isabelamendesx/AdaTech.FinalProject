using Model.Domain.Entities;
using FluentAssertions;
using Model.Service.Services.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Service.Exceptions;

namespace ServicesTests
{
    public class RuleConflictsAndOverlapsTests
    {
        [Theory]
        [MemberData(nameof(ShouldDetectOverlap))]
        public void should_throw_exception_for_overlap(Rule newRule, List<Rule?> existingRules)
        {
            Assert.Throws<RuleOverlapException>(() =>
                RuleConflictAndOverlapChecker.CheckForConflictAndOverlap(newRule, existingRules));
        }

        [Theory]
        [MemberData(nameof(ShouldDetectConflict))]
        public void should_throw_exception_for_conflict(Rule newRule, List<Rule?> existingRules)
        {
            Assert.Throws<RuleConflictException>(() =>
                RuleConflictAndOverlapChecker.CheckForConflictAndOverlap(newRule, existingRules));
        }

        [Theory]
        [MemberData(nameof(ShouldNotThrowException))]
        public void should_not_throw_exception(Rule newRule, List<Rule?> existingRules)
        {
            var exception = Record.Exception(() =>
                RuleConflictAndOverlapChecker.CheckForConflictAndOverlap(newRule, existingRules));
            Assert.Null(exception);
        }

        public static IEnumerable<object[]> ShouldDetectOverlap()
        {
            return new[]
            {
                new object[]{
                    new Rule()
                        {
                            MinValue = 1400,
                            MaxValue = decimal.MaxValue,
                            Action = true
                        },
                     new List<Rule?>()
                        {
                            new Rule()
                            {
                                MinValue = 1000,
                                MaxValue = decimal.MaxValue,
                                Action = true
                            }
                        }
                } ,
                new object[]{
                    new Rule()
                        {
                            MinValue = 1000,
                            MaxValue = decimal.MaxValue,
                            Action = true
                        },
                     new List<Rule?>()
                        {
                            new Rule()
                            {

                                MinValue = 1400,
                                MaxValue = decimal.MaxValue,
                                Action = true
                            }
                        }
                },
                new object[]{
                    new Rule()
                        {
                            MinValue = 500,
                            MaxValue = 1500,
                            Action = true
                        },
                     new List<Rule?>()
                        {
                            new Rule()
                            {
                                MinValue = 1000,
                                MaxValue = 2000,
                                Action = true
                            }
                        }
                },
                new object[]{
                    new Rule()
                        {
                            MinValue = 500,
                            MaxValue = 1500,
                            Action = true
                        },
                     new List<Rule?>()
                        {
                            new Rule()
                            {
                                MinValue = 100,
                                MaxValue = 500,
                                Action = true
                            }
                        }
                },
                new object[]{
                    new Rule()
                        {
                            MinValue = 100,
                            MaxValue = 500,
                            Action = true
                        },
                     new List<Rule?>()
                        {
                            new Rule()
                            {
                                MinValue = 500,
                                MaxValue = 2000,
                                Action = true
                            }
                        }
                } ,
                 new object[]{
                    new Rule()
                        {
                            MinValue = 100,
                            MaxValue = 1500,
                            Action = true
                        },
                     new List<Rule?>()
                        {
                            new Rule()
                            {
                                MinValue = 500,
                                MaxValue = 1000,
                                Action = true
                            }
                        }
                } ,


            };
        }

        public static IEnumerable<object[]> ShouldDetectConflict()
        {
            return new[]
            {
                new object[]{
                    new Rule()
                        {
                            MinValue = 1400,
                            MaxValue = decimal.MaxValue,
                            Action = false
                        },
                     new List<Rule?>()
                        {
                            new Rule()
                            {
                                MinValue = 1000,
                                MaxValue = decimal.MaxValue,
                                Action = true
                            }
                        }
                } ,
                new object[]{
                    new Rule()
                        {
                            MinValue = 1000,
                            MaxValue = decimal.MaxValue,
                            Action = true
                        },
                     new List<Rule?>()
                        {
                            new Rule()
                            {

                                MinValue = 1400,
                                MaxValue = decimal.MaxValue,
                                Action = false
                            }
                        }
                },
                new object[]{
                    new Rule()
                        {
                            MinValue = 500,
                            MaxValue = 2000,
                            Action = true
                        },
                     new List<Rule?>()
                        {
                            new Rule()
                            {
                                MinValue = 1000,
                                MaxValue = 1500,
                                Action = false
                            }
                        }
                } ,
                new object[]{
                    new Rule()
                        {
                            MinValue = 1000,
                            MaxValue = 3000,
                            Action = true
                        },
                     new List<Rule?>()
                        {
                            new Rule()
                            {

                                MinValue = 2000,
                                MaxValue = decimal.MaxValue,
                                Action = false
                            }
                        }
                },
                new object[]{
                    new Rule()
                        {
                            MinValue = 500,
                            MaxValue = decimal.MaxValue,
                            Action = false
                        },
                     new List<Rule?>()
                        {
                            new Rule()
                            {

                                MinValue = 500,
                                MaxValue = 1000,
                                Action = true
                            }
                        }
                },
                new object[]{
                    new Rule()
                        {
                            MinValue = 500,
                            MaxValue = 1500,
                            Action = true
                        },
                     new List<Rule?>()
                        {
                            new Rule()
                            {

                                MinValue = 1000,
                                MaxValue = 5000,
                                Action = false
                            }
                        }
                },
            };
        }

        public static IEnumerable<object[]> ShouldNotThrowException()
        {
            return new[]
            {
                new object[]
                {
                    new Rule()
                    {
                        MinValue = 1400,
                        MaxValue = decimal.MaxValue,
                        Action = false
                    },
                    new List<Rule?>()
                    {
                        new Rule()
                        {
                            MinValue = 0,
                            MaxValue = 100,
                            Action = true
                        }
                    }
                },

                new object[]
                {
                    new Rule()
                    {
                        MinValue = 0,
                        MaxValue = 100,
                        Action = true
                    },
                    new List<Rule?>()
                    {
                        new Rule()
                        {
                            MinValue = 1400,
                            MaxValue = decimal.MaxValue,
                            Action = true
                        }
                    }
                },

                new object[]{
                    new Rule()
                        {
                            MinValue = 0,
                            MaxValue = 500,
                            Action = true
                        },
                     new List<Rule?>()
                        {
                            new Rule()
                            {
                                MinValue = 500.01M,
                                MaxValue = 1000,
                                Action = false
                            }
                        }
                } ,
                new object[]{
                    new Rule()
                        {
                            MinValue = 500,
                            MaxValue = 1000,
                            Action = false
                        },
                     new List<Rule?>()
                        {
                            new Rule()
                            {

                                MinValue = 1001,
                                MaxValue = decimal.MaxValue,
                                Action = false
                            }
                        }
                },
                new object[]{
                    new Rule()
                        {
                            MinValue = 0,
                            MaxValue = 100,
                            Action = true
                        },
                     new List<Rule?>()
                        {
                            new Rule()
                            {

                                MinValue = 500,
                                MaxValue = 700,
                                Action = true
                            }
                        }
                },
            };
        }
    }
}
