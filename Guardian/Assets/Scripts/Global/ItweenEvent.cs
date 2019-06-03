
/***
 *
 *  Title: "Guardian" 项目
 *         描述：
 *
 *  Description:
 *        功能：
 *       
 *
 *  Date: 2019
 * 
 *  Version: 1.0
 *
 *  Modify Recorder:
 *     
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using kernal;
public class ItweenEvent : MonoBehaviour {


    public void moveEndCallBack()
    {
        PoolManager.PoolsArray[GlobalParams.SkillPool].RecoverGameObjectToPools(gameObject);
    }

    public void arrowMoveEndCallBack()
    {
        PoolManager.PoolsArray[GlobalParams.BulletPool].RecoverGameObjectToPools(gameObject);
    }
}
