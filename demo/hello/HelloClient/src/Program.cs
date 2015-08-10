using System;
using System.IO;
using HelloClient.Properties;
using omg.org.CORBA;
using scs.core;
using scs.demos.helloworld;

namespace Client
{
  class Program
  {
    static void Main() {

      Console.WriteLine("Pressione 'enter' quando o componente Hello estiver no ar.");
      Console.ReadLine();

      OrbServices.CreateAndRegisterIiopChannel();
      String helloIorPath = Resources.IorFilename;
      StreamReader stream = new StreamReader(helloIorPath);
      String helloIor;
      try {
        helloIor = stream.ReadToEnd();
      }
      finally {
        stream.Close();
      }
            
      OrbServices orb = OrbServices.GetSingleton();
      IComponent icomponent = orb.string_to_object(helloIor) as IComponent;
      Hello hello = icomponent.getFacetByName("Hello") as Hello;
      hello.sayHello();
                  
      Console.ReadLine();
    }
  }
}
