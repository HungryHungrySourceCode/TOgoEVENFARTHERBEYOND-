using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCowControl : MonoBehaviour
{
    public GameObject Food;             //Food's GameObject
    public float DistanceToFood = 0;    //Distance to food
    public float AngleToFood = 0;       //Angle to food

    public float Turn = 0;              //Turn of cow
    public float Move = 0;              //Move of cow
    public float Side = 0;
    public float Satiety = 40;          //Satiety of cow
    public bool Death = false;          //If true - cow will die (reset position)

    public float LifeTime = 0;

    void Update()
    {
        //Max & min turn
        if (Turn > 1)
            Turn = 1;
        else if (Turn < -1)
            Turn = -1;

        //Max & min move
        if (Move > 1)
            Move = 1;
        else if (Move < -1F)
            Move = -1F;

        if (Side > 1)
            Side = 1;
        else if (Side < -1)
            Side = -1;

        //Cow reset
        if (Death)
        {
            Satiety = 40;
            transform.position = new Vector3(0, 0.5F, 0);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            Death = false;
            LifeTime = 0;
        }

        //Controls of cow
        transform.Rotate(0, Turn * 20F, 0);
        transform.Translate(0, 0, Move / 3F);
        transform.Translate(Move / 3F, 0, 0);

        //Food info
        DistanceToFood = Vector3.Distance(transform.position, Food.transform.position);
		AngleToFood = Vector3.Angle(transform.forward, Food.transform.position - transform.position) * Mathf.Sign(transform.InverseTransformPoint(Food.transform.position).x); 

        //The satiety of the cow decreases with time
        Satiety -= Time.deltaTime * 12f;
        if (Satiety < 0 || Satiety > 50)
            Death = true;

        LifeTime += Time.deltaTime - (Mathf.Abs(AngleToFood) / 180F) * Time.deltaTime;
    }
}
