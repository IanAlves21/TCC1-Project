using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Project.Online.Scripts
{
    public class LobbyController : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject roomListPanel;
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject characterMenuPanel;
        [SerializeField] private GameObject CreateRoomMenuPanel;
        [SerializeField] private GameObject characterMenuBackButton;

        [SerializeField] private GameObject startButton;
        [SerializeField] private GameObject joinButton;
        [SerializeField] private GameObject rowItemListPrefab;
        [SerializeField] private GameObject rootListGameObject;
        [SerializeField] private GameObject cancelButton;
        [SerializeField] private GameObject[] gameObjectsToActive;
        [SerializeField] private GameObject[] gameObjectsToConfig;
        [SerializeField] private GameObject[] availableCharactersPrefabs;
        [SerializeField] private int roomSize;

        [SerializeField] private Button nextButton;
        [SerializeField] private InputField RoomNameInputField;
        [SerializeField] private InputField MaxPlayersInputField;

        private LobbyController lobby;
        
        private Dictionary<string, RoomInfo> cachedRoomList;
        private Dictionary<string, GameObject> roomListEntries;
        // private Dictionary<int, GameObject> playerListEntries;
        
        void Awake()
        {
            lobby = this;
            cachedRoomList = new Dictionary<string, RoomInfo>();
            roomListEntries = new Dictionary<string, GameObject>();
            LoadPrefabsToResourcesCash();
        }

        public void StartButtonClick()
        {
            mainMenuPanel.SetActive(false);
            characterMenuPanel.SetActive(true);
        }
        
        public void ConfigButtonClick()
        {
            foreach (var go in gameObjectsToConfig)
            {
                go.SetActive(!go.activeSelf);
            }
        }
        
        public void CancelButtonClick()
        {
            startButton.SetActive(true);
            cancelButton.SetActive(false);
            PhotonNetwork.LeaveRoom();
            Debug.Log("Cancel");
        }
        
        public void JoinButtonClick()
        {
            if (!PhotonNetwork.InLobby)
            {
                PhotonNetwork.JoinLobby();
            }

            roomListPanel.SetActive(true);
            mainMenuPanel.SetActive(false);
        }

        public void CharacterMenuBackButtonClick()
        {
            switch (RoomController.Room.operation)
            {
                case "JoinRoom":
                    characterMenuPanel.SetActive(false);
                    roomListPanel.SetActive(true);
                    break;

                case "CreateRoom":
                    characterMenuPanel.SetActive(false);
                    CreateRoomMenuPanel.SetActive(true);
                    break;

                default:
                    mainMenuPanel.SetActive(true);
                    characterMenuPanel.SetActive(false);
                    break;
            }

            Debug.Log("Start game");
        }

        public void CreateRoomButtonClick()
        {
            string roomName = RoomNameInputField.text;
            roomName = (roomName.Equals(string.Empty)) ? "Room " + Random.Range(1000, 10000) : roomName;

            byte maxPlayers;
            byte.TryParse(MaxPlayersInputField.text, out maxPlayers);
            maxPlayers = (byte)Mathf.Clamp(maxPlayers, 2, 8);

            RoomOptions options = new RoomOptions { MaxPlayers = maxPlayers };

            RoomController.Room.operation = "CreateRoom";

            RoomController.Room.roomName = roomName;
            RoomController.Room.roomOptions = options;

            characterMenuPanel.SetActive(true);
            CreateRoomMenuPanel.SetActive(false);
        }

        public void CreateButtonClick()
        {
            roomListPanel.SetActive(false);
            CreateRoomMenuPanel.SetActive(true);
        }

        public void CreateRoomCancelButtonClick()
        {
            roomListPanel.SetActive(true);
            CreateRoomMenuPanel.SetActive(false);
        }

        public void CharacterButtonClick(int selectedCharacter)
        {
            switch(selectedCharacter)
            {
                case 0:
                    RoomController.Room.selectedCharacter = "CyborgPlayer";
                    break;
                case 1:
                    RoomController.Room.selectedCharacter = "PunkPlayer";
                    break;
                case 2:
                    RoomController.Room.selectedCharacter = "BikerPlayer";
                    break;
                default:
                    RoomController.Room.selectedCharacter = "BikerPlayer";
                    break;
            }

            nextButton.interactable = true;
        }
        
        public void NextButtonClick()
        {
            startButton.SetActive(false);
            cancelButton.SetActive(true);

            switch (RoomController.Room.operation)
            {
                case "JoinRoom":
                    if (RoomController.Room.selectedRoomToEnter != "")
                    {
                        PhotonNetwork.JoinRoom(RoomController.Room.selectedRoomToEnter);
                    }
                    else
                    {
                        PhotonNetwork.JoinRandomRoom();
                    }
                    break;

                case "CreateRoom":
                    string name = RoomController.Room.roomName;
                    RoomOptions options = RoomController.Room.roomOptions;

                    PhotonNetwork.CreateRoom(name, options, null);
                    break;

                default:
                    PhotonNetwork.JoinRandomRoom();
                    break;
            }

            Debug.Log("Start game");
        }

        public void BackButtonListRoom()
        {
            if (PhotonNetwork.InLobby)
            {
                PhotonNetwork.LeaveLobby();
            }
            
            roomListPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
        }
        
        private void CreateRoom()
        {
            Debug.Log("Creating room now");

            int uniqueKeyId = (int) Time.time;
            Debug.Log(uniqueKeyId);
            RoomOptions roomOps = new RoomOptions()
            {
                IsVisible = true, 
                IsOpen = true, 
                MaxPlayers = (byte) MultiplayerSettings.Settings.maxPlayers
            };
            PhotonNetwork.CreateRoom("Room" + uniqueKeyId, roomOps);
        }

        private void ClearRoomListView()
        {
            foreach (GameObject entry in roomListEntries.Values)
            {
                Destroy(entry.gameObject);
            }

            roomListEntries.Clear();
        }
        
        private void UpdateCachedRoomList(List<RoomInfo> roomList)
        {
            foreach (RoomInfo info in roomList)
            {
                // Remove room from cached room list if it got closed, became invisible or was marked as removed
                if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
                {
                    if (cachedRoomList.ContainsKey(info.Name))
                    {
                        cachedRoomList.Remove(info.Name);
                    }

                    continue;
                }

                // Update cached room info
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList[info.Name] = info;
                }
                // Add new room info to cache
                else
                {
                    cachedRoomList.Add(info.Name, info);
                }
            }
        }
        
        private void UpdateRoomListView()
        {
            foreach (RoomInfo info in cachedRoomList.Values)
            {
                GameObject entry = Instantiate(rowItemListPrefab);
                entry.transform.SetParent(rootListGameObject.transform);
                entry.transform.localScale = Vector3.one;
                entry.GetComponent<RoomElementController>().Initialize(info.Name, (byte)info.PlayerCount, info.MaxPlayers, characterMenuPanel, roomListPanel);

                roomListEntries.Add(info.Name, entry);
            }
        }
        
        private void LoadPrefabsToResourcesCash()
        {
            DefaultPool poolPrefab = PhotonNetwork.PrefabPool as DefaultPool;
            
            if ((poolPrefab != null) && (availableCharactersPrefabs != null))
            {
                foreach(GameObject prefab in availableCharactersPrefabs)
                {
                    poolPrefab.ResourceCache.Add(prefab.name, prefab);
                }
            }
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            startButton.SetActive(true);
            joinButton.SetActive(true);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("Failed to join a room");
            CreateRoom();
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log("Failed to create new room... trying again");
            CreateRoom();
        }

        public override void OnJoinedLobby()
        {
            cachedRoomList.Clear();
            ClearRoomListView();
        }

        public override void OnLeftLobby()
        {
            cachedRoomList.Clear();
            ClearRoomListView();
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("aaaaaaaaaaaaaaaaaaaa");
            ClearRoomListView();

            UpdateCachedRoomList(roomList);
            UpdateRoomListView();
            
            //
            // foreach (var r in roomList)
            // {
            //     Debug.Log("room --> " + r.Name);
            // }
        }
    }
}
