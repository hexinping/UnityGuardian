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
        Texture mainT = Resources.Load<Texture>("Models/Enemys/Skeleton_Pack/Textures/warrior/skeleton_warrior__variant5");
        material.SetTexture("_BurnMap", burnN);
        material.SetTexture("_MainTex", mainT);


        //armor
        GameObject armorObj = null;
        Recursive(gameObject, "armor", ref armorObj);
        Renderer armorRender = armorObj.GetComponent<Renderer>();
        armorRender.material = material;

        //eyes
        GameObject eyesObj = null;
        Recursive(gameObject, "eyes", ref eyesObj);
        Renderer eyesRender = eyesObj.GetComponent<Renderer>();
        eyesRender.material = material;

        //helmet
        GameObject helmetObj = null;
        Recursive(gameObject, "helmet", ref helmetObj);
        Renderer helmetRender = helmetObj.GetComponent<Renderer>();
        helmetRender.material = material;


        //Skeletonl_base
        GameObject Skeletonl_baseObj = null;
        Recursive(gameObject, "Skeletonl_base", ref Skeletonl_baseObj);
        Renderer Skeletonl_baseRender = Skeletonl_baseObj.GetComponent<Renderer>();
        Skeletonl_baseRender.material = material;


        //shield
        GameObject shieldObj = null;
        Recursive(gameObject, "shield", ref shieldObj);
        Renderer shieldRender = shieldObj.GetComponent<Renderer>();
        shieldRender.material = material;

        //sword
        GameObject swordObj = null;
        Recursive(gameObject, "sword", ref swordObj);
        Renderer swordRender = swordObj.GetComponent<Renderer>();
        swordRender.material = material;
       

    }


    //递归查找子物体
    private void Recursive(GameObject parentGameObject, string name, ref GameObject targetObj)
    {
        foreach (Transform child in parentGameObject.transform)
        {
            if (child.gameObject.name == name)
            {
                targetObj = child.gameObject;
            }
            Recursive(child.gameObject, name, ref targetObj);
        }

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
        burnAmount += Time.deltaTime * burnSpeed;
		material.SetFloat("_BurnAmount", burnAmount);
        if (burnAmount >= 0.99)
        {
            Destroy(gameObject);
        }
	}
}
