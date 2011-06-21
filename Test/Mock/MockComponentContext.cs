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
      throw new NotImplementedException();
    }

    public scs.core.IComponent GetIComponent() {
      throw new NotImplementedException();
    }

    public IDictionary<string, Facet> GetFacets() {
      throw new NotImplementedException();
    }

    public Facet GetFacetByName(string name) {
      throw new NotImplementedException();
    }

    public IDictionary<string, Receptacle> GetReceptacles() {
      throw new NotImplementedException();
    }

    public Receptacle GetReceptacleByName(string name) {
      throw new NotImplementedException();
    }

    public void PutFacet(string name, string interfaceName, MarshalByRefObject servant) {
      throw new NotImplementedException();
    }

    public void RemoveFacet(string name) {
      throw new NotImplementedException();
    }

    public void PutReceptacle(string name, string interfaceName, bool isMultiple) {
      throw new NotImplementedException();
    }

    public void RemoveReceptacles(string name) {
      throw new NotImplementedException();
    }

    public void ActivateComponent() {
      throw new NotImplementedException();
    }

    public void DeactivateComponent() {
      throw new NotImplementedException();
    }

    #endregion
  }
}
