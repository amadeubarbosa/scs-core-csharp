using System;

namespace Scs.Core
{
  /// <summary>
  /// Representa a faceta do componente.
  /// </summary>
  public class Facet
  {

    #region Fields

    /// <summary>
    /// Nome da faceta
    /// </summary>
    public string Name {
      get {
        return name;
      }
    }
    private string name;

    /// <summary>
    /// Interface que a faceta implementa.
    /// </summary>
    public string RepositoryId {
      get {
        return repositoryId;
      }
    }
    private string repositoryId;

    /// <summary>
    /// Objeto CORBA da faceta.
    /// </summary>
    public MarshalByRefObject ObjectRef {
      get {
        return objectRef;
      }
    }
    private MarshalByRefObject objectRef;

    #endregion


    #region Contructors

    /// <summary>
    /// Construtor.
    /// </summary>
    /// <param name="name">O nome da faceta.</param>
    /// <param name="repositoryId">O tipo da faceta (repositoryID).</param>
    /// <param name="objRef">O objeto CORBA que representa a faceta.</param>
    public Facet(string name, string repositoryId, MarshalByRefObject objRef) {
      this.name = name;
      this.repositoryId = repositoryId;
      this.objectRef = objRef;
    }

    #endregion
  }
}
