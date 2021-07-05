using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameArchive_Mode1
{
    public string usingtime;
    public int[][] blocks;
    public int score;
    public bool mute;
}

[Serializable] 
public class GameArchive_Mode2
{
    [Serializable]
    public struct PositionVector3
    {
        public float x;
        public float y;
        public float z;
    }
    public Dictionary<PositionVector3, int> entityList;
    public int score;
    public bool mute;
}
