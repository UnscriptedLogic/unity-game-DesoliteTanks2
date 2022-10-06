using System.Collections.Generic;
using UnityEngine;

public static class MathHelper
{
    public static float RandomInRange(Vector2 range) => Random.Range(range.x, range.y);
    public static float RandomBetween(float a = 0f, float b = 100f) => Random.Range(a, b);
    public static int RandomBetween(int a = 0, int b = 100) => Random.Range(a, b);

    public static float FromFloatZeroTo(float value) => Random.Range(0f, value);
    public static int FromIntZeroTo(int value) => Random.Range(0, value);

    public static T RandomFromArray<T>(T[] list, out int index)
    {
        index = FromIntZeroTo(list.Length);
        return list[index];
    }

    public static T RandomFromList<T>(List<T> list, out int index)
    {
        index = FromIntZeroTo(list.Count);
        return list[index];
    }

    public static Vector3 RandomInArea(Vector3 spawnArea)
    {
        float xPos = Random.Range(-spawnArea.x / 2f, spawnArea.x / 2f);
        float yPos = Random.Range(-spawnArea.y / 2f, spawnArea.y / 2f);
        float zPos = Random.Range(-spawnArea.z / 2f, spawnArea.z / 2f);
        return new Vector3(xPos, yPos, zPos);
    }

    public static Vector2 RandomInAreaVector2(Vector2 spawnArea)
    {
        float xPos = Random.Range(-spawnArea.x / 2f, spawnArea.x / 2f);
        float yPos = Random.Range(-spawnArea.y / 2f, spawnArea.y / 2f);
        return new Vector2(xPos, yPos);
    }

    public static Vector3 PointAtCircumferenceXZ(Vector3 center, float radius)
    {
        float theta = FromFloatZeroTo(360);
        float opposite = radius * Mathf.Sin(theta);
        float adjacent = radius * Mathf.Cos(theta);
        return center + new Vector3(adjacent, 0f, opposite);
    }

    public static Vector3 RandomVectorDirectionAroundY()
    {
        int index = FromIntZeroTo(4);
        if (index == 0)
        {
            return Vector3.forward;
        }
        else if (index == 1)
        {
            return Vector3.back;
        }
        else if (index == 3)
        {
            return Vector3.left;
        }
        else
        {
            return Vector3.right;
        }
    }

    public static Vector3 OfVectorDirectionAny()
    {
        int index = FromIntZeroTo(6);
        if (index == 0)
        {
            return Vector3.forward;
        }
        else if (index == 1)
        {
            return Vector3.back;
        }
        else if (index == 3)
        {
            return Vector3.left;
        }
        else if (index == 4)
        {
            return Vector3.right;
        }
        else if (index == 5)
        {
            return Vector3.up;
        }
        else
        {
            return Vector3.down;
        }
    }

    //My glorious tier chance, number line, random index generator
    public static int RandomIndex<T>(T[] list, float[] chances)
    {
        float[] tierChances = new float[list.Length];
        float prevChance = 0f;
        //makes tierChances look like a number line
        //0--[chance 1]--30--[chance 2]--70--[chance 3]--100
        for (int i = 0; i < list.Length; i++)
        {
            tierChances[i] = prevChance + chances[i];
            prevChance = tierChances[i];
        }

        //simple randomizes a number and then check the ranges
        int randomTier = UnityEngine.Random.Range(0, 100);
        for (int i = 0; i < tierChances.Length; i++)
        {
            float highNum = i == tierChances.Length - 1 ? 100 : tierChances[i];
            float lowNum = i == 0 ? 0 : tierChances[i - 1];
            if (randomTier > lowNum && randomTier < highNum)
            {
                return i;
            }
        }

        return 0;
    }
}