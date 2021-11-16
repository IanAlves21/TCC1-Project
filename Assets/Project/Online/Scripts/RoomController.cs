using Photon.Pun;
using UnityEngine;

namespace Project.Online.Scripts
{
    public class RoomController : MonoBehaviourPunCallbacks
    {
        [SerializeField] private int multiplayerSceneIndex;

        private void StartGame()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Starting game");
                PhotonNetwork.LoadLevel(multiplayerSceneIndex);
            }
        }
        
        public override void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        public override void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Joined room");
            StartGame();
        }
    }
}
