using GuessingGame.Models;
using GuessingGame.Validators;
using NUnit.Framework;

namespace GuessingGame.Tests.Validators
{
    public class GuessValidatorTest
    {
        private GuessValidator _guessValidator;
        
        [SetUp]
        public void Setup()
        {
            _guessValidator = new();
        }

        [Test]
        public void ShouldRecognizeAsNotValidIfNotADigitPresents()
        {
            GuessModel guessModel = new()
            {
                FirstDigit = "k",
                SecondDigit = "m",
                ThirdDigit = "2",
                FourthDigit = "3"
            };

            Assert.IsFalse(_guessValidator.Validate(guessModel).IsValid);
        }

        [Test]
        public void ShouldRecognizeAsNotValidIfMoreThanOneDigitPresent()
        {
            GuessModel guessModel = new()
            {
                FirstDigit = "1",
                SecondDigit = "47",
                ThirdDigit = "2",
                FourthDigit = "3"
            };

            Assert.IsFalse(_guessValidator.Validate(guessModel).IsValid);
        }

        [Test]
        public void ShouldRecognizeAsNotValidIfDigitsNotUnique()
        {
            GuessModel guessModel = new()
            {
                FirstDigit = "1",
                SecondDigit = "7",
                ThirdDigit = "7",
                FourthDigit = "3"
            };

            Assert.IsFalse(_guessValidator.Validate(guessModel).IsValid);
        }

        [Test]
        public void ShouldRecognizeAsInvalidIfStartsWithZero()
        {
            GuessModel guessModel = new()
            {
                FirstDigit = "0",
                SecondDigit = "2",
                ThirdDigit = "3",
                FourthDigit = "4"
            };

            Assert.IsFalse(_guessValidator.Validate(guessModel).IsValid);
        }

        [Test]
        public void ShouldRecognizeAsValid()
        {
            GuessModel guessModel = new()
            {
                FirstDigit = "1",
                SecondDigit = "2",
                ThirdDigit = "3",
                FourthDigit = "4"
            };

            Assert.IsTrue(_guessValidator.Validate(guessModel).IsValid);
        }
    }
}
