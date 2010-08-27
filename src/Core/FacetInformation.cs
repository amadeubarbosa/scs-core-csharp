using System;

namespace Scs.Core
{
  /// <summary>
  /// Informação para criação da faceta.
  /// </summary>
  public struct FacetInformation
  {
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
  }
}
