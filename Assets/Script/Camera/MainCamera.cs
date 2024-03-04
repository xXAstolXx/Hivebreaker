using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour, ILevelLoadEvent
{
    private CinemachineVirtualCamera centerRoomCamera;

    private CinemachineVirtualCamera leftRoomCamera;

    private CinemachineVirtualCamera rightRoomCamera;

    public void GetAllVirtualCameras()
    {
        foreach (CinemachineVirtualCamera cam in FindObjectsOfType(typeof(CinemachineVirtualCamera))) 
        {
            if (cam.Priority == 10)
            {
                //if (centerRoomCamera != null) return; 
                centerRoomCamera = cam;
            }

            if (cam.Priority == 5) 
            {
                //if (leftRoomCamera != null) return;
                leftRoomCamera = cam;
            }

            if (cam.Priority == 1)
            {
                //if (rightRoomCamera != null) return;
                rightRoomCamera = cam;
            }
        }
    }

    private void ClearAllVirtualCameras()
    {
        centerRoomCamera = null;
        leftRoomCamera = null;
        rightRoomCamera = null;
    }

    public void ChangeCameraTo(DoorDirection direction)
    {
        DisableCamera();
        switch (direction) 
        {
            case (DoorDirection.CENTER):
                centerRoomCamera.enabled = true;
                break;
            case (DoorDirection.LEFT):
                leftRoomCamera.enabled = true;
                break;
            case (DoorDirection.RIGHT):
                rightRoomCamera.enabled = true;
                break;
        }
    }

    private void DisableCamera()
    {
        centerRoomCamera.enabled = false;
        leftRoomCamera.enabled = false;
        rightRoomCamera.enabled = false;
    }

    #region ILevelLoadEvent

    public void OnLevelLoadEvent()
    {

    }

    public void OnLevelUnloadEvent()
    {
        ClearAllVirtualCameras();
    }

    public void OnPlayerWasInstanced()
    {

    }

    #endregion ILevelLoadEvent
}
