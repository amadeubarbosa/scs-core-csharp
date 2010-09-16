using System;
using System.Collections.Generic;
using System.Reflection;
using scs.core;
using Scs.Core.Exception;
using Scs.Core.Servant;
using Scs.Core.Util;

namespace Scs.Core
{
  /// <summary>
  /// Responsável por instanciar componentes.
  /// </summary>
  public class ComponentBuilder
  {

    /// <summary>
    /// Cria um componente.
    /// </summary>
    /// <param name="facets">As informações da faceta.</param>
    /// <param name="receptacles">As informações do receptáculo</param>
    /// <param name="componentId">O identificador do componente.</param>
    /// <returns>A representação do componente localmente.</returns>
    /// <exception cref="SCSException">
    /// Caso algum erro ocorra na criação do componente.
    /// </exception>
    public ComponentContext NewComponent(List<FacetInformation> facets,
      List<ReceptacleInformation> receptacles, ComponentId componentId) {

      if (String.IsNullOrEmpty(componentId.name))
        throw new ArgumentException("'ComponentId' não foi criado corretamente");
      if (facets == null)
        throw new ArgumentNullException("facets");
      if (facets.Count < 1)
        throw new ArgumentException("'Facets' está vazio");

      ComponentContext context = new DefaultComponentContext(componentId);

      AddBasicFacets(facets);

      foreach (FacetInformation facet in facets) {
        MarshalByRefObject facetObj = CreateFacet(facet, context);
        if (facetObj == null) {
          string errorMsg = "Faceta não pode ser instanciada como um" +
            "objeto remoto";
          throw new SCSException(errorMsg);
        }
        IiopNetUtil.ActivateFacet(facetObj);
        AddFacetToComponent(facet, facetObj, context);
      }

      if (receptacles != null) {
        foreach (ReceptacleInformation recepacle in receptacles) {
          AddReceptacleToComponent(recepacle, context);
        }
      }

      return context;
    }

    #region Private Methods

    /// <summary>
    /// Cria uma faceta.
    /// </summary>
    /// <param name="facet">A instância da faceta.</param>
    /// <param name="context">O componente representado pelo contexto.</param>
    /// <returns>A faceta instanciada.</returns>
    /// <exception cref="SCSException">Falha na criação do objeto.</exception>    
    private MarshalByRefObject CreateFacet(FacetInformation facet,
        ComponentContext context) {
      Type facetType = facet.Type;

      ConstructorInfo constructor = facetType.GetConstructor(
        new Type[] { typeof(ComponentContext) });
      if (constructor == null) {
        string errorMsg = "Implementação da faceta deve possuir um" +
          "contrutor com um parametro do tipo 'ComponentContext'";
        throw new SCSException(errorMsg);
      }

      return constructor.Invoke(
          new object[] { context }) as MarshalByRefObject;
    }

    /// <summary>
    /// Adiciona a faceta ao componente.
    /// </summary>
    /// <param name="facet">As informações da facet.</param>
    /// <param name="facetObj">A faceta instanciada (objeto CORBA).</param>
    /// <param name="context">O contexto do componente.</param>
    private void AddFacetToComponent(FacetInformation facet,
        MarshalByRefObject facetObj, ComponentContext context) {
      string facetName = facet.Name;
      string facetInterface = facet.RepositoryId;

      IDictionary<String, Facet> contexFacets = context.GetFacets();
      Facet newFacet = new Facet(facetName, facetInterface, facetObj);
      contexFacets.Add(facetName, newFacet);
    }

    /// <summary>
    /// Adiciona um receptáculo ao componente.
    /// </summary>
    /// <param name="receptacleInfo">A descrição do receptáculo.</param>
    /// <param name="context">O contexto do componente.</param>
    private void AddReceptacleToComponent(ReceptacleInformation receptacleInfo,
    ComponentContext context) {
      string name = receptacleInfo.Name;
      string repositoryId = receptacleInfo.RepositoryId;
      bool isMultiple = receptacleInfo.IsMultiple;

      Receptacle receptacle = new Receptacle(name, repositoryId, isMultiple);
      IDictionary<String, Receptacle> receptacles = context.GetReceptacles();
      receptacles.Add(name, receptacle);
    }

    /// <summary>
    /// Adiciona as facetas básicas à lista de Facetas criadas pelo usuário.
    /// </summary>
    /// <param name="facets">A lista de facetas criada pelo usuário.</param>
    private void AddBasicFacets(List<FacetInformation> facets) {

      Type componentType = typeof(IComponent);
      string componentName = componentType.Name;
      string componentRepId = IiopNetUtil.GetRepositoryId(componentType);
      FacetInformation componnetFacetsFind = facets.Find(
        delegate(FacetInformation facet) { return facet.Name == componentName; });
      if (componnetFacetsFind == null) {
        FacetInformation componentFacet = new FacetInformation(componentName,
            componentRepId, typeof(IComponentServant));
        facets.Add(componentFacet);
      }

      Type receptacleType = typeof(IReceptacles);
      string receptacleName = receptacleType.Name;
      string receptacleRepId = IiopNetUtil.GetRepositoryId(receptacleType);
      FacetInformation receptacleFacetsFind = facets.Find(
        delegate(FacetInformation facet) { return facet.Name == receptacleName; });
      if (receptacleFacetsFind == null) {
        FacetInformation receptacleFacet = new FacetInformation(receptacleName,
            receptacleRepId, typeof(IReceptaclesServant));
        facets.Add(receptacleFacet);
      }

      Type metaInterfaceType = typeof(IMetaInterface);
      string metaInterfaceName = metaInterfaceType.Name;
      string metaInterfaceRepId = IiopNetUtil.GetRepositoryId(metaInterfaceType);
      FacetInformation metaInterfaceFind = facets.Find(
        delegate(FacetInformation facet) { return facet.Name == metaInterfaceName; });
      if (metaInterfaceFind == null) {
        FacetInformation metaInterfaceFacet = new FacetInformation(
            metaInterfaceName, metaInterfaceRepId, typeof(IMetaInterfaceServant));
        facets.Add(metaInterfaceFacet);
      }
    }

    #endregion
  }
}
