
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MemData", menuName = "Custom/Mem Data")]
public class MemData : ScriptableObject
{
    public List<Mem> mems;
}


[System.Serializable]
public class Mem
{
    public string name;
    public Sprite sprite;       
    public AudioClip music;
}
