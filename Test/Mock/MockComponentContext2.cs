using System;
using System.Collections.Generic;
using Scs.Core;

namespace Test.Mock
{
  /// <summary>
  /// Mock do ComponentContext que não possui um contrutor que receba um 
  /// ComponetId.
  /// </summary>
  class MockComponentContext2 : ComponentContext
  {
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

    public void AddFacet(string name, string interfaceName, MarshalByRefObject servant) {
      throw new NotImplementedException();
    }

    public void RemoveFacet(string name) {
      throw new NotImplementedException();
    }

    public void PutReceptacle(string name, string interfaceName, bool isMultiple) {
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
