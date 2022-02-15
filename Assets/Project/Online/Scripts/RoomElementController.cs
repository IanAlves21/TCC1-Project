using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Online.Scripts
{
    public class RoomElementController : MonoBehaviour
    {
        [SerializeField] private Text RoomNameText;
        [SerializeField] private Text RoomPlayersText;
        [SerializeField] private Button JoinRoomButton;
        [SerializeField] private TextButtonTransition buttonTextUpdate;
        [SerializeField] private Text buttonText;

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

                RoomController.Room.operation = "JoinRoom";
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

            if(currentPlayers == maxPlayers)
            {
                Color disabledColor;
                ColorUtility.TryParseHtmlString("#A6A6A6", out disabledColor);

                JoinRoomButton.interactable = false;

                buttonText.color = disabledColor;
                buttonTextUpdate.HoverColor = disabledColor;
                buttonTextUpdate.NormalColor = disabledColor;
            }
        }
    }
}
