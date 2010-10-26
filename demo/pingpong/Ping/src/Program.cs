using System;
using System.Collections.Generic;
using Ch.Elca.Iiop.Idl;
using scs.core;
using scs.demos.pingpong;
using Scs.Core;
using Ch.Elca.Iiop;
using System.Runtime.Remoting.Channels;
using omg.org.CORBA;
using System.IO;
using System.Threading;

namespace Server
{
  class Program
  {
    static void Main(string[] args) {

      IiopChannel chan = new IiopChannel(0);
      ChannelServices.RegisterChannel(chan,false);

      ComponentBuilder builder = new ComponentBuilder();

      Type pingType = typeof(PingServant);      
      string pingpongRepID = Repository.GetRepositoryID(typeof(PingPongServer));

      //Criando o componente Ping.
      List<FacetInformation> pingFacets = new List<FacetInformation>();
      pingFacets.Add(new FacetInformation("Ping", pingpongRepID, pingType));

      List<ReceptacleInformation> pingReceptacles = new List<ReceptacleInformation>();
      pingReceptacles.Add(new ReceptacleInformation("PongRec", pingpongRepID, false));

      ComponentId pingComponentId = new ComponentId("PingPong", 1, 0, 0, "");
      ComponentContext pingContext = builder.NewComponent(pingFacets, pingReceptacles, pingComponentId);

      //Escrevendo a IOR do IComponent no arquivo.
      IComponent pingComponent = pingContext.GetIComponent();
      OrbServices orb = OrbServices.GetSingleton();
      String ior = orb.object_to_string(pingComponent);      

      String iorPath = Ping.Properties.Resources.IorFilename;
      StreamWriter stream = new StreamWriter(iorPath);
      try {
        stream.Write(ior);
      }
      finally {
        stream.Close();
      }

      Console.WriteLine("Componente ping está no ar.");
      Thread.Sleep(Timeout.Infinite);
    }
  }
}
