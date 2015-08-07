using System;
using System.Collections.Generic;
using log4net;
using scs.core;
using Scs.Core.Exception;
using Scs.Core.Servant;
using Scs.Core.Util;

namespace Scs.Core
{
  /// <summary>
  /// Implementação padrão da interface <i>ComponentContext</i>
  /// </summary>
  /// <see cref="ComponentContext"/>
  public class DefaultComponentContext : ComponentContext
  {

    #region Fields

    /// <summary>
    /// O log
    /// </summary>
    private static ILog logger = LogManager.GetLogger(typeof(DefaultComponentContext));

    /// <summary>
    /// O identificador do componente.
    /// </summary>
    private ComponentId componentId;

    /// <summary>
    /// Coleção de facetas.
    /// </summary>
    private IDictionary<String, Facet> facets;

    /// <summary>
    /// Coleção dos receptáculos.
    /// </summary>
    private IDictionary<String, Receptacle> receptacles;

    #endregion

    #region Constructors

    /// <summary>
    /// Construtor.
    /// </summary>
    /// <param name="componentId">O contexto do componente.</param>
    public DefaultComponentContext(ComponentId componentId) {
      this.componentId = componentId;
      this.facets = new Dictionary<String, Facet>();
      this.receptacles = new Dictionary<String, Receptacle>();

      AddBasicFacets();
    }

    #endregion

    #region ComponentContext Members

    /// <see cref="GetComponentId" />
    public ComponentId GetComponentId() {
      return this.componentId;
    }

    /// <see cref="AddFacet" />
    /// <exception cref="ArgumentException">Caso os argumentos estejam
    /// incorretos</exception>
    /// <exception cref="ArgumentNullException">Caso os argumentos estejam
    /// nulos</exception>
    public void AddFacet(String name, String interfaceName, MarshalByRefObject servant) {
      if (String.IsNullOrEmpty(name)) {
        throw new ArgumentException("O campo 'name' não pode ser nulo ou vazio.", "name");
      }

      if (facets.ContainsKey(name)) {
        throw new FacetAlreadyExistsException(name);
      }
      Facet facet = new Facet(name, interfaceName, servant);
      facets[name] = facet;
    }

    /// <see cref="UpdateFacet" />
    /// <exception cref="ArgumentException">Caso os argumentos estejam
    /// incorretos</exception>
    /// <exception cref="ArgumentNullException">Caso os argumentos estejam
    /// nulos</exception>
    public void UpdateFacet(String name, MarshalByRefObject servant) {
      if (String.IsNullOrEmpty(name))
        throw new ArgumentException("O campo 'name' não pode ser nulo ou vazio.", "name");
      if (!facets.ContainsKey(name)) {
        throw new FacetDoesNotExistException(name);
      }

      Facet facet = facets[name];
      facet.UpdateReference(servant);
    }

    /// <see cref="RemoveFacet" />
    public void RemoveFacet(String name) {
      if (!facets.ContainsKey(name)) {
        logger.WarnFormat("Não existe a faceta '{0}' no componente.", name);
        return;
      }
      Facet facet = facets[name];
      facets.Remove(name);
      facet.Deactivate();
    }

    /// <see cref="AddReceptacle" />
    /// <exception cref="ArgumentException">Caso os argumentos estejam
    /// incorretos</exception>
    /// <exception cref="ArgumentNullException">Caso os argumentos estejam
    /// nulos</exception>
    public void AddReceptacle(String name, String interfaceName, Boolean isMultiple) {
      if (String.IsNullOrEmpty(name))
        throw new ArgumentException("O campo 'name' não pode ser nulo ou vazio.", "name");

      if (receptacles.ContainsKey(name)) {
        throw new ReceptacleAlreadyExistsException(name);
      }
      Receptacle receptacle = new Receptacle(name, interfaceName, isMultiple);
      receptacles[name] = receptacle;
    }

    /// <see cref="RemoveReceptacle" />
    public void RemoveReceptacle(String name) {
      if (!receptacles.ContainsKey(name)) {
        logger.WarnFormat("Não existe o receptáculo '{0}' no componente.", name);
        return;
      }
      receptacles.Remove(name);
    }

    /// <see cref="GetIComponent" />
    public IComponent GetIComponent() {
      string iComponentName = typeof(IComponent).Name;
      if (!facets.ContainsKey(iComponentName)) {
        logger.Warn("O componente não possui a faceta IComponent.");
        return null;
      }
      return facets[iComponentName].Reference as IComponent;
    }

    /// <see cref="GetFacets" />
    public IDictionary<String, Facet> GetFacets() {
      return new Dictionary<String, Facet>(facets);
    }

    /// <see cref="GetFacetByName" />
    public Facet GetFacetByName(String name) {
      if (name == null) {
        logger.Info("Erro ao fornecer faceta. O parâmetro 'name' está nulo");
        return null;
      }
      if (!facets.ContainsKey(name)) {
        logger.WarnFormat("Não existe a faceta '{0}' no componente.", name);
        return null;
      }
      return facets[name];
    }

    /// <see cref="GetReceptacles" />
    public IDictionary<String, Receptacle> GetReceptacles() {
      return new Dictionary<String, Receptacle>(receptacles);
    }

    /// <see cref="GetReceptacleByName" />    
    public Receptacle GetReceptacleByName(String name) {
      if (name == null) {
        logger.Info("O parâmetro 'name' está nulo");
        return null;
      }
      if (!receptacles.ContainsKey(name)) {
        logger.WarnFormat("Não existe o receptáculo '{0}' no componente.", name);
        return null;
      }
      return receptacles[name];
    }

    #endregion

    #region Private Members

    /// <summary>
    /// Adiciona as facetas bÃ¡sicas Ã  lista de Facetas criadas pelo usuÃ¡rio.
    /// </summary>
    private void AddBasicFacets() {
      Type icomponentType = typeof(IComponent);
      string icomponentName = icomponentType.Name;
      string icomponentInterfaceName = IiopNetUtil.GetRepositoryId(icomponentType);
      if (!facets.ContainsKey(icomponentName)) {
        IComponent icomponent = new IComponentServant(this);
        MarshalByRefObject icomponentServant = icomponent as MarshalByRefObject;
        AddFacet(icomponentName, icomponentInterfaceName, icomponentServant);
        logger.DebugFormat("Faceta '{0}' adicionada com sucesso", icomponentName);
      }

      Type ireceptacleType = typeof(IReceptacles);
      string ireceptacleName = ireceptacleType.Name;
      string ireceptacleInterfaceName = IiopNetUtil.GetRepositoryId(ireceptacleType);
      if (!facets.ContainsKey(ireceptacleName)) {
        IReceptacles receptacle = new IReceptaclesServant(this);
        MarshalByRefObject receptacleServant = receptacle as MarshalByRefObject;
        AddFacet(ireceptacleName, ireceptacleInterfaceName, receptacleServant);
        logger.DebugFormat("Faceta '{0}' adicionada com sucesso", ireceptacleName);
      }

      Type imetaInterfaceType = typeof(IMetaInterface);
      string imetaInterfaceName = imetaInterfaceType.Name;
      string imetaInterfaceInterfaceName = IiopNetUtil.GetRepositoryId(imetaInterfaceType);
      if (!facets.ContainsKey(imetaInterfaceName)) {
        IMetaInterface metaInterface = new IMetaInterfaceServant(this);
        MarshalByRefObject metaInterfaceServant = metaInterface as MarshalByRefObject;
        AddFacet(imetaInterfaceName, imetaInterfaceInterfaceName, metaInterfaceServant);
        logger.DebugFormat("Faceta '{0}' adicionada com sucesso", imetaInterfaceName);
      }
    }

    #endregion

    #region Override Methods

    /// <see cref="Equals" />
    public override bool Equals(object obj) {
      if (obj == null || GetType() != obj.GetType()) {
        return false;
      }

      ComponentContext objContext = (ComponentContext)obj;
      IDictionary<string, Facet> objFacets = objContext.GetFacets();
      if (this.facets.Count != objFacets.Count)
        return false;
      foreach (Facet facet in this.facets.Values) {
        if (!objFacets.ContainsKey(facet.Name))
          return false;
        Facet objFacet = objFacets[facet.Name];
        if (!facet.Equals(objFacet))
          return false;
      }

      IDictionary<string, Receptacle> objReceptacles = objContext.GetReceptacles();
      if (this.receptacles.Count != objReceptacles.Count)
        return false;
      foreach (Receptacle receptacle in this.receptacles.Values) {
        if (!objReceptacles.ContainsKey(receptacle.Name))
          return false;
        Receptacle objReceptacle = objReceptacles[receptacle.Name];
        if (!receptacle.Equals(objReceptacle))
          return false;
      }

      if (this.componentId.name != objContext.GetComponentId().name)
        return false;
      if (this.componentId.major_version != objContext.GetComponentId().major_version)
        return false;
      if (this.componentId.minor_version != objContext.GetComponentId().minor_version)
        return false;
      if (this.componentId.patch_version != objContext.GetComponentId().patch_version)
        return false;
      if (this.componentId.platform_spec != objContext.GetComponentId().platform_spec)
        return false;

      return true;
    }

    /// <see cref="GetHashCode" />
    public override int GetHashCode() {
      return unchecked(String.Format("{0}:{1}.{2}.{3}", componentId.name,
          componentId.major_version, componentId.minor_version,
          componentId.patch_version)).GetHashCode();
    }
    #endregion
  }
}