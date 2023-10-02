using UnityEngine;
using UnityEngine.Serialization;

namespace RoomSystem
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private Room.Directions direction;
        [FormerlySerializedAs("RedMaterial")] [SerializeField] private Material redMaterial;
        [FormerlySerializedAs("GreenMaterial")] [SerializeField] private Material greenMaterial;
        public Room.Directions GetDoorDirection() => direction;

        public void InitializeDoor(bool state)
        {
            var l_mesh = GetComponent<MeshRenderer>();
            
            if (state)
            {
                l_mesh.material = greenMaterial;
                return;
            }
            
            l_mesh.material = redMaterial;
        }
    }
}

