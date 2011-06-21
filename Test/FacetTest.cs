using System;
using Ch.Elca.Iiop.Idl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using scs.core;
using Scs.Core;
using Scs.Core.Servant;

namespace Test
{
  [TestClass()]
  public class FacetTest
  {
    private static String facetName;
    private static String interfaceName;
    private static MarshalByRefObject servant;

    private TestContext testContextInstance;
    public TestContext TestContext {
      get {
        return testContextInstance;
      }
      set {
        testContextInstance = value;
      }
    }

    [ClassInitialize()]
    public static void BeforeClass(TestContext testContext) {
      facetName = "Faceta";
      interfaceName = Repository.GetRepositoryID(typeof(IMetaInterface));

      ComponentId componentId = new ComponentId("Component1", 1, 0, 0, "none");
      ComponentContext context = new DefaultComponentContext(componentId);
      servant = new IMetaInterfaceServant(context);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentException))]
    public void FacetConstructorTest1() {
      Facet target = new Facet(null, interfaceName, servant);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentException))]
    public void FacetConstructorTest2() {
      Facet target = new Facet(facetName, null, servant);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void FacetConstructorTest3() {
      Facet target = new Facet(facetName, interfaceName, null);
    }

    [TestMethod()]
    public void FacetConstructorTest4() {
      Facet target = new Facet(facetName, interfaceName, servant);
      Assert.IsNotNull(target);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void FacetConstructorTest5() {
      ComponentId componentId = new ComponentId("Component1", 1, 0, 0, "none");
      ComponentContext context = new DefaultComponentContext(componentId);

      string interfaceName = typeof(IMetaInterface).Name;
      MarshalByRefObject servant = new IComponentServant(context);
      Facet target = new Facet(facetName, interfaceName, servant);
    }

    [TestMethod()]
    public void ReferenceTest() {
      Facet target = new Facet(facetName, interfaceName, servant);
      Assert.AreEqual(servant, target.Reference);
    }

    [TestMethod()]
    public void NameTest() {
      Facet target = new Facet(facetName, interfaceName, servant);
      Assert.AreEqual(facetName, target.Name);
    }

    [TestMethod()]
    public void InterfaceNameTest() {
      Facet target = new Facet(facetName, interfaceName, servant);
      Assert.AreEqual(interfaceName, target.InterfaceName);
    }

    [TestMethod()]
    public void GetDescriptionTest() {
      Facet target = new Facet(facetName, interfaceName, servant);
      FacetDescription expected = new FacetDescription(facetName, interfaceName, servant);
      FacetDescription actual = target.GetDescription();
      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void EqualsTests() {
      Facet target = new Facet(facetName, interfaceName, servant);
      Facet obj1 = new Facet(facetName, interfaceName, servant);
      Facet obj2 = new Facet("Equals", interfaceName, servant);

      bool actual = target.Equals(obj1);
      Assert.AreEqual(true, actual);
      actual = target.Equals(obj2);
      Assert.AreEqual(false, actual);

      actual = target.Equals(null);
      Assert.AreEqual(false, actual);
      actual = target.Equals(new Object());
      Assert.AreEqual(false, actual);
    }
  }
}
