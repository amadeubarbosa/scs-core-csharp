using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using log4net;
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
    /// O log
    /// </summary>
    private static ILog logger = LogManager.GetLogger(typeof(Receptacle));

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
    public Receptacle(string name, string interfaceName, bool isMultiple) {
      if (String.IsNullOrEmpty(name))
        throw new ArgumentException("O campo 'name' não pode ser nulo ou vazio.", "name");
      if (String.IsNullOrEmpty(interfaceName))
        throw new ArgumentException("O campo 'interfaceName' não pode ser nulo ou vazio.", "interfaceName");
      if (!checkInterface(interfaceName))
        throw new ArgumentException("O campo 'interfaceName' não está de acordo com o padrão");

      this.name = name;
      this.interfaceName = interfaceName;
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
    /// <param name="connection">Objeto CORBA do provedor de serviço.</param>
    /// <returns>
    /// Retorna o identificador caso o objeto tenha sido adicionado, ou -1 caso
    /// contrário.
    /// </returns>
    internal int AddConnections(MarshalByRefObject connection) {
      if (connection == null) {
        logger.Warn("Erro ao adicionar a conexão. O parâmetro 'connection' está nulo.");
        return -1;
      }

      int id = Interlocked.Increment(ref Receptacle.connectionId);
      this.connections.Add(id, connection);
      return id;
    }

    /// <summary>
    /// Remove uma conexão.
    /// </summary>
    /// <param name="id">O identificador da conexão.</param>
    /// <returns>
    /// <c>True</c> tenha removido com sucesso. <c>False</c> caso contrário.
    /// </returns>
    internal bool RemoveConnetions(int id) {
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
    /// Fornece a quantidade de conexões.
    /// </summary>
    /// <returns>A quantidade das conexões.</returns>
    public int GetConnectionsSize() {
      return this.connections.Count;
    }

    /// <summary>
    /// Remove todas as conexões.
    /// </summary>
    internal void ClearConnections() {
      connections.Clear();
    }

    #endregion

    #region Private Methods


    /// <summary>
    /// Verifica se a interface equivale a um RepositoryId
    /// </summary>
    /// <param name="interfaceName">A interface</param>
    /// <returns></returns>
    private bool checkInterface(string interfaceName) {
      Regex repositoryIDMacher = new Regex(@"^IDL:[\w/]+:\d+.\d+$");
      return repositoryIDMacher.IsMatch(interfaceName);
    }

    #endregion

    #region Override Methods

    /// <see cref="Equals" />
    public override bool Equals(object obj) {
      if (obj == null || GetType() != obj.GetType()) {
        return false;
      }
      Receptacle receptacle = (Receptacle)obj;

      if (this.name != receptacle.name)
        return false;
      if (this.interfaceName != receptacle.interfaceName)
        return false;
      if (this.isMultiple != receptacle.isMultiple)
        return false;
      return true;
    }

    /// <see cref="GetHashCode" />
    public override int GetHashCode() {
      return unchecked(name.GetHashCode() + interfaceName.GetHashCode());
    }

    #endregion


  }
}
