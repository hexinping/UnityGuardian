using UnityEngine;
using System.Collections;

public class BurnHelper : MonoBehaviour {

	private Material material;

	[Range(0.01f, 1.0f)]
	public float burnSpeed = 0.3f;

	private float burnAmount = 0.0f;

    void Awake()
    { 
        Shader dissoveS = Resources.Load<Shader>("Shaders/DissolveNoNormal");
        material = new Material(dissoveS);
        material.hideFlags = HideFlags.DontSave;
        Texture burnN = Resources.Load<Texture>("Texture/burn_noise");
        material.SetTexture("_BurnMap", burnN);
        Renderer renderer = gameObject.GetComponent<Renderer>();
        renderer.material = material;
    }

	// Use this for initialization
	void Start () {
		if (material == null) {
			this.enabled = false;
		} else {
			material.SetFloat("_BurnAmount", 0.0f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		burnAmount = Mathf.Repeat(Time.time * burnSpeed, 1.0f);
		material.SetFloat("_BurnAmount", burnAmount);
        if (burnAmount >= 0.99)
        {
            Destroy(gameObject);
        }
	}
}
