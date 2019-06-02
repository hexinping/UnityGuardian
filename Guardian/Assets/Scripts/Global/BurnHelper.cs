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
    private List<string> _gameObjectNameList = new List<string>();

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
        if (_mainTexture != null)
        {
            material.SetTexture("_MainTex", _mainTexture);
        }

        if (_gameObjectNameList != null && _gameObjectNameList.Count > 0)
        {
            for (int i = 0; i < _gameObjectNameList.Count; i++)
            {
                string name = _gameObjectNameList[i];
                GameObject obj = null;
                Recursive(gameObject, name, ref obj);
                if (obj == null)
                {
                    Debug.Log("name======" + name);
                }
                Renderer render = obj.GetComponent<Renderer>();
                render.material = material;
            }
        }
        else
        { 
            //直接把自身的material替换
            Renderer render = gameObject.GetComponent<Renderer>();
            render.material = material;
        }
    }

    public void setNameList(params object[] values)
    {
        if (values != null && values.Length > 0)
        {
            for (int i = 0; i < values.Length;i++ )
            {
                string name = (string)values[i];
                _gameObjectNameList.Add(name);
            }
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
        if (burnTick + delayTime <= GlobalParams.totalTime)
        {
            if (!isReplaceMat)
            {
                initDatas();
            }
           
            burnAmount += Time.deltaTime * burnSpeed;
            material.SetFloat("_BurnAmount", burnAmount);
            if (burnAmount >= 0.99)
            {
                Destroy(gameObject);
            }
        }
        
	}
}
