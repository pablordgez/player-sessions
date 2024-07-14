using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace player_sessions.Server
{
    public class SessionManager
    {
        ServerSession defaultSession = new ServerSession("Free mode", false, 1);

        List<(Player, Session)> playerList;
        public SessionManager()
        {
            playerList = new List<(Player, Session)>();
        }

        public bool AddPlayer(Player player)
        {
            if (playerList.Any((t => t.Item1 == player)))
            {
                return false;
            }
            else
            {
                playerList.Add((player, defaultSession));
                return true;
            }
        }

       public bool RemovePlayer(Player player)
        {
            return playerList.Remove(playerList.Find(t => t.Item1 == player));
        }

        public bool SetPlayerSession(Player player, ServerSession session)
        {
            Session currentSession = playerList.Find(t => t.Item1 == player).Item2;
            if(currentSession is PlayerSession)
            {
                PlayerSession playerSession = (PlayerSession)currentSession;
                if(playerSession.Host == player)
                {
                    List<Player> playersInSession = playerList.FindAll(t => t.Item2 == playerSession).Select(t => t.Item1).ToList();
                    foreach(Player p in  playersInSession)
                    {
                        SetPlayerSession(p, defaultSession);
                    }
                }
                
                for(int i = 0; i < playerList.Count; i++)
                {
                    if (playerList[i].Item1 == player)
                    {
                        playerList[i] = (player, session);
                        break;
                    }
                }


                SetPlayerRoutingBucket(player.Handle, session.SessionId);
                session.AddPlayer(player);
                return true;

                
            }
            else
            {
                for (int i = 0; i < playerList.Count; i++)
                {
                    if (playerList[i].Item1 == player)
                    {
                        playerList[i] = (player, session);
                        break;
                    }
                }

                SetPlayerRoutingBucket(player.Handle, session.SessionId);
                session.AddPlayer(player);
                return true;
            }
        }
    }
}
