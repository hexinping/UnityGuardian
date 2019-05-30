using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour {
	
	public string followTagName;
    private Transform targetTransform;
    private Transform selfTransform;
	
	// Use this for initialization
	void Start () {

        GameObject target = GameObject.FindGameObjectWithTag(followTagName);
        targetTransform = target.transform;
        selfTransform = gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {
		
		//this.transform.position = new Vector3(target.transform.position.x,transform.position.y,target.transform.position.z);
        if (targetTransform)
        {
            selfTransform.position = targetTransform.position;        
        }
	}
}
