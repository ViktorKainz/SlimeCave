using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeele : MonoBehaviour
{
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public float attackRange;
    public float attackDmg;

    public GameObject AttackParticleSystem;
    // Update is called once per frame
    void Update()
    {
        
        if (timeBtwAttack < 0)
        {
            if (!Input.GetKeyDown(KeyCode.Space)) return;
            var effect = Instantiate(AttackParticleSystem, attackPos.position, Quaternion.identity);
            Destroy(effect, 0.5f);
            //ToDo implement Enemy script with health var or function
            var enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
            Debug.Log(enemiesToDamage);
            for (var i = 0; i < enemiesToDamage.Length; i++)
            {
                Debug.Log("Hit");
                //enemiesToDamage[i].GetComponent<Enemy>().health -= attackDmg;
            }

            timeBtwAttack = startTimeBtwAttack;
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
