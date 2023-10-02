using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities;
using Unity.Mathematics;
using UnityEngine;

namespace RoomSystem
{
    public class RoomSystemGenerator : MonoBehaviour
    {
        [SerializeField] private RoomGeneratorData data;
        [SerializeField] private Transform firstRoomSpawnPosition;

        private void Awake()
        {
            GenerateRooms();
        }

        private async void GenerateRooms()
        {
            var l_list = new List<Room>();
            var l_listPrefabs = new List<Room>();
            var l_prevRoom = Instantiate(data.firstRoom);
            l_prevRoom.transform.position = firstRoomSpawnPosition.position;
            l_list.Add(l_prevRoom);

            foreach (var l_roomId in data.roomsIdList)
            {
                var l_roomPrefab = data.GetPoolRoomById(l_roomId).GetRandomItemFromPool();
                var l_watchDog = 10000;

                while (l_listPrefabs.Contains(l_roomPrefab) && l_watchDog > 0)
                {
                    l_watchDog--;
                    l_roomPrefab = data.GetPoolRoomById(l_roomId).GetRandomItemFromPool();
                }

                l_listPrefabs.Add(l_roomPrefab);

                await InstanceRoom(l_prevRoom, l_roomPrefab, out var l_newRoom);
                await Task.Delay(1000);
                await ConnectRooms(l_prevRoom.GetExitDoor(), l_newRoom.GetEntranceDoor());
                await Task.Delay(1000);
                l_prevRoom = l_newRoom;
                l_list.Add(l_prevRoom);
            }
            
            foreach (var l_room in l_list)
                l_room.InitializeDoors();
        }

        private static bool CheckIsHorizontal(Room.Directions p_direction)
        {
            return p_direction is Room.Directions.Right or Room.Directions.Left;
        }

        private static Room.Directions GetOppositeDirection(Room.Directions p_dir)
        {
            return p_dir switch
            {
                Room.Directions.Right => Room.Directions.Left,
                Room.Directions.Left => Room.Directions.Right,
                Room.Directions.Top => Room.Directions.Down,
                Room.Directions.Down => Room.Directions.Top,
                _ => throw new ArgumentOutOfRangeException(nameof(p_dir), p_dir, null)
            };
        }

        private Vector3 GetPositionNewRoom(Room p_previousRoom, Room p_newRoom)
        {
            var l_directionExitDoor = p_previousRoom.SetRandomExitDoorDirection();
            var l_directionEntranceDoor = GetOppositeDirection(l_directionExitDoor);
            var l_isPossible = p_newRoom.TrySetEntranceDoorDirection(l_directionEntranceDoor);

            while (!l_isPossible)
            {
                l_directionExitDoor = p_previousRoom.SetRandomExitDoorDirection();
                l_directionEntranceDoor = GetOppositeDirection(l_directionExitDoor);
                l_isPossible = p_newRoom.TrySetEntranceDoorDirection(l_directionEntranceDoor);
            }
            
            var l_previousRoomSize = (p_previousRoom.RoomSize / 2).XOZ();
            var l_newRoomSize = (p_newRoom.RoomSize / 2).XOZ();

            var l_distanceToMove = l_previousRoomSize + l_newRoomSize + data.RoomsOffset;
            l_distanceToMove = new Vector3(Mathf.Abs(l_distanceToMove.x), Mathf.Abs(l_distanceToMove.y),
                Mathf.Abs(l_distanceToMove.z));

            Vector3 l_position;

            if (CheckIsHorizontal(l_directionEntranceDoor))
            {
                l_position = l_directionExitDoor == Room.Directions.Right
                    ? (p_previousRoom.transform.position + Vector3.right * l_distanceToMove.x).XOZ()
                    : (p_previousRoom.transform.position + Vector3.left * l_distanceToMove.x).XOZ();
            }
            else
            {
                l_position = l_directionExitDoor == Room.Directions.Top
                    ? (p_previousRoom.transform.position + Vector3.forward * l_distanceToMove.z).XOZ()
                    : (p_previousRoom.transform.position + (-Vector3.forward * l_distanceToMove.z)).XOZ();
            }

            var l_watchDog = 100;
            while (Physics.CheckBox(l_position, p_newRoom.RoomSize / 2) && l_watchDog > 0)
            {
                l_watchDog--;

                l_directionExitDoor = p_previousRoom.SetRandomExitDoorDirection();
                l_directionEntranceDoor = GetOppositeDirection(l_directionExitDoor);
                l_isPossible = p_newRoom.TrySetEntranceDoorDirection(l_directionEntranceDoor);

                while (!l_isPossible)
                {
                    l_directionExitDoor = p_previousRoom.SetRandomExitDoorDirection();
                    l_directionEntranceDoor = GetOppositeDirection(l_directionExitDoor);
                    l_isPossible = p_newRoom.TrySetEntranceDoorDirection(l_directionEntranceDoor);
                }

                if (CheckIsHorizontal(l_directionEntranceDoor))
                {
                    l_position = l_directionExitDoor == Room.Directions.Right
                        ? (p_previousRoom.transform.position + Vector3.right * l_distanceToMove.x).XOZ()
                        : (p_previousRoom.transform.position + Vector3.left * l_distanceToMove.x).XOZ();
                }
                else
                {
                    l_position = l_directionExitDoor == Room.Directions.Top
                        ? (p_previousRoom.transform.position + Vector3.forward * l_distanceToMove.z).XOZ()
                        : (p_previousRoom.transform.position + (-Vector3.forward * l_distanceToMove.z)).XOZ();
                }
            }

            return l_position;
        }

        private static Vector3 GetAlignDoors(Door p_exitDoor, Door p_entranceDoor, Vector3 p_newRoomPosition)
        {
            var l_distance = p_exitDoor.transform.position - p_entranceDoor.transform.position;

            var l_distanceZ = math.abs(l_distance.z);
            var l_distanceX = math.abs(l_distance.x);
            switch (p_exitDoor.GetDoorDirection())
            {
                case Room.Directions.Right:
                    if (p_exitDoor.transform.position.z > p_entranceDoor.transform.position.z)
                        p_newRoomPosition -= p_exitDoor.transform.right.normalized * l_distanceZ;
                    else
                        p_newRoomPosition += p_exitDoor.transform.right.normalized * l_distanceZ;

                    return p_newRoomPosition;

                case Room.Directions.Left:
                    if (p_exitDoor.transform.position.z > p_entranceDoor.transform.position.z)
                        p_newRoomPosition += p_exitDoor.transform.right.normalized * l_distanceZ;
                    else
                        p_newRoomPosition -= p_exitDoor.transform.right.normalized * l_distanceZ;
                    return p_newRoomPosition;


                case Room.Directions.Top:
                    if (p_exitDoor.transform.position.x > p_entranceDoor.transform.position.x)
                        p_newRoomPosition += p_exitDoor.transform.right.normalized * l_distanceX;
                    else
                        p_newRoomPosition -= p_exitDoor.transform.right.normalized * l_distanceX;

                    return p_newRoomPosition;


                case Room.Directions.Down:
                    if (p_exitDoor.transform.position.x > p_entranceDoor.transform.position.x)
                        p_newRoomPosition -= p_exitDoor.transform.right.normalized * l_distanceX;
                    else
                        p_newRoomPosition += p_exitDoor.transform.right.normalized * l_distanceX;

                    return p_newRoomPosition;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Task InstanceRoom(Room p_previousRoom, Room p_newRoomPrefab, out Room p_newRoom)
        {
            p_newRoom = Instantiate(p_newRoomPrefab);

            var l_position = GetPositionNewRoom(p_previousRoom, p_newRoom);
            p_newRoom.transform.position = l_position;

            l_position = GetAlignDoors(p_previousRoom.GetExitDoor(), p_newRoom.GetEntranceDoor(), l_position);
            p_newRoom.transform.position = l_position;

            return Task.CompletedTask;
        }

        private async Task ConnectRooms(Door p_exitDoor, Door p_entranceDoor)
        {
            await SpawnHallways(p_exitDoor.transform.position, p_entranceDoor.transform.position);
        }

        private Task SpawnHallways(Vector3 p_start, Vector3 p_end)
        {
            var l_direction = p_end - p_start;
            var l_distance = l_direction.magnitude;
            var l_normalizedDirection = l_direction.normalized;

            var l_numberOfHallways = Mathf.CeilToInt(l_distance) - 1;

            p_start += l_normalizedDirection * 0.5f;
            
            for (var l_i = 0; l_i < l_numberOfHallways; l_i++)
            {
                var l_position = p_start + l_normalizedDirection * l_i;
                l_position.y = data.HallwayPositionY;
                var l_hallway = Instantiate(data.HallwayPrefab);
                l_hallway.transform.position = l_position;
                l_hallway.transform.forward = l_normalizedDirection;
                l_hallway.transform.parent = transform;
            }
            
            return Task.CompletedTask;
        }
    }
}
