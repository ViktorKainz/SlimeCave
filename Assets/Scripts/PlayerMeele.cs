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
        if (timeBtwAttack <= 0)
        {
            //todo fix this if
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Space pressed");
                GameObject effect = Instantiate(AttackParticleSystem, attackPos.position, Quaternion.identity);
                Destroy(effect, 5f);
                //ToDo implement Enemy script wiht health var or funktion
                //Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
                //for (int i = 0; i < enemiesToDamage.Length; i++)
                //{
                    //enemiesToDamage[i].GetComponent<Enemy>().health -= damage;
                //}

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
