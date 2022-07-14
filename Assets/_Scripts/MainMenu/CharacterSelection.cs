using System;
using Rewired;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    private Rewired.Player rewiredPlayer;
    
    private int selectedCharacterIndex;

    private bool isPlayer, isLocked;

    private CharacterSelectionManager.SCharacter[] characters;
    
    [Header("Rewired")]
    [SerializeField] private int playerId;

    [Header("UI")]
    [SerializeField] private Image characterImage;
    [SerializeField] private GameObject arrowsParent;
    [SerializeField] private GameObject selectedObject;

    private void Awake()
    {
        isPlayer = false;
    }

    public void Initialize(CharacterSelectionManager.SCharacter[] _characters)
    {
        characters = _characters;
        
        rewiredPlayer = ReInput.players.GetPlayer(playerId);
        
        rewiredPlayer.controllers.maps.SetMapsEnabled(false, "Default");
        rewiredPlayer.controllers.maps.SetMapsEnabled(true, "Menu");
    }

    private void Update()
    {
        if (isPlayer)
        {
            if (rewiredPlayer.GetButtonDown("Select"))
            {
                isLocked = !isLocked;
                arrowsParent.SetActive(!arrowsParent.activeSelf);
                selectedObject.SetActive(!selectedObject.activeSelf);
            }
            
            if (!isLocked)
            {
                if (rewiredPlayer.GetButtonDown("Left"))
                {
                    Left();
                }
                else if (rewiredPlayer.GetButtonDown("Right"))
                {
                    Right();
                }
            }
        }
        else if (rewiredPlayer != null && rewiredPlayer.GetButtonDown("Join"))
        {
            isPlayer = true;
            arrowsParent.SetActive(true);
        }
    }

    private void Left()
    {
        selectedCharacterIndex--;

        if (selectedCharacterIndex < 0)
            selectedCharacterIndex = characters.Length-1;

        characterImage.sprite = characters[selectedCharacterIndex].characterSprite;
    }

    private void Right()
    {
        selectedCharacterIndex++;

        if (selectedCharacterIndex >= characters.Length)
            selectedCharacterIndex = 0;

        characterImage.sprite = characters[selectedCharacterIndex].characterSprite;
    }

    public bool IsReady()
    {
        if (!isPlayer)
            return true;

        if (isPlayer && isLocked)
            return true;

        return false;
    }

    public bool IsPlaying()
    {
        return isPlayer;
    }

    public int CharacterChooseIndex()
    {
        return selectedCharacterIndex;
    }
}
