using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scs.Core.Exception;

namespace Scs.Core.Exception
{
  /// <summary>
  /// Indica que um receptáculo já existe.
  /// </summary>
  class ReceptacleAlreadyExistsException : SCSException
  {

    /// <summary>
    /// Cria uma exceção ReceptacleAlreadyExistsException com o nome do 
    /// receptáculo que já foi inicializada. </summary>
    /// <param name="facetName"></param>
    public ReceptacleAlreadyExistsException(string facetName)
      : base(String.Format("Receptáculo '{0}' já existe.", facetName)) { }

    /// <summary>
    /// Cria uma exceção ReceptacleAlreadyExistsException com uma mensagem e 
    /// uma causa associada. </summary>
    /// <param name="message"></param>
    /// <param name="inner"></param>
    public ReceptacleAlreadyExistsException(string message, System.Exception inner)
      : base(message, inner) { }
  }
}
