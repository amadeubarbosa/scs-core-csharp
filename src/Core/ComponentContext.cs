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
    /// Fornece a coleção de faceta. Onde a chave é o nome da faceta e o valor
    /// é a própria faceta.
    /// </summary>
    /// <returns>A coleção de <i>FaceDescriptions</i>.</returns>
    IDictionary<String, Facet> GetFacets();

    /// <summary>
    /// Fornece a coleção de receptáculos. Onde a chave é o nome do receptáculo
    /// e o valor é o próprio receptáculo.
    /// </summary>
    /// <returns>A coleção de receptáculos.</returns>
    IDictionary<String, Receptacle> GetReceptacles();
  }
}