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
  public class ReceptacleTest
  {
    private static string name;
    private static string interfaceName;
    private static bool isMultiple;
    private static List<MarshalByRefObject> connections;

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
      name = "Faceta";
      interfaceName = Repository.GetRepositoryID(typeof(IComponent));
      isMultiple = false;

      ComponentId componentId = new ComponentId("Component1", 1, 0, 0, "none");
      ComponentContext context = new DefaultComponentContext(componentId);
      connections = new List<MarshalByRefObject>();

      for (int i = 0; i < 5; i++) {
        MarshalByRefObject connection = new IComponentServant(context);
        connections.Add(connection);
      }
    }

    [TestMethod()]
    public void NameTest() {
      Receptacle target = new Receptacle(name, interfaceName, isMultiple);
      string actual = target.Name;
      Assert.AreEqual(name, actual);
    }

    [TestMethod()]
    public void IsMultipleTest() {
      Receptacle target = new Receptacle(name, interfaceName, isMultiple);
      bool actual = target.IsMultiple;
      Assert.AreEqual(isMultiple, actual);
    }

    [TestMethod()]
    public void InterfaceNameTest() {
      Receptacle target = new Receptacle(name, interfaceName, isMultiple);
      string actual = target.InterfaceName;
      Assert.AreEqual(interfaceName, actual);
    }

    [TestMethod()]
    public void RemoveConnetionsTest() {
      Receptacle target = new Receptacle(name, interfaceName, isMultiple);
      MarshalByRefObject connection = connections[0];
      int id = target.AddConnections(connection);
      bool actual = target.RemoveConnetions(id);
      Assert.AreEqual(true, actual);
    }

    [TestMethod()]
    public void RemoveConnetionsTest2() {
      Receptacle target = new Receptacle(name, interfaceName, isMultiple);
      int id = -1;
      bool actual = target.RemoveConnetions(id);
      Assert.AreEqual(false, actual);
    }

    [TestMethod()]
    public void RemoveConnetionsTest3() {
      Receptacle target = new Receptacle(name, interfaceName, isMultiple);
      int id = Int32.MaxValue;
      bool actual = target.RemoveConnetions(id);
      Assert.AreEqual(false, actual);
    }

    [TestMethod()]
    public void GetDescriptionTest1() {
      Receptacle target = new Receptacle(name, interfaceName, isMultiple);
      ReceptacleDescription expected = new ReceptacleDescription(name, interfaceName, isMultiple, new ConnectionDescription[0]);
      ReceptacleDescription actual = target.GetDescription();
      Assert.AreEqual(expected.name, actual.name);
      Assert.AreEqual(expected.interface_name, actual.interface_name);
      Assert.AreEqual(expected.is_multiplex, actual.is_multiplex);
    }

    /// <summary>
    /// Verifica se o descritor foi alterado quando existe uma nova conexão.
    /// </summary>
    [TestMethod()]
    public void GetDescriptionTest2() {
      Receptacle target = new Receptacle(name, interfaceName, isMultiple);
      foreach (var connection in connections) {
        target.AddConnections(connection);
      }
      List<ConnectionDescription> receptacleConnectionsList = target.GetConnections();
      ConnectionDescription[] connectionDescription = receptacleConnectionsList.ToArray();
      ReceptacleDescription expected = new ReceptacleDescription(name, interfaceName, isMultiple, connectionDescription);
      ReceptacleDescription actual = target.GetDescription();
      Assert.AreEqual(actual.name, expected.name);
      Assert.AreEqual(actual.interface_name, expected.interface_name);
      Assert.AreEqual(actual.is_multiplex, expected.is_multiplex);
      Assert.AreEqual(actual.connections.Length, expected.connections.Length);
    }

    [TestMethod()]
    public void GetConnectionsSizeTest() {
      Receptacle target = new Receptacle(name, interfaceName, isMultiple);
      int actual;
      int expected;
      for (int i = 0; i < connections.Count; i++) {
        target.AddConnections(connections[i]);
        actual = target.GetConnectionsSize();
        expected = i + 1;
        Assert.AreEqual(expected, actual);
      }

      int id = target.AddConnections(connections[0]);
      expected = connections.Count + 1;
      actual = target.GetConnectionsSize();
      Assert.AreEqual(expected, actual);

      target.RemoveConnetions(id);
      expected = connections.Count;
      actual = target.GetConnectionsSize();
      Assert.AreEqual(expected, actual);
    }

    [TestMethod()]
    public void GetConnectionsTest() {
      Receptacle target = new Receptacle(name, interfaceName, isMultiple);
      List<ConnectionDescription> expected = new List<ConnectionDescription>();
      List<ConnectionDescription> actual = target.GetConnections();
      Assert.AreEqual(expected.Count, actual.Count);
      for (int i = 0; i < connections.Count; i++) {
        target.AddConnections(connections[i]);
      }

      actual = target.GetConnections();
      Assert.AreEqual(connections.Count, actual.Count);
    }

    [TestMethod()]
    public void EqualsTest() {
      Receptacle target = new Receptacle(name, interfaceName, isMultiple);
      Receptacle obj1 = new Receptacle(name, interfaceName, isMultiple);
      Receptacle obj2 = new Receptacle("Equals", interfaceName, isMultiple);
      Receptacle obj3 = new Receptacle(name, "IDL:receptacle/equals/test:1.0", isMultiple);
      Receptacle obj4 = new Receptacle(name, interfaceName, !isMultiple);
      bool actual = target.Equals(obj1);
      Assert.AreEqual(true, actual);
      actual = target.Equals(obj2);
      Assert.AreEqual(false, actual);
      actual = target.Equals(obj3);
      Assert.AreEqual(false, actual);
      actual = target.Equals(obj4);
      Assert.AreEqual(false, actual);

      actual = target.Equals(null);
      Assert.AreEqual(false, actual);
      actual = target.Equals(new Object());
      Assert.AreEqual(false, actual);
    }

    [TestMethod()]
    public void ClearConnectionsTest() {
      Receptacle target = new Receptacle(name, interfaceName, isMultiple);
      List<int> idList = new List<int>(connections.Count);
      foreach (var connection in connections) {
        int id = target.AddConnections(connection);
        idList.Add(id);
      }
      List<ConnectionDescription> actual = target.GetConnections();
      Assert.AreEqual(connections.Count, actual.Count);

      target.ClearConnections();
      actual = target.GetConnections();
      Assert.AreEqual(0, actual.Count);

    }
  }
}
