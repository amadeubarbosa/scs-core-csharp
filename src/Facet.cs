using System;
using System.Text.RegularExpressions;
using scs.core;
using Scs.Core.Exception;
using Scs.Core.Util;

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
      if (!CheckInterface(interfaceName))
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
      Activate();
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

    /// <summary>
    /// Define um servant. Caso já exista um servant definido, o método 
    /// desativa o servant anterior antes de ativar o novo servant. 
    /// </summary>
    /// <param name="servant"></param>
    public void UpdateReference(MarshalByRefObject servant) {
      if (servant == null) {
        throw new ArgumentNullException("servant", "O campo 'servant' não pode ser nulo.");
      }
      if (!IiopNetUtil.CheckInterface(servant, interfaceName)) {
        string errorMsg = String.Format("O campo 'servant' não suporta a interface {0}", interfaceName);
        throw new ArgumentException(errorMsg);
      }

      this.reference = servant;
      Activate();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Verifica se a interface equivale a um RepositoryId
    /// </summary>
    /// <param name="interfaceName">A interface</param>
    /// <returns></returns>
    private bool CheckInterface(string interfaceName) {
      Regex repositoryIDMacher = new Regex(@"^IDL:[\w/]+:\d+.\d+$");
      return repositoryIDMacher.IsMatch(interfaceName);
    }

    /// <summary>
    /// Ativa a faceta.
    /// </summary>
    /// <exception cref="SCSException"></exception>
    private void Activate() {
      try {
        IiopNetUtil.ActivateFacet(reference);
      }
      catch (System.Security.SecurityException e) {
        throw new SCSException("Falha na ativação da Faceta", e);
      }
      catch (System.Runtime.Remoting.RemotingException e) {
        throw new SCSException("Falha na ativação da Faceta", e);
      }
    }

    /// <summary>
    /// Desativa a faceta
    /// </summary>
    /// <exception cref="SCSException"></exception>
    internal void Deactivate() {
      try {
        IiopNetUtil.DeactivateFacet(reference);
      }
      catch (System.Security.SecurityException e) {
        throw new SCSException("Falha na desativação da Faceta", e);
      }
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
