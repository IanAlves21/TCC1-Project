using System;
using Photon.Pun;
using UnityEngine;

namespace Project.Online.Scripts
{
    public class NetworkController : MonoBehaviourPunCallbacks
    {
        void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("We are now connected to the " + PhotonNetwork.CloudRegion + " Server!");
        }
    }
}
