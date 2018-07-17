using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowLearning : MonoBehaviour
{

    public TutorialCowControl TCС;
    public CowPerceptron TCP;

    private PerceptronRandomGenerationInterface PRGI;


    // Use this for initialization
    void Start()
    {

               PRGI = gameObject.AddComponent<PerceptronRandomGenerationInterface>();
               PRGI.PCT = TCP.PCT;
               PRGI.PLBRG.StudentData(TCP.gameObject, TCP, "PCT", TCС, "Death", "LifeTime");


        
    }
}
