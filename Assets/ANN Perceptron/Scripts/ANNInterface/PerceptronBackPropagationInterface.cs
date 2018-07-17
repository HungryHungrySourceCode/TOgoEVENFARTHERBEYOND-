using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface of perceptron's back propagation learning method.
/// </summary>
public class PerceptronBackPropagationInterface : MonoBehaviour
{
    private InterfaceGUI IGUI = new InterfaceGUI();

    /// <summary>
    /// Perceptron.
    /// </summary>
    public Perceptron PCT;

    /// <summary>
    /// Array of tasks.
    /// </summary>
    public float[][] Task;

    /// <summary>
    /// Array of answers.
    /// </summary>
    public float[][] Answer;

    /// <summary>
    /// Is persetron learning?
    /// </summary>
    public bool Learn = false;

    /// <summary>
    /// Perceptron's back propagation learning method.
    /// </summary>
    public PerceptronLernByBackPropagation PLBBP = new PerceptronLernByBackPropagation();
    
    private bool ModificateStartWeights = false;

    private Rect WindowRect = new Rect(Screen.width, Screen.height, 400, 230);

    void Start()
    {
        if (PCT==null)
            Debug.LogWarning("Perceptron back propagation interface does not have perceptron.");
    }

    void Update()
    {
        PerceptronInterface PI = gameObject.GetComponent<PerceptronInterface>();    // check if this GO have perceptron interface

        if (Learn && PCT != null && Answer!=null)
        {
            if (Task==null)
                PLBBP.Learn(PCT, Answer[0]);                                               // perceptron's learn
            else
                PLBBP.Learn(PCT, Task, Answer);                                         // perceptron's learn
            Learn = !PLBBP.Learned;
            
            if (PI != null && PI.PV!=null)
                PI.PV.DisplayPerceptronModel(null);                                     // display perceptron model
        }

        if (PI != null)
            if (PI.Reload)
            {
                PLBBP.LearnIteration = 0;
                PLBBP.ModificateStartWeights(PCT, ModificateStartWeights);
                if (PI != null && PI.PV != null)
                    PI.PV.DisplayPerceptronModel(null);                                     // display perceptron model
            }
    }

    void OnGUI()
    {
        if (PCT != null)
            WindowRect = GUI.Window(GUIUtility.GetControlID(FocusType.Passive), WindowRect, Window, "Back propagation of " + transform.name);    // interface window
    }

    void Window(int ID)                                                                             // interface window
    {
        if (WindowRect.height == 80)                                                                // small window
        {
            if (GUI.Button(new Rect(WindowRect.width - 20, 0, 20, 20), "+"))                        // change window scale
            {
                WindowRect.width = 400;
                WindowRect.height = 230;
                WindowRect.x = WindowRect.x - 200;
            }

            Learn = IGUI.Button(1, 2, "Learn OFF", "Learn ON", Learn);                              // start learn
        }
        else                                                                                        // big window
        {
            if (GUI.Button(new Rect(WindowRect.width - 20, 0, 20, 20), "-"))                        // change window scale
            {
                WindowRect.width = 200;
                WindowRect.height = 80;
                WindowRect.x = WindowRect.x+200;
            }
            if (Learn)                                                                              
                GUI.enabled = false;
            bool Temp = false;                                                                     
            ModificateStartWeights = IGUI.Button(1, 2, "Weights from -0.5 to 0.5", "Mod Weights", ModificateStartWeights, ref Temp);
            if (Temp)
            {
                PLBBP.ModificateStartWeights(PCT, ModificateStartWeights);
                PerceptronInterface PI = gameObject.GetComponent<PerceptronInterface>();
                if (PI != null)
                    if (PI.ShowVisualization)
                        PI.PV.DisplayPerceptronModel(null);
            }

            PLBBP.ShuffleSamples = IGUI.Button(1, 3, "Samples one by one", "Shuffle samples", PLBBP.ShuffleSamples);
            GUI.enabled = true;

            if (Learn && Task == null)
                GUI.enabled = false;
            PLBBP.LearningSpeed = (int)IGUI.HorizontalSlider(1, 4, "Learning speed", PLBBP.LearningSpeed, 1, 1800);
            GUI.enabled = true;
            PLBBP.LearningRate = IGUI.HorizontalSlider(1, 5, "Learning rate", PLBBP.LearningRate, 0, 1);
            if (Learn && Task == null)
                GUI.enabled = false;
            PLBBP.DesiredMaxError = IGUI.HorizontalSlider(1, 6, "Desired max error", PLBBP.DesiredMaxError, 0, 1);
            GUI.enabled = true;

            Learn = IGUI.Button(1, 7, "Learn OFF", "Learn ON", Learn);                                         // start learn

            IGUI.Info(2, 2, "Iteration", PLBBP.LearnIteration);
            if (Task != null)
                IGUI.Info(2, 3, "Max error", PLBBP.MaxError);
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
