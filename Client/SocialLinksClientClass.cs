using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RustBuster2016.API;
using UnityEngine;

namespace SocialLinksClient
{
    public class SocialLinksClientClass : RustBusterPlugin
    {
        public override string Name { get { return "SocialLinksClient"; } }
        public override string Author { get { return " by salva/juli"; } }
        public override Version Version { get { return new Version("1.0"); } }
        public override void Initialize()
        {
            if (this.IsConnectedToAServer)
            {
                RustBuster2016.API.Hooks.OnRustBusterClientConsole += OnRustBusterClientConsole;
                return;
            }
        }
        public override void DeInitialize()
        {
            RustBuster2016.API.Hooks.OnRustBusterClientConsole -= OnRustBusterClientConsole;
        }
        public void OnRustBusterClientConsole(string message)
        {
            if (message.ToLower().Contains("sociallinks"))
            {
                string[] partir = message.ToString().Split(new char[] { '=' });
                var link = partir[1];
                Rust.Notice.Popup("", "A new web browser has been opened", 15);
                Application.OpenURL(link);
            }
        }
    }
}