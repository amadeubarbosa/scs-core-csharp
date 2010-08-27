using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ch.Elca.Iiop.Idl;
using omg.org.CORBA;
using System.Runtime.Remoting;

namespace Scs.Core.Util
{
  internal static class IiopNetUtil
  {

    /// <summary>
    /// Verifica se o objeto CORBA implementa a interface (repository ID).
    /// </summary>
    /// <param name="obj">O objeto CORBA</param>
    /// <param name="repositoryId">a interface (repository ID)</param>
    /// <returns></returns>
    public static bool CheckInterface(MarshalByRefObject obj, string repositoryId) {
      OrbServices orb = OrbServices.GetSingleton();
      return orb.is_a(obj, repositoryId);
    }

    public static string GetRepositoryId(Type type) {
      return Repository.GetRepositoryID(type);
    }

    public static void ActivateFacet(MarshalByRefObject facetObj) {
      RemotingServices.Marshal(facetObj);
    }

    
  }
}
