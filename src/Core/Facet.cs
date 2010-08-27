using System;

namespace Scs.Core
{
  public class Facet
  {

    #region Fields

    /// <summary>
    /// Nome da faceta
    /// </summary>
    private string _name;
    public string Name {
      get {
        return _name;
      }
    }

    /// <summary>
    /// Interface que a faceta implementa.
    /// </summary>
    private string _repositoryId;
    public string RepositoryId {
      get {
        return _repositoryId;
      }
    }

    /// <summary>
    /// Objeto CORBA da faceta.
    /// </summary>
    private MarshalByRefObject _objectRef;
    public MarshalByRefObject ObjectRef {
      get {
        return _objectRef;
      }
    }
    #endregion


    #region Contructors

    /// <summary>
    /// Construtor
    /// </summary>
    /// <param name="name"></param>
    /// <param name="repositoryId"></param>
    /// <param name="objRef"></param>
    public Facet(string name, string repositoryId, MarshalByRefObject objRef) {
      this._name = name;
      this._repositoryId = repositoryId;
      this._objectRef = objRef;
    }

    #endregion

  }
}
