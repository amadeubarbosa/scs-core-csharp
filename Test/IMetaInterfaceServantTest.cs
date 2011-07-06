using System;
using System.Collections.Generic;
using Ch.Elca.Iiop.Idl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using scs.core;
using Scs.Core;
using Scs.Core.Servant;

namespace Test
{
  [TestClass()]
  public class IMetaInterfaceServantTest
  {
    private TestContext testContextInstance;
    private ComponentContext context;
    private MarshalByRefObject servant;
    private static List<string> receptacleNames;
    private static string interfaceName;
    private static ComponentId componentId;

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
      componentId = new ComponentId("Test", 1, 0, 0, "none");

      interfaceName = Repository.GetRepositoryID(typeof(IComponent));
      receptacleNames = new List<string>() { "Receptacle1", "Receptacle2", "Receptacle3" };

    }

    [TestInitialize()]
    public void BeforeTest() {
      context = new DefaultComponentContext(componentId);
      servant = new IComponentServant(context);
    }

    [TestMethod()]
    public void IMetaInterfaceServantConstructorTest1() {
      IMetaInterfaceServant target = new IMetaInterfaceServant(context);
      Assert.IsNotNull(target);
    }

    [TestMethod()]
    [ExpectedException(typeof(ArgumentNullException))]
    public void IMetaInterfaceServantConstructorTest2() {
      IMetaInterfaceServant target = new IMetaInterfaceServant(null);
    }

    [TestMethod()]
    public void getReceptaclesByNameTest() {
      IMetaInterfaceServant target = new IMetaInterfaceServant(context);
      foreach (var name in receptacleNames) {
        context.AddReceptacle(name, interfaceName, false);
      }

      string[] names = receptacleNames.ToArray();
      ReceptacleDescription[] actual = target.getReceptaclesByName(names);
      Assert.AreEqual(receptacleNames.Count, actual.Length);
      foreach (var receptacle in actual) {
        Receptacle expected = context.GetReceptacleByName(receptacle.name);
        Assert.IsNotNull(expected);
      }
    }

    [TestMethod()]
    public void getReceptaclesByNameTest2() {
      IMetaInterfaceServant target = new IMetaInterfaceServant(context);
      foreach (var name in receptacleNames) {
        context.AddReceptacle(name, interfaceName, false);
      }

      string[] names = receptacleNames.ToArray();
      // Alterando o último nome para um nome inválido.
      names[names.Length - 1] = "InvalidName";
      ReceptacleDescription[] actual = target.getReceptaclesByName(names);
      Assert.AreEqual(names.Length - 1, actual.Length);
      foreach (var receptacle in actual) {
        Receptacle expected = context.GetReceptacleByName(receptacle.name);
        Assert.IsNotNull(expected);
      }
    }

    [TestMethod()]
    public void getReceptaclesTest() {
      IMetaInterfaceServant target = new IMetaInterfaceServant(context);
      foreach (var name in receptacleNames) {
        context.AddReceptacle(name, interfaceName, false);
      }

      IDictionary<String, Receptacle> expected = context.GetReceptacles();
      ReceptacleDescription[] actual = target.getReceptacles();
      Assert.AreEqual(expected.Count, actual.Length);
      for (int i = 0; i < actual.Length; i++) {
        Assert.IsTrue(expected.ContainsKey(actual[i].name));
      }
    }

    [TestMethod()]
    public void getReceptaclesTest2() {
      IMetaInterfaceServant target = new IMetaInterfaceServant(context);

      ReceptacleDescription[] actual = target.getReceptacles();
      Assert.AreEqual(0, actual.Length);
    }

    [TestMethod()]
    public void getFacetsByNameTest() {
      IMetaInterfaceServant target = new IMetaInterfaceServant(context);
      string[] names = new string[3];
      names[0] = typeof(IComponent).Name;
      names[1] = typeof(IReceptacles).Name;
      names[2] = typeof(IMetaInterface).Name;
      FacetDescription[] actual = target.getFacetsByName(names);
      Assert.AreEqual(names.Length, actual.Length);
      foreach (var facet in actual) {
        Facet expected = context.GetFacetByName(facet.name);
        Assert.IsNotNull(expected);
      }
    }

    [TestMethod()]
    public void getFacetsByNameTest2() {
      IMetaInterfaceServant target = new IMetaInterfaceServant(context);
      string[] names = new string[3];
      names[0] = typeof(IComponent).Name;
      names[1] = "InvalidName";
      names[2] = typeof(IMetaInterface).Name;
      FacetDescription[] actual = target.getFacetsByName(names);
      Assert.AreEqual(names.Length - 1, actual.Length);
      foreach (var facet in actual) {
        Facet expected = context.GetFacetByName(facet.name);
        Assert.IsNotNull(expected);
      }
    }

    [TestMethod()]
    public void getFacetsTest() {
      IMetaInterfaceServant target = new IMetaInterfaceServant(context);
      IDictionary<String, Facet> expected = context.GetFacets();
      FacetDescription[] actual = target.getFacets();
      Assert.AreEqual(expected.Count, actual.Length);
      for (int i = 0; i < actual.Length; i++) {
        Assert.IsTrue(expected.ContainsKey(actual[i].name));
      }
    }
  }
}
