using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPerceptron : MonoBehaviour {

    private EnemyController ENC;             //Cow control
    public Perceptron EnemyPCT = new Perceptron();   //Perceptron
    private PerceptronInterface PI;             //Perceptron interface

    void Start()
    {
        //Find cow control 
        ENC = gameObject.GetComponent<EnemyController>();

        //Hiden layers and neurons
        int[] Layers = new int[2];
        Layers[0] = 9;
        Layers[1] = 9;

        //Create perceptron
        EnemyPCT.CreatePerceptron(1, false, true, 5, Layers, 2);

        //Add perceptron interface to game object & add perceptron to interface
        PI = gameObject.AddComponent<PerceptronInterface>();
        PI.PCT = EnemyPCT;
    }

    void Update()
    {

        //EnemyPCT.Input[1] = ENC.distanceToTarget;    //Work with distance. Max vaule = 41 
        EnemyPCT.Input[0] = ENC.Satiety / 100F;           //Work with satiety. Min vaule = 0, max vaule = 50 
        EnemyPCT.Input[1] = ENC.difX;

        //EnemyPCT.Input[2] = ENC.clearDown;      //Work with angles. Min vaule = -180, max vaule = 180
        //EnemyPCT.Input[3] = ENC.clearDownLeft;      //Work with angles. Min vaule = -180, max vaule = 180
        //EnemyPCT.Input[4] = ENC.clearDownRight;      //Work with angles. Min vaule = -180, max vaule = 180
        //EnemyPCT.Input[5] = ENC.clearUp;      //Work with angles. Min vaule = -180, max vaule = 180
        //EnemyPCT.Input[6] = ENC.clearUpLeft;      //Work with angles. Min vaule = -180, max vaule = 180
        //EnemyPCT.Input[7] = ENC.clearUpRight;      //Work with angles. Min vaule = -180, max vaule = 180
        //EnemyPCT.Input[8] = ENC.clearLeft;      //Work with angles. Min vaule = -180, max vaule = 180
        //EnemyPCT.Input[9] = ENC.clearRight;      //Work with angles. Min vaule = -180, max vaule = 180

        EnemyPCT.Input[2] = ENC.difY;      //Work with angles. Min vaule = -180, max vaule = 180
        EnemyPCT.Input[3] = ENC.angleToTarget / 180f;
        EnemyPCT.Input[4] = ENC.distanceToTarget;
        EnemyPCT.Solution();                             //Perceptron solution 

        //For this tutorial not need to convert vaule 
        ENC.moveLeftRight = EnemyPCT.Output[0];
        ENC.moveUpDown = EnemyPCT.Output[1];

        //ENC.checkDown = EnemyPCT.Output[2];
        //ENC.checkUp = EnemyPCT.Output[3];
        //ENC.checkLeft = EnemyPCT.Output[4];
        //ENC.checkRight = EnemyPCT.Output[5];
        //ENC.stop = EnemyPCT.Output[2];
    }


}
