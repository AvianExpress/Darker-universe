using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingHandler : MonoBehaviour
{
    public List<GameObject> allies; 
    public List<GameObject> enemies; 
    public List<GameObject> all; 
    public int randomInt;
    public int randomInt2;

    // Start is called before the first frame update
    void Awake()
    {
        Invoke("MakeThePlay", 4f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     void MakeThePlay(){
        randomInt = Random.Range(0, all.Count-1);
        randomInt2 = Random.Range(0, all.Count-1);
        while (randomInt2 == randomInt)
        {
            randomInt2 = Random.Range(0, all.Count-1);
        }
        all[randomInt].GetComponent<PlanetHandler>().MakeAlly();
        all[randomInt2].GetComponent<PlanetHandler>().MakeEnemy();
    }
}
