using System;
using System.Collections.Generic;
using Ch.Elca.Iiop.Idl;
using scs.core;
using Scs.Core;
using scs.demos.pingpong;

namespace Server
{
  class Program
  {
    static void Main(string[] args) {

      ComponentBuilder builder = new ComponentBuilder();

      Type pingpongType = typeof(PingPongServant);
      string pingpongRepID = Repository.GetRepositoryID(typeof(PingPongServer));            

      //Criando o componente Ping.
      List<FacetInformation> pingFacets = new List<FacetInformation>();
      pingFacets.Add(new FacetInformation("Ping", pingpongRepID, pingpongType));      

      ComponentId pingComponentId = new ComponentId("PingPong",1,0,0,"");
      ComponentContext pingContext = builder.newComponent(pingFacets, null, pingComponentId);

      //Criando o component Pong.
      List<FacetInformation> pongFacets = new List<FacetInformation>();
      pongFacets.Add(new FacetInformation("Pong", pingpongRepID, pingpongType));

      ComponentId pongComponentId = new ComponentId("PingPong", 1, 0, 0, "");
      ComponentContext pongContext = builder.newComponent(pongFacets, null, pongComponentId);
      
      IComponent pingComponent = pongContext.GetIComponent();
      MarshalByRefObject obj = pingComponent.getFacetByName("IComponent");


      Console.WriteLine();

/*          ComponentBuilder builder = new ComponentBuilder(bus.getRootPOA(), orb);
    ExtendedFacetDescription[] descriptions = new ExtendedFacetDescription[1];
    descriptions[0] =
      new ExtendedFacetDescription("IHello", IHelloHelper.id(), HelloImpl.class
        .getCanonicalName());
    ComponentContext context =
      builder.newComponent(descriptions, new ComponentId("Hello", (byte) 1,
        (byte) 0, (byte) 0, "Java"));
      */
    }
  }
}
