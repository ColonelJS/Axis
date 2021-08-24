using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalLinks : MonoBehaviour
{
	public void OpenLink(string _url)
	{
		Application.OpenURL(_url);
	}
}
