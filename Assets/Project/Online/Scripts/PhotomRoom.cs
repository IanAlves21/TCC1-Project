using System;
using System.IO;
using Photon.Pun;
using Photon.Realtime;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Project.Online.Scripts
{
    public class PhotomRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
    {
        public static PhotomRoom room;
        private PhotonView photonView;

        public bool isGameLoaded;
        public int currentScene;

        Photon.Realtime.Player[] photonPlayers;
        public int playersInRoom;
        public int myNumberInRoom;

        public int playersInGame;

        private bool readyToCount;
        private bool readyToStart;
        public float startingTime;
        private float lessThanMaxPlayers;
        private float atMaxPlayers;
        private float timeToStart;

        private void Awake()
        {
            if (PhotomRoom.room == null)
            {
                PhotomRoom.room = this;
            }
            else
            {
                if (PhotomRoom.room != this)
                {
                    Destroy(PhotomRoom.room.gameObject);
                    PhotomRoom.room = this;
                }
            }
            DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            photonView = GetComponent<PhotonView>();
            readyToCount = false;
            readyToStart = false;
            lessThanMaxPlayers = startingTime;
            atMaxPlayers = 6;
            timeToStart = startingTime;
        }

        private void Update()
        {
            if (MultiplayerSettings.Settings.delayStart)
            {
                if (playersInRoom == 1)
                {
                    RestartTimer();
                }

                if (!isGameLoaded)
                {
                    if (readyToStart)
                    {
                        atMaxPlayers -= Time.deltaTime;
                        lessThanMaxPlayers = atMaxPlayers;
                        timeToStart = atMaxPlayers;
                    }
                    else if(readyToCount)
                    {
                        lessThanMaxPlayers -= Time.deltaTime;
                        timeToStart = lessThanMaxPlayers;
                    }
                    Debug.Log("Display time to start to the players " + timeToStart);
                    if (timeToStart <= 0)
                    {
                        StartGame();
                    }
                }
            }
        }

        private void RestartTimer()
        {
            lessThanMaxPlayers = startingTime;
            timeToStart = startingTime;
            atMaxPlayers = 6;
            readyToCount = false;
            readyToStart = false;
        }

        private void StartGame()
        {
            isGameLoaded = true;
            if (!PhotonNetwork.IsMasterClient) return;
            if (MultiplayerSettings.Settings.delayStart)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
            PhotonNetwork.LoadLevel(MultiplayerSettings.Settings.multiplayerScene);
        }

        private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            currentScene = scene.buildIndex;
            if (currentScene == MultiplayerSettings.Settings.multiplayerScene)
            {
                isGameLoaded = true;
                if (MultiplayerSettings.Settings.delayStart)
                {
                    photonView.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
                }
                else
                {
                    RPC_CreatePlayer();
                }
            }
        }

        [PunRPC]
        private void RPC_CreatePlayer()
        {
            PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player"), Vector3.zero, quaternion.identity);
        }

        [PunRPC]
        private void RPC_LoadedGameScene()
        {
            playersInGame++;
            if (playersInGame == PhotonNetwork.PlayerList.Length)
            {
                photonView.RPC("RPC_CreatePlayer", RpcTarget.All);
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();
            PhotonNetwork.AddCallbackTarget(this);
            SceneManager.sceneLoaded += OnSceneFinishedLoading;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            PhotonNetwork.RemoveCallbackTarget(this);
            SceneManager.sceneLoaded -= OnSceneFinishedLoading;
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            photonPlayers = PhotonNetwork.PlayerList;
            playersInRoom = photonPlayers.Length;
            myNumberInRoom = playersInRoom;
            PhotonNetwork.NickName = myNumberInRoom.ToString();

            if (MultiplayerSettings.Settings.delayStart)
            {
                Debug.Log("Display players in room out of max players possible (" + playersInRoom + " : " + MultiplayerSettings.Settings.maxPlayers + ")");
                if (playersInRoom > 1)
                {
                    readyToCount = true;
                }
                if (playersInRoom == MultiplayerSettings.Settings.maxPlayers)
                {
                    readyToStart = true;
                    if (!PhotonNetwork.IsMasterClient)
                        return;
                    PhotonNetwork.CurrentRoom.IsOpen = false;
                }
            }
            else
            {
                StartGame();
            }
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            Debug.Log("A new player entered the room");
            photonPlayers = PhotonNetwork.PlayerList;
            playersInRoom++;
            if (MultiplayerSettings.Settings.delayStart)
            {
                Debug.Log("Display players in room out of max players possible (" + playersInRoom + " : " + MultiplayerSettings.Settings.maxPlayers + ")");
                if (playersInRoom > 1)
                {
                    readyToCount = true;
                }
                if (playersInRoom == MultiplayerSettings.Settings.maxPlayers)
                {
                    readyToStart = true;
                    if (!PhotonNetwork.IsMasterClient)
                        return;
                    PhotonNetwork.CurrentRoom.IsOpen = false;
                }
            }
        }
    }
}
