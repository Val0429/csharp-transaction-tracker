using System;
using Constant;

namespace App
{
    public abstract class AppClientPropertiesBase
    {
        public abstract String DefaultLanguage { get; set; }
        public abstract String DefaultHistory { get; set; }
        public abstract String DefaultScreenName { get; set; }
        public abstract String DefaultWindowState { get; set; }

        public abstract Int32 DefaultWindowLocationX { get; set; }
        public abstract Int32 DefaultWindowLocationY { get; set; }
        public abstract Int32 DefaultWindowWidth { get; set; }
        public abstract Int32 DefaultWindowHeight { get; set; }

        public abstract Boolean DefaultRemeberMe { get; set; }
        public abstract Boolean DefaultAutoSignIn { get; set; }

        public abstract void SaveProperties();

        public abstract void RemoveProperties(ServerCredential credential);
    }
}
