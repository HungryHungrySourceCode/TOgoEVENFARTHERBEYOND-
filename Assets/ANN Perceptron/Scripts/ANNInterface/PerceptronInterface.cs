using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Perceptron interface.
/// </summary>
public class PerceptronInterface : MonoBehaviour
{
    /// <summary>
    /// Perceptron.
    /// </summary>
    public Perceptron PCT;

    /// <summary>
    /// How meny enters have perceptron.
    /// </summary>
    public int Enters = 0;

    private int Layers = 0;
    private int SelectLayer = 0;
    private int[] Layer;

    /// <summary>
    /// Reload perceptron.
    /// </summary>
    public bool Reload = true;

    private InterfaceGUI IGUI = new InterfaceGUI();

    private Rect WindowRect = new Rect(0, Screen.height, 400, 260);

    /// <summary>
    /// Perceptron visualization script.
    /// </summary>
    public PerceptronVisualization PV;

    /// <summary>
    /// Show visualization.
    /// </summary>
    public bool ShowVisualization = false;

    private string PerceptronName = "";

    void Start()
    {
        if (gameObject.name == "StudentChild")
            Destroy(gameObject.GetComponent<PerceptronInterface>());
        else
        {
            if (PCT != null)
            {
                Enters = PCT.Input.Length;              //how many enters in perceptron for interface
                if (PCT.B)
                    Enters = PCT.Input.Length - 1;

                Layer = new int[PCT.NIHL.Length];       //array of hiden layer for interface
                int i = 0;
                while (i < Layer.Length)
                {
                    if (PCT.B)
                        Layer[i] = PCT.NIHL[i] - 1;
                    else
                        Layer[i] = PCT.NIHL[i];
                    i++;
                }
            }
            else
                Debug.LogWarning("Рerceptron interface does not have perceptron.");
        }
    }

    void Update()
    {
        if (ShowVisualization && PCT != null)                                                    // perceptron's visualization (if it ON)
            if (PV != null)
                PV.DisplayPerceptronModel(null);
    }

    void OnGUI()
    {
        if (PCT != null)
            WindowRect = GUI.Window(GUIUtility.GetControlID(FocusType.Passive), WindowRect, Window, "Perceptron of " + transform.name);  // interface window
    }

    void Window(int ID)                                                                     // interface window
    {
        bool SV = false;
        Reload = false;
        if (WindowRect.height == 80)                                                        // small window
        {
            if (GUI.Button(new Rect(WindowRect.width - 20, 0, 20, 20), "+"))                // change window scale
            {
                WindowRect.width = 400;
                WindowRect.height = 260;
                WindowRect.x = WindowRect.x;
            }
            ShowVisualization = IGUI.Button(1, 2, "Visualization OFF", "Visualization ON", ShowVisualization, ref SV);  // perceptron's visualization (ON / OFF)
        }
        else                                                                                // big window
        {
            if (GUI.Button(new Rect(WindowRect.width - 20, 0, 20, 20), "-"))                // change window scale
            {
                WindowRect.width = 200;
                WindowRect.height = 80;
                WindowRect.x = WindowRect.x;
            }
            PerceptronBackPropagationInterface PBPI = gameObject.GetComponent<PerceptronBackPropagationInterface>();
            if (PBPI != null)
                if (PBPI.Learn)
                    GUI.enabled = false;                                                                // disable GUI if GO have lerning interface and perceptron is study
            PerceptronRandomGenerationInterface PRGI = gameObject.GetComponent<PerceptronRandomGenerationInterface>();
            if (PRGI != null)
                if (PRGI.Learn)
                    GUI.enabled = false;                                                                // disable GUI if GO have lerning interface and perceptron is study
            PCT.AFWM = IGUI.Button(1, 2, "Without Minus", "With Minus", PCT.AFWM, ref Reload);
            PCT.B = IGUI.Button(1, 3, "Bias OFF", "Bias ON", PCT.B, ref Reload);
            PCT.AFS = IGUI.HorizontalSlider(1, 4, "Scale", PCT.AFS, 0.1F, 5F, ref Reload);
            Enters = IGUI.IntArrows(1, 5, "Enters", false, Enters, 1, true, 0);
            Layers = IGUI.IntArrows(1, 6, "Layers", false, PCT.NIHL.Length, 0, true, 0, ref Reload);

            if (Enters != PCT.Input.Length && !PCT.B)
            {
                Reload = true;
            }
            else if (Enters != PCT.Input.Length - 1 && PCT.B)
            {
                Reload = true;
            }

            if (Reload)
            {
                LayersModification();       // create new array of hiden layer
            }
            if (Layers != 0)
            {
                SelectLayer = IGUI.IntArrows(1, 7, "Select Layer", true, SelectLayer, 0, false, Layers - 1);
                Layer[SelectLayer] = IGUI.IntArrows(1, 8, "Neurons in layer", false, Layer[SelectLayer], 1, true, 0, ref Reload);
            }
            GUI.enabled = true;
            IGUI.Info(2, 5, "Exits", PCT.Output.Length);

            ShowVisualization = IGUI.Button(2, 2, "Visualization OFF", "Visualization ON", ShowVisualization, ref SV);          // perceptron's visualization (ON / OFF)

            PerceptronName = GUI.TextField(new Rect(200, 160, 180, 30), PerceptronName);
            bool Save = false;
            Save = IGUI.Button(2, 7, "Save", "Save", Save);
            bool Load = false;
            Load = IGUI.Button(2, 8, "Load", "Load", Load);
            if (Save)
                PCT.Save(PerceptronName);
            else if (Load)
            {
                PCT.Load(PerceptronName);
                Layers = PCT.NIHL.Length;
                Layer = new Formulas().FromArray(PCT.NIHL);
                if (PCT.B)
                {
                    Enters = PCT.Input.Length - 1;
                    int i = 0;
                    while (i < Layers)
                    {
                        Layer[i]--;
                        i++;
                    }
                }
                else
                    Enters = PCT.Input.Length;
                if (PV != null)
                {
                    PV.DestroyPerceptronModel();
                    PV.PCT = PCT;
                    PV.CreatePerceptronModel(false, 0, 0);
                }
            }
        }

        if (Reload)
        {
            if (PV != null)
                PV.DestroyPerceptronModel();

            PCT.CreatePerceptron(PCT.AFS, PCT.B, PCT.AFWM, Enters, Layer, PCT.Output.Length);   // create perceptron
        }

        if (SV || (ShowVisualization && Reload))
        {
            if (ShowVisualization)                                              // for perceptron's visualization
            {
                if (PV == null)
                    PV = new PerceptronVisualization();

                PV.PCT = PCT;
                PV.CreatePerceptronModel(false, 0, 0);
            }
            else
            {
                if (PV != null)
                {
                    PV.DestroyPerceptronModel();
                    PV = null;
                }
            }
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

    void LayersModification()       // create new array of hiden layer
    {
        if (Layers != 0)
        {
            int i = 0;
            int[] OldLayer = new int[Layer.Length];
            while (i < OldLayer.Length)
            {
                OldLayer[i] = Layer[i];
                i++;
            }
            Layer = new int[Layers];
            i = 0;
            while (i < Mathf.Min(OldLayer.Length, Layer.Length))
            {
                Layer[i] = OldLayer[i];
                i++;
            }
            while (i < Layer.Length)
            {
                Layer[i] = 1;
                i++;
            }
        }
        else
            Layer = new int[0];
    }
}
