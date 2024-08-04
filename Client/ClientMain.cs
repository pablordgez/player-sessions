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
            EventHandlers["playerSessionsReceiveServerMessage"] += new Action<string>(ReceiveServerMessage);
        }

        private void OnClientResourceStart(string resource)
        {
            if (GetCurrentResourceName() != resource)
            {
                return;
            }
            else
            {

                RegisterCommand("changeSession", new Action<int, List<object>, string>((source, args, rawCommand) => 
                {
                    dynamic resultMessage = new ExpandoObject();
                    resultMessage.args = new string[2];
                    resultMessage.args[0] = "Server";
                    if (args.Count == 0)
                    {
                        resultMessage.args[1] = "Not enough arguments";
                        TriggerEvent("chat:addMessage", resultMessage);
                        return;
                    } else
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
                        string password = "";
                        if (args.Count == 2) { 
                            password = args[1].ToString();
                        }
                        TriggerServerEvent("changeSession", id, password);
                    }
                }), false);



                RegisterCommand("createSession", new Action<int, List<object>, string>((source, args, rawCommand) =>
                {

                    dynamic resultMessage = new ExpandoObject();
                    resultMessage.args = new string[2];
                    resultMessage.args[0] = "Server";
                    if (args.Count < 3)
                    {
                        resultMessage.args[1] = "Not enough arguments";
                        resultMessage.color = new int[] { 255, 0, 0 };
                        TriggerEvent("chat:addMessage", resultMessage);
                        return;
                    }
                    else
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


                        string password = "";

                        if (args[1].ToString().ToLower() != "true")
                        {
                            if (args.Count == 3)
                            {
                                resultMessage.args[1] = "You need to specify a password if you don't want to make the session open";
                                resultMessage.color = new int[] { 255, 0, 0 };
                                TriggerEvent("chat:addMessage", resultMessage);
                                return;
                            }
                            else if (args[1].ToString().ToLower() == "false")
                            {
                                password = args[3].ToString();
                            }
                            else
                            {
                                resultMessage.args[1] = "The open option specified isn't valid";
                            }
                        }


                        if (args[2].ToString().ToLower() != "true" && args[2].ToString().ToLower() != "false")
                        {
                            resultMessage.args[1] = "The passive option specified isn't valid";
                            resultMessage.color = new int[] { 255, 0, 0 };
                            TriggerEvent("chat:addMessage", resultMessage);
                            return;
                        }

                        bool open = bool.Parse(args[1].ToString());
                        bool passive = bool.Parse(args[2].ToString());
                        TriggerServerEvent("createSession", id, open, passive, password);


                    }
                }), false);
            }


        }

        private void ReceiveServerMessage(string message)
        {
            dynamic chatMessage = new ExpandoObject();
            chatMessage.args = new string[2] { "Server", message };
            chatMessage.color = new int[] { 255, 255, 255 };
            TriggerEvent("chat:addMessage", chatMessage);
        }

    }
}