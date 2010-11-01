using System;
using Scs.Core.Util;

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
        return isActive ? objectRef : null;
      }
    }
    private MarshalByRefObject objectRef;

    /// <summary>
    /// <para>Informa se a faceta está ativa.</para>
    /// <para>
    /// Esta <i>flag</i> é necessária porque o método de desativação não 
    /// funciona adequadamente.
    /// </para>
    /// </summary>
    private bool isActive;

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
      this.isActive = false;
    }

    /// <summary>
    /// Ativa a faceta.
    /// </summary>
    public void Activate() {
      IiopNetUtil.ActivateFacet(this.ObjectRef);
      this.isActive = true;
    }

    /// <summary>
    /// Desativa a faceta.
    /// </summary>
    public void Deactivate() {
      IiopNetUtil.DeactivateFacet(this.ObjectRef);
      this.isActive = false;
    }

    #endregion
  }
}
