using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Skin")]
public class Skin : ScriptableObject
{
    [SerializeField] private string skinName;
    [SerializeField] public int index;
    [SerializeField] private Sprite sprite;
    //public int id = Random.Range(0, 99999);
}
