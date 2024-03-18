using Model.Domain.Entities;
using Model.Service.Exceptions;
using Model.Service.Services.Util;

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
                            Id = 98,
                            MinValue = 1400,
                            MaxValue = decimal.MaxValue,
                            Action = true,
                            Category = new Category
                                {
                                    Name = "Category name"
                                }
                        },
                     new List<Rule?>()
                        {
                            new Rule()
                            {
                                Id = 99,
                                MinValue = 1000,
                                MaxValue = decimal.MaxValue,
                                Action = true,
                                Category = new Category
                                {
                                    Name = "Category name"
                                }
                            }
                        }
                } ,
                new object[]{
                    new Rule()
                        {
                            Id = 98,
                            MinValue = 1000,
                            MaxValue = decimal.MaxValue,
                            Action = true,
                            Category = new Category
                                {
                                    Name = "Category name"
                                }
                        },
                     new List<Rule?>()
                        {
                            new Rule()
                            {
                                Id = 99,
                                MinValue = 1400,
                                MaxValue = decimal.MaxValue,
                                Action = true,
                                Category = new Category
                                {
                                    Name = "Category name"
                                }
                            }
                        }
                },
                new object[]{
                    new Rule()
                        {
                            Id = 98,
                            MinValue = 500,
                            MaxValue = 1500,
                            Action = true,
                            Category = new Category
                                {
                                    Name = "Category name"
                                }
                        },
                     new List<Rule?>()
                        {
                            new Rule()
                            {
                                Id = 99,
                                MinValue = 1000,
                                MaxValue = 2000,
                                Action = true,
                                Category = new Category
                                {
                                    Name = "Category name"
                                }
                            }
                        }
                },
                new object[]{
                    new Rule()
                        {
                            Id = 98,
                            MinValue = 500,
                            MaxValue = 1500,
                            Action = true,
                            Category = new Category
                                {
                                    Name = "Category name"
                                }
                        },
                     new List<Rule?>()
                        {
                            new Rule()
                            {
                                Id = 99,
                                MinValue = 100,
                                MaxValue = 500,
                                Action = true,
                                Category = new Category
                                {
                                    Name = "Category name"
                                }
                            }
                        }
                },
                new object[]{
                    new Rule()
                        {
                            Id = 98,
                            MinValue = 100,
                            MaxValue = 500,
                            Action = true,
                            Category = new Category
                                {
                                    Name = "Category name"
                                }
                        },
                     new List<Rule?>()
                        {
                            new Rule()
                            {
                                Id = 99,
                                MinValue = 500,
                                MaxValue = 2000,
                                Action = true,
                                Category = new Category
                                {
                                    Name = "Category name"
                                }
                            }
                        }
                } ,
                 new object[]{
                    new Rule()
                        {
                            Id = 98,
                            MinValue = 100,
                            MaxValue = 1500,
                            Action = true,
                            Category = new Category
                                {
                                    Name = "Category name"
                                }
                        },
                     new List<Rule?>()
                        {
                            new Rule()
                            {
                                Id = 99,
                                MinValue = 500,
                                MaxValue = 1000,
                                Action = true,
                                Category = new Category
                                {
                                    Name = "Category name"
                                }
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
                            Id = 1,
                            MinValue = 1400,
                            MaxValue = decimal.MaxValue,
                            Action = false,
                            Category = new Category
                                {
                                    Name = "Category name"
                                }
                        },
                     new List<Rule?>()
                        {
                            new Rule()
                            {
                                Id = 3,
                                MinValue = 1000,
                                MaxValue = decimal.MaxValue,
                                Action = true,
                                Category = new Category
                                {
                                    Name = "Category name"
                                }
                            }
                        }
                } ,
                new object[]{
                    new Rule()
                        {
                            Id = 2,
                            MinValue = 1000,
                            MaxValue = decimal.MaxValue,
                            Action = true,
                            Category = new Category
                                {
                                    Name = "Category name"
                                }
                        },
                     new List<Rule?>()
                        {
                            new Rule()
                            {
                                Id = 4,
                                MinValue = 1400,
                                MaxValue = decimal.MaxValue,
                                Action = false,
                                Category = new Category
                                {
                                    Name = "Category name"
                                }
                            }
                        }
                },
                new object[]{
                    new Rule()
                        {
                            Id = 6,
                            MinValue = 500,
                            MaxValue = 2000,
                            Action = true,
                            Category = new Category
                                {
                                    Name = "Category name"
                                }
                        },
                     new List<Rule?>()
                        {
                            new Rule()
                            {
                                Id = 7,
                                MinValue = 1000,
                                MaxValue = 1500,
                                Action = false,
                                Category = new Category
                                {
                                    Name = "Category name"
                                }
                            }
                        }
                } ,
                new object[]{
                    new Rule()
                        {
                            Id = 8,
                            MinValue = 1000,
                            MaxValue = 3000,
                            Action = true,
                            Category = new Category
                                {
                                    Name = "Category name"
                                }
                        },
                     new List<Rule?>()
                        {
                            new Rule()
                            {
                                Id = 9,
                                MinValue = 2000,
                                MaxValue = decimal.MaxValue,
                                Action = false,
                                Category = new Category
                                {
                                    Name = "Category name"
                                }
                            }
                        }
                },
                new object[]{
                    new Rule()
                        {
                            Id = 10,
                            MinValue = 500,
                            MaxValue = decimal.MaxValue,
                            Action = false,
                            Category = new Category
                                {
                                    Name = "Category name"
                                }
                        },
                     new List<Rule?>()
                        {
                            new Rule()
                            {
                                Id = 11,
                                MinValue = 500,
                                MaxValue = 1000,
                                Action = true,
                                Category = new Category
                                {
                                    Name = "Category name"
                                }
                            }
                        }
                },
                new object[]{
                    new Rule()
                        {
                            Id = 12,
                            MinValue = 500,
                            MaxValue = 1500,
                            Action = true,
                            Category = new Category
                                {
                                    Name = "Category name"
                                }
                        },
                     new List<Rule?>()
                        {
                            new Rule()
                            {
                                Id = 13,
                                MinValue = 1000,
                                MaxValue = 5000,
                                Action = false,
                                Category = new Category
                                {
                                    Name = "Category name"
                                }
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
                        Id = 12,
                        MinValue = 1400,
                        MaxValue = decimal.MaxValue,
                        Action = false,
                        Category = new Category
                                {
                                    Name = "Category name"
                                }
                    },
                    new List<Rule?>()
                    {
                        new Rule()
                        {
                            Id = 13,
                            MinValue = 0,
                            MaxValue = 100,
                            Action = true,
                            Category = new Category
                                {
                                    Name = "Category name"
                                }
                        }
                    }
                },

                new object[]
                {
                    new Rule()
                    {
                        Id = 14,
                        MinValue = 0,
                        MaxValue = 100,
                        Action = true,
                        Category = new Category
                                {
                                    Name = "Category name"
                                }
                    },
                    new List<Rule?>()
                    {
                        new Rule()
                        {
                            Id = 25,
                            MinValue = 1400,
                            MaxValue = decimal.MaxValue,
                            Action = true,
                            Category = new Category
                                {
                                    Name = "Category name"
                                }
                        }
                    }
                },

                new object[]{
                    new Rule()
                        {
                            Id = 12,
                            MinValue = 0,
                            MaxValue = 500,
                            Action = true,
                            Category = new Category
                                {
                                    Name = "Category name"
                                }
                        },
                     new List<Rule?>()
                        {
                            new Rule()
                            {
                                Id = 18,
                                MinValue = 500.01M,
                                MaxValue = 1000,
                                Action = false,
                                Category = new Category
                                {
                                    Name = "Category name"
                                }
                            }
                        }
                } ,
                new object[]{
                    new Rule()
                        {
                            Id = 12,
                            MinValue = 500,
                            MaxValue = 1000,
                            Action = false,
                            Category = new Category
                                {
                                    Name = "Category name"
                                }
                        },
                     new List<Rule?>()
                        {
                            new Rule()
                            {
                                Id = 34,
                                MinValue = 1001,
                                MaxValue = decimal.MaxValue,
                                Action = false,
                                Category = new Category
                                {
                                    Name = "Category name"
                                }
                            }
                        }
                },
                new object[]{
                    new Rule()
                        {
                            Id = 13,
                            MinValue = 0,
                            MaxValue = 100,
                            Action = true,
                            Category = new Category
                                {
                                    Name = "Category name"
                                }
                        },
                     new List<Rule?>()
                        {
                            new Rule()
                            {
                                Id = 89,
                                MinValue = 500,
                                MaxValue = 700,
                                Action = true,
                                Category = new Category
                                {
                                    Name = "Category name"
                                }
                            }
                        }
                },
            };
        }
    }
}
