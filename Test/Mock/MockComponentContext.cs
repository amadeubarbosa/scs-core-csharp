using System;
using System.Collections.Generic;
using scs.core;
using Scs.Core;

namespace Test.Mock
{
  /// <summary>
  /// Mock do ComponentContext.
  /// </summary>
  class MockComponentContext : ComponentContext
  {
    public MockComponentContext(ComponentId id) { }

    #region ComponentContext Members

    public scs.core.ComponentId GetComponentId() {
      return new ComponentId();
    }

    public scs.core.IComponent GetIComponent() {
      throw new NotImplementedException();
    }

    public IDictionary<string, Facet> GetFacets() {
      return new Dictionary<string, Facet>();
    }

    public Facet GetFacetByName(string name) {
      throw new NotImplementedException();
    }

    public IDictionary<string, Receptacle> GetReceptacles() {
      return new Dictionary<string, Receptacle>();
    }

    public Receptacle GetReceptacleByName(string name) {
      throw new NotImplementedException();
    }

    public void AddFacet(string name, string interfaceName, MarshalByRefObject servant) {
      throw new NotImplementedException();
    }

    public void RemoveFacet(string name) {
      throw new NotImplementedException();
    }

    public void AddReceptacle(string name, string interfaceName, bool isMultiple) {
      throw new NotImplementedException();
    }

    public void RemoveReceptacle(string name) {
      throw new NotImplementedException();
    }

    public void ActivateComponent() {
      throw new NotImplementedException();
    }

    public void DeactivateComponent() {
      throw new NotImplementedException();
    }

    public void UpdateFacet(string name, MarshalByRefObject servant) {
      throw new NotImplementedException();
    }

    #endregion
  }
}
