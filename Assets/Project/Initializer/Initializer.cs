using System;
using Photon.Pun;
using Project.Online.Scripts;
using Unity.Mathematics;
using UnityEngine;

namespace Project.Initializer
{
    public class Initializer : MonoBehaviour
    {
        [SerializeField] private Vector3[] spawnPoints;
        [SerializeField] private String[] objectsName;
        
        void Awake()
        {
            if(!PhotonNetwork.IsMasterClient) return;
            
            for(int i=0; i < objectsName.Length; i++)
            {
                GameObject objectReference = PhotonNetwork.Instantiate(objectsName[i], spawnPoints[i], quaternion.identity);
                string uniqueObjectNameKey = objectReference.name + GenerateUniqueKey();
                
                objectReference.GetComponent<PhotonView>().RPC("RPC_ApplyUniquePlayerName", RpcTarget.AllBuffered, uniqueObjectNameKey, objectReference.name);
            }
        }

        void Update()
        {
            Debug.Log("IsMasterClient --> " + PhotonNetwork.IsMasterClient);
        }
        
        private string GenerateUniqueKey()
        {
            return DateTime.UtcNow.TimeOfDay.ToString().Replace(":", "");
        }
    }
}
