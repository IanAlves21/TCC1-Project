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
            
            float axisXPosition = 0.0f;
            float axisYPosition = 0.0f;
            int qtd = 0;
            
            foreach (var element in players)
            {
                if (element.GetComponent<PlayerInfo>().GetIsAlive())
                {
                    qtd++;
                    axisXPosition += element.transform.position.x;
                    axisYPosition += element.transform.position.y;
                }
            }

            if (qtd == 0)
            {
                this.gameObject.transform.position =
                    new Vector3(transform.position.x, transform.position.y, transform.position.z);
            }
            else
            {
                this.gameObject.transform.position =
                    new Vector3(axisXPosition/qtd, axisYPosition/qtd, transform.position.z);
            }
        }
    }
}
