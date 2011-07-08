using System;
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
      log4net.Config.XmlConfigurator.Configure();

      Console.WriteLine("Pressione 'enter' quando o componente Ping estiver no ar.");
      Console.ReadLine();

      IiopChannel chan = new IiopChannel(0);
      ChannelServices.RegisterChannel(chan, false);

      ComponentId pongComponentId = new ComponentId("PingPong", 1, 0, 0, "");
      ComponentContext pongContext = new DefaultComponentContext(pongComponentId);

      MarshalByRefObject pongServant = new PongServant(pongContext);
      string pingpongRepID = Repository.GetRepositoryID(typeof(PingPongServer));

      //Criando o component Pong.
      pongContext.AddFacet("Pong", pingpongRepID, pongServant);
      pongContext.AddReceptacle("PingRec", pingpongRepID, false);

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

      MarshalByRefObject pingMetaInterfaceObj = pingComponent.getFacetByName("IMetaInterface");
      IMetaInterface pingMetaInterface = pingMetaInterfaceObj as IMetaInterface;
      FacetDescription[] PingFacets = pingMetaInterface.getFacets();
      foreach (var facet in PingFacets) {
        Console.WriteLine(facet.name + " --- " + facet.interface_name);
      }

      Console.WriteLine("Fim");
      Console.ReadLine();
    }
  }
}
