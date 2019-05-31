using UnityEngine;
using System.Collections;
using kernal;

public class DestroyForTime : MonoBehaviour {

	public float time;
    public bool isUsePool = false; //是否使用对象缓冲池
    public string poolName = "";  //使用对象缓冲池需要告知缓冲池名字
    private float playTime = 0.0f;
    private float endTime;

	void Start () {
        playTime = Time.time;
        endTime = Time.time + time;
        
	}

    void OnEnable()
    {
        playTime = Time.time;
        endTime = Time.time + time;
    }
    void Update()
    {
        playTime += Time.deltaTime;
        if (playTime >= endTime)
        {
            if (!isUsePool)
            {
                Destroy(gameObject);
            }
            else
            {
                //加入到缓冲池的非活动集合
                if (poolName!= string.Empty)
                    PoolManager.PoolsArray[poolName].RecoverGameObjectToPools(gameObject);
            }
        }
    
    }

}
