using System;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace player_sessions.Server
{
    public class ServerMain : BaseScript
    {
        private SessionManager playerManager;

        public ServerMain()
        {
            EventHandlers["playerConnecting"] += new Action<Player, string, dynamic, dynamic>(OnPlayerConnecting);
            EventHandlers["playerDropped"] += new Action<Player, string>(OnPlayerDropped);
            EventHandlers["createSession"] += new Action<Player, int, bool, bool, string>(CreateSession);
            EventHandlers["changeSession"] += new Action<Player, int, string>(ChangeSession);
            playerManager = new SessionManager();
            
        }

        private async void OnPlayerConnecting([FromSource]Player player, string playerName, dynamic setKickReason, dynamic deferrals)
        {
            deferrals.defer();
            await Delay(0);

            bool success = playerManager.AddPlayer(player);
            if (!success)
            {
                Debug.WriteLine("Tried to add a player, but the player was already added");
            }

            deferrals.done();
        }

        private void ChangeSession([FromSource]Player player, int sessionId, string password)
        {
            playerManager.SetPlayerSession(player, sessionId, password);
        }

        private void CreateSession([FromSource]Player player, int sessionId, bool open, bool passive, string password)
        {
            if(!open && string.IsNullOrEmpty(password))
            {
                Debug.WriteLine("Tried to create private session without password");
            }
            else
            {
                string result = playerManager.CreateSession(player, sessionId, open, passive, password);
                TriggerClientEvent(player, "playerSessionsReceiveServerMessage", result);
            }
        }

        

        private void OnPlayerDropped([FromSource] Player player, string reason)
        {
            bool success = playerManager.RemovePlayer(player);
            if (!success)
            {
                Debug.WriteLine("Tried to remove player, but player was never added");
            }
        }
    }
}