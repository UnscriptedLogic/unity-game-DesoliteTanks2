using LevelManagement;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
    //IDs
    public const string PROJECTILE_ID = "projectile";
    public const string TANK_ID = "tank";
    public const string PLAYER_ID = "player";
    public const string ITEM_ID = "item";

    //Level Transferring
    public static bool hasLevelDetails => levelDetails != null;
    public static LevelDetailsSO levelDetails;
    public const string FALLBACK_MAP = "Lanes";
}
