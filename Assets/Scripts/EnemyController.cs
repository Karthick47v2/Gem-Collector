using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour{
    public NavMeshAgent agent;
    public GameObject player;
    public Animator anim;
    public int health = 0;
    private float yVelocity = 0;
    private float smoothness = .3f;
    private float coolDown  = 0f;
    public int attack = 0;
    private bool alive = true;
    //public Slider healthBar;

    private void Start(){
        // healthBar.maxValue = health;
        // healthBar.value = health;
    }

    private void FixedUpdate(){
        coolDown += 0.1f;
        if(player != null){
            float dis = Vector3.Distance(transform.position, player.transform.position);
            if(dis < 15f){
                if(dis < 2f){
                    ReadyToAttack();
                    anim.SetBool("Walk", false);
                    anim.SetBool("Attack1",true);
                    if(coolDown > 2f){
                        coolDown = 0f;
                        player.GetComponent<PlayerMovement>().TakeDamage(attack);
                    }

                }
                else{
                    agent.SetDestination(player.transform.position);
                    agent.isStopped = false;
                    anim.SetBool("Rest", false);
                    anim.SetBool("Walk", true);
                    anim.SetBool("Attack1",false);
                }
            }
            else{
                ReturnToRest();
            } 
        }
        else{
            ReturnToRest();
        }        
    }

    private void ReturnToRest(){
        agent.isStopped = true;
        anim.SetBool("Walk", false);
        anim.SetBool("Attack1",false);
        anim.SetBool("Rest", true);
    }

    public void TakeDamage(int damage){
        health -= damage;
        //healthBar.value = health;
        if(health <= 0 && alive){
            alive = false;
            Destroy(gameObject);
        }
    }

    private void ReadyToAttack(){
        Quaternion rotationToLookAt = Quaternion.LookRotation(player.transform.position - transform.position);
        float yAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationToLookAt.eulerAngles.y, ref yVelocity, smoothness);
        transform.eulerAngles = new Vector3(0, yAngle, 0);
        agent.isStopped = true;
    }
}
