
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

public class PlayerManager {

    private static PlayerManager _instance;
    private PlayerModel _mode;

    private PlayerManager()
    {
        _mode = new PlayerModel();
    }

    public static PlayerManager getInstance()
    {
        if (_instance == null)
        {
            _instance = new PlayerManager();
        }
        return _instance;
    }

    public void setPlayerName(string name)
    {
        _mode.setName(name);
    }

}
