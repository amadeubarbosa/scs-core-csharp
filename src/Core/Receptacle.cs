using System;
using System.Collections.Generic;
using System.Threading;
using scs.core;

namespace Scs.Core
{
  public class Receptacle
  {
    #region Fields

    /// <summary>
    /// Nome do receptáculo.
    /// </summary>
    private string name;
    public string Name {
      get { return name; }
    }

    /// <summary>
    /// Interface que o receptáculo permite conexão.
    /// </summary>
    private string repositoryId;
    public string RepositoryId {
      get { return repositoryId; }
    }

    /// <summary>
    /// Indica se o receptáculo aceita múltiplas conexões.
    /// </summary>
    private bool isMultiple;
    public bool IsMultiple {
      get { return isMultiple; }
    }
    /// <summary>
    /// Coleção de conexões do receptáculo.
    /// </summary>
    private IDictionary<Int32, MarshalByRefObject> connections;

    /// <summary>
    /// Identificador único para as conexões.
    /// </summary>
    private static int connectionId = 0;

    #endregion

    #region Contructors

    /// <summary>
    /// Contrutor.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="repositoryId"></param>
    /// <param name="isMultiple"></param>
    public Receptacle(string name, string repositoryId, bool isMultiple) {
      this.name = name;
      this.repositoryId = repositoryId;
      this.isMultiple = isMultiple;
      this.connections = new Dictionary<Int32, MarshalByRefObject>();
    }

    #endregion

    #region Public Members

    /// <summary>
    /// Adiciona uma conexão.
    /// </summary>
    /// <param name="obj">Objeto CORBA do provedor de serviço.</param>
    /// <returns>
    /// Retorna o identificador caso o objeto tenha sido adicionado, ou -1 caso
    /// contrário.
    /// </returns>
    public int AddConnections(MarshalByRefObject obj) {
      if (obj == null)
        return -1;


      int id = Interlocked.Increment(ref Receptacle.connectionId);
      this.connections.Add(id, obj);
      return id;
    }

    /// <summary>
    /// Remove uma conexão.
    /// </summary>
    /// <param name="id">O identificador da conexão.</param>
    /// <returns></returns>
    public bool RemoveConnetions(int id) {
      return this.connections.Remove(id);
    }

    /// <summary>
    /// Fornece todas as conexões.
    /// </summary>
    /// <returns></returns>
    public ConnectionDescription[] GetConnections() {
      return CreateConnectionDescriptionVector();
    }

    /// <summary>
    /// Fornece a conexão desejada.
    /// </summary>
    /// <param name="id">O identificador da conexão.</param>
    /// <exception cref="KeyNotFoundException">
    /// Caso não exista uma chave associada ao id.
    /// </exception>
    /// <returns></returns>
    public ConnectionDescription GetConnection(int id) {
      MarshalByRefObject obj = this.connections[id];
      if (obj == null)
        throw new KeyNotFoundException();

      return new ConnectionDescription(id, obj);
    }

    /// <summary>
    /// Fornece a quantidade de conexões.
    /// </summary>
    /// <returns>A quantidade das conexões.</returns>
    public int GetConnectionsSize() {
      return this.connections.Count;
    }

    #endregion

    #region Private Members

    /// <summary>
    /// Cria um vetor de <i>ConnectionDescription</i>.
    /// </summary>
    /// <returns></returns>
    private ConnectionDescription[] CreateConnectionDescriptionVector() {
      ConnectionDescription[] connectionsDesc =
          new ConnectionDescription[this.connections.Count];
      int counter = 0;

      foreach (var connetion in this.connections) {
        connectionsDesc[counter++] = new ConnectionDescription(
            connetion.Key, connetion.Value);
      }
      return connectionsDesc;
    }

    #endregion


  }
}
