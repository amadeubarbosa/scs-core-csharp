﻿using System;
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

      return NewComponent(facets, receptacles, context);
    }

    /// <summary>
    /// Cria um componente.
    /// </summary>
    /// <param name="facets">As informações da faceta.</param>
    /// <param name="receptacles">As informações do receptáculo</param>
    /// <param name="context">O componente representado localmente.</param>
    /// <returns>A representação do componente localmente.</returns>
    /// <exception cref="SCSException">
    /// Caso algum erro ocorra na criação do componente.
    /// </exception>
    public ComponentContext NewComponent(List<FacetInformation> facets,
        List<ReceptacleInformation> receptacles, ComponentContext context) {

      if (context == null)
        throw new ArgumentNullException("context");

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
          instantiatedReceptacles, context);
    }

    /// <summary>
    /// Cria um componente a partir de facetas já instanciada.
    /// </summary>
    /// <param name="facets">A lista de facetas instanciadas.</param>
    /// <param name="receptacles">A lista de receptáculos instanciados.</param>
    /// <param name="componentId">O identificador do componente.</param>
    /// <returns>A representação do componente localmente.</returns>
    /// <exception cref="SCSException">
    /// Falha na criação do componente.
    /// </exception>  
    public ComponentContext NewComponent(List<Facet> facets,
        List<Receptacle> receptacles, ComponentId componentId) {
      if (String.IsNullOrEmpty(componentId.name))
        throw new ArgumentException("'ComponentId' não foi criado corretamente");

      ComponentContext context = new DefaultComponentContext(componentId);
      return NewComponent(facets, receptacles, context);
    }

    /// <summary>
    /// Cria um componente a partir de facetas já instanciada.
    /// </summary>
    /// <param name="facets">A lista de facetas instanciadas.</param>
    /// <param name="receptacles">A lista de receptáculos instanciados.</param>
    /// <param name="context">O componente representado localmente.</param>
    /// <returns>A representação do componente localmente.</returns>
    /// <exception cref="SCSException">
    /// Falha na criação do componente.
    /// </exception>  
    public ComponentContext NewComponent(List<Facet> facets,
        List<Receptacle> receptacles, ComponentContext context) {
      if (context == null)
        throw new ArgumentNullException("context");

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

    /// <summary>
    /// Desativa o componente.
    /// </summary>
    /// <param name="context">O componente representado localmente.</param>
    /// <exception cref="System.Security.SecurityException"></exception>
    public void DeactivateComponent(ComponentContext context) {
      if (context == null)
        throw new ArgumentNullException("context");

      ICollection<Facet> facets = context.GetFacets().Values;
      foreach (Facet facet in facets) {
        facet.Deactivate();
      }      
    }

    #region Private Methods

    /// <summary>
    /// Cria uma faceta.
    /// </summary>
    /// <param name="facetInfo">
    /// As informações necessárias para criação da faceta.
    /// </param>
    /// <param name="context">O componente representado localmente.</param>
    /// <returns>A faceta instanciada.</returns>
    /// <exception cref="SCSException">Falha na criação do objeto.</exception>
    private Facet CreateFacet(FacetInformation facetInfo,
        ComponentContext context) {
      Type facetType = facetInfo.Type;

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
      string facetName = facetInfo.Name;
      string facetInterface = facetInfo.RepositoryId;
      Facet facet = new Facet(facetName, facetInterface, facetObj);
      facet.Activate();

      return facet;
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
        Facet icomponentFacet = new Facet(
            icomponentName, icomponentRepId, icomponentObj);
        icomponentFacet.Activate();

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
        Facet receptacleFacet = new Facet(
            receptacleName, receptacleRepId, receptacleObj);
        receptacleFacet.Activate();

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
        Facet metaInterfaceFacet = new Facet(
            metaInterfaceName, metaInterfaceRepId, metaInterfaceObj);
        metaInterfaceFacet.Activate();
      }
    }

    #endregion
  }
}