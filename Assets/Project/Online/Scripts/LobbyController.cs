using System;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit.Forms;
using Photon.Realtime;
using UnityEngine;

namespace Project.Online.Scripts
{
    public class LobbyController : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject startButton;
        [SerializeField] private GameObject cancelButton;
        [SerializeField] private GameObject[] availableCharactersPrefabs;
        [SerializeField] private int roomSize;
        
        private LobbyController lobby;
        
        void Awake()
        {
            lobby = this;
            LoadPrefabsToResourcesCash();
        }

        public void StartButtonClick()
        {
            startButton.SetActive(false);
            cancelButton.SetActive(true);
            PhotonNetwork.JoinRandomRoom();
            Debug.Log("Start game");
        }
        
        public void CancelButtonClick()
        {
            startButton.SetActive(true);
            cancelButton.SetActive(false);
            PhotonNetwork.LeaveRoom();
            Debug.Log("Cancel");
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
