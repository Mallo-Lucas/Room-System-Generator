using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoomSystem
{
    
    public class Room : MonoBehaviour
    {
        [SerializeField] private List<Door> doors;
        [field: SerializeField] public Vector3 RoomSize { get; private set; }
        
        private Door m_entranceDoor;
        private Door m_exitDoor;
        public Door GetEntranceDoor() => m_entranceDoor;
        public Door GetExitDoor() => m_exitDoor;
        
        public enum Directions
        {
            Right,
            Left,
            Top,
            Down
        }
        
        public Directions SetRandomExitDoorDirection()
        {
            var l_doorList = new List<Door>(doors);
            if (m_entranceDoor != default)
            {
                if (l_doorList.Contains(m_entranceDoor))
                    l_doorList.Remove(m_entranceDoor);
            }

            m_exitDoor = l_doorList[Random.Range(0, l_doorList.Count)];
            return m_exitDoor.GetDoorDirection();
        }

        public bool TrySetEntranceDoorDirection(Directions p_entranceDoorDirection)
        {
            foreach (var l_door in doors)
            {
                if (l_door.GetDoorDirection() == p_entranceDoorDirection)
                {
                    m_entranceDoor = l_door;
                    return true;
                }
            }

            return false;
        }
        
        public void InitializeDoors()
        {
            foreach (var l_door in doors)
            {
                if (l_door == m_entranceDoor || l_door == m_exitDoor)
                    l_door.InitializeDoor(true);
                else
                    l_door.InitializeDoor(false);
            }
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, RoomSize);
        }
#endif
    }
}

