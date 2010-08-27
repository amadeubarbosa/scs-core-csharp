using System;
using System.Collections.Generic;
using scs.core;
using Scs.Core.Util;

namespace Scs.Core
{
  /// <summary>
  /// Implementação padrão da interface <i>ComponentContext</i>
  /// </summary>
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

    public DefaultComponentContext(ComponentId componentId) {
      this.componentId = componentId;

      this.facets = new Dictionary<String, Facet>();
      this.receptacles = new Dictionary<String, Receptacle>();
    }

    #endregion

    #region ComponentContext Members

    public ComponentId GetComponentId() {
      return this.componentId;
    }

    public IComponent GetIComponent() {
      ICollection<Facet> facets = this.facets.Values;
      if (facets == null)
        return null;

      string icomponentRepId = IiopNetUtil.GetRepositoryId(typeof(IComponent));
      foreach (Facet facet in facets) {
        if (facet.RepositoryId == icomponentRepId)
          return facet.ObjectRef as IComponent;
      }

      return null;
    }

    public IDictionary<String, Facet> GetFacets() {
      return this.facets;
    }

    public IDictionary<String, Receptacle> GetReceptacles() {
      return this.receptacles;
    }

    #endregion
  }
}