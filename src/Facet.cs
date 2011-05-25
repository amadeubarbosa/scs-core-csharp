using System;
using Scs.Core.Util;
using scs.core;

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
    public string InterfaceName {
      get {
        return interfaceName;
      }
    }
    private string interfaceName;

    /// <summary>
    /// Referência da faceta como um objeto CORBA.
    /// </summary>
    public MarshalByRefObject Reference {
      get {
        return reference;
      }
    }
    private MarshalByRefObject reference;

    #endregion

    #region Contructors

    /// <summary>
    /// Construtor.
    /// </summary>
    /// <param name="name">O nome da faceta.</param>
    /// <param name="interfaceName">O tipo da faceta (repositoryID).</param>
    /// <param name="objRef">O objeto CORBA que representa a faceta.</param>
    public Facet(string name, string interfaceName, MarshalByRefObject objRef) {
      this.name = name;
      this.interfaceName = interfaceName;
      this.reference = objRef;
    }

    #endregion

    #region Facet Members

    /// <summary>
    /// Fornece a descrição da faceta.
    /// </summary>
    /// <returns>A descrição da faceta.</returns>
    public FacetDescription GetDescription() {
      return new FacetDescription(name, interfaceName, reference);
    }

    #endregion
  }
}
