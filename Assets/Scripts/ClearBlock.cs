using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClearBlock : MonoBehaviour{
    public List<GameObject> enemies;
    public List<GameObject> enemiesDummy;
    public int enemyCount = 0;
    public GameObject explode;
    private bool isNotDone = true;
    public TextMeshProUGUI userName; 
    public GameObject player;
    public Material mat;

    private void Start(){
        enemyCount = enemies.Count;
    }

    private void Update(){
        if(enemies.Count <= 0 && isNotDone){
            GetComponent<MeshRenderer>().material = mat;
            userName.text = "Press E";
            float dis = Vector3.Distance(transform.position, player.transform.position);
            if(Input.GetKeyDown(KeyCode.E) && dis < 4f){
                explode.GetComponent<MeshRenderer>().enabled = false;
                explode.GetComponent<ParticleSystem>().Play();
                StartCoroutine(DisableParticle());
                isNotDone = false;
            }
        }

        foreach(GameObject go in enemies){
            if(go != null){
                enemiesDummy.Add(go);
            }
        }

        enemies.Clear();

        foreach(GameObject go in enemiesDummy){
            if(go != null){
                enemies.Add(go);
            }
        }
        enemiesDummy.Clear();        
    }

    private IEnumerator DisableParticle(){
        yield return new WaitForSeconds(2f);
        explode.GetComponent<ParticleSystem>().Stop();
        yield return new WaitForSeconds(2f);
        Destroy(explode);
    }


}
