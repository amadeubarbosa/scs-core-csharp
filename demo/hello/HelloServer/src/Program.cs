using System;
using System.IO;
using System.Runtime.Remoting.Channels;
using System.Xml;
using Ch.Elca.Iiop;
using HelloServer.Properties;
using omg.org.CORBA;
using scs.core;
using Scs.Core;
using Scs.Core.Builder;

namespace Server
{
  class Program
  {
    static void Main(string[] args) {

      IiopChannel chan = new IiopChannel(0);
      ChannelServices.RegisterChannel(chan, false);
      
      String componentModel = Resources.ComponentDesc;
      TextReader file = new StringReader(componentModel);
      XmlTextReader componentInformation = new XmlTextReader(file);
      XMLComponentBuilder builder = new XMLComponentBuilder(componentInformation);
      ComponentContext context = builder.build();

      //Escrevendo a IOR do IComponent no arquivo.
      IComponent component = context.GetIComponent();
      OrbServices orb = OrbServices.GetSingleton();
      String ior = orb.object_to_string(component);

      String iorPath = Resources.IorFilename;
      StreamWriter stream = new StreamWriter(iorPath);
      try {
        stream.Write(ior);
      }
      finally {
        stream.Close();
      }

      Console.WriteLine("Componente Hello está no ar.");
      Console.ReadLine();
    }
  }
}
