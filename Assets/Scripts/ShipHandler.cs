using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipHandler : MonoBehaviour
{
    public Transform target;
    public Vector3 pointA;
    public Vector3 pointB;
    private float movementSpeed = 50f;
    public bool isEnemy = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var dir = pointB-pointA;
        var angle = Mathf.Atan2(dir.y, dir.x) *Mathf.Rad2Deg;
        var rotateto = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateto, 7f );
        transform.position= Vector3.MoveTowards(transform.position, pointB, movementSpeed*Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {   
        /*
        * В оригинале "поиск пути" сделан именно так, если присмотреться
        * Корабли сталкиваются с планетами и, если это не таргет, уходят восвояси дальше
        * Костыльно как по мне, лучшим решением было бы использовать А*, но писать его самостоятельно времязатратно, а засорять проект импортом не хочется
        * Так что властью, данной мне партией и Гейцом, отдаю уважение истокам и нарекаю этот костыль фичей, а не багом
        */
        if(other.CompareTag("Enemy") && target.transform.position == other.transform.position){
            if(!isEnemy){
            PlanetHandler ph = other.GetComponent<PlanetHandler>();
            ph.shipCounter -= 1;
            if (ph.shipCounter<0)
            ph.MakeAlly();
            Destroy(gameObject);
            }
            else {
            PlanetHandler ph = other.GetComponent<PlanetHandler>();
            ph.shipCounter += 1;
            Destroy(gameObject);
            }
        }
         if(other.CompareTag("Allied") && target.transform.position == other.transform.position){
             if(!isEnemy){
                PlanetHandler ph = other.GetComponent<PlanetHandler>();
                ph.shipCounter += 1;
                Destroy(gameObject);
            }
            else {
                PlanetHandler ph = other.GetComponent<PlanetHandler>();
                ph.shipCounter -= 1;
                if (ph.shipCounter<0)
                ph.MakeEnemy();
                Destroy(gameObject);
            }
        }
        if(other.CompareTag("Neutral") && target.transform.position == other.transform.position){
            PlanetHandler ph = other.GetComponent<PlanetHandler>();
            ph.shipCounter -= 1;
            if (ph.shipCounter<0 && isEnemy)
            ph.MakeEnemy();
            Destroy(gameObject);
        }
       
    }
    public void MakeEnemyColour(){
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f);
    }
}
