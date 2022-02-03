using UnityEngine;

namespace Project.Player.Scripts
{
    public class PlayerInfo : MonoBehaviour
    {
        [SerializeField] private float healthPoint = 100.0f;
        [SerializeField] private bool isAlive = true;

        public void SetHealthPoint(float damage)
        {
            this.healthPoint += damage;
            
            if (this.healthPoint < 0)
                this.healthPoint = 0;
        }
        
        public float GetHealthPoint()
        {
            return (this.healthPoint);
        }
        
        public void SetIsAlive(bool value)
        {
            this.isAlive = value;
        }
        
        public bool GetIsAlive()
        {
            return(this.healthPoint>0);
        }
    }
}
