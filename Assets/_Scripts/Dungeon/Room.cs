using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

public class Room : MonoBehaviour
{
    #region RoomSpawn
    
    private RoomSpawner[] spawners;
    private Transform initialRoom;

    private Door[] doors;
    
    public SDoors ownDoors;
    
    [Header("Init Part")]
    [SerializeField] private Transform spawnPointsParent;
    [SerializeField] private Transform doorsParent;

    private void Awake()
    {
        spawners = new RoomSpawner[spawnPointsParent.childCount];

        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i] = spawnPointsParent.GetChild(i).GetComponent<RoomSpawner>();
        }

        doors = new Door[doorsParent.childCount];
        
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i] = doorsParent.GetChild(i).GetComponent<Door>();
        }
        
        FindOwnDoors();
    }

    private void FindOwnDoors()
    {
        ownDoors = new SDoors();
        
        for (int i = 0; i < doors.Length; i++)
        {
            int xSign = doors[i].transform.localPosition.x > 1f ? 1 : doors[i].transform.localPosition.x < -1f ? -1 : 0;

            if (xSign > 0)
            {
                ownDoors.R = true;
                doors[i].SetDoorDirection(1);
                continue;
            }
            
            if (xSign < 0)
            {
                ownDoors.L = true;
                doors[i].SetDoorDirection(3);
                continue;
            }
            
            int ySign = doors[i].transform.localPosition.y > 1f ? 1 : doors[i].transform.localPosition.y < -1f ? -1 : 0;

            if (ySign > 0)
            {
                ownDoors.T = true;
                doors[i].SetDoorDirection(0);
                continue;
            }
            
            if (ySign < 0)
            {
                ownDoors.B = true;
                doors[i].SetDoorDirection(2);
            }
        }
    }
    
    public void InitializeDistances(Transform _initialRoom, int _floor)
    {
        initialRoom = _initialRoom;
        floor = _floor;
        
        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i].CalculateDistance(initialRoom);
        }
    }

    public void InitializeBossRoom(GameObject _boss)
    {
        isBossRoom = true;
        boss = _boss;
    }

    public RoomSpawner[] GetSpawnerArray()
    {
        return spawners;
    }

    public void InitializeDoors()
    {
        for (int i = 0; i < spawners.Length; i++)
        {
            Collider2D col = Physics2D.OverlapBox(spawners[i].transform.position, Vector2.one, 0f);

            if (col != null)
            {
                doors[i].SetDoorNextRoom(col.GetComponent<Room>());
                doors[i].DoorState(!isClear);
            }
        }
    }
    
    #endregion

    #region GameRoom

    private List<Enemy> ennemies;

    private int floor;
    
    private bool isBossRoom;
    private GameObject boss;
    
    private bool isCombatRoom;
    private bool isFrozen;
    private bool isClear;
    
    [Header("Game Part")]
    [SerializeField] private Transform monsterSpawnPointsParent;
    [SerializeField] private GameObject[] monsterPrefabs;
    [Space]
    [SerializeField] private Transform lootSpawnPointsParent;
    [SerializeField] private GameObject[] lootPrefabs;

    public void Spawn()
    {
        isFrozen = true;

        if (monsterSpawnPointsParent != null)
            isCombatRoom = monsterSpawnPointsParent.childCount > 0;
        else
            isCombatRoom = false;
        
        if (isCombatRoom)
        {
            isCombatRoom = true;
        }
        else
        {
            isClear = true;
        }
        
        SpawnEntities();
    }

    private void SpawnEntities()
    {
        if (isCombatRoom)
        {
            ennemies = new List<Enemy>();
            
            for (int i = 0; i < monsterSpawnPointsParent.childCount; i++)
            {
                int randEnemy = Random.Range(0, monsterPrefabs.Length);
                GameObject nEnemy = Instantiate(monsterPrefabs[randEnemy],
                    monsterSpawnPointsParent.GetChild(i).position, Quaternion.identity);
                nEnemy.transform.SetParent(transform);
                Enemy enemy = nEnemy.GetComponent<Enemy>();
                enemy.Initialize(() => EnemyDeath(enemy));
                
                ennemies.Add(enemy);

                nEnemy.SetActive(false);
            }
        }

        if (lootSpawnPointsParent != null && lootSpawnPointsParent.childCount > 0)
        {
            for (int i = 0; i < lootSpawnPointsParent.childCount; i++)
            {
                int rand = Random.Range(0, lootPrefabs.Length);
                GameObject nLoot = Instantiate(lootPrefabs[rand],
                    lootSpawnPointsParent.GetChild(i).position, Quaternion.identity);
            }
        }
    }

    public void RoomEntitiesState(bool _isShowing)
    {
        if (ennemies == null || ennemies.Count <= 0) 
            return;
        
        foreach (Enemy enemy in ennemies)
        {
            enemy.gameObject.SetActive(_isShowing);
        }
    }

    public void CloseOtherDoors(Door _doorOpen)
    {
        foreach (Door door in doors)
        {
            if (door != _doorOpen)
            {
                door.DoorState(true);
            }
        }
    }

    public void SwapToThisRoom()
    {
        isFrozen = false;

        if (isClear)
        {
            foreach (Door door in doors)
            {
                door.DoorState(false);
            }
        }
    }

    private void EnemyDeath(Enemy _enemy)
    {
        ennemies.Remove(_enemy);

        if (ennemies.Count == 0)
        {
            isClear = true;
            SwapToThisRoom();
        }
    }

    #endregion
}

[Serializable]
public struct SDoors
{
    public bool T;
    public bool R;
    public bool B;
    public bool L;
}