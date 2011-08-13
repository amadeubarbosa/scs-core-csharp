using System;
using System.IO;
using System.Runtime.Remoting.Channels;
using System.Xml;
using Ch.Elca.Iiop;
using omg.org.CORBA;
using Ping.Properties;
using scs.core;
using Scs.Core;
using Scs.Core.Builder;

namespace Server
{
  class Program
  {
    static void Main(string[] args) {
      log4net.Config.XmlConfigurator.Configure();

      IiopChannel chan = new IiopChannel(0);
      ChannelServices.RegisterChannel(chan, false);

      String componentModel = Resources.ComponentDesc;
      TextReader file = new StringReader(componentModel);
      XmlTextReader componentInformation = new XmlTextReader(file);
      XmlComponentBuilder builder = new XmlComponentBuilder(componentInformation);
      ComponentContext pingContext = builder.build();

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
      Console.ReadLine();
    }
  }
}
