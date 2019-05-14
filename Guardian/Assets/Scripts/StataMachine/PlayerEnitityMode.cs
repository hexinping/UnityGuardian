using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnitityMode : BaseMode
{

    public string file;

    public float hp;
    public float maxHp;
    public float atk;  //攻击力
    public float maxAtk;
    public float magic; //魔法值
    public float maxMagic;
    public float defence; //防御力
    public float maxDefence; 
    public float dexterity; //敏捷度
    public float maxDexterity;

    public float atkByPro;          //道具攻击力
    public float defenceByPro;      //道具防御力
    public float dexterityByPro;    //道具敏捷度


    private PlayerManager _playerMgr;


    public PlayerEnitityMode()
    {

        file = "Models/SwordsMan/GreateWarrior";

        maxAtk = 5.0f;
        atk = maxAtk;
        maxHp = 20.0f;
        hp = maxHp;
        maxMagic = 20.0f;
        magic = maxMagic;
        maxDefence = 20.0f;
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

    public void updateHp(float fHp)
    {
        hp = fHp;
    }

    public void updateMaxHp(float fHp)
    {
        maxHp = fHp;
    }

    public void updateAtk(float fAtk)
    {
        atk = fAtk;
    }

    public void updateMaxAtk(float fAtk)
    {
        maxAtk = fAtk;
    }

    public void updateMagic(float fMagic)
    {
        magic = fMagic;
    }

    public void updateMaxMagic(float fMagic)
    {
        maxMagic = fMagic;
    }

    public void updateDefence(float fDefence)
    {
        defence = fDefence;
    }

    public void updateMaxDefence(float fDefence)
    {
        maxDefence = fDefence;
    }

    public void updateDexterity(float fDexterity)
    {
        dexterity = fDexterity;
    }

    public void updateMaxDexterity(float fDexterity)
    {
        maxDexterity = fDexterity;
    }


    public void updateAtkByPro(float fAtkByPro)
    {
        atkByPro = fAtkByPro;
    }

    public void updateDefenceByPro(float fDefenceByPro)
    {
        defenceByPro = fDefenceByPro;
    }

    public void updateDexterityByPro(float fDexterityByPro)
    {
        dexterityByPro = fDexterityByPro;
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

    public float getHpValue()
    {
        float result = hp;
        return result;
    }

}
