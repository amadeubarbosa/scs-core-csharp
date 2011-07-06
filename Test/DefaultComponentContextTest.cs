using System;
using System.Collections.Generic;
using Ch.Elca.Iiop.Idl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using scs.core;
using Scs.Core;
using Scs.Core.Servant;
using Scs.Core.Exception;

namespace Test
{
  /// <summary>
  /// Teste da classe DefaultComponentContext.
  ///</summary>
  [TestClass()]
  public class DefaultComponentContextTest
  {
    private TestContext testContextInstance;
    private static ComponentId componentId;
    private ComponentContext context;
    private List<FacetInformation> facetList;
    private List<ReceptacleInfomation> receptacleList;

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

    }

    [TestInitialize()]
    public void BeforeTest() {
      context = new DefaultComponentContext(componentId);

      Type icomponentType = typeof(IComponent);
      Type ireceptacleType = typeof(IReceptacles);
      Type imetaInterfaceType = typeof(IMetaInterface);
      String icomponentInterfaceName = Repository.GetRepositoryID(icomponentType);
      string ireceptacleInterfaceName = Repository.GetRepositoryID(ireceptacleType);
      string imetaInterfaceInterfaceName = Repository.GetRepositoryID(imetaInterfaceType);
      facetList = new List<FacetInformation>();
      facetList.Add(new FacetInformation(
          "Faceta3", icomponentInterfaceName, new IComponentServant(context)));
      facetList.Add(new FacetInformation(
          "Faceta1", ireceptacleInterfaceName, new IReceptaclesServant(context)));
      facetList.Add(new FacetInformation(
          "Faceta2", imetaInterfaceInterfaceName, new IMetaInterfaceServant(context)));

      receptacleList = new List<ReceptacleInfomation>();
      receptacleList.Add(new ReceptacleInfomation(
          "IComponentReceptacle", icomponentInterfaceName, false));
      receptacleList.Add(new ReceptacleInfomation(
          "IComponentReceptacleMutex", icomponentInterfaceName, true));
      receptacleList.Add(new ReceptacleInfomation(
          "IReceptacleReceptacle", ireceptacleInterfaceName, false));
      receptacleList.Add(new ReceptacleInfomation(
          "IMetaInterfaceReceptacle", imetaInterfaceInterfaceName, false));
    }

    /// <summary>
    ///A test for DefaultComponentContext Constructor
    ///</summary>
    [TestMethod()]
    public void DefaultComponentContextConstructorTest() {
      ComponentId componentId = new ComponentId("Component1", 1, 0, 0, "none");
      DefaultComponentContext expected = new DefaultComponentContext(componentId);
      Assert.IsNotNull(expected);
    }

    /// <summary>
    /// Verifica se foram adicionadas as três facetas báscias.
    /// Verifica se as facetas estão com os nomes e as intefaces corretas.
    /// </summary>
    [TestMethod]
    public void AddBasicFacetsTest() {
      IDictionary<String, Facet> target = context.GetFacets();
      Assert.IsNotNull(target);
      Assert.AreEqual(3, target.Count);

      Type icomponentType = typeof(IComponent);
      String icomponentFacetName = icomponentType.Name;
      String icomponentInterfaceName = Repository.GetRepositoryID(icomponentType);
      Assert.IsNotNull(target.ContainsKey(icomponentFacetName));
      Facet icomponentFacet = target[icomponentFacetName];
      Assert.AreEqual(icomponentInterfaceName, icomponentFacet.InterfaceName);

      Type ireceptacleType = typeof(IReceptacles);
      string ireceptacleName = ireceptacleType.Name;
      string ireceptacleInterfaceName = Repository.GetRepositoryID(ireceptacleType);
      Assert.IsNotNull(target.ContainsKey(ireceptacleName));
      Facet ireceptacleFacet = target[ireceptacleName];
      Assert.AreEqual(ireceptacleInterfaceName, ireceptacleFacet.InterfaceName);

      Type imetaInterfaceType = typeof(IMetaInterface);
      string imetaInterfaceName = imetaInterfaceType.Name;
      string imetaInterfaceInterfaceName = Repository.GetRepositoryID(imetaInterfaceType);
      Assert.IsNotNull(target.ContainsKey(imetaInterfaceName));
      Facet imetaInterfaceFacet = target[imetaInterfaceName];
      Assert.AreEqual(imetaInterfaceInterfaceName, imetaInterfaceFacet.InterfaceName);
    }

    [TestMethod]
    public void GetFacetByNameTest1() {
      string name = typeof(IComponent).Name;
      string interfaceName = Repository.GetRepositoryID(typeof(IComponent));
      MarshalByRefObject servant = new IComponentServant(context);
      Facet expected = new Facet(name, interfaceName, servant);
      context.UpdateFacet(name, servant);
      Facet actual = context.GetFacetByName(name);
      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetFacetByNameTest2() {
      string name = "InvalidFacet";
      Facet actual = context.GetFacetByName(name);
      Assert.IsNull(actual);
    }

    [TestMethod]
    public void GetFacetByNameTest3() {
      Facet actual = context.GetFacetByName(null);
      Assert.IsNull(actual);
    }

    /// <summary>
    /// A test for AddFacet null Name
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AddFacetTestNull1() {
      string interfaceName = Repository.GetRepositoryID(typeof(IComponent));
      MarshalByRefObject servant = new IComponentServant(context);
      context.AddFacet(null, interfaceName, servant);
    }

    /// <summary>
    /// A test for AddFacet null InterfaceName
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AddFacetTestNull2() {
      MarshalByRefObject servant = new IComponentServant(context);
      context.AddFacet("facetName", null, servant);
    }

    /// <summary>
    /// A test for AddFacet null Servant
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddFacetTestNull3() {
      string interfaceName = Repository.GetRepositoryID(typeof(IComponent));
      context.AddFacet("facetName", interfaceName, null);
    }

    /// <summary>
    /// A test for invalid Name
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AddFacetTestInvalid1() {
      string interfaceName = Repository.GetRepositoryID(typeof(IComponent));
      MarshalByRefObject servant = new IComponentServant(context);
      context.AddFacet(String.Empty, interfaceName, servant);
    }

    /// <summary>
    /// A test for invalid InterfaceName
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AddFacetTestInvalid2() {
      MarshalByRefObject servant = new IComponentServant(context);
      context.AddFacet("facetName", String.Empty, servant);
    }

    /// <summary>
    /// A test for invalid InterfaceName
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AddFacetTestInvalid3() {
      MarshalByRefObject servant = new IComponentServant(context);
      context.AddFacet("facetName", "InvalidName", servant);
    }

    /// <summary>
    /// Verificar se é possível adicionar facetas do mesmo tipo.
    /// </summary>
    [TestMethod()]
    public void AddFacetTest1() {
      string interfaceName = Repository.GetRepositoryID(typeof(IMetaInterface));
      context.AddFacet("SameFacet1", interfaceName, new IMetaInterfaceServant(context));
      context.AddFacet("SameFacet2", interfaceName, new IMetaInterfaceServant(context));
    }

    /// <summary>
    /// A test for AddFacet
    /// </summary>
    [TestMethod()]
    public void AddFacetTest2() {
      foreach (var facet in facetList) {
        context.AddFacet(facet.name, facet.interfaceName, facet.servant);
      }

      IDictionary<String, Facet> target = context.GetFacets();
      //Número de facetas esperadas: Facetas adicionadas mais as três facetas básicas
      int expected = facetList.Count + 3;
      int actual = target.Count;
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Verifica se o método está atualizando a faceta.
    /// </summary>
    [TestMethod()]
    [ExpectedException(typeof(FacetAlreadyExistsException))]
    public void AddFacetTest3() {
      string icomponentName = typeof(IComponent).Name;
      Facet expected = context.GetFacetByName(icomponentName);

      string interfaceName = Repository.GetRepositoryID(typeof(IMetaInterface));
      MarshalByRefObject servant = new IMetaInterfaceServant(context);
      context.AddFacet(icomponentName, interfaceName, servant);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UpdateFacetNull1() {
      MarshalByRefObject servant = new IComponentServant(context);
      context.UpdateFacet(null, servant);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void UpdateFacetNull2() {
      string name = typeof(IComponent).Name;
      context.UpdateFacet(name, null);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UpdateFacetInvalid1() {
      MarshalByRefObject servant = new IComponentServant(context);
      context.UpdateFacet(String.Empty, servant);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UpdateFacetInvalid2() {
      string name = typeof(IComponent).Name;
      MarshalByRefObject servant = new IReceptaclesServant(context);
      context.UpdateFacet(name, servant);
    }

    [TestMethod]
    public void UpdateFacet1() {
      string name = typeof(IMetaInterface).Name;
      MarshalByRefObject servant = new IMetaInterfaceServant(context);
      Facet oldFacet = context.GetFacetByName(name);

      context.UpdateFacet(name, servant);
      Facet actual = context.GetFacetByName(name);

      Assert.AreEqual(servant, actual.Reference);
      Assert.AreEqual(oldFacet.Name, actual.Name);
      Assert.AreNotEqual(oldFacet.Reference, actual.Reference);
    }

    [TestMethod]
    [ExpectedException(typeof(FacetDoesNotExistException))]
    public void UpdateFacet2() {
      MarshalByRefObject servant = new IMetaInterfaceServant(context);
      context.UpdateFacet("invalidFacetName", servant);
    }

    [TestMethod]
    public void GetFacetsTest() {
      IDictionary<string, Facet> actual = context.GetFacets();
      Assert.AreEqual(3, actual.Count);
    }

    [TestMethod]
    public void RemoveFacetTest() {
      FacetInformation facetInfo = facetList[0];
      Facet actual = context.GetFacetByName(facetInfo.name);
      Assert.IsNull(actual);

      context.AddFacet(facetInfo.name, facetInfo.interfaceName, facetInfo.servant);
      actual = context.GetFacetByName(facetInfo.name);
      Assert.IsNotNull(actual);

      context.RemoveFacet(facetInfo.name);
      actual = context.GetFacetByName(facetInfo.name);
      Assert.IsNull(actual);
    }


    /// <summary>
    ///A test for PutReceptacles
    ///</summary>
    [TestMethod()]
    [ExpectedException(typeof(ArgumentException))]
    public void PutReceptaclesTestNull1() {
      string interfaceName = Repository.GetRepositoryID(typeof(IComponent));
      context.PutReceptacle(null, interfaceName, false);
    }

    /// <summary>
    ///A test for PutReceptacles
    ///</summary>
    [TestMethod()]
    [ExpectedException(typeof(ArgumentException))]
    public void PutReceptaclesTestNull2() {
      context.PutReceptacle("ReceptacleName", null, false);
    }

    /// <summary>
    ///A test for PutReceptacles
    ///</summary>
    [TestMethod()]
    [ExpectedException(typeof(ArgumentException))]
    public void PutReceptaclesTestInvalid1() {
      string interfaceName = Repository.GetRepositoryID(typeof(IComponent));
      context.PutReceptacle(String.Empty, interfaceName, true);
    }

    /// <summary>
    ///A test for PutReceptacles
    ///</summary>
    [TestMethod()]
    [ExpectedException(typeof(ArgumentException))]
    public void PutReceptaclesTestInvalid2() {
      context.PutReceptacle("ReceptacleName", String.Empty, false);
    }

    /// <summary>
    ///A test for PutReceptacles
    ///</summary>
    [TestMethod()]
    [ExpectedException(typeof(ArgumentException))]
    public void PutReceptaclesTestInvalid3() {
      context.PutReceptacle("ReceptacleName", "InvalidName", false);
    }

    /// <summary>
    ///A test for PutReceptacles
    ///</summary>
    [TestMethod()]
    public void PutReceptaclesTest1() {
      foreach (var receptacle in receptacleList) {
        context.PutReceptacle(receptacle.name, receptacle.interfaceName, receptacle.isMultiple);
      }
      IDictionary<String, Receptacle> target = context.GetReceptacles();
      Assert.AreEqual(receptacleList.Count, target.Count);
    }

    /// <summary>
    ///A test for PutReceptacles
    ///</summary>
    [TestMethod()]
    public void PutReceptaclesTest2() {
      String name = "ReceptacleName";
      String interfaceName = Repository.GetRepositoryID(typeof(IComponent));
      context.PutReceptacle(name, interfaceName, false);
      Receptacle expected = context.GetReceptacleByName(name);
      Assert.IsNotNull(expected);

      String newInterfaceName = Repository.GetRepositoryID(typeof(IMetaInterface));
      context.PutReceptacle(name, newInterfaceName, true);
      Receptacle target = context.GetReceptacleByName(name);
      Assert.AreNotEqual(expected.InterfaceName, target.InterfaceName);
      Assert.AreNotEqual(expected.IsMultiple, target.IsMultiple);
    }

    /// <summary>
    ///A test for RemoveReceptacles
    ///</summary>
    [TestMethod()]
    public void RemoveReceptaclesTest() {
      ReceptacleInfomation recInfo = receptacleList[0];
      Receptacle actual = context.GetReceptacleByName(recInfo.name);
      Assert.IsNull(actual);

      context.PutReceptacle(recInfo.name, recInfo.interfaceName, recInfo.isMultiple);
      actual = context.GetReceptacleByName(recInfo.name);
      Assert.IsNotNull(actual);

      context.RemoveReceptacle(recInfo.name);
      actual = context.GetReceptacleByName(recInfo.name);
      Assert.IsNull(actual);
    }

    /// <summary>
    ///A test for GetReceptacles
    ///</summary>
    [TestMethod()]
    public void GetReceptaclesTest() {
      IDictionary<string, Receptacle> expected = new Dictionary<string, Receptacle>();
      foreach (var receptacle in receptacleList) {
        expected.Add(receptacle.name, new Receptacle(
            receptacle.name, receptacle.interfaceName, receptacle.isMultiple));
        context.PutReceptacle(
          receptacle.name, receptacle.interfaceName, receptacle.isMultiple);
      }
      IDictionary<string, Receptacle> actual = context.GetReceptacles();
      Assert.AreEqual(expected.Count, actual.Count);
    }

    /// <summary>
    ///A test for GetReceptacleByName
    ///</summary>
    [TestMethod()]
    public void GetReceptacleByNameTest1() {
      string name = typeof(IComponent).Name;
      string interfaceName = Repository.GetRepositoryID(typeof(IComponent));
      Receptacle expected = new Receptacle(name, interfaceName, true);
      context.PutReceptacle(name, interfaceName, true);
      Receptacle actual = context.GetReceptacleByName(name);
      Assert.AreEqual(expected, actual);
    }

    [TestMethod()]
    public void GetReceptacleByNameTest2() {
      string name = "InvalidReceptacle";
      Receptacle actual = context.GetReceptacleByName(name);
      Assert.IsNull(actual);
    }

    [TestMethod()]
    public void GetReceptacleByNameTest3() {
      Receptacle actual = context.GetReceptacleByName(null);
      Assert.IsNull(actual);
    }

    /// <summary>
    ///A test for GetIComponent
    ///</summary>
    [TestMethod()]
    public void GetIComponentTest() {
      string name = typeof(IComponent).Name;
      Facet facet = context.GetFacetByName(name);
      Assert.IsNotNull(facet);
      IComponent expected = facet.Reference as IComponent;
      IComponent actual = context.GetIComponent();
      Assert.AreEqual(expected, actual);
    }

    /// <summary>
    ///A test for GetComponentId
    ///</summary>
    [TestMethod()]
    public void GetComponentIdTest() {
      string name = "componetName";
      byte major = 10;
      byte minor = 20;
      byte patch = 30;
      string spec = "none";
      ComponentId expected = new ComponentId(name, major, minor, patch, spec);
      DefaultComponentContext target = new DefaultComponentContext(expected);
      ComponentId actual = target.GetComponentId();
      Assert.AreEqual(expected, actual);
    }

    #region Private Members

    private struct FacetInformation
    {
      public String name;
      public String interfaceName;
      public MarshalByRefObject servant;

      public FacetInformation(String name, String interfaceName, MarshalByRefObject servant) {
        this.name = name;
        this.interfaceName = interfaceName;
        this.servant = servant;
      }
    }

    private struct ReceptacleInfomation
    {
      public String name;
      public String interfaceName;
      public Boolean isMultiple;

      public ReceptacleInfomation(String name, String interfaceName, Boolean isMultiple) {
        this.name = name;
        this.interfaceName = interfaceName;
        this.isMultiple = isMultiple;
      }
    }

    #endregion
  }
}
