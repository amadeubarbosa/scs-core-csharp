using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting;
using Ch.Elca.Iiop.Idl;
using Scs.Core.Util;
using Scs.Core.Exception;
using scs.core;

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

      foreach (FacetInformation facet in facets) {
        MarshalByRefObject facetObj = CreateFacet(facet);
        if (facetObj == null) {
          string errorMsg = "Faceta não pode ser instanciada como um" +
            "objeto remoto";
          throw new SCSException(errorMsg);
        }
        IiopNetUtil.ActivateFacet(facetObj);
        AddFacetToComponent(facet, facetObj,context);
      }


      return null;
    }


    #region Private Methods

    /// <summary>
    /// Cria uma faceta.
    /// </summary>
    /// <param name="facet">A instância da faceta.</param>
    /// <returns>Falha na criação do objeto.</returns>
    private MarshalByRefObject CreateFacet(FacetInformation facet) {
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
        new object[] { typeof(ComponentContext) }) as MarshalByRefObject;
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


    #endregion

  }
}
