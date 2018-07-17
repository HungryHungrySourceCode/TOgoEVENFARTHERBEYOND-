using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLearning : MonoBehaviour {

    public EnemyController TCС;
    public EnemyPerceptron TCP;

    private PerceptronRandomGenerationInterface PRGI;


    // Use this for initialization
    void Start()
    {
        PRGI = gameObject.AddComponent<PerceptronRandomGenerationInterface>();
        PRGI.PCT = TCP.EnemyPCT;
        PRGI.PLBRG.StudentData(TCP.gameObject, TCP, "EnemyPCT", TCС, "death", "lifeTime");
    }
}
