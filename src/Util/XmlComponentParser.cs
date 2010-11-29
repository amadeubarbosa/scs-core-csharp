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
    private const string COMPONENT_PLATFORM_SPEC = "platform_spec";
    private const string FACETS_ELEMENT = "facets";
    private const string FACET_ELEMENT = "facet";
    private const string FACET_NAME = "name";
    private const string FACET_REP_ID = "repId";
    private const string FACET_TYPE = "type";
    private const string FACET_TYPE_NAME = "fullName";
    private const string FACET_TYPE_ASSEMBLY = "assembly";
    private const string RECEPTACLES_ELEMENT = "receptacles";
    private const string RECEPTACLE_ELEMENT = "receptacle";
    private const string RECEPTACLE_NAME = "name";
    private const string RECEPTACLE_REP_ID = "repId";
    private const string RECEPTACLE_MULTIPLE = "isMultiple";

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

    public ComponentId getComponentId() {
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

    public List<FacetInformation> getFacets() {
      XmlNodeList facetsNodeList =
          xmlComponent.GetElementsByTagName(FACET_ELEMENT);

      List<FacetInformation> facets = new List<FacetInformation>();
      foreach (XmlNode facetNode in facetsNodeList) {
        String name = facetNode[FACET_NAME].InnerText;
        String repId = facetNode[FACET_REP_ID].InnerText;
        XmlElement typeElement = facetNode[FACET_TYPE];
        String nameType= typeElement[FACET_TYPE_NAME].InnerText;
        String assemblyType = typeElement[FACET_TYPE_ASSEMBLY].InnerText;
        String type = nameType + ", " + assemblyType;

        facets.Add(new FacetInformation(name, repId, Type.GetType(type)));
      }

      return facets;
    }

    public List<ReceptacleInformation> getReceptacles() {
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

    #endregion


    #region Private Members

    #endregion



  }
}

