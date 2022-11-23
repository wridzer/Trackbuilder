using System.Collections;
using UnityEngine;

[System.Serializable]
public class Settings
{
    public string assetBundlePath { get; set; }
    public string exportPath { get; set; }
    public int gridHeight { get; set; }
    public int gridWidth { get; set; }
    public int gridLength { get; set; }
}