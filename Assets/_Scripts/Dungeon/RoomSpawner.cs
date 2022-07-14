using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

public class RoomSpawner : MonoBehaviour, IHeapItem<RoomSpawner>
{
    private bool hasSpawned;

    private float distanceFromCenter;

    private SDoors roomNeeded;

    private void FindDoors(bool _hasToCloseRoom)
    {
        roomNeeded = new SDoors();

        Collider2D col = Physics2D.OverlapBox(transform.position + new Vector3(-19f, 0f, 0f), Vector2.one, 0f);
        Room room = col == null ? null : col.GetComponent<Room>();
        
        if (room != null && room.ownDoors.R)
        {
            roomNeeded.L = true;
        }
        else if (room == null && !_hasToCloseRoom)
        {
            roomNeeded.L = Random.Range(0, 2) != 0;
        }
        
        col = Physics2D.OverlapBox(transform.position + new Vector3(19f, 0f, 0f), Vector2.one, 0f);
        room = col == null ? null : col.GetComponent<Room>();
        
        if (room != null && room.ownDoors.L)
        {
            roomNeeded.R = true;
        }
        else if (room == null && !_hasToCloseRoom)
        {
            roomNeeded.R = Random.Range(0, 2) != 0;
        }
        
        col = Physics2D.OverlapBox(transform.position + new Vector3(0f, -11f, 0f), Vector2.one, 0f);
        room = col == null ? null : col.GetComponent<Room>();

        if (room != null && room.ownDoors.T)
        {
            roomNeeded.B = true;
        }
        else if (room == null && !_hasToCloseRoom)
        {
            roomNeeded.B = Random.Range(0, 2) != 0;
        }
        
        col = Physics2D.OverlapBox(transform.position + new Vector3(0f, 11f, 0f), Vector2.one, 0f);
        room = col == null ? null : col.GetComponent<Room>();
        
        if (room != null && room.ownDoors.B)
        {
            roomNeeded.T = true;
        }
        else if (room == null && !_hasToCloseRoom)
        {
            roomNeeded.T = Random.Range(0, 2) != 0;
        }
    }

    public Room SpawnRoom(RoomTemplates _templates, bool _hasToCloseRoom)
    {
        if (hasSpawned || Physics2D.OverlapBox(transform.position, Vector2.one, 0f))
            return null;

        FindDoors(_hasToCloseRoom);

        GameObject rObject = _templates.GetGoodRoom(roomNeeded);

        if (rObject != null)
            rObject = Instantiate(rObject, transform.position, Quaternion.identity);

        hasSpawned = true;

        if (rObject != null)
        {
            Room room = rObject.GetComponent<Room>();
            return room;
        }

        return null;
    }

    public void CalculateDistance(Transform _initialRoom)
    {
        distanceFromCenter = Vector2.Distance(transform.position, _initialRoom.position);
    }

    public int CompareTo(RoomSpawner _toCompare)
    {
        int compare = distanceFromCenter.CompareTo(_toCompare.distanceFromCenter);

        return -compare;
    }

    private int heapIndex;
    
    public int HeapIndex
    {
        get { return heapIndex;}
        set { heapIndex = value; }
    }
}