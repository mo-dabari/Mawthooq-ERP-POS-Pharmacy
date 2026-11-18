using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Domain.SharedKernel.ValueObjects;
using Xunit;

namespace Mawthooq.Domain.Tests.SharedKernel.ValueObjects
{
    public class ImageTests
    {

        [Fact]
        public void Should_CreateImage_When_ImageIsValid()
        {
            Image sut = Image.Create("products/2/new.png", "new.png", "image/png");

            sut.Should().Be(Image.Create("products/2/new.png", "new.png", "image/png"));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Should_ThrowArgumentException_When_CreateImageIsInvalid(string? paramName)
        {
            var act = () => Image.Create(paramName!, paramName!, paramName!);

            act.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(Image.MaxStorageKeyLength + 1, 1, 1)]
        [InlineData(1, Image.MaxFileNameLength + 1, 1)]
        [InlineData(1, 1, Image.MaxMimeTypeLength + 1)]
        public void Should_Throw_When_AnImageFieldExceedsItsMaxLength(int keyLength, int nameLength, int mimeLength)
        {
            Action act = () => Image.Create(new string('k', keyLength), new string('f', nameLength), new string('m', mimeLength));
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Should_AcceptValuesAtTheMaxLengths_AndTrimThem()
        {
            Image image = Image.Create($"  {new string('k', Image.MaxStorageKeyLength)}  ",
                                            new string('f', Image.MaxFileNameLength),
                                            new string('m', Image.MaxMimeTypeLength));

            image.StorageKey.Should().HaveLength(Image.MaxStorageKeyLength);
            image.FileName.Should().HaveLength(Image.MaxFileNameLength);
        }
    }
}
