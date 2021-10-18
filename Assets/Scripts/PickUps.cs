using UnityEngine;

public class PickUps : MonoBehaviour{
    private float ySpeed = 100f;
    public int points;
    public ParticleSystem splash;

    private void Update() {
        transform.Rotate(0, ySpeed * Time.deltaTime,0);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Proto"){
            other.GetComponent<PlayerMovement>().IncreasePoints(points);
            splash.Play();
            Destroy(gameObject, 0.2f);
        }
    }

}
