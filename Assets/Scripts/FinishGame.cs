using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FinishGame : MonoBehaviour{
    public GameObject fin;
    public TextMeshProUGUI score; 

    void Update(){
         if(Input.GetKeyDown(KeyCode.Escape)){
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Proto"){
            score.text = (PlayerMovement.pts + PlayerMovement.Seconds).ToString();
            fin.SetActive(true);
        }
    }
}
