using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Bits.IntegrationTests.Infrastructure
{
    public class TranslationClientFacts : IUseFixture<TranslatorFixture>
    {
        private TranslatorFixture translator;

        public void SetFixture(TranslatorFixture data)
        {
            translator = data;
        }

        [Fact]
        public void TranslateReturnsFrenchSaultForEnglishHello()
        {
            var text = "Hello";
            var from = "en";
            var to = "fr";
            var expected = "Salut";

            var actual = translator.Container.Translate(text, to, from).Execute().ToList();

            actual.Should().OnlyContain(x => x.Text == expected);
        }

        // TranslateEmpty/Null/InvalidText???
        // TranslateEmpty/Null/InvalidFrom???
        // TranslateEmpty/Null/InvalidTo???? 
        // TranslateInvalidServiceUriThrows???
        // TranslateInvalidServiceCredentialsThrows???
      }
}
