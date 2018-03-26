using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fougerite;
using System.IO;
using UnityEngine;

namespace SocialLinks
{
    public class SocialLinksClassServer : Fougerite.Module
    {
        public override string Name { get { return "SocialLinks"; } }
        public override string Author { get { return "Salva/Juli"; } }
        public override string Description { get { return "SocialLinks"; } }
        public override Version Version { get { return new Version("1.2"); } }
        public string green = "[color #82FA58]";

        public bool EnableAnnounce = true;
        public int AnnounceTimeMins = 5;
        public string AnnounceMessage = " ";

        public string Web = " ";
        public string Discord = " ";
        public string Facebook = " ";
        public string Instagram = " ";
        public string VKontakte = " ";

        public string ClientMessage = " ";
        public int MessageDuration = 15;

        public IniParser Settings;

        public override void Initialize()
        {
            Hooks.OnServerLoaded += OnServerLoaded;
            Hooks.OnCommand += OnCommand;
        }
        public override void DeInitialize()
        {
            Hooks.OnServerLoaded -= OnServerLoaded;
            Hooks.OnCommand -= OnCommand;
        }
        public void OnServerLoaded()
        {
            ReloadConfig();
            
        }
        private void ReloadConfig()
        {
            if (!File.Exists(Path.Combine(ModuleFolder, "Settings.ini")))
            {
                File.Create(Path.Combine(ModuleFolder, "Settings.ini")).Dispose();
                Settings = new IniParser(Path.Combine(ModuleFolder, "Settings.ini"));
                Settings.AddSetting("Timer", "EnableAnnounce", "true");
                Settings.AddSetting("Timer", "AnnounceTimeMins", "5");
                Settings.AddSetting("Timer", "AnnounceMessage", "Commands: /web /discord /facebook /instagram /vk");

                Settings.AddSetting("Links", "Web", "http://fougerite.com/");
                Settings.AddSetting("Links", "Discord", "https://discord.gg/aw6N4pm");
                Settings.AddSetting("Links", "Facebook", "http://fougerite.com/");
                Settings.AddSetting("Links", "Instagram", "http://fougerite.com/");
                Settings.AddSetting("Links", "VKontakte", "http://fougerite.com/");

                Settings.AddSetting("ClientSide", "ClientMessage", "A new web browser has been opened");
                Settings.AddSetting("ClientSide", "MessageDuration", "15");
                Settings.Save();
                Logger.Log(Name + " Plugin: New Settings File Created!");
                ReloadConfig();
            }
            else
            {
                Settings = new IniParser(Path.Combine(ModuleFolder, "Settings.ini"));
                if (Settings.ContainsSetting("Timer", "EnableAnnounce") &&
                    Settings.ContainsSetting("Timer", "AnnounceTimeMins") &&
                    Settings.ContainsSetting("Timer", "AnnounceMessage") &&
                    
                    Settings.ContainsSetting("Links", "Web") &&
                    Settings.ContainsSetting("Links", "Discord") &&
                    Settings.ContainsSetting("Links", "Facebook") &&
                    Settings.ContainsSetting("Links", "Instagram") &&
                    Settings.ContainsSetting("Links", "VKontakte") &&

                    Settings.ContainsSetting("ClientSide", "ClientMessage") &&
                    Settings.ContainsSetting("ClientSide", "MessageDuration"))
                {

                    try
                    {
                        EnableAnnounce = Settings.GetBoolSetting("Timer", "EnableAnnounce");
                        AnnounceTimeMins = int.Parse(Settings.GetSetting("Timer", "AnnounceTimeMins"));
                        AnnounceMessage = Settings.GetSetting("Timer", "AnnounceMessage");

                        Web = Settings.GetSetting("Links", "Web");
                        Discord = Settings.GetSetting("Links", "Discord");
                        Facebook = Settings.GetSetting("Links", "Facebook");
                        Instagram = Settings.GetSetting("Links", "Instagram");
                        VKontakte = Settings.GetSetting("Links", "VKontakte");

                        ClientMessage = Settings.GetSetting("ClientSide", "ClientMessage");
                        MessageDuration = int.Parse(Settings.GetSetting("ClientSide", "MessageDuration"));

                        Logger.Log(Name + " Plugin: Settings file Loaded!");

                        if (EnableAnnounce)
                        {
                            Timer1(30 * 1000, null).Start();
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(Name + " Plugin: Detected a problem in the configuration");
                        Logger.Log("ERROR -->" + ex.Message);
                        File.Delete(Path.Combine(ModuleFolder, "Settings.ini"));
                        Logger.Log(Name + " Plugin: Deleted the old configuration file");
                        ReloadConfig();
                    }
                }
                else
                {
                    Logger.LogError(Name + " Plugin: Detected a problem in the configuration (lost key)");
                    File.Delete(Path.Combine(ModuleFolder, "Settings.ini"));
                    Logger.LogError(Name + " Plugin: Deleted the old configuration file");
                    ReloadConfig();
                }
                return;
            }
        }

        public void OnCommand(Fougerite.Player player, string cmd, string[] args)
        {
            if (cmd == "web")
            {
                player.SendConsoleMessage("messagesociallinks=" + Web + "=" + ClientMessage + "=" + MessageDuration);
            }
            else if (cmd == "discord")
            {
                player.SendConsoleMessage("messagesociallinks=" + Discord + "=" + ClientMessage + "=" + MessageDuration);
            }
            else if (cmd == "facebook")
            {
                player.SendConsoleMessage("messagesociallinks=" + Facebook + "=" + ClientMessage + "=" + MessageDuration);
            }
            else if (cmd == "instagram")
            {
                player.SendConsoleMessage("messagesociallinks=" + Instagram + "=" + ClientMessage + "=" + MessageDuration);
            }
            else if (cmd == "vk")
            {
                player.SendConsoleMessage("messagesociallinks=" + VKontakte + "=" + ClientMessage + "=" + MessageDuration);
            }
            else if (cmd == "sociallinks" && args[0] == "reload" && player.Admin)
            {
                ReloadConfig();
                player.MessageFrom(Name, green + "Settings has been Reloaded :)");
            }
        }

        public TimedEvent Timer1(int timeoutDelay, Dictionary<string, object> args)
        {
            TimedEvent timedEvent = new TimedEvent(timeoutDelay);
            timedEvent.Args = args;
            timedEvent.OnFire += CallBack;
            return timedEvent;
        }
        public void CallBack(TimedEvent e)
        {
            e.Kill();
            if (EnableAnnounce)
            {
                Server.GetServer().BroadcastFrom(Name + " " + Version, AnnounceMessage);
                Timer1(AnnounceTimeMins * 60000, null).Start();
            }
        }
    }
}
