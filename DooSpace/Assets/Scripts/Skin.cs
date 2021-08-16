using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Skin")]
public class Skin : ScriptableObject
{
    [SerializeField] public string skinName;
    [SerializeField] public int index;
    [SerializeField] public Sprite sprite;
	[SerializeField] public int rarety;
	public int id;

	private void Awake()
	{
		id = Random.Range(0, 99999);
	}
}
