using System;
using scs.core;
using Scs.Core;

namespace Test.Mock
{
  /// <summary>
  /// Mock do IMetaInterface
  /// </summary>
  public class MockIMetaInterfaceServant : MarshalByRefObject, IMetaInterface
  {
    public MockIMetaInterfaceServant(ComponentContext context) { }

    #region IMetaInterface Members

    public FacetDescription[] getFacets() {
      return null;
    }

    public FacetDescription[] getFacetsByName(string[] names) {
      throw new NotImplementedException();
    }

    public ReceptacleDescription[] getReceptacles() {
      throw new NotImplementedException();
    }

    public ReceptacleDescription[] getReceptaclesByName(string[] names) {
      throw new NotImplementedException();
    }

    #endregion
  }
}
