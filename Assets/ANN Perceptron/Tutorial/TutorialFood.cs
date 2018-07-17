using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFood : MonoBehaviour
{
	private bool Moving = false;
		
	void Start ()
    {
        MoveFood();         //Move food
    }

	void Update()
    {
        if (Moving)
            Moving = false;

		if ((transform.position - Vector3.forward).magnitude >= 20f) 
		{
			MoveFood();
		}
    }
	
    void OnCollisionEnter(Collision col)
    {
        TutorialCowControl TPC = col.gameObject.GetComponent<TutorialCowControl>();
        if (TPC != null && !Moving)
        {
            //The cow must eat at a certain angle
			if (Mathf.Abs (TPC.AngleToFood) > 180) {
				TPC.Death = true;
				MoveFood ();
			}
            else
            {
                TPC.Satiety += 15;
                MoveFood();
            }
        }
    }

    //Move food
    void MoveFood()
    {
        //Random position
		transform.position = new Vector3(Random.Range(-20F, 20F), Random.Range(0.5f, 1F), Random.Range(-14F, 14F));
		Moving = true;
    }
}
