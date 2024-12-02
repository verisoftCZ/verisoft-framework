using NUnit.Framework;

namespace Verisoft.Core.Common.Test
{
    public class StringExtensionTest
    {
        public class ReplaceAccents
        {
            [Test]
            public void ShouldReturnNullWhenInputIsNull()
            {
                string value = null;

                var actual = value.ReplaceAccents();

                Assert.That(actual, Is.Null);
            }

            [Test]
            public void ShouldReturnEmptyStringWhenInputIsEmptyString()
            {
                var value = string.Empty;

                var actual = value.ReplaceAccents();

                Assert.That(actual, Is.Empty);
            }

            [Test]
            public void ShouldRemoveAccentsWhenInputContainsAccentsChar()
            {
                var value = "Příliš žluťoučký kůň úpěl ďábelské ódy";

                var actual = value.ReplaceAccents();

                Assert.That(actual, Is.EqualTo("Prilis zlutoucky kun upel dabelske ody"));
            }

            [Test]
            public void ShouldRemoveAccentsAndRemainUpperCharsWhenInputContainsAccentsChar()
            {
                var value = "Příliš Žluťoučký Kůň Úpěl Ďábelské Ódy";

                var actual = value.ReplaceAccents();

                Assert.That(actual, Is.EqualTo("Prilis Zlutoucky Kun Upel Dabelske Ody"));
            }
        }

        public class SubstringAndTrimToLastWord
        {
            [Test]
            public void ShouldReturnNullWhenInputIsNull()
            {
                string value = null;

                var actual = value.ReplaceAccents();

                Assert.That(actual, Is.Null);
            }

            [Test]
            public void ShouldReturnEmptyStringWhenInputIsEmptyString()
            {
                var value = string.Empty;

                var actual = value.ReplaceAccents();

                Assert.That(actual, Is.Empty);
            }

            [Test]
            public void ShouldFullStringWhenStringIsShorterThenMaximumAndDoesNotContainSpace()
            {
                var value = "ShortString";

                var actual = value.SubstringAndTrimToLastWord(15);

                Assert.That(actual, Is.EqualTo(value));
            }

            [Test]
            public void ShouldTrimStringWhenStringIsLongerThenMaximumAndDoesNotContainSpace()
            {
                var value = "LongStringWithoutSpace";

                var actual = value.SubstringAndTrimToLastWord(10);

                Assert.That(actual, Is.EqualTo("LongString"));
            }

            [Test]
            public void ShouldTrimStringToLastWordWhenStringIsLongerThenMaximumAndContainsSpace()
            {
                var value = "Long String With Space";

                var actual = value.SubstringAndTrimToLastWord(10);

                Assert.That(actual, Is.EqualTo("Long"));
            }
        }
    }
}