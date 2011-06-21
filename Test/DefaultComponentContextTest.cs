using System;
using System.Collections.Generic;
using Ch.Elca.Iiop.Idl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using scs.core;
using Scs.Core;
using Scs.Core.Servant;

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
    private static ComponentContext context;
    private static List<FacetInformation> facetList;
    private static List<ReceptacleInfomation> receptacleList;

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

    [TestInitialize()]
    public void BeforeTest() {
      context = new DefaultComponentContext(componentId);
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
      context.PutFacet(name, interfaceName, servant);
      Facet actual = context.GetFacetByName(name);
      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetFacetByNameTest2() {
      string name = "InvalidFacet";
      Facet actual = context.GetFacetByName(name);
      Assert.IsNull(actual);
    }

    /// <summary>
    /// A test for PutFacet null Name
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void PutFacetTestNull1() {
      string interfaceName = Repository.GetRepositoryID(typeof(IComponent));
      MarshalByRefObject servant = new IComponentServant(context);
      context.PutFacet(null, interfaceName, servant);
    }

    /// <summary>
    /// A test for PutFacet null InterfaceName
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void PutFacetTestNull2() {
      MarshalByRefObject servant = new IComponentServant(context);
      context.PutFacet("facetName", null, servant);
    }

    /// <summary>
    /// A test for PutFacet null Servant
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void PutFacetTestNull3() {
      string interfaceName = Repository.GetRepositoryID(typeof(IComponent));
      context.PutFacet("facetName", interfaceName, null);
    }

    /// <summary>
    /// A test for invalid Name
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void PutFacetTestInvalid1() {
      string interfaceName = Repository.GetRepositoryID(typeof(IComponent));
      MarshalByRefObject servant = new IComponentServant(context);
      context.PutFacet(String.Empty, interfaceName, servant);
    }

    /// <summary>
    /// A test for invalid InterfaceName
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void PutFacetTestInvalid2() {
      MarshalByRefObject servant = new IComponentServant(context);
      context.PutFacet("facetName", String.Empty, servant);
    }

    /// <summary>
    /// A test for invalid InterfaceName
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void PutFacetTestInvalid3() {
      MarshalByRefObject servant = new IComponentServant(context);
      context.PutFacet("facetName", "InvalidName", servant);
    }

    /// <summary>
    /// Verificar se é possível adicionar facetas do mesmo tipo.
    /// </summary>
    [TestMethod()]
    public void PutFacetTest1() {
      string interfaceName = Repository.GetRepositoryID(typeof(IMetaInterface));
      context.PutFacet("SameFacet1", interfaceName, new IMetaInterfaceServant(context));
      context.PutFacet("SameFacet2", interfaceName, new IMetaInterfaceServant(context));
    }

    /// <summary>
    /// A test for PutFacet
    /// </summary>
    [TestMethod()]
    public void PutFacetTest2() {
      foreach (var facet in facetList) {
        context.PutFacet(facet.name, facet.interfaceName, facet.servant);
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
    public void PutFacetTest3() {
      string icomponentName = typeof(IComponent).Name;
      Facet expected = context.GetFacetByName(icomponentName);

      string interfaceName = Repository.GetRepositoryID(typeof(IMetaInterface));
      MarshalByRefObject servant = new IMetaInterfaceServant(context);
      context.PutFacet(icomponentName, interfaceName, servant);
      Facet actual = context.GetFacetByName(icomponentName);
      Assert.AreNotEqual(expected.InterfaceName, actual.InterfaceName);
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

      context.PutFacet(facetInfo.name, facetInfo.interfaceName, facetInfo.servant);
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

      context.RemoveReceptacles(recInfo.name);
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
