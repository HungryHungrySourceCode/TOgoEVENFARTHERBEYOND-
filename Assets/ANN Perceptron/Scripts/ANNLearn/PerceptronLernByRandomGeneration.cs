using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Perceptron learning by randon generation method.
/// Non-MonoBehaviour script.
/// </summary>
public class PerceptronLernByRandomGeneration
{


    /// <summary>
    /// Amount of "children" in each generation.
    /// </summary>
    public int AmountOfChildren = 20;

    /// <summary>
    /// The best generation.
    /// </summary>
    public int BestGeneration = 0;

    /// <summary>
    /// Total number of generations.
    /// </summary>
    public int Generation;

    /// <summary>
    /// The number of "children" in the last generation.
    /// </summary>
    public int ChildrenInGeneration;

    /// <summary>
    /// The best "longevity" at the moment.
    /// </summary>
    public float BestLongevity = -Mathf.Infinity;

    /// <summary>
    /// The difference of weights between generations.
    /// </summary>
    public float ChildrenDifference = 2F;

    /// <summary>
    /// The difference of weights with the coefficient of influence between generations.
    /// </summary>
    public float ChildrenDifferenceAfterEffects = 2F;

    /// <summary>
    /// Linearly smooths the difference of weights between "children" in the generation.
    /// </summary>
    public bool ChildrenGradient = false;

    /// <summary>
    /// The coefficient of Reducing of influence on the difference of weights between generations. Effects on condition that it is not equal to zero and the current generation was better than the previous one.
    /// </summary>
    public float GenerationEffect = 0F;

    /// <summary>
    /// The coefficient of increasing of influence on the difference in weight between generations. Effects on condition that it is not zero and the present generation was worse than the previous one.
    /// </summary>
    public float GenerationSplashEffect = 0F;

    /// <summary>
    /// Chance to choose the current worst generation. It changes with each new generation.
    /// </summary>
    public float Chance = 100;

    /// <summary>
    /// The coefficient of influence on the chance of choosing the current worst generation. Effects on condition that it is not zero and the present generation was worse than the previous one.
    /// </summary>
    public float ChanceCoefficient = 0F;

    private GameObject Stud;
    private Object StudCtrl;
    private Object HIANN;
    private string PCTname;
    private string StudCrash;
    private string StudLife;
    private bool HaveStudentData = false;
    private bool HaveStudent = false;
    private GameObject[] Child;
    private Perceptron[] ChildPCT;
    private bool[] ChildCrash;
    private float[] Longevity;
    private int BestChildInGeneration = -1;
    private float BestLongevityInGeneration = -Mathf.Infinity;
    private float[][][] BestChildPCT;
    private float GenerationSplashEffectStorage = 0;
    private float GenerationEffectStorage = 0F;

    /// <summary>
    /// Data collection for training.
    /// </summary>
    /// <param name="Student">The main game object (GameObject) is to learn.</param>
    /// <param name="HereIsANN">Script containing perceptron.</param>
    /// <param name="PerceptronName">The name of the variable (Perceptron) which is called the perceptron in the script that contains the perceptron (HereIsANN).</param>
    /// <param name="StudentControls">Script for control the main game object.</param>
    /// <param name="StudentCrash">The name of the variable (bool) which is called the cause of the "crash" of the game object in the control script (StudentControls).</param>
    /// <param name="StudentLife">The name of the variable (float) is called the "longevity" of the game object in the control script (StudentControls).</param>
    public void StudentData(GameObject Student, Object HereIsANN, string PerceptronName, Object StudentControls, string StudentCrash, string StudentLife)
    {
        if (Student != null && HereIsANN != null && StudentControls != null)
        {
            Stud = Student;
            HIANN = HereIsANN;
            PCTname = PerceptronName;
            StudCtrl = StudentControls;
            StudCrash = StudentCrash;
            StudLife = StudentLife;
            HaveStudentData = true;
        }
    }

    /// <summary>
    /// The training of the perceptron by the random generation.
    /// </summary>
    /// <param name="PCT">Perceptron.</param>
    public void Learn(Perceptron PCT)
    {
        if (ChanceCoefficient < 0)
            ChanceCoefficient = 0;
        else if (ChanceCoefficient > 0.5F)
            ChanceCoefficient = 0.5F;
        if (GenerationSplashEffect < 0)
            GenerationSplashEffect = 0;
        else if (GenerationSplashEffect > 1)
            GenerationSplashEffect = 1;
        if (HaveStudentData)
        {
            if (!HaveStudent)
            {
                if (!(bool)StudCtrl.GetType().GetField(StudCrash).GetValue(Stud.GetComponent(StudCtrl.GetType())))
                {
                    StudCtrl.GetType().GetField(StudCrash).SetValue(Stud.GetComponent(StudCtrl.GetType()), true);
                }

                BestChildPCT = new Formulas().FromArray(PCT.NeuronWeight);
                HaveStudent = true;
            }
            else
            {
                if (Child == null)
                    Child = new GameObject[0];
                if (Child.Length != AmountOfChildren)
                    CreateChildren();
                else
                {
                    int i = 0;
                    bool NewGeneration = true;
                    while (i < Child.Length)
                    {
                        if (Child[i] != null)
                        {
                            NewGeneration = false;
                            Object TempObject = Child[i].GetComponent(HIANN.GetType());
                            //Modification of new children's weight
                            if (ChildPCT[i] == null)
                            {
                                ChildPCT[i] = (Perceptron)HIANN.GetType().GetField(PCTname).GetValue(TempObject);
                                if (PCT.B)
                                    ChildPCT[i].CreatePerceptron(PCT.AFS, PCT.B, PCT.AFWM, PCT.Input.Length - 1, new Formulas().FromArray(PCT.NIHL, -1), PCT.Output.Length);
                                else
                                    ChildPCT[i].CreatePerceptron(PCT.AFS, PCT.B, PCT.AFWM, PCT.Input.Length, PCT.NIHL, PCT.Output.Length);

                                int l = 0;
                                while (l < BestChildPCT.Length)
                                {
                                    int k = 0;
                                    while (k < BestChildPCT[l].Length)
                                    {
                                        int j = 0;
                                        while (j < BestChildPCT[l][k].Length)
                                        {
                                            ChildrenDifferenceAfterEffects = ChildrenDifference;
                                            if (GenerationSplashEffect != 0 && GenerationSplashEffectStorage != 0)
                                                ChildrenDifferenceAfterEffects = ChildrenDifferenceAfterEffects * (1F + GenerationSplashEffectStorage);
                                            if (GenerationEffect != 0 && GenerationEffectStorage != 0)
                                                ChildrenDifferenceAfterEffects = ChildrenDifferenceAfterEffects / (1F + GenerationEffectStorage);
                                            if (ChildrenGradient)
                                                ChildrenDifferenceAfterEffects = ChildrenDifferenceAfterEffects * ((i + 1F) / AmountOfChildren);

                                            ChildPCT[i].NeuronWeight[l][k][j] = BestChildPCT[l][k][j] + Random.Range(-ChildrenDifferenceAfterEffects, ChildrenDifferenceAfterEffects);
                                            j++;
                                        }
                                        k++;
                                    }
                                    l++;
                                }
                            }

                            TempObject = Child[i].GetComponent(StudCtrl.GetType());

                            if (!ChildCrash[i])
                                Longevity[i] = (float)StudCtrl.GetType().GetField(StudLife).GetValue(TempObject);
                            else
                            {
                                if (BestLongevityInGeneration < Longevity[i])
                                {
                                    BestLongevityInGeneration = Longevity[i];
                                    BestChildInGeneration = i;
                                    if (BestLongevity < BestLongevityInGeneration)
                                    {
                                        BestGeneration = Generation;
                                        BestLongevity = BestLongevityInGeneration;
                                        BestChildPCT = ChildPCT[BestChildInGeneration].NeuronWeight;
                                    }
                                }
                            }
                            if (ChildCrash[i] && Child[i] != null)
                            {
                                Object.Destroy(Child[i]);
                                ChildrenInGeneration--;
                            }
                            ChildCrash[i] = (bool)StudCtrl.GetType().GetField(StudCrash).GetValue(TempObject);
                        }
                        i++;
                    }

                    if (NewGeneration)
                    {
                        StudCtrl.GetType().GetField(StudCrash).SetValue(Stud.GetComponent(StudCtrl.GetType()), true);
                        Child = new GameObject[0];
                        if (BestLongevity <= BestLongevityInGeneration)
                        {
                            if (GenerationEffect != 0)
                                GenerationEffectStorage += GenerationEffect;
                            if (GenerationSplashEffect != 0)
                                GenerationSplashEffectStorage = 0;
                        }
                        else
                        {
                            if (ChanceCoefficient!=0)
                            {
                                float P = 100 * Mathf.Exp((BestLongevityInGeneration - BestLongevity) / Chance);
                                if (P > Random.Range(0F, 100F))
                                {
                                    BestGeneration = Generation;
                                    BestLongevity = BestLongevityInGeneration;
                                    BestChildPCT = ChildPCT[BestChildInGeneration].NeuronWeight;
                                    if (GenerationEffect != 0)
                                        GenerationEffectStorage = 0;
                                    if (GenerationSplashEffect != 0)
                                        GenerationSplashEffectStorage = 0;
                                }
                                else
                                {
                                    if (GenerationEffect != 0)
                                        GenerationEffectStorage = 0;
                                    if (GenerationSplashEffect != 0)
                                        GenerationSplashEffectStorage += GenerationSplashEffect;
                                }
                            }
                            else
                            {
                                if (GenerationEffect != 0)
                                    GenerationEffectStorage = 0;
                                if (GenerationSplashEffect != 0)
                                    GenerationSplashEffectStorage += GenerationSplashEffect;
                            }
                        }
                        if (ChanceCoefficient != 0)
                            Chance = Chance * (1 - ChanceCoefficient);
                    }
                }
            }
        }
    }

        private void CreateChildren()
    {
        Generation++;
        ClearGeneration();
        BestLongevityInGeneration = -Mathf.Infinity;
        Child = new GameObject[AmountOfChildren];
        ChildPCT = new Perceptron[AmountOfChildren];
        ChildCrash = new bool[AmountOfChildren];
        Longevity = new float[AmountOfChildren];
        BestChildInGeneration = -1;

        ChildrenInGeneration = AmountOfChildren;
        int i = 0;
        while (i < AmountOfChildren)
        {
            Child[i] = Object.Instantiate(Stud);
            Child[i].name = "StudentChild";

            foreach (PerceptronInterface pi in Child[i].GetComponentsInChildren<PerceptronInterface>())
            {
                Object.Destroy(pi);
            }
            foreach (PerceptronRandomGenerationInterface prgi in Child[i].GetComponentsInChildren<PerceptronRandomGenerationInterface>())
            {
                Object.Destroy(prgi);
            }

            StudCtrl.GetType().GetField(StudCrash).SetValue(Child[i].GetComponent(StudCtrl.GetType()), false);

            foreach (Collider col in Child[i].GetComponentsInChildren<Collider>())
            {
                foreach (Collider colp in Stud.GetComponentsInChildren<Collider>())
                {
                    Physics.IgnoreCollision(col, colp);
                }
            }

            foreach (Collider2D col in Child[i].GetComponentsInChildren<Collider2D>())
            {
                foreach (Collider2D colp in Stud.GetComponentsInChildren<Collider2D>())
                {
                    Physics2D.IgnoreCollision(col, colp);
                }
            }

            if (i != 0)
            {
                foreach (Collider col in Child[i].GetComponentsInChildren<Collider>())
                {
                    int c = 0;
                    while (c < i)
                    {
                        foreach (Collider colp in Child[c].GetComponentsInChildren<Collider>())
                        {
                            Physics.IgnoreCollision(col, colp);
                        }
                        c++;
                    }
                }

                foreach (Collider2D col in Child[i].GetComponentsInChildren<Collider2D>())
                {
                    int c = 0;
                    while (c < i)
                    {
                        foreach (Collider2D colp in Child[c].GetComponentsInChildren<Collider2D>())
                        {
                            Physics2D.IgnoreCollision(col, colp);
                        }
                        c++;
                    }
                }
            }
            i++;
        }
    }

    /// <summary>
    /// Immediate stop learning with the transfer of information of the best perceptron from the best generation to the perceptron who is studying.
    /// </summary>
    /// <param name="PCT">Perceptron.</param>
    public void StopLearn(Perceptron PCT)
    {
        if (PCT!=null && ChildCrash!=null)
        {
            int i = 0;
            while (i < ChildCrash.Length)
            {
                ChildCrash[i] = true;
                i++;
            }
            Learn(PCT);
        }
        if (PCT != null && BestChildPCT != null)
            PCT.NeuronWeight = new Formulas().FromArray(BestChildPCT);
        ClearGeneration();
        GenerationEffectStorage = 0;
        GenerationSplashEffectStorage = 0;
        HaveStudent = false;
        StudCtrl.GetType().GetField(StudCrash).SetValue(Stud.GetComponent(StudCtrl.GetType()), false);
    }

    private void ClearGeneration()
    {
        if (Child != null)
        {
            int i = 0;
            while (i < Child.Length)
            {
                if (Child[i] != null)
                {
                    Object.Destroy(Child[i]);
                }
                i++;
            }
            Child = new GameObject[0];
        }
    }

    /// <summary>
    /// Reset learning info.
    /// </summary>
    public void Reset()
    {
        ClearGeneration();
        Generation = 0;
        BestChildInGeneration = -1;
        BestLongevity = -Mathf.Infinity;
        BestLongevityInGeneration = -Mathf.Infinity;
        Chance = 100;
        HaveStudent = false;
    }
}
