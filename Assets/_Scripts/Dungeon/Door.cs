using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private SpriteRenderer sr;
    private Collider2D col;
    
    private Room actualRoom;
    private Room nextRoom;

    private List<Player> players;

    private uint direction;
    private bool isClosed;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        actualRoom = transform.parent.parent.GetComponent<Room>();
        
        players = new List<Player>();
    }

    public void SetDoorDirection(uint _direction)
    {
        direction = _direction;
    }

    public void SetDoorNextRoom(Room _nextRoom)
    {
        nextRoom = _nextRoom;
    }

    public void DoorState(bool _isClosed)
    {
        sr.enabled = _isClosed;
        col.isTrigger = !_isClosed;
        
        if (isClosed == _isClosed)
            // Sound
            return;
        
        isClosed = _isClosed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isClosed || nextRoom == null || !other.CompareTag("Player"))
            return;

        int nbPlayers = FindObjectsOfType<Player>().Length;

        Player player = other.GetComponent<Player>();
        if (player != null && !players.Contains(player))
            players.Add(player);
        
        // freeze player
        player.FreezeState(true);

        Vector2 roomPos = Vector2.zero;
        
        switch (direction)
        {
            case 0: // TOP
                roomPos = new Vector2(players.Count == 1 ? -0.6f : 0.6f, 7.55f);
                break;
            
            case 1: // RIGHT
                roomPos = new Vector2(11.55f, players.Count == 1 ? -0.6f : 0.6f);
                break;
            
            case 2: // BOTTOM
                roomPos = new Vector2(players.Count == 1 ? -0.6f : 0.6f, -7.55f);
                break;
            
            case 3: // LEFT
                roomPos = new Vector2(-11.55f, players.Count == 1 ? -0.6f : 0.6f);
                break;
        }

        player.transform.position = actualRoom.transform.position + (Vector3)roomPos;

        if (players.Count >= nbPlayers)
        {
            nextRoom.RoomEntitiesState(true);
            
            CameraManager.instance.SwapRoom(nextRoom, (() =>
            {
                nextRoom.SwapToThisRoom();

                foreach (Player oPlayer in players)
                {
                    oPlayer.FreezeState(false);
                }

                players = new List<Player>();
            }));
        }
    }
}
