using System;
using System.Collections.Generic;
using scs.core;

namespace Scs.Core.Servant
{
  /// <summary>
  /// Servant da interface <i>IComponent</i>. Implementação padrão do 
  /// <i>IComponent</i>.
  /// </summary>
  /// <see cref="IComponent"/>
  public class IComponentServant : MarshalByRefObject, IComponent
  {
    #region Field

    /// <summary>
    /// O contexto do componente.
    /// </summary>
    ComponentContext context;

    #endregion

    #region Contructors

    /// <summary>
    /// Constutor obrigatório de uma faceta SCS.
    /// </summary>
    /// <param name="context">O contexto do Componente.</param>
    public IComponentServant(ComponentContext context) {
      if (context == null)
        throw new ArgumentNullException("context", "context is null.");
      this.context = context;
    }

    #endregion

    #region IComponent Members

    /// <summary>
    /// Obtém o identificador do componente.
    /// </summary>
    /// <returns>O identificador do componente.</returns>
    public ComponentId getComponentId() {
      return context.GetComponentId();
    }

    /// <summary>
    /// Obtém a faceta do componente através do nome da interface.
    /// </summary>
    /// <param name="facet_interface">
    /// Nome da interface utilizada pela faceta que se deseja obter.
    /// </param>
    /// <returns>Referência para a faceta do componente.</returns>
    public MarshalByRefObject getFacet(string facet_interface) {
      IDictionary<String, Facet> facets = context.GetFacets();
      foreach (Facet facet in facets.Values) {
        if (facet.InterfaceName == facet_interface)
          return facet.Reference;
      }
      return null;
    }

    /// <summary>
    /// Obtém a faceta do componente através do nome da faceta.
    /// </summary>
    /// <param name="facetName">Nome da faceta que se deseja obter.</param>
    /// <returns>Referência para a faceta do componente.</returns>
    public MarshalByRefObject getFacetByName(string facetName) {
      Facet facet = context.GetFacetByName(facetName);
      if (facet == null) {
        return null;
      }
      return facet.Reference;
    }

    /// <summary>
    /// Ativa o componente.
    /// </summary>
    public void shutdown() { }

    /// <summary>
    /// Desativa o componente.
    /// </summary>
    public void startup() { }

    #endregion

    public override object InitializeLifetimeService() {
      return null;
    }
  }
}
