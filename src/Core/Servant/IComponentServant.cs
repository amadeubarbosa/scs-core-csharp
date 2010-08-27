﻿using System;
using System.Collections;
using System.Collections.Generic;
using scs.core;

namespace Scs.Core.Servant
{
  public class IComponentServant : MarshalByRefObject, IComponent
  {
    #region Field

    /// <summary>
    /// O contexto do componente.
    /// </summary>
    ComponentContext context;

    #endregion

    #region Contructors

    public IComponentServant(ComponentContext context) {
      this.context = context;
    }

    #endregion

    #region IComponent Members

    public ComponentId getComponentId() {
      return context.GetComponentId();
    }

    public MarshalByRefObject getFacet(string facet_interface) {
      IDictionary<String, Facet> facets = this.context.GetFacets();

      foreach (Facet facet in facets.Values) {
        if (facet.RepositoryId == facet_interface)
          return facet.ObjectRef;
      }

      return null;
    }

    public MarshalByRefObject getFacetByName(string facet) {
      IDictionary<String, Facet> facets = this.context.GetFacets();

      return facets[facet].ObjectRef;
    }

    public void shutdown() { }

    public void startup() { }

    #endregion
  }
}
