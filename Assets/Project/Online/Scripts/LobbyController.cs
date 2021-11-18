using System;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit.Forms;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Online.Scripts
{
    public class LobbyController : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject startButton;
        [SerializeField] private GameObject cancelButton;
        [SerializeField] private GameObject[] gameObjectsToActive;
        [SerializeField] private GameObject[] availableCharactersPrefabs;
        [SerializeField] private int roomSize;

        [SerializeField] private Button nextButton;
        
        private LobbyController lobby;
        
        void Awake()
        {
            lobby = this;
            LoadPrefabsToResourcesCash();
        }

        public void StartButtonClick()
        {
            // startButton.SetActive(false);
            // cancelButton.SetActive(true);
            // PhotonNetwork.JoinRandomRoom();
            // Debug.Log("Start game");
            foreach (var go in gameObjectsToActive)
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
            PhotonNetwork.JoinRandomRoom();
            Debug.Log("Start game");
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
    }
}
