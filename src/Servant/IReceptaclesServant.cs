using System;
using System.Collections;
using System.Collections.Generic;
using scs.core;
using Scs.Core.Util;

namespace Scs.Core.Servant
{
  /// <summary>
  /// Servant da interface <i>IReceptacles</i>. Implementação padrão do 
  /// <i>IReceptacles</i>.
  /// </summary>
  /// <see cref="IReceptacles"/>
  public class IReceptaclesServant : MarshalByRefObject, IReceptacles
  {
    #region Field

    /// <summary>
    /// O contexto do componente.
    /// </summary>
    private ComponentContext context;

    #endregion

    #region Contructors

    /// <summary>
    /// Constutor obrigatório de uma faceta SCS.
    /// </summary>
    /// <param name="context">O contexto do Componente.</param>
    public IReceptaclesServant(ComponentContext context) {
      if (context == null)
        throw new ArgumentNullException("context", "context is null.");
      this.context = context;
    }

    #endregion

    #region IReceptacles Members

    /// <summary>
    /// Conecta uma faceta a um receptáculo.
    /// </summary>
    /// <param name="receptacle">
    /// O nome do receptáculo que se deseja conectar.
    /// </param>
    /// <param name="obj">
    /// A referência para a faceta que se deseja conectar.
    /// </param>
    /// <exception cref="InvalidName">
    /// Caso o nome do receptáculo seja inválido.
    /// </exception>
    /// <exception cref="InvalidConnection">
    /// Caso a conexão não possa ser estabelecida, este erro pode acontecer 
    /// caso o <c>obj</c> não implemente a interface do receptáculo.
    /// </exception>
    /// <exception cref="AlreadyConnected">
    /// Caso a faceta já esteja conectada.
    /// </exception>
    /// <exception cref="ExceededConnectionLimit">
    /// Caso o número de conexões tenha excedido o limite configurado.
    /// </exception>    
    public int connect(string receptacle, MarshalByRefObject obj) {
      IDictionary<string, Receptacle> receptacles =
          this.context.GetReceptacles();
      if (!receptacles.ContainsKey(receptacle))
        throw new InvalidName();

      Receptacle rec = receptacles[receptacle];
      if ((!rec.IsMultiple) &&
          (rec.GetConnectionsSize() > 0))
        throw new AlreadyConnected();

      if (!IiopNetUtil.CheckInterface(obj, rec.InterfaceName))
        throw new InvalidConnection();

      return rec.AddConnections(obj);
    }

    /// <summary>
    /// Disconecta uma faceta. 
    /// </summary>
    /// <param name="id">A indentificação da conexão.</param>
    /// <exception cref="InvalidConnection">
    /// Caso a conexão não seja estabelecida. 
    /// </exception>
    /// <exception cref="NoConnection">
    /// Caso a conexão não exista.
    /// </exception>
    public void disconnect(int id) {
      if (id < 0)
        throw new InvalidConnection();

      IDictionary<String, Receptacle> receptacles = context.GetReceptacles();
      foreach (Receptacle receptacle in receptacles.Values) {
        Boolean removed = receptacle.RemoveConnetions(id);
        if (removed) {
          return;
        }
      }
      throw new NoConnection();
    }

    /// <summary>
    /// Obtém as conexões existentes no receptáculo.
    /// </summary>
    /// <param name="receptacle">O nome do receptáculo.</param>
    /// <returns>O conjunto de descritores de conexão.</returns>
    /// <exception cref="InvalidName">Caso um nome seja inválido.</exception>
    public ConnectionDescription[] getConnections(string receptacle) {
      IDictionary<string, Receptacle> receptacles = context.GetReceptacles();
      if (!receptacles.ContainsKey(receptacle))
        throw new InvalidName();

      Receptacle rec = receptacles[receptacle];
      return rec.GetConnections().ToArray();
    }

    #endregion
  }
}
