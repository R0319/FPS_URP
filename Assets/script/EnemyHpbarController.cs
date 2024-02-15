using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpbarController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("敵が死ぬときのエフェクト")]
    private GameObject effectDeathPrefab;

    [SerializeField]
    [Tooltip("hitしたときのエフェクト")]
    private GameObject effectHitPrefab;

    bool isDeath = false;

    [SerializeField]
    Slider slider;

    //敵のmaxHP
    [SerializeField]
    private int maxHp = 100;
    //敵のHP
    [SerializeField]
    private int hp = 0;
    //敵の攻撃力
    [SerializeField]
    int atttackPower = 5;
    //HP表示用スライダーUI
    [SerializeField]
    int hpUI;

    EnemySpowner spowner;

    public void SetUpEnemy(EnemySpowner enemySpowner)
    {
        spowner = enemySpowner;
    }
    
    void Start()
    {
        //hpを初期値に設定
        hp = maxHp;
        //スライダーを初期値に設定
        slider.value = maxHp;
    }
    void Update()
    {

    }

    public void Damage(int damage)
    {
        slider.value -= damage;
        hp -= damage;
        if (hp <= 0 && isDeath == false) 
        {
            isDeath = true;
            spowner.enmeyKillCount++;
            Destroy(gameObject);
            //敵が死ぬときのエフェクト
            GameObject effect = Instantiate(effectDeathPrefab, this.transform.position, effectDeathPrefab.transform.rotation);
            Destroy(effect, 2.0f);
            return;
        }
        if (isDeath == false)
        {
            GameObject hiteffect = Instantiate(effectHitPrefab, this.transform.position, effectHitPrefab.transform.rotation);
            Destroy(hiteffect, 1.0f);
        }
    }
}
