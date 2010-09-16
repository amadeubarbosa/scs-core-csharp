﻿using System;
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
      Receptacle rec = receptacles[receptacle];
      if (rec == null)
        throw new InvalidName();

      if ((!rec.IsMultiple) &&
          (rec.GetConnectionsSize() > 0))
        throw new AlreadyConnected();

      if (!IiopNetUtil.CheckInterface(obj, rec.RepositoryId))
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

      Receptacle receptacle = FindReceptacle(id);
      if (receptacle == null)
        throw new NoConnection();

      receptacle.RemoveConnetions(id);
    }

    /// <summary>
    /// Obtém as conexões existentes no receptáculo.
    /// </summary>
    /// <param name="receptacle">O nome do receptáculo.</param>
    /// <returns>O conjunto de descritores de conexão.</returns>
    /// <exception cref="InvalidName">Caso um nome seja inválido.</exception>
    public ConnectionDescription[] getConnections(string receptacle) {
      IDictionary<string, Receptacle> receptacles = context.GetReceptacles();
      Receptacle rec = receptacles[receptacle];
      if (rec == null)
        throw new InvalidName();

      return rec.GetConnections().ToArray();
    }

    #endregion

    #region Private Members

    /// <summary>
    /// Busca um receptáculo a partir de um identificador.
    /// </summary>
    /// <param name="id">A indentificação da conexão.</param>
    /// <returns>O receptáculo.</returns>    
    private Receptacle FindReceptacle(int id) {
      IDictionary<string, Receptacle> receptacles = context.GetReceptacles();
      foreach (Receptacle receptacle in receptacles.Values) {
        try {
          receptacle.GetConnection(id);
          return receptacle;
        }
        catch (KeyNotFoundException) { }
      }
      return null;
    }

   #endregion
  }
}
