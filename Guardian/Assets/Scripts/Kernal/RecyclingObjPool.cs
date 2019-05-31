using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kernal;
public class RecyclingObjPool : MonoBehaviour {

    public float recyclTime = 1.0f;
    public string poolName = "";  //使用对象缓冲池需要告知缓冲池名字
	// Use this for initialization
	void Start () {
        StartCoroutine("startRecyclObj");
	}

    void OnEnable()
    {
        StartCoroutine("startRecyclObj");
    }

    void OnDisable()
    {
        StopCoroutine("startRecyclObj");
    }
    IEnumerator startRecyclObj()
    {
        yield return new WaitForSeconds(recyclTime);
        if (poolName != string.Empty)
            PoolManager.PoolsArray[poolName].RecoverGameObjectToPools(gameObject);
    }

	
}
