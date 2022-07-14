using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionManager : MonoBehaviour
{
    [Serializable]
    public struct SCharacter
    {
        public GameObject characterObject;
        public Sprite characterSprite;
    }
    
    private SCharacter[] GetCharactersInfos()
    {
        return characters;
    }

    [SerializeField] private SCharacter[] characters;
    [SerializeField] private CharacterSelection[] selections;
    
    private int[] characterChoosed;

    private void Start()
    {
        foreach (CharacterSelection selection in selections)
        {
            selection.Initialize(characters);
        }
    }

    private void Update()
    {
        for (int i = 0; i < ReInput.players.playerCount; i++)
        {
            bool isReady = false;
            if (ReInput.players.GetPlayer(i).GetButtonDown("Join"))
            {
                foreach (CharacterSelection selection in selections)
                {
                    isReady = selection.IsReady() && (selection.IsPlaying() || isReady);

                    if (!isReady)
                        break;
                }

                if (isReady)
                {
                    StartGame();
                }

                break;
            }
        }
    }

    private void StartGame()
    {
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);

        int nbPlayer = 0;

        foreach (CharacterSelection selection in selections)
        {
            if (selection.IsPlaying())
                nbPlayer++;
        }
        
        characterChoosed = new int[nbPlayer];

        for (int i = 0; i < characterChoosed.Length; i++)
        {
            characterChoosed[i] = selections[i].CharacterChooseIndex();
        }
        
        StartCoroutine(LoadGame());
    }

    private IEnumerator LoadGame()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        op.allowSceneActivation = false;
        op.completed += InitializeGame;

        while (op.progress < 0.9f)
        {
            yield return null;
        }

        op.allowSceneActivation = true;
    }

    private void InitializeGame(AsyncOperation _op)
    {
        _op.completed -= InitializeGame;

        int[] nIndexes = characterChoosed;
        SCharacter[] nCharacters = characters;

        RoomSpawnerManager sp = FindObjectOfType<RoomSpawnerManager>();
        PlayerSpawner ps = FindObjectOfType<PlayerSpawner>();
        sp.Initialize(() => ps.SpawnPlayers(nIndexes, nCharacters));
        
        Destroy(gameObject);
    }
}
