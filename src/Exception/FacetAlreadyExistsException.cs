using System;

namespace Scs.Core.Exception
{
  /// <summary>
  /// Indica que uma faceta já existe.
  /// </summary>
  public class FacetAlreadyExistsException : SCSException
  {
    /// <summary>
    /// Cria uma exceção FacetAlreadyExists com o nome da faceta que já foi inicializada.
    /// </summary>
    /// <param name="facetName"></param>
    public FacetAlreadyExistsException(string facetName)
      : base(String.Format("Faceta '{0}' já existe.", facetName)) { }

    /// <summary>
    /// Cria uma exceção FacetAlreadyExists com uma mensagem e uma causa associada.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="inner"></param>
    public FacetAlreadyExistsException(string message, System.Exception inner) : base(message, inner) { }
  }
}
