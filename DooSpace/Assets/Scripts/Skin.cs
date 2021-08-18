using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Skin")]
public class Skin : ScriptableObject
{
	/*public enum PartType { TOP, BASE, WINGS }; 
	public enum PartSize { SMALL, MEDIUM, LARGE };*/

	[SerializeField] public string skinName;
	[SerializeField] public Sprite sprite;
	[SerializeField] public Sprite spriteDisplayed;
	[SerializeField] public int index;
	[Space(6)]
	[SerializeField] public SkinManager.ColorName colorName;
	[SerializeField] public SkinManager.PartType partType;
	[SerializeField] public SkinManager.PartSize partSize;
	[Space(6)]
	[SerializeField] public SkinManager.Rarety rarety;

	[HideInInspector] public int id;

	private void Awake()
	{
		id = Random.Range(0, 99999);
	}
}
