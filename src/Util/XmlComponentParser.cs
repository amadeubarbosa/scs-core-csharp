using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using scs.core;
using Scs.Core.Exception;
using Scs.Core.Properties;

namespace Scs.Core.Util
{
  internal class XmlComponentParser
  {
    #region Constants

    /* Elementos do ComponentModel Schema. */
    private const string COMPONENT_ID_ELEMENT = "id";
    private const string COMPONENT_ID_NAME = "name";
    private const string COMPONENT_VERSION = "version";
    private const string COMPONENT_PLATFORM_SPEC = "platformSpec";
    private const string COMPONENT_CONTEXT_ELEMENT = "context";
    private const string COMPONENT_CONTEXT_TYPE = "type";
    private const string COMPONENT_CONTEXT_ASSEMBLY_ATTRIBUTE = "assembly";
    private const string FACETS_ELEMENT = "facets";
    private const string FACET_ELEMENT = "facet";
    private const string FACET_NAME = "name";
    private const string FACET_REP_ID = "interfaceName";
    private const string FACET_SERVANT = "facetImpl";
    private const string FACET_SERVANT_ASSEMBLY_ATTRIBUTE = "assembly";
    private const string RECEPTACLES_ELEMENT = "receptacles";
    private const string RECEPTACLE_ELEMENT = "receptacle";
    private const string RECEPTACLE_NAME = "name";
    private const string RECEPTACLE_REP_ID = "interfaceName";
    private const string RECEPTACLE_MULTIPLE = "isMultiplex";

    #endregion

    #region Fields

    /// <summary>
    /// O Xml do componente.
    /// </summary>
    private XmlDocument xmlComponent;

    #endregion


    #region Contructors

    /// <summary>
    /// Construtor.
    /// </summary>
    /// <param name="componentInformation"></param>
    public XmlComponentParser(XmlTextReader componentInformation) {
      MemoryStream memorySchema = new MemoryStream(Resources.ComponentModel);
      XmlTextReader xsdReader = new XmlTextReader(memorySchema);

      XmlReaderSettings xmlSettings = new XmlReaderSettings();
      xmlSettings.ValidationType = ValidationType.Schema;
      xmlSettings.Schemas.Add(null, xsdReader);

      xmlComponent = new XmlDocument();
      using (XmlReader reader = XmlReader.Create(componentInformation, xmlSettings)) {
        try {
          xmlComponent.Load(reader);
        }
        catch (XmlSchemaValidationException e) {
          throw new SCSException("Erro na validação do Xml.", e);
        }
      }
    }

    #endregion


    #region Public Members

    public ComponentId GetComponentId() {
      XmlNode componentIdNode =
          xmlComponent.GetElementsByTagName(COMPONENT_ID_ELEMENT)[0];
      String componetName = componentIdNode[COMPONENT_ID_NAME].InnerText;
      String componentSpec = componentIdNode[COMPONENT_PLATFORM_SPEC].InnerText;
      String componentVersion = componentIdNode[COMPONENT_VERSION].InnerText;
      String[] version = componentVersion.Split('.');
      Byte majorVersion = Convert.ToByte(version[0]);
      Byte minorVersion = Convert.ToByte(version[1]);
      Byte patchVersion = Convert.ToByte(version[2]);

      return new ComponentId(componetName, majorVersion, minorVersion,
          patchVersion, componentSpec);
    }

    public List<FacetInformation> GetFacets() {
      XmlNodeList facetsNodeList =
          xmlComponent.GetElementsByTagName(FACET_ELEMENT);

      List<FacetInformation> facets = new List<FacetInformation>();
      foreach (XmlNode facetNode in facetsNodeList) {
        String name = facetNode[FACET_NAME].InnerText;
        String repId = facetNode[FACET_REP_ID].InnerText;
        XmlNode servant = facetNode[FACET_SERVANT];
        String servantName = servant.InnerText;
        String servantAssembly = servant.Attributes[FACET_SERVANT_ASSEMBLY_ATTRIBUTE].InnerText;
        String type = servantName + ", " + servantAssembly;

        facets.Add(new FacetInformation(name, repId, Type.GetType(type)));
      }

      return facets;
    }

    public List<ReceptacleInformation> GetReceptacles() {
      XmlNodeList receptaclesNodeList =
          xmlComponent.GetElementsByTagName(RECEPTACLE_ELEMENT);

      List<ReceptacleInformation> receptacles = new List<ReceptacleInformation>();
      foreach (XmlNode receptacleNode in receptaclesNodeList) {
        String name = receptacleNode[RECEPTACLE_NAME].InnerText;
        String repId = receptacleNode[RECEPTACLE_REP_ID].InnerText;
        Boolean isMultiple =
            XmlConvert.ToBoolean(receptacleNode[RECEPTACLE_MULTIPLE].InnerText);

        receptacles.Add(new ReceptacleInformation(name, repId, isMultiple));
      }

      return receptacles;
    }

    public ComponentContext GetComponentContext(ComponentId componentId) {
      XmlNode componentContextNode =
          xmlComponent.GetElementsByTagName(COMPONENT_CONTEXT_ELEMENT)[0];
      if (componentContextNode == null) {
        return null;
      }
      XmlNode context = componentContextNode[COMPONENT_CONTEXT_TYPE];
      String contextName = context.InnerText;
      String contextAssembly = context.Attributes[COMPONENT_CONTEXT_ASSEMBLY_ATTRIBUTE].InnerText;
      String type = contextName + ", " + contextAssembly;
      Type contextType = Type.GetType(type);
      if (contextType == null) {
        throw new SCSException(String.Format(
          "Não foi possível encontrar a classe '{0}'", type));
      }

      System.Reflection.ConstructorInfo constructor =
          contextType.GetConstructor(new Type[] { typeof(ComponentId) });
      if (constructor == null) {
        string errorMsg = "Implementação do componentContext deve possuir um" +
          "contrutor com um parametro do tipo 'ComponentId'";
        throw new SCSException(errorMsg);
      }
      return constructor.Invoke(new Object[] { componentId }) as ComponentContext;
    }

    #endregion


    #region Private Members

    #endregion



  }
}

