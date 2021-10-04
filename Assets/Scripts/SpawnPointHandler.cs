using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointHandler : MonoBehaviour
{

    public float randomTime;
    public GameObject planet;
    public GameObject[] all; 
    private ThingHandler thing;
    // Start is called before the first frame update
    void Awake()
    {
        thing = GameObject.FindGameObjectWithTag("Thing").GetComponent<ThingHandler>();
        randomTime = Random.Range(0.3f, 3f);
        Invoke("SpawnThis", randomTime);
        Destroy(gameObject, 3.5f);
    }


    // Update is called once per frame
    void SpawnThis()
    {
        Instantiate(planet, transform.position, Quaternion.identity);
    }

}
