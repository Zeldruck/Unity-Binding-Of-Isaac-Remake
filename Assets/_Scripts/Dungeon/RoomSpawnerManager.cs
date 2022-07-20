using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class RoomSpawnerManager : MonoBehaviour
{
    public static RoomSpawnerManager instance;
    
    private RoomTemplates roomTemplates;
    
    private Heap<RoomSpawner> roomsToSpawn;

    public delegate void OnSpawnFinish();
    private OnSpawnFinish onSpawnFinish;
    
    private int nbRTS;

    [SerializeField] private GameObject initialRoom;
    [SerializeField] private int nbRoomToSpawn;

    public int floor = 0;

    private void Awake()
    {
        instance = this;
        
        roomTemplates = GetComponent<RoomTemplates>();

        roomsToSpawn = new Heap<RoomSpawner>(nbRoomToSpawn);

        nbRTS = nbRoomToSpawn;
    }
    
    public void Initialize(OnSpawnFinish _onSpawnFinish)
    {
        onSpawnFinish = _onSpawnFinish;
        
        Room initialRoomInit = initialRoom.GetComponent<Room>();
        initialRoomInit.InitializeDistances(initialRoom.transform, floor);

        RoomSpawner[] roomsInit = initialRoomInit.GetSpawnerArray();
        for (int i = 0; i < roomsInit.Length; i++)
        {
            roomsToSpawn.Add(roomsInit[i]);
            nbRTS--;
        }
        
        StartCoroutine(SpawnRooms(initialRoomInit));
    }

    private IEnumerator SpawnRooms(Room _initial)
    {
        List<Room> roomsToInitialize = new List<Room>();
        
        roomsToInitialize.Add(_initial);

        while(roomsToSpawn.Count > 0)
        {
            Room room = roomsToSpawn.RemoveFirst().SpawnRoom(roomTemplates, nbRTS < 0);

            if (room != null)
            {
                room.InitializeDistances(initialRoom.transform, floor);
                
                RoomSpawner[] roomSpawners = room.GetSpawnerArray();

                for (int i = 0; i < roomSpawners.Length; i++)
                {
                    if (!roomsToSpawn.Contains(roomSpawners[i]))
                    {
                        roomsToSpawn.Add(roomSpawners[i]);
                        nbRTS--;
                    }
                }

                if (roomsToSpawn.Count == 0)
                {
                    // Boss room
                    room.InitializeBossRoom(roomTemplates.bosses[floor >= roomTemplates.bosses.Length ? roomTemplates.bosses.Length-1 : floor]);
                }
                
                roomsToInitialize.Add(room);
            }

            yield return null;
        }

        while (roomsToInitialize.Count > 0)
        {
            roomsToInitialize[0].Spawn();
            roomsToInitialize[0].InitializeDoors();
            roomsToInitialize.RemoveAt(0);

            yield return null;
        }
        
        //TO MOVE SOMEWHERE ELSE
        CameraManager.instance.SwapRoom(_initial, null);
        
        onSpawnFinish?.Invoke();
    }
}
