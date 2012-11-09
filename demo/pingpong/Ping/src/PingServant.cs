using System;
using scs.core;
using scs.demos.pingpong;
using Scs.Core;

namespace Server
{
  public class PingServant : MarshalByRefObject, PingPongServer
  {
    #region Fields

    private ComponentContext context;

    #endregion

    #region Contructors

    public PingServant(ComponentContext context) {
      this.context = context;
    }

    #endregion

    #region PingPongServer Members

    public void _stop() {
      throw new NotImplementedException();
    }

    public int getId() {
      throw new NotImplementedException();
    }

    public void ping() {
      Console.WriteLine("Ping!");
    }

    public void pong() {
      Console.WriteLine("[Executando o método pong no componente Pong]");
      Receptacle receptacle = context.GetReceptacles()["PongRec"];
      foreach (ConnectionDescription connection in receptacle.GetConnections()) {
        PingPongServer pingPongServer = connection.objref as PingPongServer;
        pingPongServer.pong();
      }
    }

    public void setId(int identifier) {
      throw new NotImplementedException();
    }

    public void start() {
      throw new NotImplementedException();
    }

    #endregion

    public override object InitializeLifetimeService() {
      return null;
    }
  }
}
