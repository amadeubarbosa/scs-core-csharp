using System;
using System.Collections.Generic;
using scs.core;

namespace Scs.Core
{

  /// <summary>
  /// Interface da infra-estrutura do SCS que disponibiliza funcionalidades
  /// básicas como:
  /// 
  /// (i) Facilitar ao objeto utilitário do ComponentBuilder.
  /// (ii) Fornecer a faceta IComponent.
  /// (iii) Acesso as descrições de facetas e receptáculos.
  /// </summary>
  public interface ComponentContext
  {
    /// <summary>
    /// Fornece o id do componente.
    /// </summary>
    /// <returns>O id do componente.</returns>
    ComponentId GetComponentId();

    /// <summary>
    /// Fornece um objeto CORBA referente ao IComponent.
    /// </summary>
    /// <returns>O IComponent</returns>
    IComponent GetIComponent();

    /// <summary>
    /// Fornece uma cópia da coleção de faceta. Onde a chave é o nome da 
    /// faceta e o valor é a própria faceta.
    /// </summary>
    /// <returns>A coleção de facetas.</returns>
    IDictionary<String, Facet> GetFacets();

    /// <summary>
    /// Fornece a faceta do componente através do nome da faceta.
    /// </summary>
    /// <param name="name">Nome da faceta que se deseja obter.</param>
    /// <returns>A faceta.</returns>
    Facet GetFacetByName(String name);

    /// <summary>
    /// Fornece uma cópia da coleção de receptáculos. Onde a chave é o nome do 
    /// receptáculo e o valor é o próprio receptáculo.
    /// </summary>
    /// <returns>A coleção de receptáculos.</returns>
    IDictionary<String, Receptacle> GetReceptacles();

    /// <summary>
    /// Fornece o receptáculo do componente através do nome do receptáculo.
    /// </summary>
    /// <param name="name">Nome do receptáculo.</param>
    /// <returns>O receptáculo.</returns>
    Receptacle GetReceptacleByName(String name);

    /// <summary>
    /// Adiciona a faceta ao componente. O método ativa a faceta.
    /// </summary>
    /// <param name="name">Nome da faceta.</param>
    /// <param name="interfaceName">Nome da interface (repositoryID)</param>
    /// <param name="servant">A instância da implementação da faceta.</param>
    /// <exception cref="Scs.Core.Exception.FacetAlreadyExistsException">
    /// Caso exista uma faceta com o mesmo nome.</exception>
    void AddFacet(String name, String interfaceName, MarshalByRefObject servant);

    /// <summary>
    /// Atualiza a faceta do componente identificada pelo nome.
    /// </summary>
    /// <param name="name">Nome da faceta.</param>
    /// <param name="servant">A nova instância da implementação da faceta.    
    /// </param>
    /// <exception cref="Scs.Core.Exception.FacetDoesNotExistException">
    /// Caso não exista uma faceta com o mesmo nome.</exception>
    void UpdateFacet(String name, MarshalByRefObject servant);

    /// <summary>
    /// Remove a faceta do componente. O método desativa a faceta do POA.
    /// </summary>
    /// <param name="name">Nome da faceta.</param>
    void RemoveFacet(String name);

    /// <summary>
    /// Adiciona o receptáculo ao componente.
    /// 
    /// Se já existir um receptáculo com o mesmo nome, o receptáculo antigo
    /// será sobrescrito.
    /// </summary>
    /// <param name="name">Nome do receptáculo.</param>
    /// <param name="interfaceName">Inteface que o receptáculo permite conexão.</param>
    /// <param name="isMultiple"><code>True</code> caso o recepátulo aceitar 
    /// múltiplas conexões, <code>false</code> caso contrário.</param>
    void AddReceptacle(String name, String interfaceName, Boolean isMultiple);

    /// <summary>
    /// Remove o receptáculo do componente.
    /// </summary>
    /// <param name="name"></param>
    void RemoveReceptacle(String name);
  }
}