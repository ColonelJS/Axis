using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Skin")]
public class Skin : ScriptableObject
{
	[SerializeField] public string skinName;
	[SerializeField] public string skinNameFr;
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
	[HideInInspector] public bool isNew = false;
	[HideInInspector] public Color rarityColor;

	private void Awake()
	{
		id = Random.Range(0, 99999);
    }
}
