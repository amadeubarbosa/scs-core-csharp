using System;
using System.Runtime.Remoting;
using Ch.Elca.Iiop.Idl;
using omg.org.CORBA;

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

    /// <summary>
    /// Obtém o repositoryID do tipo informado.
    /// </summary>
    /// <param name="type">O tipo da classe.</param>
    /// <returns>O repositoryID.</returns>
    public static string GetRepositoryId(Type type) {
      return Repository.GetRepositoryID(type);
    }

    /// <summary>
    /// Permite que a faceta seja vista remotamente.
    /// </summary>
    /// <param name="facetObj">Uma instância do servant.</param>
    public static void ActivateFacet(MarshalByRefObject facetObj) {
      RemotingServices.Marshal(facetObj);
    }
  }
}
