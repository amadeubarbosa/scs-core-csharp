using System;
using System.Collections;
using System.Collections.Generic;
using Ch.Elca.Iiop.Idl;
using omg.org.CORBA;
using scs.core;
using Scs.Core.Util;

namespace Scs.Core.Servant
{
  public class IReceptaclesServant : MarshalByRefObject, IReceptacles
  {
    #region Field

    /// <summary>
    /// O contexto do componente.
    /// </summary>
    private ComponentContext context;

    #endregion

    #region Contructors

    public IReceptaclesServant(ComponentContext context) {
      this.context = context;
    }

    #endregion

    #region IReceptacles Members

    public int connect(string receptacle, MarshalByRefObject obj) {
      IDictionary<string, Receptacle> receptacles =
          this.context.GetReceptacles();
      Receptacle rec = receptacles[receptacle];
      if (rec == null)
        throw new InvalidName();

      if ((!rec.IsMultiple) &&
          (rec.GetConnectionsSize() > 0))
        throw new AlreadyConnected();

      if (!IiopNetUtil.CheckInterface(obj, rec.RepositoryId))
        throw new InvalidConnection();

      return rec.AddConnections(obj);
    }

    public void disconnect(int id) {
      if (id < 0)
        throw new InvalidConnection();

      Receptacle receptacle = FindReceptacle(id);
      if (receptacle == null)
        throw new NoConnection();

      receptacle.RemoveConnetions(id);
    }


    public ConnectionDescription[] getConnections(string receptacle) {
      IDictionary<string, Receptacle> receptacles = context.GetReceptacles();
      Receptacle rec = receptacles[receptacle];
      if (rec == null)
        throw new InvalidName();

      return rec.GetConnections();
    }

    #endregion

    #region Private Members

    /// <summary>
    /// Busca um receptáculo a partir de um identificador.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private Receptacle FindReceptacle(int id) {
      IDictionary<string, Receptacle> receptacles = context.GetReceptacles();
      foreach (Receptacle receptacle in receptacles.Values) {
        try {          
          receptacle.GetConnection(id);
          return receptacle;
        }
        catch(KeyNotFoundException) {}        
      }
      return null;
    }

    #endregion
  }
}
