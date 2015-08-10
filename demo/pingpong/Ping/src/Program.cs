using System;
using System.IO;
using System.Xml;
using omg.org.CORBA;
using Ping.Properties;
using scs.core;
using Scs.Core;
using Scs.Core.Builder;

namespace Server
{
  static class Program
  {
    static void Main() {
      log4net.Config.XmlConfigurator.Configure();

      OrbServices.CreateAndRegisterIiopChannel(0);

      String componentModel = Resources.ComponentDesc;
      TextReader file = new StringReader(componentModel);
      XmlTextReader componentInformation = new XmlTextReader(file);
      XmlComponentBuilder builder = new XmlComponentBuilder(componentInformation);
      ComponentContext pingContext = builder.build();

      //Escrevendo a IOR do IComponent no arquivo.
      IComponent pingComponent = pingContext.GetIComponent();
      OrbServices orb = OrbServices.GetSingleton();
      String ior = orb.object_to_string(pingComponent);

      String iorPath = Resources.IorFilename;
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
