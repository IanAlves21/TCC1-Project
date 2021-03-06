using System;
using Photon.Pun;
using Project.Player.Scripts;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Initializer
{
    public class Initializer : MonoBehaviour
    {
        [SerializeField] private Text healthText;
        [SerializeField] private Vector3[] spawnPoints;
        [SerializeField] private String[] objectsName;
        
        private void Awake()
        {
            if(!PhotonNetwork.IsMasterClient) return;
            
            for(int i=0; i < objectsName.Length; i++)
            {
                GameObject objectReference = PhotonNetwork.Instantiate(objectsName[i], spawnPoints[i], quaternion.identity);
                string uniqueObjectNameKey = objectReference.name + GenerateUniqueKey();
                
                objectReference.GetComponent<PhotonView>().RPC("RPC_ApplyUniquePlayerName", RpcTarget.AllBuffered, uniqueObjectNameKey, objectReference.name);
            }
        }

        private void Start()
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            foreach (var p in players)
            {
                if (p.GetComponent<PhotonView>().IsMine)
                {
                    p.GetComponent<PlayerController>().SetHealthText(healthText);
                }
            }
        }
        
        private string GenerateUniqueKey()
        {
            return DateTime.UtcNow.TimeOfDay.ToString().Replace(":", "");
        }
    }
}
