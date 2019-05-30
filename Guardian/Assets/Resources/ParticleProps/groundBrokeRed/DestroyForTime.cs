using UnityEngine;
using System.Collections;
using kernal;

public class DestroyForTime : MonoBehaviour {

	public float time;
    public bool isUsePool = false; //是否使用对象缓冲池
    public string poolName = "";  //使用对象缓冲池需要告知缓冲池名字
    private float createTime;
    private float endTime;

	void Start () {
        createTime = Time.time;
        endTime = createTime + time;
        
	}

    void onEnable()
    {
        createTime = Time.time;
        endTime = createTime + time;
    }
    void Update()
    {
        if (createTime +Time.deltaTime >= endTime)
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
