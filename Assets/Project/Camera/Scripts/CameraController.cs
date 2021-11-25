using System;
using System.Collections.Generic;
using Project.Online.Scripts;
using Project.Player.Scripts;
using UnityEngine;

namespace Project.Camera.Scripts
{
    public class CameraController : MonoBehaviour
    {
        private void Update()
        {
            List<GameObject> players = RoomController.Room.GetPlayers();
            
            float axisPosition = 0.0f;
            int qtd = 0;
            
            foreach (var element in players)
            {
                if (element.GetComponent<PlayerInfo>().GetIsAlive())
                {
                    qtd++;
                    axisPosition += element.transform.position.x;
                }
            }
            
            this.gameObject.transform.position =
                new Vector3(axisPosition/qtd, transform.position.y, transform.position.z);
        }
    }
}
