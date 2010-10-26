using System;
using scs.core;
using scs.demos.pingpong;
using Scs.Core;

namespace Server
{
  public class PongServant : MarshalByRefObject, PingPongServer
  {
    #region Fields

    private ComponentContext context;

    #endregion


    #region Contructors

    public PongServant(ComponentContext context) {
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
      Console.WriteLine("[Executando o método ping no componente Ping]");
      Receptacle receptacle = context.GetReceptacles()["PingRec"];
      foreach (ConnectionDescription connection in receptacle.GetConnections()) {
        PingPongServer pingPongServer = connection.objref as PingPongServer;
        pingPongServer.ping();
      }
    }

    public void pong() {
      Console.WriteLine("Pong!");
    }

    public void setId(int identifier) {
      throw new NotImplementedException();
    }

    public void start() {
      throw new NotImplementedException();
    }

    #endregion
  }
}
