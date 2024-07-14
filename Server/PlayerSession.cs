using System;
using System.Collections.Generic;
using System.Text;
using CitizenFX.Core;

namespace player_sessions.Server
{
    public class PlayerSession : Session
    {
        private Player host;
        private bool open;
        private string password;

        public PlayerSession(Player host, bool passive, int sessionId)
        {
            open = true;
            this.host = host;
            this.passive = passive;
            this.sessionId = sessionId;
        }

        public PlayerSession(string password, Player host, bool passive, int sessionId)
        {
            this.open = false;
            this.password = password;
            this.host = host;
            this.passive = passive;
            this.sessionId = sessionId;
        }

        public Player Host
        {
            get { return host; }
        }
    }
}
