using System;
using System.Collections.Generic;
using System.Text;

namespace player_sessions.Server
{
    public class ServerSession : Session
    {
        private string name;
        public ServerSession(string name, bool passive, int sessionId)
        {
            this.name = name;
            this.passive = passive;
            this.sessionId = sessionId;
        }
    }
}
