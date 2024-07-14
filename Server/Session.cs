using System;
using System.Collections.Generic;
using System.Text;
using CitizenFX.Core;

namespace player_sessions.Server
{
    public abstract class Session
    {
        private List<Player> players;
        protected int sessionId;
        protected bool passive;


        public bool AddPlayer(Player player)
        {
            if (players.Contains(player))
            {
                return false;
            }
            else
            {
                players.Add(player);
                return true;
            }
        }

        public bool RemovePlayer(Player player)
        {
            return players.Remove(player);
        }

        public int SessionId
        {
            get { return sessionId; }
        }
    }

    
}
