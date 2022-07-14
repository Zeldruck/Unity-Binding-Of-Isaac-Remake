using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] playerSpawnPoints;
    
    public void SpawnPlayers(int[] indexes, CharacterSelectionManager.SCharacter[] characters)
    {
        List<int> pointsIndex = new List<int>();
        for (int x = 0; x < playerSpawnPoints.Length; x++)
        {
            pointsIndex.Add(x);
        }
        
        for (int i = 0; i < indexes.Length; i++)
        {
            int randIndex = Random.Range(0, pointsIndex.Count);

            GameObject nPlayer = Instantiate(characters[indexes[i]].characterObject, playerSpawnPoints[randIndex].position, Quaternion.identity);
            Player player = nPlayer.GetComponent<Player>();
            player.SetPlayerID(i);

            pointsIndex.RemoveAt(randIndex);
        }
    }
}
