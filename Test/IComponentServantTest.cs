using System;
using Ch.Elca.Iiop.Idl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using scs.core;
using Scs.Core;
using Scs.Core.Servant;

namespace Test
{
  [TestClass()]
  public class IComponentServantTest
  {
    private TestContext testContextInstance;
    private static ComponentContext context;

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
      ComponentId componentId = new ComponentId("Test", 1, 0, 0, "none");
      context = new DefaultComponentContext(componentId);
    }

    [TestMethod()]
    public void IComponentServantConstructorTest1() {
      IComponentServant target = new IComponentServant(context);
      Assert.IsNotNull(target);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void IComponentServantConstructorTest2() {
      IComponentServant target = new IComponentServant(null);
    }

    [TestMethod()]
    public void getFacetByNameTest1() {
      IComponentServant target = new IComponentServant(context);
      string facetName = typeof(IReceptacles).Name;
      Facet facet = context.GetFacetByName(facetName);
      MarshalByRefObject expected = facet.Reference;
      MarshalByRefObject actual = target.getFacetByName(facetName);
      Assert.AreEqual(expected, actual);
    }

    [TestMethod()]
    public void getFacetByNameTest3() {
      IComponentServant target = new IComponentServant(context);
      MarshalByRefObject actual = target.getFacetByName("InvalidFacetName");
      Assert.IsNull(actual);
    }

    [TestMethod()]
    public void getFacetTest1() {
      IComponentServant target = new IComponentServant(context);
      Type facetType = typeof(IReceptacles);
      string facetName = typeof(IReceptacles).Name;
      string facetInterface = Repository.GetRepositoryID(facetType);
      Facet facet = context.GetFacetByName(facetName);
      MarshalByRefObject expected = facet.Reference;
      MarshalByRefObject actual = target.getFacet(facetInterface);
      Assert.AreEqual(expected, actual);
    }

    [TestMethod()]
    public void getFacetTest2() {
      IComponentServant target = new IComponentServant(context);
      MarshalByRefObject actual = target.getFacet("IDL:invalid/interface:1.0");
    }

    [TestMethod()]
    public void getComponentIdTest() {
      ComponentId expected = new ComponentId("Name", 10, 10, 10, "none");
      ComponentContext context = new DefaultComponentContext(expected);
      IComponentServant target = new IComponentServant(context);
      ComponentId actual = target.getComponentId();
      Assert.AreEqual(expected.name, actual.name);
    }
  }
}
