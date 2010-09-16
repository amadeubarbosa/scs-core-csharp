using System;

namespace Scs.Core
{
  /// <summary>
  /// Informação para criação do receptáculo.
  /// </summary>
  public class ReceptacleInformation
  {
    #region Fields

    /// <summary>
    /// O nome do receptáculo
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
    /// Indica se o receptáculo aceita múltiplas conexões.
    /// </summary>    
    public bool IsMultiple {
      get {
        return isMultiple;
      }
    }
    private bool isMultiple;

    #endregion

    #region Contructors

    /// <summary>
    /// Contrutor.
    /// </summary>
    /// <param name="name">O nome do receptáculo.</param>
    /// <param name="repositoryId">O tipo do receptáculo (RepositoryID).</param>
    /// <param name="isMultiple">Infomra se o receptáculo é múltiplo.</param>
    public ReceptacleInformation(string name, string repositoryId, bool isMultiple) {
      this.name = name;
      this.repositoryId = repositoryId;
      this.isMultiple = isMultiple;
    }

    #endregion
  }
}
