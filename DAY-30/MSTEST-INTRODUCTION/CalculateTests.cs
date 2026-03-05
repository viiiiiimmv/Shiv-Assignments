using TESTING_INTRODUCTION;
namespace MSTEST_INTRODUCTION;
using System;

[TestClass]
public class CalculateTests
    {
        private Calculate _calculator;
        [TestInitialize]
        public void Setup()
        {
            _calculator = new Calculate();
        }

        [TestMethod]
        public void Addition_ShouldReturnCorrectSum()
        {
            int result = _calculator.Addition(5, 3);
            Assert.AreEqual(8, result);
        }

        [TestMethod]
        public void Subtract_ShouldReturnCorrectResult_WhenNum1IsGreater()
        {
            int result = _calculator.Subtraction(10, 3);
            Assert.AreEqual(7, result);
        }

        [TestMethod]
        public void Subtract_ShouldReturnCorrectResult_WhenNum2IsGreater()
        {
            int result = _calculator.Subtraction(3, 10);
            Assert.AreEqual(7, result);
        }

        [TestMethod]
        public void Multiplication_ShouldReturnCorrectProduct()
        {
            int result = _calculator.Multiplication(5, 4);
            Assert.AreEqual(20, result);
        }

        [TestMethod]
        public void Divide_ShouldReturnCorrectQuotient()
        {
            int result = _calculator.Divide(10, 2);
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        [ExpectedException(typeof(DivideByZeroException))]
        public void Divide_ShouldThrowDivideByZeroException_WhenDividingByZero()
        {
            _calculator.Divide(10, 0);
        }

        [DataTestMethod]
        [DataRow("", 0)] // Empty password
        [DataRow("12345", 1)] // Digits only
        [DataRow("password123", 2)] // Lowercase + digits
        [DataRow("Password123", 3)] // Uppercase + lowercase + digits
        [DataRow("Password@123", 4)] // Special char + uppercase + lowercase + digits
        public void GetPasswordStrength_ShouldReturnExpectedStrength(string password, int expectedStrength)
        {
            int result = _calculator.PasswordStrength(password);
            Assert.AreEqual(expectedStrength, result);
        }
    }