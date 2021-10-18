using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum Move { Moveable, NotMoveable }

public class PlayerMovement : Character{

    public static PlayerMovement Instance { get; private set; }
    public bool Moveable { get; set; } = true;
    private Vector3 velocity = Vector3.zero;
    public CharacterController controller;
    public Animator animator;
    public Transform _camera;
    public TextMeshProUGUI pointsTxt, timeD; 

    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 8f;

    public Move air = Move.Moveable;
    public Move land = Move.NotMoveable;

    public KeyCode jumpKeyCode = KeyCode.Space;
    public KeyCode runKeyCode = KeyCode.LeftShift;
    //[Range(.1f, 5f)] 
    private float animatorMovementSpeed = 1f;

    private float range = 100f;
    public Transform firePoint;
    public ParticleSystem fire;
    public GameObject impactEffect;
    private float impactForce = 20f;
    public static int pts = 0;
    public int health = 100;
    public float coolDown = 0f;
    private bool alive = true;
    public Slider healthBar;
    public GameObject died;
    public static int Seconds = 0;

    private float timer = 0f;

    private void Awake(){
        if (Instance == null)
            Instance = this;
        else Destroy(this.gameObject);
    }

    public bool IsMoving{
        get => Mathf.Abs(controller.velocity.x) + Mathf.Abs(controller.velocity.z) > Mathf.Epsilon;
    }

    public float Speed{
        get => Input.GetKey(runKeyCode) ? runSpeed : walkSpeed;
    }

    
    private void Start(){
        Cursor.lockState = CursorLockMode.Locked;
        healthBar.maxValue = health;
        healthBar.value = health;
        if (animator)
            animator.SetFloat("MovementSpeed", animatorMovementSpeed);
    }

    public override void Update(){
        base.Update();
        Gravity();
        timer += Time.deltaTime;
        Seconds = (int)timer;

        timeD.text = (300 - Seconds).ToString();

        if(air == Move.Moveable || IsGrounded)
            Movement();
        if(Input.GetMouseButton(0)){
            animator.SetBool("Shoot", true);
            StartCoroutine(Fire());
        }
        else{
            fire.Stop();
            animator.SetBool("Shoot", false);
        }
    }

    public override void FixedUpdate(){
        base.FixedUpdate();
        coolDown++;
        if (IsMoving)
            LookAtCamera();
    }

    private void Gravity(){
        const float GROUNDED_VELOCITY = -9.81f;

        if(animator)
            animator.SetBool("Float", !IsGrounded);

        if(IsGrounded && velocity.y < 0)
            velocity.y = GROUNDED_VELOCITY;

        if(!IsGrounded && WasGrounded && velocity.y < 0)
            velocity.y = 0f;

        if(Input.GetButtonDown("Jump") && IsGrounded){
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void Movement(){
        if(Moveable){
            var x = Input.GetAxis("Horizontal");
            var y = Input.GetAxis("Vertical");

            var movement = transform.right * x + transform.forward * y;
            controller.Move(movement * Speed * Time.deltaTime);
            if(animator){
                var strength = (Speed == walkSpeed ? 1f : 2f);
                animator.SetFloat("Movement", y * strength);
                animator.SetFloat("Direction", x * strength);
            }
        }
    }

    private void LookAtCamera(){
        const float SMOOTH_TIME = 5f;
        var camAngles = Vector3X.IgnoreXZ(_camera.eulerAngles);
        var targetRot = Quaternion.Euler(camAngles);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, SMOOTH_TIME * Time.deltaTime);
    }

    private IEnumerator Fire(){
        yield return new WaitForSeconds(1);
        fire.Play();
        if(coolDown > 50f){
            coolDown = 0f;
            RaycastHit hit;
            if(Physics.Raycast(firePoint.position, firePoint.forward, out hit, range)){
                if(hit.transform.tag == "Enemy"){
                    hit.transform.GetComponent<EnemyController>().TakeDamage(10);
                    hit.rigidbody.AddForce(-hit.normal * impactForce);
                }
                GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, .5f);
            }
        }
    }

    public void TakeDamage(int damage){
        health -= damage;
        healthBar.value = health;
        if(health <= 0 && alive){
            alive = false;
            died.SetActive(true);
            Destroy(gameObject);
            SceneManager.LoadScene("Main");
        }
    }


    public void IncreasePoints(int points){
        pts += points;
        pointsTxt.text = pts.ToString();
    }

}
