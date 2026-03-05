namespace MOCK_TESTING;
using INTRO_MODULE;
using Moq;

[TestClass]
public class MockTesting
{
    [TestMethod]
    public void TestMethod1()
    {
        Mock<CheckEmployee> check = new  Mock<CheckEmployee>();
        check.Setup(x => x.CheckEMP()).Returns(true);
        check.Setup(x => x.Subtract(It.IsAny<int>(), It.IsAny<int>())).Returns(1);
        
        ProcessEmployee obj = new ProcessEmployee();
        Assert.AreEqual(obj.InsertEmployee(check.Object), true);
        Assert.AreEqual(obj.Subtract(check.Object),1);
    }
    
    [TestMethod]
    public void TestMethod2()
    {
        CheckEmployee chk1 = new CheckEmployee();
        int k = chk1.Subtract(4, 1);
        int expected = 3;
        Assert.AreEqual(k, expected);

    }
}