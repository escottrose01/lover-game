using UnityEngine;

[System.Serializable]
public struct Dialogue
{
    public string name;
    [TextArea]
    public string[] sentences;
}
