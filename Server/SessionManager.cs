using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace player_sessions.Server
{
    public class SessionManager : BaseScript
    {
        ServerSession defaultSession;


        List<PlayerSession> playerSessions;
        List<ServerSession> serverSessions;
        List<(Player, Session)> playerList;
        public SessionManager()
        {
            playerList = new List<(Player, Session)>();
            playerSessions = new List<PlayerSession>();
            defaultSession = new ServerSession("Free mode", false, 1);
            serverSessions.Add(defaultSession);
        }

        public string CreateSession(Player player, int sessionId, bool open, bool passive, string password)
        {
            if(playerSessions.Find((s) => s.SessionId == sessionId) != null || serverSessions.Find((s) => s.SessionId == sessionId) != null)
            {
                return "A session with this id already exists";
            }
            else
            {
                if (open)
                {
                    PlayerSession session = new PlayerSession(player, passive, sessionId);
                    playerSessions.Add(session);
                    SetPlayerSession(player, session, password);
                }
                else
                {
                    PlayerSession session = new PlayerSession(password, player, passive, sessionId);
                    playerSessions.Add(session);
                    SetPlayerSession(player, session, password);
                }
                return "Session created succesfully, moving player to new session...";
                
                
            }
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

        private Session GetSessionFromId(int id)
        {
            Session session = playerSessions.Find((s) => s.SessionId == id);
            if(session == null)
            {
                session = serverSessions.Find((s) => s.SessionId == id);
            }

            return session;
        }

        public void SetPlayerSession(Player player, int sessionId, string password)
        {
            Session session = GetSessionFromId(sessionId);
            if(session == null)
            {
                TriggerClientEvent(player, "playerSessionsReceiveServerMessage", "There is no session with the specified id");
            }
            else
            {
                SetPlayerSession(player, session, password);
            }
        }

        private bool SetPlayerSession(Player player, Session session, string password)
        {
            if (session is PlayerSession)
            {
                if (!((PlayerSession)session).Open)
                {
                    if (password != ((PlayerSession)session).Password)
                    {
                        TriggerClientEvent(player, "playerSessionsReceiveServerMessage", "Incorrect password");
                        return false;
                    }
                }
            }
            Session currentSession = playerList.Find(t => t.Item1 == player).Item2;
            if (currentSession is PlayerSession)
            {
                PlayerSession playerSession = (PlayerSession)currentSession;
                if (playerSession.Host == player)
                {
                    List<Player> playersInSession = playerList.FindAll(t => t.Item2 == playerSession).Select(t => t.Item1).ToList();
                    foreach (Player p in playersInSession)
                    {
                        SetPlayerSession(p, defaultSession, "");
                    }
                }

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
                TriggerClientEvent(player, "playerSessionsReceiveServerMessage", "Successfully changed session");
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
                TriggerClientEvent(player, "playerSessionsReceiveServerMessage", "Successfully changed session");
                return true;
            }

        }
    }
}
