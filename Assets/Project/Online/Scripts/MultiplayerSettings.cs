using System;
using UnityEngine;

namespace Project.Online.Scripts
{
    public class MultiplayerSettings : MonoBehaviour
    {
        public static MultiplayerSettings Settings;

        public bool delayStart;
        public int maxPlayers;

        public int menuScene;
        public int multiplayerScene;

        private void Awake()
        {
            if (MultiplayerSettings.Settings == null)
            {
                MultiplayerSettings.Settings = this;
            }
            else
            {
                if (MultiplayerSettings.Settings != null)
                {
                    Destroy(this.gameObject);
                }
            }
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
