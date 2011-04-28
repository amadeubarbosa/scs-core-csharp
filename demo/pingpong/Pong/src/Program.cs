using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Channels;
using Ch.Elca.Iiop;
using Ch.Elca.Iiop.Idl;
using omg.org.CORBA;
using scs.core;
using scs.demos.pingpong;
using Scs.Core;

namespace Server
{
  class Program
  {
    static void Main(string[] args) {

      Console.WriteLine("Pressione 'enter' quando o componente Ping estiver no ar.");
      Console.ReadLine();

      IiopChannel chan = new IiopChannel(0);
      ChannelServices.RegisterChannel(chan, false);

      ComponentBuilder builder = new ComponentBuilder();

      Type pongType = typeof(PongServant);
      string pingpongRepID = Repository.GetRepositoryID(typeof(PingPongServer));

      //Criando o component Pong.      
      List<FacetInformation> pongFacets = new List<FacetInformation>();
      pongFacets.Add(new FacetInformation("Pong", pingpongRepID, pongType));

      List<ReceptacleInformation> pongReceptacles = new List<ReceptacleInformation>();
      pongReceptacles.Add(new ReceptacleInformation("PingRec", pingpongRepID, false));

      ComponentId pongComponentId = new ComponentId("PingPong", 1, 0, 0, "");
      ComponentContext pongContext = builder.NewComponent(pongFacets, pongReceptacles, pongComponentId);

      String pingIorPath = Pong.Properties.Resources.IorFilename;
      StreamReader stream = new StreamReader(pingIorPath);
      String pingIor;
      try {
        pingIor = stream.ReadToEnd();
      }
      finally {
        stream.Close();
      }

      //Recuperar a faceta Ping.
      OrbServices orb = OrbServices.GetSingleton();
      IComponent pingComponent = orb.string_to_object(pingIor) as IComponent;
      MarshalByRefObject pingObj = pingComponent.getFacetByName("Ping");

      //Recuperar a faceta Pong.
      IComponent pongComponent = pongContext.GetIComponent();
      MarshalByRefObject pongObj = pongComponent.getFacetByName("Pong");

      //Conectar o Ping em Pong.      
      IReceptacles pingIReceptacles = pingComponent.getFacetByName("IReceptacles") as IReceptacles;
      pingIReceptacles.connect("PongRec", pongObj);

      //Conectar o Pong em Ping.      
      IReceptacles pongIReceptacles = pongComponent.getFacetByName("IReceptacles") as IReceptacles;
      pongIReceptacles.connect("PingRec", pingObj);

      Console.WriteLine("Executando ping e pong no Componente Ping");
      PingPongServer pingServer = pingComponent.getFacet(pingpongRepID) as PingPongServer;
      pingServer.ping();
      pingServer.pong();
      Console.WriteLine("--\n");

      Console.WriteLine("Executando ping e pong no Componente Pong");
      PingPongServer pongServer = pongComponent.getFacet(pingpongRepID) as PingPongServer;
      pongServer.ping();
      pongServer.pong();
      Console.WriteLine("--\n");

      MarshalByRefObject faceta = pingComponent.getFacetByName("IMetaInterface");
      IMetaInterface pingMetaInterface = faceta as IMetaInterface;
      FacetDescription[] getFacets1 = pingMetaInterface.getFacets();
      foreach (var f in getFacets1) {
        Console.WriteLine(f.name + " --- " + f.interface_name);
      }

      Console.WriteLine("Fim");
      Console.ReadLine();
    }
  }
}
