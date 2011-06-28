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
  public class IReceptaclesServantTest
  {
    private TestContext testContextInstance;
    private ComponentContext context;
    private static List<string> receptacleNames;
    private MarshalByRefObject servant;
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
      foreach (var name in receptacleNames) {
        context.PutReceptacle(name, interfaceName, true);
      }
    }

    [TestMethod()]
    public void IReceptaclesServantConstructorTest() {
      IReceptaclesServant actual = new IReceptaclesServant(context);
      Assert.IsNotNull(actual);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void IComponentServantConstructorTest2() {
      IReceptaclesServant target = new IReceptaclesServant(null);
    }

    [TestMethod()]
    public void connectDisconnectTest() {
      IReceptaclesServant target = new IReceptaclesServant(context);
      int numConections = 5;
      int actual;
      HashSet<int> ids = new HashSet<int>();
      for (int i = 0; i < numConections; i++) {
        string receptacle = receptacleNames[i % receptacleNames.Count];
        actual = target.connect(receptacle, servant);
        Assert.IsTrue(actual > 0);
        bool added = ids.Add(actual);
        Assert.IsTrue(added);
      }
      actual = 0;
      foreach (var receptacle in context.GetReceptacles().Values) {
        actual += receptacle.GetConnections().Count;
      }
      Assert.AreEqual(numConections, actual);

      foreach (var id in ids) {
        target.disconnect(id);
      }
    }

    [TestMethod()]
    [ExpectedException(typeof(AlreadyConnected))]
    public void connectTest1() {
      IReceptaclesServant target = new IReceptaclesServant(context);
      string name = "SimpleReceptacle";
      string interfaceName = Repository.GetRepositoryID(typeof(IMetaInterface));
      MarshalByRefObject servant = new IMetaInterfaceServant(context);
      context.PutReceptacle(name, interfaceName, false);

      target.connect(name, servant);
      target.connect(name, servant);
    }

    [TestMethod()]
    [ExpectedException(typeof(InvalidName))]
    public void connectTest2() {
      IReceptaclesServant target = new IReceptaclesServant(context);
      target.connect("InvalidReceptacle", servant);
    }

    [TestMethod()]
    [ExpectedException(typeof(InvalidConnection))]
    public void connectTest3() {
      IReceptaclesServant target = new IReceptaclesServant(context);
      string name = "SimpleReceptacle";
      string interfaceName = Repository.GetRepositoryID(typeof(IMetaInterface));
      context.PutReceptacle(name, interfaceName, false);

      MarshalByRefObject servant = new IComponentServant(context);
      target.connect(name, servant);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidConnection))]
    public void desconnect1() {
      IReceptaclesServant target = new IReceptaclesServant(context);
      target.disconnect(-1);
    }

    [TestMethod]
    [ExpectedException(typeof(NoConnection))]
    public void desconnect2() {
      IReceptaclesServant target = new IReceptaclesServant(context);
      int invalidId = 100;
      target.disconnect(invalidId);
    }

    [TestMethod()]
    public void getConnectionsTest1() {
      IReceptaclesServant target = new IReceptaclesServant(context);

      string receptacle = receptacleNames[0];
      int numConections = 5;
      ConnectionDescription[] expected = new ConnectionDescription[numConections];

      for (int i = 0; i < numConections; i++) {
        int id = target.connect(receptacle, servant);
        expected[i] = new ConnectionDescription(id, servant);
      }
      ConnectionDescription[] actual = target.getConnections(receptacle);

      for (int i = 0; i < numConections; i++) {
        Assert.AreEqual(expected[i].id, actual[i].id);
        Assert.AreEqual(expected[i].objref, actual[i].objref);
      }
    }

    [TestMethod()]
    public void getConnectionsTest2() {
      IReceptaclesServant target = new IReceptaclesServant(context);
      string receptacle = receptacleNames[0];
      ConnectionDescription[] connections = target.getConnections(receptacle);
      int actual = connections.Length;
      Assert.AreEqual(0, actual);
    }

    [TestMethod()]
    [ExpectedException(typeof(InvalidName))]
    public void getConnectionsTest3() {
      IReceptaclesServant target = new IReceptaclesServant(context);
      target.getConnections("invalidName");
    }
  }
}
