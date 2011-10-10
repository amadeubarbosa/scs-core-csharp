using System;
using scs.core;
using Scs.Core;

namespace Test.Mock
{
  /// <summary>
  /// Mock do IComponent.
  /// </summary>
  public class MockIComponentServant : MarshalByRefObject, IComponent
  {
    private ComponentContext context;
    public MockIComponentServant(ComponentContext context) {
      this.context = context;
    }

    #region IComponent Members

    public ComponentId getComponentId() {
      throw new NotImplementedException();
    }

    public MarshalByRefObject getFacet(string facet_interface) {
      return null;
    }

    public MarshalByRefObject getFacetByName(string facet) {
      throw new NotImplementedException();
    }

    public void shutdown() {
      throw new NotImplementedException();
    }

    public void startup() {
      throw new NotImplementedException();
    }

    #endregion
  }
}
