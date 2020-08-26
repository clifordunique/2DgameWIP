using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    private int level;

    public Level()
    {
        level = 1;
    }

    public int GetLevel()
    {
        return level;
    }

    public void LevelUp()
    {
        level++;
    }
    public void LevelUp(int levels)
    {
        level += levels;
    }
    public void Reset()
    {
        level = 1;
    }
}
