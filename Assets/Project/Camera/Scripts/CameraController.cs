using System;
using System.Collections.Generic;
using Project.Online.Scripts;
using UnityEngine;

namespace Project.Camera.Scripts
{
    public class CameraController : MonoBehaviour
    {
        private void Update()
        {
            List<GameObject> players = RoomController.Room.GetPlayers();
            
            if (players.Count == 1)
            {
                this.gameObject.transform.position =
                    new Vector3(players[0].transform.position.x, transform.position.y, transform.position.z);
            }
            else
            {
                float axisPosition = 0.0f;
                
                foreach (var element in players)
                {
                    axisPosition += element.transform.position.x;
                }
                
                this.gameObject.transform.position =
                    new Vector3(axisPosition/players.Count, transform.position.y, transform.position.z);
            }
        }
    }
}
