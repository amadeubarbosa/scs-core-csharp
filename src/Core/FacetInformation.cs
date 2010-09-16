using System;

namespace Scs.Core
{
  /// <summary>
  /// Informação para criação da faceta.
  /// </summary>
  public class FacetInformation
  {

    #region Fields

    /// <summary>
    /// Nome da faceta.
    /// </summary>
    public string Name {
      get {
        return name;
      }
    }
    private string name;

    /// <summary>
    /// Tipo da interface.
    /// </summary>
    public string RepositoryId {
      get {
        return repositoryId;
      }
    }
    private string repositoryId;

    /// <summary>
    /// O tipo da classe que implementa a faceta.
    /// </summary>
    public Type Type {
      get {
        return type;
      }
    }
    private Type type;

    #endregion


    #region Contructors

    /// <summary>
    /// Contrutor
    /// </summary>
    /// <param name="name">O nome da faceta.</param>
    /// <param name="repositoryId">O tipo da faceta (repositoryID).</param>
    /// <param name="type">A classe que representa a faceta.</param>
    public FacetInformation(string name, string repositoryId, Type type) {
      this.name = name;
      this.repositoryId = repositoryId;
      this.type = type;
    }

    #endregion
  }
}
