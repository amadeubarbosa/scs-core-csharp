using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting;
using Ch.Elca.Iiop.Idl;
using Scs.Core.Util;
using Scs.Core.Exception;
using scs.core;
using Scs.Core.Servant;

namespace Scs.Core
{
  /// <summary>
  /// Responsável por instanciar componentes.
  /// </summary>
  public class ComponentBuilder
  {

    public ComponentContext newComponent(List<FacetInformation> facets,
      List<ReceptacleDescription> receptacles, ComponentId componentId) {

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

      return context;
    }

    #region Private Methods

    /// <summary>
    /// Cria uma faceta.
    /// </summary>
    /// <param name="facet">A instância da faceta.</param>
    /// <returns>Falha na criação do objeto.</returns>
    private MarshalByRefObject CreateFacet(FacetInformation facet,
        ComponentContext context) {
      string facetName = facet.name;
      string facetInterface = facet.interfaceName;
      Type facetType = facet.type;

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
      string facetName = facet.name;
      string facetInterface = facet.interfaceName;

      IDictionary<String, Facet> contexFacets = context.GetFacets();
      Facet newFacet = new Facet(facetName, facetInterface, facetObj);
      contexFacets.Add(facetName, newFacet);
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
        delegate(FacetInformation facet) { return facet.name == componentName; });
      if (componnetFacetsFind == null) {
        FacetInformation componentFacet = new FacetInformation(componentName,
            componentRepId, typeof(IComponentServant));
        facets.Add(componentFacet);
      }

      Type receptacleType = typeof(IReceptacles);
      string receptacleName = receptacleType.Name;
      string receptacleRepId = IiopNetUtil.GetRepositoryId(receptacleType);
      FacetInformation receptacleFacetsFind = facets.Find(
        delegate(FacetInformation facet) { return facet.name == receptacleName; });
      if (receptacleFacetsFind == null) {
        FacetInformation receptacleFacet = new FacetInformation(receptacleName,
            receptacleRepId, typeof(IReceptaclesServant));
        facets.Add(receptacleFacet);
      }

      Type metaInterfaceType = typeof(IMetaInterface);
      string metaInterfaceName = metaInterfaceType.Name;
      string metaInterfaceRepId = IiopNetUtil.GetRepositoryId(metaInterfaceType);
      FacetInformation metaInterfaceFind = facets.Find(
        delegate(FacetInformation facet) { return facet.name == metaInterfaceName; });
      if (metaInterfaceFind == null) {
        FacetInformation metaInterfaceFacet = new FacetInformation(
            metaInterfaceName, metaInterfaceRepId, typeof(IMetaInterfaceServant));
        facets.Add(metaInterfaceFacet);
      }
    }

    #endregion

  }
}
