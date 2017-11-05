using UnityEngine;
using UnityEditor;
using System.Collections;

public class EnableDepthTexture : MonoBehaviour
{
	private Material material;
	Camera targetCamera;

	// Creates a private material used to the effect
	void Awake()
	{
		material = new Material(Shader.Find("Render Depth"));
		targetCamera = GetComponent<Camera>();
		targetCamera.depthTextureMode = DepthTextureMode.Depth;
	}

	// Postprocess the image
	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination, material);
	}

	private void Update()
	{
		targetCamera.transform.position = SceneView.lastActiveSceneView.camera.transform.position;
		targetCamera.transform.rotation = SceneView.lastActiveSceneView.camera.transform.rotation;
	}
}