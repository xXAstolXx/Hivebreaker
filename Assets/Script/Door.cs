using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public enum DoorDirection
{
    CENTER,
    LEFT,
    RIGHT
}

public class Door : MonoBehaviour
{
    private RoomManager roomManager;

    [SerializeField]
    private DoorDirection doorDirection;

    private Transform spawnPoint;

    [SerializeField]
    private bool isDisabled = false;

    private bool isDoorOpen = false;

    [SerializeField]
    private PlayableDirector playableDirector;

    [SerializeField]
    private GameObject barrierTape;

    private void Awake()
    {
        if (barrierTape != null && isDisabled) 
        { 
            barrierTape.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDisabled) return;

        if (!isDoorOpen) return;

        if (other.GetComponent<Player>() != null)
        {
            var mainCamera = Camera.main.GetComponent<MainCamera>();

            GetChildSpawnPointTransform();
            mainCamera.ChangeCameraTo(doorDirection);
            other.GetComponent<Player>().EnterDoor(spawnPoint);
            roomManager = Game.Instance.roomManagerRef;
            roomManager.FromMainToOtherRoom(doorDirection);
        }
    }

    private void GetChildSpawnPointTransform()
    {
        spawnPoint = gameObject.GetComponentsInChildren<Transform>()[1];
    }

    public void OpenDoor()
    {
        if (isDisabled) return;

        isDoorOpen = true;
        playableDirector.Play();
    }
}
