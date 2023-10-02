using System.Collections.Generic;
using UnityEngine;

namespace RoomSystem
{
    [CreateAssetMenu(menuName = "RoomSystem/RoomGeneratorData")]
    public class RoomGeneratorData : ScriptableObject
    {
        [field: SerializeField] public Vector3 RoomsOffset {get; private set; }
        [field: SerializeField] public float HallwayPositionY {get; private set; }
        [field: SerializeField] public GameObject HallwayPrefab {get; private set; }
        [field: SerializeField] public Room firstRoom;
        [field: SerializeField] public List<RoomId> roomsIdList;
        [field: SerializeField] public List<RoomsPool> RoomsPools {get; private set; }
        private Dictionary<RoomId, RoomsPool> _dictionary;

        public RoomsPool GetPoolRoomById(RoomId roomId)
        {
            if (_dictionary != default)
                return _dictionary[roomId];

            _dictionary = new Dictionary<RoomId, RoomsPool>();
            
            foreach (var l_roomPool in RoomsPools)
            {
                if (!_dictionary.TryAdd(l_roomPool.RoomsId, l_roomPool))
                {
                    Debug.LogError($"Could not add the following element: {l_roomPool} RoomId: {l_roomPool.RoomsId}");
                }
            }

            return _dictionary[roomId];
        }
    }
    
    public enum RoomId
    {
        Enemy,
        Boss,
    }
}
