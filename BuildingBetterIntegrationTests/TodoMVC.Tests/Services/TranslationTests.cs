using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace TodoMVC.Tests.Services
{
    public class TranslationTests : IUseFixture<TranslationFixture>
    {
        private TranslationFixture translation;

        public void SetFixture(TranslationFixture data)
        {
            translation = data;
        }

        [Fact]
        public void HelloIsSalut()
        {
            // Arrange
            var expected = "Salut";
            var text = "Hello";
            var to = "fr";
            var from = "en";

            // Act
            var request = translation.TranslatorContainer.Translate(text, to, from);
            var results = request.Execute().ToList();

            // Assert
            results.ToList().Should().OnlyContain(x => x.Text == expected);
        }
    }
}
