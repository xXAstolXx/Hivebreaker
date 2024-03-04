using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RoomManager : MonoBehaviour, ILevelLoadEvent
{
    [SerializeField]
    private GameObject mainRoom;

    [SerializeField]
    private GameObject leftRoom;

    [SerializeField]
    private GameObject rightRoom;

    private void RoomDefaultSetup()
    {
        if (mainRoom != null)
        {
            mainRoom.SetActive(true);
        }

        if (leftRoom != null)
        {
            leftRoom.SetActive(false);
        }

        if (rightRoom != null)
        {
            rightRoom.SetActive(false);
        }
    }

    private void SwitchRooms (GameObject roomOff, GameObject roomOn)
    {
        EnableRoom(roomOff, false);
        EnableRoom(roomOn, true);
    }

    private void EnableRoom(GameObject room, bool value)
    {
        room.SetActive(value);
        room.GetComponent<Room>().OnRoomEnabled();
    }

    public void FromMainToOtherRoom(DoorDirection location)
    {
        Camera.main.GetComponent<MainCamera>().GetAllVirtualCameras();

        switch (location)
        {
            case (DoorDirection.CENTER):
                RoomDefaultSetup();
                break;
            case (DoorDirection.LEFT):
                SwitchRooms(mainRoom, leftRoom);
                break;
            case (DoorDirection.RIGHT):
                SwitchRooms(mainRoom, rightRoom);
                break;
        }
    }

    #region ILevelLoadEvent

    public void OnLevelLoadEvent()
    {
        Game.Instance.roomManagerRef = gameObject.GetComponent<RoomManager>();
        var mainCam = Camera.main.GetComponent<MainCamera>();
        Camera.main.GetComponent<MainCamera>().GetAllVirtualCameras();
        RoomDefaultSetup();
    }

    public void OnLevelUnloadEvent()
    {
        
    }

    public void OnPlayerWasInstanced()
    {

    }

    #endregion ILevelLoadEvent
}
