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

      String name = "IComponent_1";
      String interfaceName = "IDL:scs/core/IComponent:1.0";
      MarshalByRefObject servant = new IComponentServant(expected);
      expected.AddFacet(name, interfaceName, servant);
      name = "IComponent_2";
      interfaceName = "IDL:scs/core/IComponent:1.0";
      servant = new IComponentServant(expected);
      expected.AddFacet(name, interfaceName, servant);
      name = "IMetaInterface_3";
      interfaceName = "IDL:scs/core/IMetaInterface:1.0";
      servant = new IMetaInterfaceServant(expected);
      expected.AddFacet(name, interfaceName, servant);

      String componentModel = Resources.Component1;
      TextReader file = new StringReader(componentModel);
      XmlTextReader componentInformation = new XmlTextReader(file);
      XmlComponentBuilder target = new XmlComponentBuilder(componentInformation);
      ComponentContext actual = target.build();

      Assert.IsTrue(expected.Equals(actual));
    }

    [TestMethod()]
    public void buildTest2() {
      ComponentId componetId = new ComponentId("Test1", 1, 0, 0, ".Net FrameWork 3.5");
      ComponentContext expected = new DefaultComponentContext(componetId);

      String name = "Receptacle1";
      String interfaceName = "IDL:scs/core/IMetaInterface:1.0";
      expected.AddReceptacle(name, interfaceName, false);
      name = "Receptacle2";
      interfaceName = "IDL:scs/core/IMetaInterface:1.0";
      expected.AddReceptacle(name, interfaceName, true);
      name = "Receptacle3";
      interfaceName = "IDL:scs/core/IComponent:1.0";
      expected.AddReceptacle(name, interfaceName, false);

      String componentModel = Resources.Component5;
      TextReader file = new StringReader(componentModel);
      XmlTextReader componentInformation = new XmlTextReader(file);
      XmlComponentBuilder target = new XmlComponentBuilder(componentInformation);
      ComponentContext actual = target.build();

      Assert.IsTrue(expected.Equals(actual));
    }

    [TestMethod()]
    public void buildTest3() {
      ComponentId componetId = new ComponentId("Test1", 1, 0, 0, ".Net FrameWork 3.5");
      ComponentContext expected = new DefaultComponentContext(componetId);

      String componentModel = Resources.Component6;
      TextReader file = new StringReader(componentModel);
      XmlTextReader componentInformation = new XmlTextReader(file);
      XmlComponentBuilder builder = new XmlComponentBuilder(componentInformation);
      ComponentContext actual = builder.build();

      Assert.IsTrue(expected.Equals(actual));
    }

    [TestMethod()]
    public void buildTest4() {
      ComponentId componetId = new ComponentId("Test1", 1, 0, 0, ".Net FrameWork 3.5");
      ComponentContext expected = new DefaultComponentContext(componetId);

      String componentModel = Resources.Component7;
      TextReader file = new StringReader(componentModel);
      XmlTextReader componentInformation = new XmlTextReader(file);
      XmlComponentBuilder builder = new XmlComponentBuilder(componentInformation);
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
      XmlComponentBuilder builder = new XmlComponentBuilder(componentInformation);
      ComponentContext target = builder.build();
    }

    [TestMethod]
    [ExpectedException(typeof(SCSException))]
    public void buildTest_InvalidAssembly() {
      String componentModel = Resources.Component2;
      TextReader file = new StringReader(componentModel);
      XmlTextReader componentInformation = new XmlTextReader(file);
      XmlComponentBuilder target = new XmlComponentBuilder(componentInformation);

      ComponentContext actual = target.build();
    }

    [TestMethod]
    [ExpectedException(typeof(SCSException))]
    public void buildTest_FacetInvalidClassName() {
      String componentModel = Resources.Component3;
      TextReader file = new StringReader(componentModel);
      XmlTextReader componentInformation = new XmlTextReader(file);
      XmlComponentBuilder target = new XmlComponentBuilder(componentInformation);

      ComponentContext actual = target.build();
    }

    [TestMethod]
    [ExpectedException(typeof(SCSException))]
    public void buildTest_ContextInvalidClassName() {
      String componentModel = Resources.Component10;
      TextReader file = new StringReader(componentModel);
      XmlTextReader componentInformation = new XmlTextReader(file);
      XmlComponentBuilder target = new XmlComponentBuilder(componentInformation);

      ComponentContext actual = target.build();
    }

    [TestMethod]
    [ExpectedException(typeof(SCSException))]
    public void buildTest_FacetNoAssembly() {
      String componentModel = Resources.Component4;
      TextReader file = new StringReader(componentModel);
      XmlTextReader componentInformation = new XmlTextReader(file);
      XmlComponentBuilder target = new XmlComponentBuilder(componentInformation);

      ComponentContext actual = target.build();
    }

    [TestMethod]
    [ExpectedException(typeof(SCSException))]
    public void buildTest_ContextNoAssembly() {
      String componentModel = Resources.Component9;
      TextReader file = new StringReader(componentModel);
      XmlTextReader componentInformation = new XmlTextReader(file);
      XmlComponentBuilder target = new XmlComponentBuilder(componentInformation);

      ComponentContext actual = target.build();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void buildTestNull1() {
      XmlComponentBuilder builder = new XmlComponentBuilder(null);
    }
  }
}
