using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    public delegate void OnSwapEnd();
    private event OnSwapEnd onSwapEnd;

    private Room actualRoom, lastRoom;

    [SerializeField] private float swapSpeed;
    
    private void Awake()
    {
        instance = this;
    }

    public void SwapRoom(Room _newRoom, OnSwapEnd _onSwapEnd)
    {
        if (actualRoom == _newRoom)
            return;

        if (onSwapEnd != null)
        {
            StopAllCoroutines();
        }

        onSwapEnd = _onSwapEnd;
        
        if (actualRoom == null)
        {
            actualRoom = _newRoom;
            transform.position = new Vector3(actualRoom.transform.position.x, actualRoom.transform.position.y, transform.position.z);
        }
        else
        {
            lastRoom = actualRoom;
            actualRoom = _newRoom;
            
            StartCoroutine(LerpCameraNewRoom());
        }
    }

    private IEnumerator LerpCameraNewRoom()
    {
        float t = 0f;
        
        
        while (t < 1f)
        {
            t += Time.deltaTime * swapSpeed;

            transform.position =
                Vector3.Slerp(
                    new Vector3(lastRoom.transform.position.x, lastRoom.transform.position.y, transform.position.z),
                    new Vector3(actualRoom.transform.position.x, actualRoom.transform.position.y, transform.position.z),
                    t);
            
            yield return null;
        }
        
        onSwapEnd?.Invoke();
    }
}
