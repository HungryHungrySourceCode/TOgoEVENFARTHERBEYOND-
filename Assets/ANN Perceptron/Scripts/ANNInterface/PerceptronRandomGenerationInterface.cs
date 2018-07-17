using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface of perceptron's random generation learning method.
/// </summary>
public class PerceptronRandomGenerationInterface : MonoBehaviour
{
    private InterfaceGUI IGUI = new InterfaceGUI();

    /// <summary>
    /// Perceptron.
    /// </summary>
    public Perceptron PCT;

    /// <summary>
    /// Perceptron' random generation learning method.
    /// </summary>
    public PerceptronLernByRandomGeneration PLBRG = new PerceptronLernByRandomGeneration();

    /// <summary>
    /// Is perseptron learning?
    /// </summary>
    public bool Learn = false;

    private Rect WindowRect = new Rect(Screen.width, Screen.height, 400, 260);

    void Start()
    {
        if (gameObject.name == "StudentChild")
            Destroy(gameObject.GetComponent<PerceptronRandomGenerationInterface>());
        else
        {
            if (PCT == null)
                Debug.LogWarning("Perceptron random generation interface does not have perceptron.");
        }
    }

    void Update()
    {
        if (PCT!=null && Learn)
            PLBRG.Learn(PCT);
        PerceptronInterface PI = gameObject.GetComponent<PerceptronInterface>();
        if (PI != null)
            if (PI.Reload)
                PLBRG.Reset();
    }

    void OnGUI()
    {
        if (PCT != null)
            WindowRect = GUI.Window(GUIUtility.GetControlID(FocusType.Passive), WindowRect, Window, "Random generation of " + transform.name);    // interface window
    }

    void Window(int ID)                                                                             // interface window
    {
        if (WindowRect.height == 80)                                                                // small window
        {
            if (GUI.Button(new Rect(WindowRect.width - 20, 0, 20, 20), "+"))                        // change window scale
            {
                WindowRect.width = 400;
                WindowRect.height = 260;
                WindowRect.x = WindowRect.x - 200;
            }

            bool Activte = false;
            Learn = IGUI.Button(1, 2, "Learn OFF", "Learn ON", Learn, ref Activte);

            if (Activte && !Learn)
            {
                PLBRG.StopLearn(PCT);
            }
        }
        else                                                                                        // big window
        {
            if (GUI.Button(new Rect(WindowRect.width - 20, 0, 20, 20), "-"))                        // change window scale
            {
                WindowRect.width = 200;
                WindowRect.height = 80;
                WindowRect.x = WindowRect.x + 200;
            }

            if (Learn)
                GUI.enabled = false;
            PLBRG.AmountOfChildren = IGUI.IntArrows(1, 2, "Children amount", false, PLBRG.AmountOfChildren, 1, true, 0);
            PLBRG.ChildrenDifference = IGUI.HorizontalSlider(1, 3, "Children difference", PLBRG.ChildrenDifference, 0.01F, 10F);
            PLBRG.ChildrenGradient = IGUI.Button(1, 4, "Children gradient OFF", "Children gradient ON", PLBRG.ChildrenGradient);
            PLBRG.GenerationEffect = IGUI.HorizontalSlider(1, 5, "Generation effect", PLBRG.GenerationEffect, 0F, 1F);
            PLBRG.GenerationSplashEffect = IGUI.HorizontalSlider(1, 6, "Splash effect coefficient", PLBRG.GenerationSplashEffect, 0F, 1F);
            PLBRG.ChanceCoefficient = IGUI.HorizontalSlider(1, 7, "Chance coefficient", PLBRG.ChanceCoefficient, 0F, 0.5F);

            GUI.enabled = true;

            bool Activte = false;
            Learn = IGUI.Button(1, 8, "Learn OFF", "Learn ON", Learn, ref Activte);

            if (Activte && !Learn)
                PLBRG.StopLearn(PCT);

            IGUI.Info(2, 2, "Best generation", PLBRG.BestGeneration);
            IGUI.Info(2, 3, "Generation", PLBRG.Generation);
            IGUI.Info(2, 4, "Children", PLBRG.ChildrenInGeneration);
            IGUI.Info(2, 5, "Best longevity", PLBRG.BestLongevity);

            if (PLBRG.GenerationSplashEffect != 0 || PLBRG.GenerationEffect != 0)
                IGUI.InfoF2(2, 6, "Children difference", PLBRG.ChildrenDifferenceAfterEffects);
            if (PLBRG.ChanceCoefficient!=0)
                IGUI.Info(2, 7, "Chance", PLBRG.Chance);
        }

        if (WindowRect.x < 0)                                           //window restriction on the screen
            WindowRect.x = 0;
        else if (WindowRect.x + WindowRect.width > Screen.width)
            WindowRect.x = Screen.width - WindowRect.width;
        if (WindowRect.y < 0)
            WindowRect.y = 0;
        else if (WindowRect.y + WindowRect.height > Screen.height)
            WindowRect.y = Screen.height - WindowRect.height;

        GUI.DragWindow(new Rect(0, 0, WindowRect.width, 20));
    }
}
