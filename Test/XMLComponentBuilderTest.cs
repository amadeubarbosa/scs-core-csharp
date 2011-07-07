using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using scs.core;
using Scs.Core;
using Scs.Core.Builder;
using Scs.Core.Exception;
using Scs.Core.Servant;
using Test.Mock;
using Test.Properties;

namespace Test
{
  [TestClass()]
  public class XMLComponentBuilderTest
  {
    private TestContext testContextInstance;

    public TestContext TestContext {
      get {
        return testContextInstance;
      }
      set {
        testContextInstance = value;
      }
    }

    [TestMethod()]
    public void buildTest1() {
      ComponentId componetId = new ComponentId("Test1", 1, 0, 0, ".Net FrameWork 3.5");
      ComponentContext expected = new DefaultComponentContext(componetId);
      List<FacetInformation> facetInfomationList = new List<FacetInformation>();

      MarshalByRefObject servant1 = new IComponentServant(expected);
      FacetInformation facetInfo1 = new FacetInformation("IComponent_1", "IDL:scs/core/IComponent:1.0", servant1);
      facetInfomationList.Add(facetInfo1);
      MarshalByRefObject servant2 = new IComponentServant(expected);
      FacetInformation facetInfo2 = new FacetInformation("IComponent_2", "IDL:scs/core/IComponent:1.0", servant2);
      facetInfomationList.Add(facetInfo2);
      MarshalByRefObject servant3 = new IMetaInterfaceServant(expected);
      FacetInformation facetInfo3 = new FacetInformation("IMetaInterface_3", "IDL:scs/core/IMetaInterface:1.0", servant3);
      facetInfomationList.Add(facetInfo3);

      foreach (var facetInfo in facetInfomationList) {
        expected.AddFacet(facetInfo.name, facetInfo.interfaceName, facetInfo.servant);
      }

      String componentModel = Resources.Component1;
      TextReader file = new StringReader(componentModel);
      XmlTextReader componentInformation = new XmlTextReader(file);
      XMLComponentBuilder target = new XMLComponentBuilder(componentInformation);
      ComponentContext actual = target.build();

      Assert.AreEqual(componetId.name, actual.GetComponentId().name);
      Assert.AreEqual(expected.GetFacets().Count, actual.GetFacets().Count);
      Assert.AreEqual(expected.GetReceptacles().Count, actual.GetReceptacles().Count);

      foreach (var facetInfo in facetInfomationList) {
        Facet facet = actual.GetFacetByName(facetInfo.name);
        Assert.AreEqual(facetInfo.interfaceName, facet.InterfaceName);
      }
    }

    [TestMethod()]
    public void buildTest2() {
      ComponentId componetId = new ComponentId("Test1", 1, 0, 0, ".Net FrameWork 3.5");
      ComponentContext expected = new DefaultComponentContext(componetId);
      List<ReceptacleInfomation> recInformationList = new List<ReceptacleInfomation>();

      ReceptacleInfomation receptacleInfo1 =
            new ReceptacleInfomation("Receptacle1", "IDL:scs/core/IMetaInterface:1.0", false);
      recInformationList.Add(receptacleInfo1);
      ReceptacleInfomation receptacleInfo2 =
            new ReceptacleInfomation("Receptacle2", "IDL:scs/core/IMetaInterface:1.0", true);
      recInformationList.Add(receptacleInfo2);
      ReceptacleInfomation receptacleInfo3 =
            new ReceptacleInfomation("Receptacle3", "IDL:scs/core/IComponent:1.0", false);
      recInformationList.Add(receptacleInfo3);

      foreach (var recInfo in recInformationList) {
        expected.AddReceptacle(recInfo.name, recInfo.interfaceName, recInfo.isMultiple);
      }

      String componentModel = Resources.Component5;
      TextReader file = new StringReader(componentModel);
      XmlTextReader componentInformation = new XmlTextReader(file);
      XMLComponentBuilder target = new XMLComponentBuilder(componentInformation);

      ComponentContext actual = target.build();
      Assert.AreEqual(expected.GetFacets().Count, actual.GetFacets().Count);
      Assert.AreEqual(expected.GetReceptacles().Count, actual.GetReceptacles().Count);

      foreach (var recInfo in recInformationList) {
        Receptacle receptacle = actual.GetReceptacleByName(recInfo.name);
        Assert.AreEqual(recInfo.interfaceName, receptacle.InterfaceName);
        Assert.AreEqual(recInfo.isMultiple, receptacle.IsMultiple);
      }
    }

    [TestMethod()]
    public void buildTest3() {
      ComponentId componetId = new ComponentId("Test1", 1, 0, 0, ".Net FrameWork 3.5");
      ComponentContext expected = new DefaultComponentContext(componetId);
      List<ReceptacleInfomation> recInformationList = new List<ReceptacleInfomation>();

      String componentModel = Resources.Component6;
      TextReader file = new StringReader(componentModel);
      XmlTextReader componentInformation = new XmlTextReader(file);
      XMLComponentBuilder builder = new XMLComponentBuilder(componentInformation);
      ComponentContext target = builder.build();

      int actual = target.GetFacets().Count;
      Assert.AreEqual(3, actual);
      actual = target.GetReceptacles().Count;
      Assert.AreEqual(0, actual);
    }

    [TestMethod()]
    public void buildTest4() {
      ComponentId componetId = new ComponentId("Test1", 1, 0, 0, ".Net FrameWork 3.5");
      ComponentContext expected = new DefaultComponentContext(componetId);

      String componentModel = Resources.Component7;
      TextReader file = new StringReader(componentModel);
      XmlTextReader componentInformation = new XmlTextReader(file);
      XMLComponentBuilder builder = new XMLComponentBuilder(componentInformation);
      ComponentContext target = builder.build();

      Assert.IsInstanceOfType(target, typeof(MockComponentContext));
    }

    [TestMethod()]
    [ExpectedException(typeof(SCSException))]
    public void buildTest5() {
      ComponentId componetId = new ComponentId("Test1", 1, 0, 0, ".Net FrameWork 3.5");
      ComponentContext expected = new DefaultComponentContext(componetId);

      String componentModel = Resources.Component8;
      TextReader file = new StringReader(componentModel);
      XmlTextReader componentInformation = new XmlTextReader(file);
      XMLComponentBuilder builder = new XMLComponentBuilder(componentInformation);
      ComponentContext target = builder.build();
    }

    [TestMethod]
    [ExpectedException(typeof(SCSException))]
    public void buildTest_InvalidAssembly() {
      String componentModel = Resources.Component2;
      TextReader file = new StringReader(componentModel);
      XmlTextReader componentInformation = new XmlTextReader(file);
      XMLComponentBuilder target = new XMLComponentBuilder(componentInformation);

      ComponentContext actual = target.build();
    }

    [TestMethod]
    [ExpectedException(typeof(SCSException))]
    public void buildTest_FacetInvalidClassName() {
      String componentModel = Resources.Component3;
      TextReader file = new StringReader(componentModel);
      XmlTextReader componentInformation = new XmlTextReader(file);
      XMLComponentBuilder target = new XMLComponentBuilder(componentInformation);

      ComponentContext actual = target.build();
    }

    [TestMethod]
    [ExpectedException(typeof(SCSException))]
    public void buildTest_ContextInvalidClassName() {
      String componentModel = Resources.Component10;
      TextReader file = new StringReader(componentModel);
      XmlTextReader componentInformation = new XmlTextReader(file);
      XMLComponentBuilder target = new XMLComponentBuilder(componentInformation);

      ComponentContext actual = target.build();
    }

    [TestMethod]
    [ExpectedException(typeof(SCSException))]
    public void buildTest_FacetNoAssembly() {
      String componentModel = Resources.Component4;
      TextReader file = new StringReader(componentModel);
      XmlTextReader componentInformation = new XmlTextReader(file);
      XMLComponentBuilder target = new XMLComponentBuilder(componentInformation);

      ComponentContext actual = target.build();
    }

    [TestMethod]
    [ExpectedException(typeof(SCSException))]
    public void buildTest_ContextNoAssembly() {
      String componentModel = Resources.Component9;
      TextReader file = new StringReader(componentModel);
      XmlTextReader componentInformation = new XmlTextReader(file);
      XMLComponentBuilder target = new XMLComponentBuilder(componentInformation);

      ComponentContext actual = target.build();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void buildTestNull1() {
      XMLComponentBuilder builder = new XMLComponentBuilder(null);
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
