using TESTING_INTRODUCTION;

namespace NUNIT_INTRODUCTION;

[TestFixture]
    public class Tests
    {
        Calculate cal = null;
        [SetUp]
        public void Setup()
        {
            cal = new Calculate();
        }

        [Test]

        [TestCase(15,20,35)]
        [TestCase(20, 20,40)]

        public void Addition(int x,int y,int expected)
        {
            //arrange 
            int actual;
         //   int expected;
            //act 
            actual = cal.Addition(x,y);
            //assert
            Assert.AreEqual(expected, actual);
            Assert.Pass();
        }
       // [TestCase(20, 15)]
        [TestCase(20, 20)]

        public void Subtraction(int x, int y)
        {
            //arrange 
            int actual;
            int expected = 0;
            //act 
            actual = cal.Subtraction(x, y);
            //assert
            Assert.AreEqual(expected, actual);
            Assert.Pass();
        }

      //  [TestCase(20, 15)]
        [TestCase(20, 20)]

        public void Multiply(int x, int y)
        {
            //arrange 
            int actual;
            int expected = 400;
            //act 
            actual = cal.Multiplication(x, y);
            //assert
            Assert.AreEqual(expected, actual);
            Assert.Pass();
        }

       // [TestCase(20, 15)]
        [TestCase(20, 20)]

        public void Division(int x, int y)
        {
            //arrange 
            int actual;
            int expected = 0;
            //act 
            actual = cal.Divide(x, y);
            //assert
            Assert.AreEqual(expected, actual);
            Assert.Pass();
        }
       
        [Test]
       public void DivideWithException()
        {
            Assert.Throws<DivideByZeroException>(() => cal.Divide(12, 0));
        }

        [Test]
         [Ignore("will test it later ")]
        public void Divide()
        {
            int actual = cal.Divide(12, 4);
            int expected = 3;
            Assert.AreEqual(expected, actual);
        }

        [TestCase("", 0)] // Empty password
        [TestCase("12345", 1)] // Digits only
        [TestCase("password123", 2)] // Lowercase + digits
        [TestCase("Password123", 3)] // Uppercase + lowercase + digits
        [TestCase("Password@123", 4)] // Special char + uppercase + lowercase + digits
        public void GetPasswordStrength_ShouldReturnExpectedStrength(string password, int expectedStrength)
        {
            int result = cal.PasswordStrength(password);
            Assert.AreEqual(expectedStrength, result);
        }

        [TearDown]
        public void TearDown()
        {
            cal = null;
        }

    }