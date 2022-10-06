using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IListensToOnDamage
{
    void OnAnyEntityTookDamage(string attackerID, string tankID, float prevHealth, float remainingHealth);
}
