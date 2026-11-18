using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Domain.SharedKernel.ValueObjects;
using Xunit;

namespace Mawthooq.Domain.Tests.SharedKernel.ValueObjects
{
    public class EmailTests
    {
        [Theory]
        [InlineData("john.doe@example.com", "john.doe@example.com")]
        [InlineData("  John.Doe@Example.COM  ", "john.doe@example.com")]
        public void Should_TrimAndLowerCase_When_TheEmailIsValid(string input, string expected)
        {
            var sut = CreateEmail(input);

            sut.Value.Should().Be(expected);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("plainaddress")]
        [InlineData("missing-domain@")]
        [InlineData("@missing-local.com")]
        [InlineData("no-dot@domain")]
        [InlineData("two@@example.com")]
        [InlineData("has space@example.com")]
        public void Should_ThrowArgumentException_When_TheEmailIsInvalid(string? input)
        {
            var act = () => CreateEmail(input!);

            act.Should().Throw<ArgumentException>();
        }

        private Email CreateEmail(string value)
        {
            return new Email(value);
        }
    }
}
