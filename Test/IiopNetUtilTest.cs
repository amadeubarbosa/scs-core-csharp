using System;
using Ch.Elca.Iiop.Idl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using scs.core;
using Scs.Core;
using Scs.Core.Servant;
using Scs.Core.Util;

namespace Test
{
  /// <summary>
  /// Summary description for ScsTest
  /// </summary>
  [TestClass]
  public class IiopNetUtilTest
  {
    private TestContext testContextInstance;
    private static ComponentId componentId;
    private static ComponentContext componentContext;
    private static IComponent icomponetFacet;
    private static Type icomponentType;

    /// <summary>
    ///Gets or sets the test componentContext which provides
    ///information about and functionality for the current test run.
    ///</summary>
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
      componentId = new ComponentId("Component1", 1, 0, 0, "none");
      componentContext = new DefaultComponentContext(componentId);
      icomponetFacet = new IComponentServant(componentContext);
      icomponentType = typeof(IComponent);
    }

    ///<summary>    
    /// Garente que o método retorna true caso o objeto ofereça a interface.
    ///</summary>
    [TestMethod]
    public void CheckInterface1() {
      MarshalByRefObject icomponentObj = icomponetFacet as MarshalByRefObject;
      string repositoryId = Repository.GetRepositoryID(icomponentType);
      bool expected = true;
      bool actual = IiopNetUtil.CheckInterface(icomponentObj, repositoryId);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Garente que o método retorna falso caso o objeto não ofereça a interface.
    ///</summary>
    [TestMethod]
    public void CheckInterface2() {
      MarshalByRefObject icomponentObj = icomponetFacet as MarshalByRefObject;
      Type imetaInterfaceType = typeof(IMetaInterface);
      string repositoryId = Repository.GetRepositoryID(imetaInterfaceType);
      bool expected = false;
      bool actual = IiopNetUtil.CheckInterface(icomponentObj, repositoryId);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Garante que o método retorna falso caso a interface não exista.
    ///</summary>
    [TestMethod]
    public void CheckInterfaceTest3() {
      MarshalByRefObject icomponentObj = icomponetFacet as MarshalByRefObject;
      string repositoryId = "1.0:openbus/null:IDL";
      bool expected = false;
      bool actual = IiopNetUtil.CheckInterface(icomponentObj, repositoryId);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Garente que o método retorna true caso o objeto ofereça o tipo.
    ///</summary>
    [TestMethod]
    public void CheckInterfaceTest4() {
      MarshalByRefObject icomponentObj = icomponetFacet as MarshalByRefObject;
      Type type = icomponentType;
      bool expected = true;
      bool actual = IiopNetUtil.CheckInterface(icomponentObj, type);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Garente que o método retorna falso caso o objeto não ofereça o tipo.
    ///</summary>
    [TestMethod]
    public void CheckInterfaceTest5() {
      MarshalByRefObject icomponentObj = icomponetFacet as MarshalByRefObject;
      Type type = typeof(IMetaInterface);
      bool expected = false;
      bool actual = IiopNetUtil.CheckInterface(icomponentObj, type);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Garante que o método retorna falso caso o tipo não seja válido.
    ///</summary>
    [TestMethod]
    public void CheckInterfaceTest6() {
      MarshalByRefObject icomponentObj = icomponetFacet as MarshalByRefObject;
      Type type = typeof(Nullable);
      bool expected = false;
      bool actual = IiopNetUtil.CheckInterface(icomponentObj, type);
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Garante que o método retorna o RepositoryID corretamente
    ///</summary>
    [TestMethod]
    public void GetRepositoryIdTest() {
      Type type = typeof(IComponent);
      string expected = "IDL:scs/core/IComponent:1.0";
      string actual = IiopNetUtil.GetRepositoryId(type);
      Assert.AreEqual(expected, actual);
    }
  }
}
