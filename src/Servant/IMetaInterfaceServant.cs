using System;
using System.Collections.Generic;
using System.Linq;
using scs.core;

namespace Scs.Core.Servant
{
  /// <summary>
  /// Servant da interface <i>IMetaInterface</i>. Implementação padrão do
  /// <i>IMetaInterface</i>.
  /// </summary>
  /// <see cref="IMetaInterface"/>
  public class IMetaInterfaceServant : MarshalByRefObject, IMetaInterface
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
    public IMetaInterfaceServant(ComponentContext context) {
      if (context == null)
        throw new ArgumentNullException("context", "context is null.");
      this.context = context;
    }

    #endregion

    #region IMetaInterface Members

    /// <summary>
    /// Obtém todas as facetas do componente.
    /// </summary>
    /// <returns>O conjunto de descritores de facetas.</returns>
    public FacetDescription[] getFacets() {
      IDictionary<String, Facet> facets = context.GetFacets();

      return CreateFacetDescriptionVector(facets);
    }

    /// <summary>
    /// Obtém um conjunto de facetas através de um conjunto de nomes
    /// de facetas.
    /// </summary>
    /// <param name="names">Conjunto de nomes de facetas.</param>
    /// <returns>O conjunto de descritores de facetas.</returns>
    /// <exception cref="InvalidName">
    /// Caso um nome seja inválido.
    /// </exception>
    public FacetDescription[] getFacetsByName(string[] names) {
      IDictionary<String, Facet> facets = context.GetFacets();

      var filteredFacets =
          from facet in facets
          where names.Contains(facet.Value.Name.Trim())
          select facet;

      IDictionary<String, Facet> selectedFacets =
        filteredFacets.ToDictionary(k => k.Key, c => c.Value);
      return CreateFacetDescriptionVector(selectedFacets);
    }

    /// <summary>
    /// Obtém todos os receptáculos do componente.
    /// </summary>
    /// <returns>O conjunto de descritores de receptáculos.</returns>
    public ReceptacleDescription[] getReceptacles() {
      IDictionary<String, Receptacle> receptacles =
          context.GetReceptacles();

      return CreateReceptacleDescriptionVector(receptacles);
    }

    /// <summary>
    ///  Obtém um conjunto de receptáculos através de um conjunto de
    ///  nomes de receptáculos.
    /// </summary>
    /// <param name="names">Conjunto de nomes de receptáculos.</param>
    /// <returns>O conjunto de descritores de receptáculos.</returns>
    public ReceptacleDescription[] getReceptaclesByName(string[] names) {
      IDictionary<String, Receptacle> receptacles =
          context.GetReceptacles();

      var filteredReceptacles =
          from receptacle in receptacles
          where names.Contains(receptacle.Value.Name)
          select receptacle;

      IDictionary<String, Receptacle> selectedReceptacles =
        filteredReceptacles.ToDictionary(k => k.Key, c => c.Value);
      return CreateReceptacleDescriptionVector(selectedReceptacles);
    }

    #endregion

    #region Internal Members

    /// <summary>
    /// Cria um vetor de <i>FacetDescription</i> a partir de uma coleção de 
    /// facetas.
    /// </summary>
    /// <param name="facets">A coleção de facetas</param>
    /// <returns>O vetor de <i>FacetDescription</i></returns>    
    private FacetDescription[] CreateFacetDescriptionVector(IDictionary<String, Facet> facets) {
      FacetDescription[] facetDesc = new FacetDescription[facets.Count];
      int counter = 0;

      foreach (Facet facet in facets.Values) {
        facetDesc[counter++] = new FacetDescription(facet.Name,
          facet.InterfaceName, facet.Reference);
      }
      return facetDesc;
    }

    /// <summary>
    /// Cria um vetor de <i>ReceptacleDescription</i> a partir de uma coleção de 
    /// facetas.
    /// </summary>
    /// <param name="receptacles">A coleção de receptáculos.</param>
    /// <returns>O vetor de <i>ReceptacleDescription</i></returns>    
    private ReceptacleDescription[] CreateReceptacleDescriptionVector(IDictionary<String, Receptacle> receptacles) {
      ReceptacleDescription[] receptacleDesc =
          new ReceptacleDescription[receptacles.Count];
      int counter = 0;

      foreach (Receptacle recep in receptacles.Values) {
        receptacleDesc[counter++] = new ReceptacleDescription(recep.Name,
            recep.InterfaceName, recep.IsMultiple,
            recep.GetConnections().ToArray());
      }
      return receptacleDesc;
    }

    #endregion

    /// <summary>
    /// Sempre retorna null para assinalar que este objeto não tem um ciclo de vida definido e portando não deve ser destruído.
    /// </summary>
    /// <returns>Null.</returns>
    public override object InitializeLifetimeService() {
      return null;
    }
  }
}
