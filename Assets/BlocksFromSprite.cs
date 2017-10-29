using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BlocksFromSprite : MonoBehaviour {

	SpriteRenderer rend;
	public GameObject block;

	// Use this for initialization
	void Start () {
		rend = GetComponent<SpriteRenderer>();
		Texture2D tex = rend.sprite.texture;
		
		for (int i = 0; i < tex.width; i++)
		{
			for(int j = 0; j < tex.height; j++)
			{
				Color color = tex.GetPixel(i, j);
				if (color.a == 0)
					continue;

				GameObject go = Instantiate(block, new Vector3(i, j, 0), Quaternion.identity);

				string colorString = ColorUtility.ToHtmlStringRGB(color);

				Material material = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/GodsColors/" + colorString + ".mat");
				if (material == null)
				{
					material = new Material(Shader.Find("Standard"));
					material.color = color;
					AssetDatabase.CreateAsset(material, "Assets/Materials/GodsColors/" + colorString + ".mat");
				}

				go.GetComponent<MeshRenderer>().material = material;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
