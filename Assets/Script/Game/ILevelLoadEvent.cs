using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelLoadEvent
{
    public void OnLevelLoadEvent();

    public void OnLevelUnloadEvent();

    public void OnPlayerWasInstanced();
}
