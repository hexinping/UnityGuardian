
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

public class PlayerModel : BaseModel
{

    private static PlayerModel _instance;

    public string playerId;
    public string name;
    public int level;

    public PlayerModel()
    {
        playerId = "";
        name = "";
        level = 1;
    }

    public void setName(string playerName)
    {
        name = playerName;
    }

}
