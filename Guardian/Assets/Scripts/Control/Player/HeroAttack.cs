
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


/*
    持续按 ，普通按
 */
public class HeroAttack : MonoBehaviour {

    private PlayerEnitity _playerEnitity;
    private Coroutine _resetIdleCor;


    private float delayTime = 2.0f;
    public float _lastPressTime = 0.0f;
    public bool _isLongPrees = false;

    private GameObject _skillLayer;
    private RadialBlurEffect _radialPostEffect;
    void Start()
    {
        _skillLayer = GameObject.Find("_Manager/_ViewManager/_Scene/Skill");
        GameObject _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        _radialPostEffect = _mainCamera.GetComponent<RadialBlurEffect>();
    }
    public void setPlayerEnitity(PlayerEnitity enitity)
    {
        _playerEnitity = enitity;
    }


    public void heroNormalAttack(bool isCheckLongPrees = true)
    {

        //普通攻击
        //多个攻击动作区分或者整合 todo
        //print(GetType() + "/heroNormalAttack Attack");
        if (_playerEnitity != null)
        {
            int comIndex = _playerEnitity.getCurrComIndex() + 1;
            string effectName = "ParticleProps/Hero_attack0" + comIndex;
            Transform _playerTransform = _playerEnitity._gameObject.transform;
            int offfactor = 1;
            if (comIndex == 2)
            {
                offfactor = 3;
            }
            Vector3 forwardOffset = _playerTransform.forward * offfactor;
            GameObject obj = _playerEnitity.createEffectNoPool(effectName, _playerTransform.position + forwardOffset, _playerEnitity.skillLayer);

            //播放普通攻击特效
            //Transform _playerTransform = _playerEnitity._gameObject.transform;
            //Vector3 forwardOffset = _playerTransform.forward * offfactor;
            //_playerEnitity.getAttackEffeectObj(_playerTransform.position + forwardOffset);
            _playerEnitity.changeStateByIndex(PlayerStateEnum.NORMALATTACK, 2.0f, false);
            startResetIdle(PlayerStateEnum.NORMALATTACK);
            _playerEnitity.AddComobIndex();

        }
    }

    public void heroMagicTrickA()
    {

        //普通技能
        //print(GetType() + "/heroMagicTrickA MagicTrickA");
        if (_playerEnitity != null)
        {
            
            _playerEnitity.changeStateByIndex(PlayerStateEnum.MAGICTRICKA, 2.0f, false);
            startResetIdle(PlayerStateEnum.MAGICTRICKA);
        }
    }

    public void heroMagicTrickB()
    {

        //大招技能
        //print(GetType() + "/heroMagicTrickB MagicTrickB");
        if (_playerEnitity != null)
        {
            _playerEnitity.changeStateByIndex(PlayerStateEnum.MAGICTRICKB, 2.0f, false);
            startResetIdle(PlayerStateEnum.MAGICTRICKB);
           
        }
    }

    public void heroMagicTrickC()
    {

        //大招技能
        //print(GetType() + "/heroMagicTrickB MagicTrickB");
        if (_playerEnitity != null)
        {
            _playerEnitity.changeStateByIndex(PlayerStateEnum.MAGICTRICKC, 2.0f, false);
            startResetIdle(PlayerStateEnum.MAGICTRICKC);

        }
    }

    public void heroMagicTrickD()
    {

        //大招技能
        //print(GetType() + "/heroMagicTrickB MagicTrickB");
        if (_playerEnitity != null)
        {
            //添加后期效果
            Vector3 playerPos = _playerEnitity._gameObject.transform.position;
            Vector3 screenPos = Camera.main.WorldToViewportPoint(playerPos);
            _radialPostEffect.enabled = true;
            _radialPostEffect.setCenter(screenPos.x, screenPos.y);

            _playerEnitity.changeStateByIndex(PlayerStateEnum.MAGICTRICKD, 2.0f, false);
            startResetIdle(PlayerStateEnum.MAGICTRICKD);

        }
    }

    IEnumerator resetIdleState(float delayTime, PlayerStateEnum preEnum)
    {
        yield return new WaitForSeconds(delayTime);
        _radialPostEffect.enabled = false;
        if(_playerEnitity != null)
        {
            _playerEnitity.changeStateByIndex(PlayerStateEnum.IDLE, 1.0f, true);
        }
    }


    public void startResetIdle(PlayerStateEnum playstateEM)
    {
        if (_resetIdleCor != null)
        {
            StopCoroutine(_resetIdleCor);
        }
        
        float time = _playerEnitity.getAnimaitionPlayTime(playstateEM);
        _resetIdleCor = StartCoroutine(resetIdleState(time, playstateEM));
    }
}
