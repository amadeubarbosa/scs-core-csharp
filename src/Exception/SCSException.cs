
namespace Scs.Core.Exception
{
  /// <summary>
  /// Indica uma exceção do SCS.
  /// </summary>
  public class SCSException : System.Exception
  {
    /// <summary>
    /// Cria uma execeção do SCS
    /// </summary>
    public SCSException() : base() { }

    /// <summary>
    /// Cria uma exceção do SCS com uma mensagem associada.
    /// </summary>
    /// <param name="message"></param>
    public SCSException(string message) : base(message) { }

    /// <summary>
    /// Cria uma exceção do SCS com uma mensagem e uma causa associada.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="inner"></param>
    public SCSException(string message, System.Exception inner)
      : base(message, inner) { }
  }
}
