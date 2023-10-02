using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;

namespace RoomSystem
{
    [CreateAssetMenu(menuName = "RoomSystem/RoomsPool")]
    public class RoomsPool : ScriptableObject
    {
        [field: SerializeField] public RoomId RoomsId { get; private set; }
        [SerializeField] private List<Room> rooms;
        [SerializeField] private List<float> roomsChances;
        private RouletteWheel<Room> m_roomsWheel;

        public Room GetRandomItemFromPool()
        {
            m_roomsWheel ??= new RouletteWheel<Room>(rooms, roomsChances);

            return m_roomsWheel.RunWithCached();
        }


#if UNITY_EDITOR
        [SerializeField] public List<float> chancePercentage = new();
        
        [ContextMenu("Check Compatibility")]
        public void CheckStorableItemAndSize()
        {
            var l_newList = rooms.Distinct().ToList();
            rooms = l_newList;

            if (rooms.Count != roomsChances.Count)
            {
                while (rooms.Count > roomsChances.Count)
                {
                    roomsChances.Add(0);
                }

                while (rooms.Count < roomsChances.Count)
                {
                    roomsChances.RemoveAt(roomsChances.Count - 1);
                }
            }

            CheckItemPercentage();
        }

        [ContextMenu("Show item percentage")]
        public void CheckItemPercentage()
        {
            var l_totalChance = roomsChances.Sum();

            chancePercentage.Clear();

            for (int i = 0; i < roomsChances.Count; i++)
            {
                chancePercentage.Add((roomsChances[i] / l_totalChance) * 100);
            }
        }

        [ContextMenu("Clear percentage")]
        public void ClearPercentage()
        {
            chancePercentage.Clear();
        }
#endif

    }
}