using System;
using System.Collections.Generic;
using System.Text;
using scs.demos.pingpong;
using Scs.Core;

namespace Server
{
  class PingPongServant : MarshalByRefObject, PingPongServer
  {
    #region Fields

    private ComponentContext context;

    #endregion


    #region Contructors

    public PingPongServant(ComponentContext context) {
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
      throw new NotImplementedException();
    }

    public void pong() {
      throw new NotImplementedException();
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
