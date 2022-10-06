using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IListensToGameState
{
    void OnGameStateChanged(bool won);
}
