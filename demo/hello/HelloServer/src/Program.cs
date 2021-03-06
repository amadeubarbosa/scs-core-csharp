﻿using System;
using System.IO;
using System.Xml;
using HelloServer.Properties;
using omg.org.CORBA;
using scs.core;
using Scs.Core;
using Scs.Core.Builder;

namespace Server
{
  class Program
  {
    static void Main() {
      log4net.Config.XmlConfigurator.Configure();

      OrbServices.CreateAndRegisterIiopChannel(0);
      
      String componentModel = Resources.ComponentDesc;
      TextReader file = new StringReader(componentModel);
      XmlTextReader componentInformation = new XmlTextReader(file);
      XmlComponentBuilder builder = new XmlComponentBuilder(componentInformation);
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
