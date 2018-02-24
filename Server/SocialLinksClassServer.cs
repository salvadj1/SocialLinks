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
        public override Version Version { get { return new Version("1.0"); } }
        public string orange = "[color #FF8000]";
        public string Web = " ";
        public string Discord = " ";
        public string Facebook = " ";

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
                Settings.AddSetting("Links", "Web", "http://fougerite.com/");
                Settings.AddSetting("Links", "Discord", "https://discord.gg/aw6N4pm");
                Settings.AddSetting("Links", "Facebook", "http://fougerite.com/");
                Settings.Save();
                Logger.Log(Name + " Plugin: New Settings File Created!");
                ReloadConfig();
            }
            else
            {
                Settings = new IniParser(Path.Combine(ModuleFolder, "Settings.ini"));
                if (Settings.ContainsSetting("Links", "Web") &&
                    Settings.ContainsSetting("Links", "Discord") &&
                    Settings.ContainsSetting("Links", "Facebook"))
                {

                    try
                    {
                        Web = Settings.GetSetting("Links", "Web");
                        Discord = Settings.GetSetting("Links", "Discord");
                        Facebook = Settings.GetSetting("Links", "Facebook");
                        Logger.Log(Name + " Plugin: Settings file Loaded!");
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
                player.SendConsoleMessage("SocialLinks=" + Web);
            }
            else if (cmd == "discord")
            {
                player.SendConsoleMessage("SocialLinks=" + Discord);
            }
            else if (cmd == "facebook")
            {
                player.SendConsoleMessage("SocialLinks=" + Facebook);
            }
            else if (cmd == "sociallinks" && args[0] == "reload" && player.Admin)
            {
                ReloadConfig();
                player.MessageFrom(Name, orange + "Settings has been Reloaded :)");
            }
        }
    }
}
