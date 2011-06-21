using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using scs.core;
using Scs.Core.Servant;
using Scs.Core.Util;
using System.Text.RegularExpressions;

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

    /// <see cref="PutFacet" />
    public void PutFacet(String name, String interfaceName, MarshalByRefObject servant) {
      Facet facet = new Facet(name, interfaceName, servant);
      if (facets.ContainsKey(name)) {
        IiopNetUtil.DeactivateFacet(facets[name].Reference);
      }
      facets[name] = facet;
      ObjRef activateFacet = IiopNetUtil.ActivateFacet(servant);
    }

    /// <see cref="RemoveFacet" />
    public void RemoveFacet(String name) {
      if (!facets.ContainsKey(name)) {
        return;
      }
      MarshalByRefObject facetObj = facets[name].Reference;
      IiopNetUtil.DeactivateFacet(facetObj);
      facets.Remove(name);
    }

    /// <see cref="PutReceptacle" />
    public void PutReceptacle(String name, String interfaceName, Boolean isMultiple) {
      Receptacle receptacle = new Receptacle(name, interfaceName, isMultiple);
      receptacles[name] = receptacle;
    }

    /// <see cref="RemoveReceptacles" />
    public void RemoveReceptacles(String name) {
      if (receptacles.ContainsKey(name)) {
        receptacles.Remove(name);
      }
    }

    /// <see cref="GetIComponent" />
    public IComponent GetIComponent() {
      string iComponentName = typeof(IComponent).Name;
      if (!facets.ContainsKey(iComponentName)) {
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
      if (!facets.ContainsKey(name)) {
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
      if (!receptacles.ContainsKey(name)) {
        return null;
      }
      return receptacles[name];
    }

    /// <see cref="ActivateComponent" />
    public void ActivateComponent() {
      foreach (Facet facet in facets.Values) {
        MarshalByRefObject facetObj = facet.Reference;
        IiopNetUtil.ActivateFacet(facetObj);
      }
    }

    /// <see cref="ActivateComponent" /> 
    public void DeactivateComponent() {
      foreach (Facet facet in facets.Values) {
        MarshalByRefObject facetObj = facet.Reference;
        IiopNetUtil.DeactivateFacet(facetObj);
      }
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
        PutFacet(icomponentName, icomponentInterfaceName, icomponentServant);
      }

      Type ireceptacleType = typeof(IReceptacles);
      string ireceptacleName = ireceptacleType.Name;
      string ireceptacleInterfaceName = IiopNetUtil.GetRepositoryId(ireceptacleType);
      if (!facets.ContainsKey(ireceptacleName)) {
        IReceptacles receptacle = new IReceptaclesServant(this);
        MarshalByRefObject receptacleServant = receptacle as MarshalByRefObject;
        PutFacet(ireceptacleName, ireceptacleInterfaceName, receptacleServant);
      }

      Type imetaInterfaceType = typeof(IMetaInterface);
      string imetaInterfaceName = imetaInterfaceType.Name;
      string imetaInterfaceInterfaceName = IiopNetUtil.GetRepositoryId(imetaInterfaceType);
      if (!facets.ContainsKey(imetaInterfaceName)) {
        IMetaInterface metaInterface = new IMetaInterfaceServant(this);
        MarshalByRefObject metaInterfaceServant = metaInterface as MarshalByRefObject;
        PutFacet(imetaInterfaceName, imetaInterfaceInterfaceName, metaInterfaceServant);
      }
    }

    #endregion
  }
}