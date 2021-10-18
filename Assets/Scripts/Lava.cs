using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour{
    private void OnTriggerEnter(Collider other) {
        if(other.tag != "Untagged"){
            Destroy(other.gameObject);
        }
    }
 
}
