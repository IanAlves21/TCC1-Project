using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Online.Scripts
{
    public class RoomElementController : MonoBehaviour
    {
        [SerializeField] private Text RoomNameText;
        [SerializeField] private Text RoomPlayersText;
        [SerializeField] private Button JoinRoomButton;
        
        private GameObject characterSelection;
        private GameObject listRoomPanel;
        private string roomName;

        public void Start()
        {
            JoinRoomButton.onClick.AddListener(() =>
            {
                if (PhotonNetwork.InLobby)
                {
                    PhotonNetwork.LeaveLobby();
                }
                
                RoomController.Room.selectedRoomToEnter = roomName;

                characterSelection.SetActive(true);
                listRoomPanel.SetActive(false);
            });
        }

        public void Initialize(string name, byte currentPlayers, byte maxPlayers, GameObject characterMenu, GameObject listRoomMenu)
        {
            roomName = name;
            characterSelection = characterMenu;
            listRoomPanel = listRoomMenu;
            RoomNameText.text = name;
            RoomPlayersText.text = currentPlayers + " / " + maxPlayers;
        }
    }
}
