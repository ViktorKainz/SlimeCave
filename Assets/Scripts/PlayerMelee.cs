using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public float attackRange;
    public int attackDmg;

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
            for (var i = 0; i < enemiesToDamage.Length; i++)
            {
                enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(attackDmg);
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
