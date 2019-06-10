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

        if (targetTransform)
        {
            selfTransform.position = new Vector3(targetTransform.position.x, targetTransform.position.y + 0.2f, targetTransform.position.z);      
        }
	}
}
