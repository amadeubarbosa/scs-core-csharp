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
    /// <param name="obj">O objeto CORBA.</param>
    /// <param name="interfaceName">A interface (repository ID).</param>
    /// <returns></returns>
    public static bool CheckInterface(MarshalByRefObject obj, string interfaceName) {
      OrbServices orb = OrbServices.GetSingleton();
      try {
        return orb.is_a(obj, interfaceName);
      }
      catch (NullReferenceException) {
        return false;
      }
    }

    /// <summary>
    /// Verifica se o objeto CORBA suporta o tipo específico.
    /// </summary>
    /// <param name="obj">O objeto CORBA.</param>
    /// <param name="type">O tipo específico.</param>
    /// <returns></returns>
    public static bool CheckInterface(MarshalByRefObject obj, Type type) {
      OrbServices orb = OrbServices.GetSingleton();
      return orb.is_a(obj, type);
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
    /// <exception cref="System.Security.SecurityException"></exception>
    /// <exception cref="System.Runtime.Remoting.RemotingException"></exception>
    public static ObjRef ActivateFacet(MarshalByRefObject facetObj) {
      return RemotingServices.Marshal(facetObj);
    }

    /// <summary>
    /// Permite que a faceta seja vista remotamente utilizando um identificador.
    /// </summary>
    /// <param name="facetObj">A faceta.</param>
    /// <param name="id">O nome identificador da faceta.</param>
    /// <returns>Referência para o objeto remoto.</returns>
    public static ObjRef ActivateFacet(MarshalByRefObject facetObj, String id) {
      return RemotingServices.Marshal(facetObj, id);
    }

    /// <summary>
    /// Permite que a faceta seja desativada.
    /// </summary>
    /// <param name="facetObj">Uma instância do servant.</param>
    /// <exception cref="System.Security.SecurityException"></exception>
    public static void DeactivateFacet(MarshalByRefObject facetObj) {
      RemotingServices.Disconnect(facetObj);
    }
  }
}
