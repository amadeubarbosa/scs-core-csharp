using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using log4net;
using scs.core;
using Scs.Core.Exception;
using Scs.Core.Properties;
using Scs.Core.Util;
using System.Text;

namespace Scs.Core.Builder
{
  /// <summary>
  /// Responsável por criar componente baseado em um arquivo de descrição 
  /// construido em XML.
  /// É necessário seguir o schema 'ComponentDescription.xsd'.
  /// </summary>
  public class XMLComponentBuilder
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
    /// O log.
    /// </summary>
    private static ILog logger = LogManager.GetLogger(typeof(XMLComponentBuilder));

    /// <summary>
    /// O Xml do componente.
    /// </summary>
    private XmlDocument xmlComponent;

    #endregion

    #region Contructors

    /// <summary>
    /// Construtor.
    /// </summary>
    /// <param name="componentInformation">
    /// Arquivo de descrição do componente.
    /// </param>
    /// <exception cref="XmlSchemaValidationException">Caso ocorra um erro na 
    /// validação do Xml.</exception>
    /// <exception cref="ArgumentNullException">Caso o argumento seja nulo.
    /// </exception>
    public XMLComponentBuilder(XmlTextReader componentInformation) {
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

    /// <summary>
    /// Constroi o componente. O componente será composto das facetas báscias
    /// e todas as facetas e receptáculos representados no arquivo de descrição.
    /// </summary>
    /// <returns>O componente.</returns>
    /// <exception cref="SCSException">Caso ocorra um erro na construção do 
    /// componente.</exception>
    public ComponentContext build() {
      ComponentId componentId = GetComponentId();

      ComponentContext component = CreateComponentContext(componentId);
      AddFacets(component);
      AddReceptacles(component);

      DumpComponent(component);
      return component;
    }

    #endregion

    #region Private Members

    /// <summary>
    /// Obtém o id do componente.
    /// </summary>
    /// <returns>Id do componente.</returns>
    private ComponentId GetComponentId() {
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

    /// <summary>
    /// Obtém um 'ComponentContext' do arquivo de descrição. Caso não exista
    /// será criado um 'DefaultComponetContext'.
    /// </summary>
    /// <param name="componentId">O id do componente.</param>
    /// <returns></returns>
    /// <exception cref="SCSException">Caso ocorra um erro na construção do 
    /// componentContext</exception>
    private ComponentContext CreateComponentContext(ComponentId componentId) {
      XmlNode componentContextNode =
          xmlComponent.GetElementsByTagName(COMPONENT_CONTEXT_ELEMENT)[0];
      if (componentContextNode == null) {
        logger.Debug("Criando o contexto utilizando a classe 'DefaultComponentContext'");
        return new DefaultComponentContext(componentId);
      }

      XmlNode context = componentContextNode[COMPONENT_CONTEXT_TYPE];
      String contextName = context.InnerText;
      String contextAssembly = context.Attributes[COMPONENT_CONTEXT_ASSEMBLY_ATTRIBUTE].InnerText;
      String contextTypeName = String.Format("{0}, {1}", contextName, contextAssembly);
      Type contextType = Type.GetType(contextTypeName);
      if (contextType == null) {
        throw new SCSException(String.Format(
          "Não foi possível encontrar a classe '{0}'", contextTypeName));
      }

      System.Reflection.ConstructorInfo constructor =
          contextType.GetConstructor(new Type[] { typeof(ComponentId) });
      if (constructor == null) {
        string errorMsg = "Implementação do componentContext deve possuir um construtor com um parâmetro do tipo 'ComponentId'";
        throw new SCSException(errorMsg);
      }
      ComponentContext component =
          constructor.Invoke(new Object[] { componentId }) as ComponentContext;
      if (component == null) {
        string errorMsg = String.Format(
            "A classe {0} não é do tipo 'ComponentContext'", contextTypeName);
        throw new SCSException(errorMsg);
      }

      logger.DebugFormat("Contexto criado utilizando a classe '{0}'", contextTypeName);
      return component;
    }

    /// <summary>
    /// Adiciona as facetas do arquivo de descrição ao componente. 
    /// </summary>
    /// <param name="context">O componente</param>
    /// <exception cref="SCSException">Caso ocorra um erro na criação das facetas.</exception>
    private void AddFacets(ComponentContext context) {
      XmlNodeList facetsNodeList =
          xmlComponent.GetElementsByTagName(FACET_ELEMENT);
      foreach (XmlNode facetNode in facetsNodeList) {
        String name = facetNode[FACET_NAME].InnerText;
        String interfaceName = facetNode[FACET_REP_ID].InnerText;
        XmlNode servantNode = facetNode[FACET_SERVANT];
        String servantName = servantNode.InnerText;
        String servantAssembly = servantNode.Attributes[FACET_SERVANT_ASSEMBLY_ATTRIBUTE].InnerText;
        String type = String.Format("{0}, {1}", servantName, servantAssembly);

        MarshalByRefObject servant = InstantiateServant(type, context);
        if (!IiopNetUtil.CheckInterface(servant, interfaceName)) {
          string errorMsg = String.Format(
            "A faceta '{0}' não suporta a interface '{1}'", name, interfaceName);
          throw new SCSException(errorMsg);
        }

        context.AddFacet(name, interfaceName, servant);
      }
    }

    /// <summary>
    /// Adiciona os receptáculos do arquivo de descrição ao componente. 
    /// </summary>
    /// <param name="context">O componente.</param>
    private void AddReceptacles(ComponentContext context) {
      XmlNodeList receptaclesNodeList =
          xmlComponent.GetElementsByTagName(RECEPTACLE_ELEMENT);

      foreach (XmlNode receptacleNode in receptaclesNodeList) {
        String name = receptacleNode[RECEPTACLE_NAME].InnerText;
        String interfaceName = receptacleNode[RECEPTACLE_REP_ID].InnerText;
        Boolean isMultiple =
            XmlConvert.ToBoolean(receptacleNode[RECEPTACLE_MULTIPLE].InnerText);

        context.AddReceptacle(name, interfaceName, isMultiple);
      }
    }

    /// <summary>
    /// Instancia um servantNode de acordo com o tipo.
    /// </summary>    
    /// <param name="type">Tipo do servantNode</param>
    /// <param name="context">
    /// componentContext necessário na contrução de um servantNode
    /// </param>
    /// <returns>Servant</returns>
    /// <exception cref="SCSException">Caso ocorra um erro na criação do servant.</exception>
    private MarshalByRefObject InstantiateServant(String type, ComponentContext context) {
      Type facetType;
      try {
        facetType = Type.GetType(type);
      }
      catch (System.Exception e) {
        string errorMsg = String.Format("Não foi possível criar uma faceta do tipo {0}. Tipo não encontrado",
            type);
        throw new SCSException(errorMsg, e);
      }
      if (facetType == null) {
        string errorMsg = String.Format("Não foi possível criar uma faceta do tipo {0}. Tipo não encontrado",
            type);
        throw new SCSException(errorMsg);
      }

      ConstructorInfo constructor = facetType.GetConstructor(
        new Type[] { typeof(ComponentContext) });
      if (constructor == null) {
        string errorMsg = "Implementação da faceta deve possuir um" +
          " contrutor com um parametro do tipo 'ComponentContext'";
        throw new SCSException(errorMsg);
      }

      MarshalByRefObject servant = constructor.Invoke(
          new object[] { context }) as MarshalByRefObject;
      if (servant == null) {
        throw new SCSException(
          "Faceta não pode ser instanciada como um objeto remoto.\n" +
          "Certifique-se que seu servantNode estenda de MarshalByRefObject");
      }
      return servant;
    }

    /// <summary>
    /// Faz um dump do component
    /// </summary>    
    /// <param name="component"></param>
    private void DumpComponent(ComponentContext component) {
      ComponentId componentId = component.GetComponentId();
      StringBuilder builder = new StringBuilder();
      builder.AppendFormat("Componente {0}:{1}.{2}.{3} criado com sucesso.\n", componentId.name,
          componentId.major_version, componentId.minor_version, componentId.patch_version);

      IDictionary<String, Facet> facets = component.GetFacets();
      IDictionary<String, Receptacle> receptacles = component.GetReceptacles();

      if (facets.Count > 0) {
        builder.AppendLine("Facetas:");
      }
      foreach (Facet facet in facets.Values) {
        builder.AppendFormat("  {0} : {1}\n", facet.Name, facet.InterfaceName);
      }

      if (receptacles.Count > 0)
        builder.AppendLine("Receptáculos:");
      foreach (Receptacle receptacle in receptacles.Values) {
        builder.AppendFormat("  {0} : {1}\n", receptacle.Name, receptacle.InterfaceName);
      }
      logger.Info(builder.Remove(builder.Length - 1, 1));
    }

    #endregion
  }
}

