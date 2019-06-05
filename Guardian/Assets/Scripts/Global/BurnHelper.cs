using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BurnHelper : MonoBehaviour {

	private Material material;

	[Range(0.01f, 1.0f)]
	public float burnSpeed = 0.3f;

	private float burnAmount = 0.0f;

    private float delayTime = 1.0f;
    private float burnTick;

    private bool isReplaceMat = false;

    private Texture _mainTexture = null;
    private Material _mainMaterial = null;

    private Dictionary<string, Material> _materialDict = new Dictionary<string, Material>();

    void Awake()
    { 
        Shader dissoveS = Resources.Load<Shader>("Shaders/DissolveNoNormal");
        material = new Material(dissoveS);
        material.hideFlags = HideFlags.DontSave;
        Texture burnN = Resources.Load<Texture>("Texture/burn_noise");
        material.SetTexture("_BurnMap", burnN);
        material.SetColor("_BurnFirstColor", new Color32(255, 255, 255, 255));
        material.SetColor("_BurnSecondColor", new Color32(158, 247, 255, 255));

        burnTick = GlobalParams.totalTime;
    }


    void initDatas()
    {
        isReplaceMat = true;
        if (_materialDict.Count > 0)
        {
            foreach (KeyValuePair<string, Material> kv in _materialDict)
            {
                string name = kv.Key;
                Material ma = kv.Value;

                //替换材质
                GameObject obj = null;
                Recursive(gameObject, name, ref obj);
                if (obj == null)
                {
                    Debug.Log("溶解shader 找不到obj name======" + name);
                }
                Renderer render = obj.GetComponent<Renderer>();
                render.material = ma;

            }
        }
        else
        {
            Renderer render = gameObject.GetComponent<Renderer>();
            render.material = _mainMaterial;
        }
        
    }


    public Material createSingleMaterail(Shader dissoveS, Texture mainTexture)
    {
        //创建一个对应的材质
        Material material = new Material(dissoveS);
        material.hideFlags = HideFlags.DontSave;
        Texture burnN = Resources.Load<Texture>("Texture/burn_noise");
        material.SetTexture("_BurnMap", burnN);
        material.SetTexture("_MainTex", mainTexture);
        material.SetColor("_BurnFirstColor", new Color32(255, 255, 255, 255));
        material.SetColor("_BurnSecondColor", new Color32(158, 247, 255, 255));
        return material;
    }
    public void createMaterail(Dictionary<string , Texture> dict)
    {
        
        Shader dissoveS = Resources.Load<Shader>("Shaders/DissolveNoNormal");
        if (dict.Count == 0)
        {
            //直接给自己替换材质
            _mainMaterial = createSingleMaterail(dissoveS, _mainTexture);
            return;
        }
        foreach (KeyValuePair<string, Texture> kv in dict)
        {
            string name = kv.Key;
            Texture tex = kv.Value;

            //创建一个对应的材质
            Material material = createSingleMaterail(dissoveS, tex);
            _materialDict.Add(name, material);
        }
        
    }

    public void setMainTex(Texture main)
    {
        _mainTexture = main;
    }

    
    //递归查找子物体
    private void Recursive(GameObject parentGameObject, string name, ref GameObject targetObj)
    {
        foreach (Transform child in parentGameObject.transform)
        {
            if (child.gameObject.name == name)
            {
                targetObj = child.gameObject;
                return;
            }
            Recursive(child.gameObject, name, ref targetObj);
        }

    }           

	
	// Update is called once per frame
	void Update () {
        if (burnTick + delayTime <= GlobalParams.totalTime)
        {
            if (!isReplaceMat)
            {
                initDatas();
            }
           
            burnAmount += Time.deltaTime * burnSpeed;

            if (_materialDict.Count > 0)
            {
                foreach (KeyValuePair<string, Material> kv in _materialDict)
                {
                    Material ma = kv.Value;
                    ma.SetFloat("_BurnAmount", burnAmount);
                }
            }
            else
            {
                _mainMaterial.SetFloat("_BurnAmount", burnAmount);
            }

            if (burnAmount >= 0.99)
            {
                Destroy(gameObject);
            }
        }
        
	}

    void OnDestory()
    {
        if (_materialDict.Count > 0)
        {
            foreach (KeyValuePair<string, Material> kv in _materialDict)
            {
                Material ma = kv.Value;
                //释放资源
                Resources.UnloadAsset(ma);
            }
        }
        _materialDict.Clear();
       
    }
}
