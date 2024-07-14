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