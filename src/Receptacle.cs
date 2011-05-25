using System;
using System.Collections.Generic;
using System.Threading;
using scs.core;

namespace Scs.Core
{
  /// <summary>
  /// Representa o receptáculo do componente.
  /// </summary>
  public class Receptacle
  {
    #region Fields

    /// <summary>
    /// Nome do receptáculo.
    /// </summary>
    public string Name {
      get { return name; }
    }
    private string name;

    /// <summary>
    /// Interface que o receptáculo permite conexão.
    /// </summary>
    public string InterfaceName {
      get { return interfaceName; }
    }
    private string interfaceName;

    /// <summary>
    /// Indica se o receptáculo aceita múltiplas conexões.
    /// </summary>
    public bool IsMultiple {
      get { return isMultiple; }
    }
    private bool isMultiple;

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
    /// <param name="name">O nome do receptáculo.</param>
    /// <param name="interfaceName">O tipo do receptáculo (RepositoryID).</param>
    /// <param name="isMultiple">Infomra se o receptáculo é múltiplo.</param>
    public Receptacle(string name, string repositoryId, bool isMultiple) {
      this.name = name;
      this.interfaceName = repositoryId;
      this.isMultiple = isMultiple;
      this.connections = new Dictionary<Int32, MarshalByRefObject>();
    }

    #endregion

    #region Public Members

    /// <summary>
    /// Fornece a descrição do receptáculo.
    /// </summary>
    /// <returns>A descrição do receptáculo.</returns>
    public ReceptacleDescription GetDescription() {
      ConnectionDescription[] connection = GetConnections().ToArray();
      return new ReceptacleDescription(name, interfaceName, isMultiple, connection);
    }

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
    /// <returns>
    /// <c>True</c> tenha removido com sucesso. <c>False</c> caso contrário.
    /// </returns>
    public bool RemoveConnetions(int id) {
      return this.connections.Remove(id);
    }

    /// <summary>
    /// Fornece todas as conexões.
    /// </summary>
    /// <returns>A lista de conexões do receptáculo.</returns>
    public List<ConnectionDescription> GetConnections() {
      List<ConnectionDescription> connectionList =
          new List<ConnectionDescription>();

      foreach (var connetion in this.connections) {
        connectionList.Add(
            new ConnectionDescription(connetion.Key, connetion.Value));
      }
      return connectionList;
    }

    /// <summary>
    /// Fornece a conexão desejada.
    /// </summary>
    /// <param name="id">O identificador da conexão.</param>    
    /// <returns>A conexão desejada.</returns>
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

    /// <summary>
    /// Remove todas as conexões.
    /// </summary>
    public void ClearConnections() {
      connections.Clear();
    }

    #endregion
  }
}
