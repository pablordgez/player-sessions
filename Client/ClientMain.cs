using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace player_sessions.Client
{
    public class ClientMain : BaseScript
    {
        public ClientMain()
        {
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
            EventHandlers["receiveSessionCreationResult"] += new Action<string>(ReceiveSessionCreationResults);
        }

        private void OnClientResourceStart(string resource)
        {
            if (GetCurrentResourceName() != resource)
            {
                return;
            }
            else
            {
                RegisterCommand("createSession", new Action<int, List<object>, string>((source, args, rawCommand) =>
                {

                    dynamic resultMessage = new ExpandoObject();
                    resultMessage.args = new string[2];
                    resultMessage.args[0] = "Server";
                    if (args.Count == 3)
                    {
                        int id;
                        try
                        {
                            id = int.Parse(args[0].ToString());
                        }
                        catch (Exception)
                        {
                            resultMessage.args[1] = "The number isn't valid";
                            resultMessage.color = new int[] { 255, 0, 0 };
                            TriggerEvent("chat:addMessage", resultMessage);
                            return;
                        }


                        if (args[1].ToString().ToLower() != "true")
                        {

                            resultMessage.args[1] = "You need to specify a password if you don't want to make the session open";
                            resultMessage.color = new int[] { 255, 0, 0 };
                            TriggerEvent("chat:addMessage", resultMessage);
                            return;
                        }


                        if (args[2].ToString().ToLower() != "true" && args[2].ToString().ToLower() != "false")
                        {
                            resultMessage.args[1] = "The passive option specified isn't valid";
                            resultMessage.color = new int[] { 255, 0, 0 };
                            TriggerEvent("chat:addMessage", resultMessage);
                            return;
                        }

                        bool passive = bool.Parse(args[2].ToString());
                        TriggerServerEvent("createSession", id, bool.Parse(args[1].ToString()), bool.Parse(args[2].ToString()));






                    }
                }), false);
            }


        }

        private void ReceiveSessionCreationResults(string message)
        {
            dynamic chatMessage = new ExpandoObject();
            chatMessage.args = new string[2] { "Server", message };
            chatMessage.color = new int[] { 255, 255, 255 };
            TriggerEvent("chat:addMessage", chatMessage);
        }
    }
}