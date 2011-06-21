using System;
using Scs.Core.Util;
using scs.core;
using System.Text.RegularExpressions;

namespace Scs.Core
{
  /// <summary>
  /// Representa a faceta do componente.
  /// </summary>
  public class Facet
  {

    #region Fields

    /// <summary>
    /// Nome da faceta
    /// </summary>
    public string Name {
      get {
        return name;
      }
    }
    private string name;

    /// <summary>
    /// Interface que a faceta implementa.
    /// </summary>
    public string InterfaceName {
      get {
        return interfaceName;
      }
    }
    private string interfaceName;

    /// <summary>
    /// Referência da faceta como um objeto CORBA.
    /// </summary>
    public MarshalByRefObject Reference {
      get {
        return reference;
      }
    }
    private MarshalByRefObject reference;

    #endregion

    #region Contructors

    /// <summary>
    /// Construtor.
    /// </summary>
    /// <param name="name">O nome da faceta.</param>
    /// <param name="interfaceName">O tipo da faceta (repositoryID).</param>
    /// <param name="servant">O objeto CORBA que representa a faceta.</param>
    /// <exception cref="ArgumentException">Caso os argumentos estejam
    /// incorretos</exception>
    /// <exception cref="ArgumentNullException">Caso os argumentos estejam
    /// nulos</exception>
    public Facet(string name, string interfaceName, MarshalByRefObject servant) {
      if (String.IsNullOrEmpty(name))
        throw new ArgumentException("O campo 'name' não pode ser nulo ou vazio.", "name");
      if (String.IsNullOrEmpty(interfaceName))
        throw new ArgumentException("O campo 'interfaceName' não pode ser nulo ou vazio.", "interfaceName");
      if (!checkInterface(interfaceName))
        throw new ArgumentException("O campo 'interfaceName' não está de acordo com o padrão", "interfaceName");
      if (servant == null)
        throw new ArgumentNullException("servant", "O campo 'servant' não pode ser nulo.");
      if (!IiopNetUtil.CheckInterface(servant, interfaceName)) {
        string errorMsg = String.Format("O campo 'servant' não suporta a interface {0}", interfaceName);
        throw new ArgumentException(errorMsg);
      }

      this.name = name;
      this.interfaceName = interfaceName;
      this.reference = servant;
    }

    #endregion

    #region Facet Members

    /// <summary>
    /// Fornece a descrição da faceta.
    /// </summary>
    /// <returns>A descrição da faceta.</returns>
    public FacetDescription GetDescription() {
      return new FacetDescription(name, interfaceName, reference);
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

      Facet facet = (Facet)obj;

      if (this.name != facet.name)
        return false;
      if (this.interfaceName != facet.interfaceName)
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
