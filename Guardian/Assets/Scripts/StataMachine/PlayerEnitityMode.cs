using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnitityMode : BaseMode
{

    public string file;
    private PlayerManager _playerMgr;


    public PlayerEnitityMode()
    {

        file = "Models/SwordsMan/GreateWarrior";

        maxAtk = 5.0f;
        atk = maxAtk;
        maxHp = 100.0f;
        hp = maxHp;
        maxMagic = 20.0f;
        magic = maxMagic;
        maxDefence = 0.0f;
        defence = maxDefence;
        maxDexterity = 20.0f;
        dexterity = maxDexterity;

        atkByPro = 0.0f;
        defenceByPro = 0.0f;
        dexterityByPro = 0.0f;

        _playerMgr = PlayerManager.getInstance();
    }

    public float countDamage(BaseEnitity target)
    {
        float resultAtk = getAtkValue();
        float resultDefence = target.getDefenceValue();
        float damage = resultAtk - resultDefence;
        if (damage <= 0)
        {
            damage = 0;
        }
        return damage;
    }

  

    public float getAtkValue()
    {
        float result = atk + atkByPro;
        return result;
    }

    public float getDefenceValue()
    {
        float result = defence + defenceByPro;
        return result;
    }

    public float getDexterityValue()
    {
        float result = dexterity + dexterityByPro;
        return result;
    }


    public void update(Dictionary<string, object> values)
    {
        base.update(values);

        //专有数据更新
       
    }

}
