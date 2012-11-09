using System;
using scs.demos.helloworld;
using Scs.Core;

namespace Server
{
  public class HelloServant : MarshalByRefObject, Hello
  {
    #region Fields

    private ComponentContext context;

    #endregion

    #region Contructors

    public HelloServant(ComponentContext context) {
      this.context = context;
    }

    #endregion

    #region Hello Members

    public void sayHello() {
      Console.WriteLine("Hello World!");
    }

    #endregion

    public override object InitializeLifetimeService() {
      return null;
    }
  }
}
