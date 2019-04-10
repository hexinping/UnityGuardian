
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

public class LoginView_Ctrl : MonoBehaviour
{

    public static LoginView_Ctrl _instance = null;

    public GameObject nextScene;
    public void Awake()
    {
        _instance = this;
    }

    public void Start()
    {
      
    }
}
