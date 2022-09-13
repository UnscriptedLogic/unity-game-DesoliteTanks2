using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBlockUpdateable
{
    void OnBlockDestroyed();

    void OnBlockCreated();
}
