using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetHandler : MonoBehaviour
{
    private SpriteRenderer sr;
    public Renderer ren;
    public bool isSelected;

    public int shipCounter;
    private float delay = 1f;
    private ThingHandler thing;
    public Transform shiptf;
    public Canvas canvas;
    public Text text;
    public int enemyProd = 5;
    public int playerProd = 5; 
    // Start is called before the first frame update
    void Awake()
    {
        canvas = gameObject.GetComponentInChildren<Canvas>();
        text = canvas.GetComponentInChildren<Text>();
        thing = GameObject.FindGameObjectWithTag("Thing").GetComponent<ThingHandler>();
        sr = this.gameObject.GetComponent<SpriteRenderer>();
        ren = GetComponent<Renderer>();
        thing.all.Add(this.gameObject);
        if (tag == "Allied")
        {
            sr.color = new Color(0f, 0.8f, 0f);
            shipCounter = Random.Range(2, 90);
            thing.allies.Add(this.gameObject);
        }
        else if (tag == "Enemy")
            sr.color = new Color(1f, 0f, 0f);
        else {
            sr.color = new Color(1f, 1f, 1f);
            shipCounter = 10;
        }
        isSelected = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (tag == "Allied" || tag == "Enemy" )
        {

            if (delay > 0)
                delay -= Time.deltaTime;
            else
            {
                if (tag =="Allied")
                shipCounter += playerProd;
                else shipCounter += enemyProd;
                delay = 1f;
            }
            if (tag == "Enemy" && shipCounter >49){
                EnemyEngage(thing.enemies);
            }
        }
        else 
        if ((tag == "Neutral" || tag == "Enemy") && shipCounter < 0){
                shipCounter = 0;
                tag = "Allied";
                sr.color = sr.color = new Color(0f, 0f, 0.8f);
                thing.allies.Add(this.gameObject);
                thing.enemies.Remove(this.gameObject);
        }
        if (tag == "Allied" && shipCounter < 0){
                shipCounter = 0;
                tag = "Enemy";
                sr.color = sr.color = new Color(1f, 0f, 0f);
                thing.allies.Remove(this.gameObject);
        }
        
        text.text = shipCounter.ToString(); 
    }

    private void OnMouseDown()
    {
        if (this.gameObject.tag == "Allied")
        {
            isSelected = !isSelected;
            if (isSelected)
                ren.material.color = new Color(0f, 0f, 2f);
            else ren.material.color = new Color(0f, 0f, 1.5f);
        }
        else MakeAttack(thing.allies);
    }

    private void OnMouseEnter()
    {
        if (this.gameObject.tag == "Allied" && !isSelected)
            ren.material.color = new Color(0f, 0f, 1.5f);
    }
    private void OnMouseExit()
    {
        if (!isSelected)
            ren.material.color = Color.white;
    }

    void OnTriggerEnter2D(Collider2D other)
    {  
        if(other.CompareTag("Neutral") || other.CompareTag("Enemy") || other.CompareTag("Allied")){
            thing.all.Remove(gameObject);
            Destroy(gameObject);
        }
       
    }

    public void MakeAlly(){
       sr.color = new Color(0f, 0f, 0.8f);
       tag = "Allied";
       thing.allies.Add(gameObject);
    }
    public void MakeEnemy(){
       sr.color = new Color(1f, 0f, 0f);
       tag = "Enemy";
       thing.enemies.Add(gameObject);
       thing.all.Remove(gameObject);
    }

    public void MakeAttack(List<GameObject> forces){
        foreach (GameObject ally in forces)
            {
                PlanetHandler ph = ally.GetComponent<PlanetHandler>();
                if (ph.isSelected)
                {
                    int toBattle = ph.shipCounter / 2;
                    ph.shipCounter = ph.shipCounter / 2;
                    for (int i = 0; i < toBattle; i++)
                    {
                        // float ofx = Random.Range(-50, 200f);
                        // float ofy = Random.Range(-200f, 200f);
                        Vector3 spawn = new Vector3(ally.transform.position.x, ally.transform.position.y, 0f);
                        Transform shipTransform = Instantiate(shiptf, spawn, Quaternion.identity);
                        ShipHandler sh = shipTransform.GetComponent<ShipHandler>();
                        sh.target = gameObject.GetComponent<Transform>();
                        sh.pointA = Camera.main.ScreenToWorldPoint(ph.transform.position);
                        sh.pointB = gameObject.transform.position;
                    }
                }
            }

    }

    public void EnemyEngage (List<GameObject> forces){
        int randomInt = Random.Range(0, thing.all.Count-1);
        GameObject target = thing.all[randomInt];
        foreach (GameObject ally in forces){
            PlanetHandler ph = ally.GetComponent<PlanetHandler>();
            int toBattle = ph.shipCounter / 2;
            ph.shipCounter = ph.shipCounter / 2;
            for (int i =0; i < toBattle; i++){
                  Vector3 spawn = new Vector3(ally.transform.position.x, ally.transform.position.y, 0f);
                  Transform shipTransform = Instantiate(shiptf, spawn, Quaternion.identity);
                  ShipHandler sh = shipTransform.GetComponent<ShipHandler>();
                  sh.target = thing.all[randomInt].gameObject.GetComponent<Transform>();
                  sh.pointA = Camera.main.ScreenToWorldPoint(ph.transform.position);
                  sh.pointB = target.transform.position;
                  sh.isEnemy = true;
                  sh.MakeEnemyColour();
            }
        }
    }
   
}
