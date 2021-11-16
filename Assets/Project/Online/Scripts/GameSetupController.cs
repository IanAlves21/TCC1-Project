using System.IO;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

namespace Project.Online.Scripts
{
    public class GameSetupController : MonoBehaviour
    {
        void Start()
        {
            CreatePlayer();
        }

        private void CreatePlayer()
        {
            Debug.Log("Creating player");
            PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player"), Vector3.zero, quaternion.identity);
        }

    }
}
