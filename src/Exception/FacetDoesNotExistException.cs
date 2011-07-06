using System;

namespace Scs.Core.Exception
{
  /// <summary>
  /// Indica que uma faceta não existe.
  /// </summary>
  public class FacetDoesNotExistException : SCSException
  {
    /// <summary>
    /// Cria uma exceção FacetDoesNotExistException com o nome da faceta.
    /// </summary>
    /// <param name="facetName"></param>
    public FacetDoesNotExistException(string facetName)
      : base(String.Format("Faceta '{0}' não existe.",facetName)) { }

    /// <summary>
    /// Cria uma exceção FacetDoesNotExistException com uma mensagem e uma causa associada.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="inner"></param>
    public FacetDoesNotExistException(string message, System.Exception inner) : base(message, inner) { }
  }
}
