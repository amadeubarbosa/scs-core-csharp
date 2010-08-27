using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using scs.core;

namespace Scs.Core.Servant
{
  public class IMetaInterfaceServant : IMetaInterface
  {

    #region Field

    /// <summary>
    /// O contexto do componente.
    /// </summary>
    private ComponentContext context;

    #endregion

    #region Contructors

    public IMetaInterfaceServant(ComponentContext context) {
      this.context = context;
    }

    #endregion

    #region IMetaInterface Members

    public FacetDescription[] getFacets() {
      IDictionary<String, Facet> facets = this.context.GetFacets();

      return CreateFacetDescriptionVector(facets);
    }

    public FacetDescription[] getFacetsByName(string[] names) {
      IDictionary<String, Facet> facets = this.context.GetFacets();
      // IDictionary<String, Facet> selectedFacets =

      var filteredFacets = (IDictionary<String, Facet>)
          from facet in facets
          where names.Contains(facet.Value.Name.Trim())
          select facet;

      IDictionary<String, Facet> selectedFacets =
          filteredFacets as IDictionary<String, Facet>;

      return CreateFacetDescriptionVector(selectedFacets);
    }

    public ReceptacleDescription[] getReceptacles() {
      IDictionary<String, Receptacle> receptacles =
          this.context.GetReceptacles();

      return CreateReceptacleDescriptionVector(receptacles);
    }

    public ReceptacleDescription[] getReceptaclesByName(string[] names) {
      IDictionary<String, Receptacle> receptacles =
          this.context.GetReceptacles();

      var filteredReceptacles =
          from receptacle in receptacles
          where names.Contains(receptacle.Value.Name.Trim())
          select receptacle;

      IDictionary<String, Receptacle> selectedReceptacles =
          (IDictionary<String, Receptacle>)filteredReceptacles;

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
          facet.RepositoryId, facet.ObjectRef);
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
            recep.RepositoryId, recep.IsMultiple, recep.GetConnections());
      }

      return receptacleDesc;
    }

    #endregion
  }
}
