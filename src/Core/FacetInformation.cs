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
    public string name;

    /// <summary>
    /// Nome da interface;
    /// </summary>
    public string interfaceName;

    /// <summary>
    /// O tipo da classe que implementa a faceta.
    /// </summary>
    public Type type;

    #endregion


    #region Contructors

    public FacetInformation(string name, string interfaceName, Type type) {
      this.name = name;
      this.interfaceName = interfaceName;
      this.type = type;
    }

    #endregion

  }
}
