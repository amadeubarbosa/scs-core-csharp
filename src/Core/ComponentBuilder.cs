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

      ComponentContext context = new DefaultComponentContext(componentId);

      List<Facet> instantiatedFacets = new List<Facet>();
      if (facets != null) {
        foreach (FacetInformation facetInfo in facets) {
          Facet facet = CreateFacet(facetInfo, context);
          instantiatedFacets.Add(facet);
        }
      }

      List<Receptacle> instantiatedReceptacles = new List<Receptacle>();
      if (receptacles != null) {
        foreach (ReceptacleInformation receptacleInfo in receptacles) {
          Receptacle receptacle = CreateReceptacle(receptacleInfo);
          instantiatedReceptacles.Add(receptacle);
        }
      }

      return NewComponent(instantiatedFacets,
          instantiatedReceptacles, componentId, context);
    }

    /// <summary>
    /// Cria um componente a partir de facetas não instanciada.
    /// </summary>
    /// <param name="facets">A lista de facetas instanciadas.</param>
    /// <param name="receptacles">A lista de receptáculos instanciados.</param>
    /// <param name="componentId">O identificador do componente.</param>
    /// <param name="context">O componente representado localmente. </param>
    /// <returns>A representação do componente localmente.</returns>
    /// <exception cref="SCSException">
    /// Falha na criação do componente.
    /// </exception>  
    public ComponentContext NewComponent(List<Facet> facets,
        List<Receptacle> receptacles, ComponentId componentId, ComponentContext context) {
      if (String.IsNullOrEmpty(componentId.name))
        throw new ArgumentException("'ComponentId' não foi criado corretamente");

      if (context == null) {
        context = new DefaultComponentContext(componentId);
      }

      AddBasicFacets(facets, context);

      IDictionary<String, Facet> facetsContext = context.GetFacets();
      foreach (Facet facet in facets) {
        facetsContext.Add(facet.Name, facet);
      }

      IDictionary<String, Receptacle> receptaclesContext = context.GetReceptacles();
      foreach (Receptacle receptacle in receptacles) {
        receptaclesContext.Add(receptacle.Name, receptacle);
      }

      return context;
    }

    #region Private Methods

    /// <summary>
    /// Cria uma faceta.
    /// </summary>
    /// <param name="facet">
    /// As informações necessárias para criação da faceta.
    /// </param>
    /// <param name="context">O componente representado localmente.</param>
    /// <returns>A faceta instanciada.</returns>
    /// <exception cref="SCSException">Falha na criação do objeto.</exception>
    private Facet CreateFacet(FacetInformation facet,
        ComponentContext context) {
      Type facetType = facet.Type;

      ConstructorInfo constructor = facetType.GetConstructor(
        new Type[] { typeof(ComponentContext) });
      if (constructor == null) {
        string errorMsg = "Implementação da faceta deve possuir um" +
          "contrutor com um parametro do tipo 'ComponentContext'";
        throw new SCSException(errorMsg);
      }

      MarshalByRefObject facetObj = constructor.Invoke(
          new object[] { context }) as MarshalByRefObject;
      if (facetObj == null) {
        throw new SCSException(
          "Faceta não pode ser instanciada como um objeto remoto");
      }
      IiopNetUtil.ActivateFacet(facetObj);

      string facetName = facet.Name;
      string facetInterface = facet.RepositoryId;

      return new Facet(facetName, facetInterface, facetObj);
    }

    /// <summary>
    /// Cria um receptáculo.
    /// </summary>
    /// <param name="receptacleInfo">
    /// As informações necessárias para criação do receptáculo.
    /// </param>    
    private Receptacle CreateReceptacle(ReceptacleInformation receptacleInfo) {
      string name = receptacleInfo.Name;
      string repositoryId = receptacleInfo.RepositoryId;
      bool isMultiple = receptacleInfo.IsMultiple;

      return new Receptacle(name, repositoryId, isMultiple);
    }

    /// <summary>
    /// Adiciona as facetas básicas à lista de Facetas criadas pelo usuário.
    /// </summary>
    /// <param name="facets">A lista de facetas criada pelo usuário.</param>
    /// <param name="context">O componente representado localmente. </param>
    private void AddBasicFacets(List<Facet> facets, ComponentContext context) {

      Type icomponentType = typeof(IComponent);
      string icomponentName = icomponentType.Name;
      string icomponentRepId = IiopNetUtil.GetRepositoryId(icomponentType);
      Facet icomponnetFacetsFind = facets.Find(
        delegate(Facet facet) { return facet.Name == icomponentName; });
      if (icomponnetFacetsFind == null) {
        IComponent icomponent = new IComponentServant(context);
        MarshalByRefObject icomponentObj = icomponent as MarshalByRefObject;
        IiopNetUtil.ActivateFacet(icomponentObj);
        Facet icomponentFacet = new Facet(
            icomponentName, icomponentRepId, icomponentObj);

        facets.Add(icomponentFacet);
      }

      Type receptacleType = typeof(IReceptacles);
      string receptacleName = receptacleType.Name;
      string receptacleRepId = IiopNetUtil.GetRepositoryId(receptacleType);
      Facet receptacleFacetsFind = facets.Find(
        delegate(Facet facet) { return facet.Name == receptacleName; });
      if (receptacleFacetsFind == null) {
        IReceptacles receptacle = new IReceptaclesServant(context);
        MarshalByRefObject receptacleObj = receptacle as MarshalByRefObject;
        IiopNetUtil.ActivateFacet(receptacleObj);
        Facet receptacleFacet = new Facet(
            receptacleName, receptacleRepId, receptacleObj);

        facets.Add(receptacleFacet);
      }

      Type metaInterfaceType = typeof(IMetaInterface);
      string metaInterfaceName = metaInterfaceType.Name;
      string metaInterfaceRepId = IiopNetUtil.GetRepositoryId(metaInterfaceType);
      Facet metaInterfaceFind = facets.Find(
        delegate(Facet facet) { return facet.Name == metaInterfaceName; });
      if (metaInterfaceFind == null) {
        IMetaInterface metaInterface = new IMetaInterfaceServant(context);
        MarshalByRefObject metaInterfaceObj = metaInterface as MarshalByRefObject;
        IiopNetUtil.ActivateFacet(metaInterfaceObj);
        Facet metaInterfaceFacet = new Facet(
            metaInterfaceName, metaInterfaceRepId, metaInterfaceObj);
      }
    }

    #endregion
  }
}
