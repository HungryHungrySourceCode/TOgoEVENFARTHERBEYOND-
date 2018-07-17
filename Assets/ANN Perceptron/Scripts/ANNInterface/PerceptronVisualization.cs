using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Perceptron's visualization.
/// </summary>
public class PerceptronVisualization
{
    private GameObject PerceprtonModel;
    private GameObject[][] NeuronModel;
    private GameObject[][][] NeuronWeightModel;
    private Camera Cam;

    /// <summary>
    /// Perceptron.
    /// </summary>
    public Perceptron PCT;

    /// <summary>
    /// Create perceptron's model.
    /// </summary>
    /// <param name="Rectangle">If inputs show like a picture then must be "true".</param>
    /// <param name="RectangleWidth">Rectangle's width in pixels.</param>
    /// <param name="RectangleHeight">Rectangle's height in pixels.</param>
    public void CreatePerceptronModel(bool Rectangle, int RectangleWidth, int RectangleHeight)
    {
        if (PCT != null)
        {
            if (PerceprtonModel != null)
                DestroyPerceptronModel();

            PerceprtonModel = new GameObject();
            PerceprtonModel.name = "Perceprton model";
            PerceprtonModel.transform.position = new Vector3(0, 5000, 0);

            NeuronModel = new GameObject[1 + PCT.NIHL.Length + 1][];
            NeuronWeightModel = new GameObject[1 + PCT.NIHL.Length][][];
            NeuronModel[0] = new GameObject[PCT.Input.Length];

            int l = 0;

            if (Rectangle)
            {
                int i = 0;
                while (i < RectangleHeight)
                {
                    int c = 0;
                    while (c < RectangleWidth)
                    {
                        NeuronModel[0][l] = GameObject.CreatePrimitive(PrimitiveType.Quad);
                        NeuronModel[0][l].transform.parent = PerceprtonModel.transform;
                        NeuronModel[0][l].transform.localPosition = new Vector3(c - RectangleWidth / 2F + 0.5F, i - RectangleHeight / 2F + 0.5F, 0);
                        NeuronModel[0][l].transform.name = "Enter №" + l.ToString();
                        l++;
                        c++;
                    }
                    i++;
                }
                if (PCT.B)
                    NeuronModel[0][l] = CreateNeuronModel(0, l, PCT.Input.Length, true);
            }
            else
            {
                while (l < PCT.Input.Length)
                {
                    if (PCT.B && l == PCT.Input.Length - 1)
                        NeuronModel[0][l] = CreateNeuronModel(0, l, PCT.Input.Length, true);
                    else
                        NeuronModel[0][l] = CreateNeuronModel(0, l, PCT.Input.Length, false);
                    l++;
                }
            }
            l = 1;
            int j = 0;
            int k = 0;
            if (PCT.NIHL.Length != 0)
            {
                while (l < PCT.NIHL.Length + 1)
                {
                    NeuronModel[l] = new GameObject[PCT.NIHL[l - 1]];
                    NeuronWeightModel[l - 1] = new GameObject[PCT.Neuron[l].Length][];
                    j = 0;
                    while (j < PCT.Neuron[l].Length)
                    {
                        if (PCT.B && j == PCT.Neuron[l].Length - 1)
                            NeuronModel[l][j] = CreateNeuronModel(l, j, PCT.Neuron[l].Length, true);
                        else
                            NeuronModel[l][j] = CreateNeuronModel(l, j, PCT.Neuron[l].Length, false);
                        NeuronWeightModel[l - 1][j] = new GameObject[PCT.Neuron[l - 1].Length];
                        k = 0;
                        if (PCT.B)
                        {
                            if (j != PCT.Neuron[l].Length - 1)
                            {
                                while (k < PCT.NeuronWeight[l - 1][j].Length)
                                {
                                    NeuronWeightModel[l - 1][j][k] = CreateNeuronWeightModel(NeuronModel[l - 1][k], NeuronModel[l][j], l, j, k);
                                    k++;
                                }
                            }
                        }
                        else
                        {
                            while (k < PCT.NeuronWeight[l - 1][j].Length)
                            {
                                NeuronWeightModel[l - 1][j][k] = CreateNeuronWeightModel(NeuronModel[l - 1][k], NeuronModel[l][j], l, j, k);
                                k++;
                            }
                        }
                        j++;
                    }
                    l++;
                }
            }
            NeuronModel[l] = new GameObject[PCT.Output.Length];
            NeuronWeightModel[l - 1] = new GameObject[PCT.Neuron[l].Length][];
            j = 0;
            while (j < PCT.Neuron[l].Length)
            {
                NeuronModel[l][j] = CreateNeuronModel(l, j, PCT.Output.Length, false);
                NeuronWeightModel[l - 1][j] = new GameObject[PCT.Neuron[l - 1].Length];
                k = 0;
                while (k < PCT.NeuronWeight[l - 1][j].Length)
                {
                    NeuronWeightModel[l - 1][j][k] = CreateNeuronWeightModel(NeuronModel[l - 1][k], NeuronModel[l][j], l, j, k);
                    k++;
                }
                j++;
            }

            if (Cam == null)
            {
                GameObject CamGO = new GameObject();
                CamGO.transform.parent = PerceprtonModel.transform;
                CamGO.name = "PerceptronVisualizationCamera";
                Cam = CamGO.gameObject.AddComponent<Camera>();
                Cam.fieldOfView = 90;
                Cam.clearFlags = CameraClearFlags.SolidColor;
                Cam.backgroundColor = Color.gray;
                Cam.depth = 100;
            }
        }
        else
            Debug.LogWarning("Рerceptron visualization does not have perceptron.");
    }

    private GameObject CreateNeuronModel(int L, int N, int SP, bool B)
    {
        float S = SP / 2F;
        GameObject NM = new GameObject();
        NM.transform.parent = PerceprtonModel.transform;
        GameObject BG = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        Object.Destroy(BG.GetComponent<CapsuleCollider>());
        if (B)
        {
            Object.Destroy(BG);
            BG = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Object.Destroy(BG.GetComponent<BoxCollider>());
        }
        BG.transform.parent = NM.transform;
        BG.transform.localPosition = new Vector3(0, 0, 0);
        BG.transform.localEulerAngles = new Vector3(90, 0, 0);
        BG.transform.localScale = new Vector3(1.5F, 0.05F, 0.75F);

        NM.name = "NeuronModel Layer " + L.ToString() + " № " + N.ToString();
        NM.transform.localPosition = new Vector3(L * 4, S - N, 0);
        TextMesh TM = NM.AddComponent<TextMesh>();
        TM.text = "1";
        TM.characterSize = 0.2F;
        TM.anchor = TextAnchor.MiddleCenter;
        TM.fontSize = 32;
        TM.color = Color.red;
        return NM;
    }

    private GameObject CreateNeuronWeightModel(GameObject GO1, GameObject GO2, int L, int N, int NN)//int L, int N, int NN, int SP, int FP)
    {
        GameObject NWM = new GameObject();
        NWM.transform.parent = PerceprtonModel.transform;
        NWM.name = "NeuronWeight Layer " + L.ToString() + " from " + N.ToString() + " to " + NN.ToString();
        LineRenderer LR = NWM.AddComponent<LineRenderer>();
        LR.startWidth = 0.025F;
        LR.endWidth = 0.025F;
        LR.positionCount = 2;
        LR.SetPosition(0, GO1.transform.position + new Vector3(0, 0, 0.1F));
        LR.SetPosition(1, GO2.transform.position + new Vector3(0, 0, 0.1F));
        return NWM;
    }

    /// <summary>
    /// Destroy all perceptron visualization models.
    /// </summary>
    public void DestroyPerceptronModel()
    {
        if (PerceprtonModel != null)
        {
            Object.Destroy(PerceprtonModel);
            NeuronModel = null;
            NeuronWeightModel = null;
            Cam = null;
        }
    }

    /// <summary>
    /// Display all changes in the perceptron.
    /// </summary>
    /// <param name="T2D">Texture for visualization of inpunts.</param>
    public void DisplayPerceptronModel(Texture2D T2D)
    {
        if (PCT != null)
        {
            int k = 0;
            TextMesh TM;
            Color color = new Color();
            color.b = 0;
            color.a = 1;

            if (T2D != null)
            {
                int i = 0;
                int n = 0;
                while (i < T2D.height)
                {
                    int c = 0;
                    while (c < T2D.width)
                    {
                        Color col = T2D.GetPixel(c, i);
                        NeuronModel[0][n].GetComponent<Renderer>().material.color = col;
                        n++;
                        c++;
                    }
                    i++;
                }
            }
            else
            {
                while (k < PCT.Input.Length)
                {
                    TM = NeuronModel[0][k].GetComponent<TextMesh>();
                    TM.text = PCT.Neuron[0][k].ToString("f2");
                    color.r = 0;
                    color.g = 0;
                    if (PCT.Neuron[0][k] > 0)
                        color.g = PCT.Neuron[0][k];
                    else if (PCT.Neuron[0][k] < 0)
                        color.r = -PCT.Neuron[0][k];
                    TM.color = color;
                    k++;
                }
            }
            int l = 1;
            k = 0;
            while (l < PCT.Neuron.Length)
            {
                k = 0;
                while (k < PCT.Neuron[l].Length)
                {
                    TM = NeuronModel[l][k].GetComponent<TextMesh>();
                    TM.text = PCT.Neuron[l][k].ToString("f2");
                    color.r = 0;
                    color.g = 0;
                    if (PCT.Neuron[l][k] > 0)
                        color.g = PCT.Neuron[l][k];
                    else if (PCT.Neuron[l][k] < 0)
                        color.r = -PCT.Neuron[l][k];
                    TM.color = color;
                    k++;
                }
                l++;
            }
            l = 0;
            k = 0;
            while (l < PCT.NeuronWeight.Length)
            {
                int j = 0;
                while (j < PCT.NeuronWeight[l].Length)
                {
                    k = 0;
                    while (k < PCT.NeuronWeight[l][j].Length)
                    {
                        float S = 0.025F * Mathf.Abs(PCT.NeuronWeight[l][j][k]);
                        LineRenderer LR = NeuronWeightModel[l][j][k].GetComponent<LineRenderer>();
                        LR.startWidth = S;
                        LR.endWidth = S * Mathf.Abs(PCT.Neuron[l][k]);
                        color.r = 0;
                        color.g = 0;
                        if (PCT.NeuronWeight[l][j][k] > 0)
                            color.g = PCT.NeuronWeight[l][j][k];
                        else if (PCT.NeuronWeight[l][j][k] < 0)
                            color.r = -PCT.NeuronWeight[l][j][k];

                        LR.material.color = color;
                        k++;
                    }
                    j++;
                }
                l++;
            }
            CameraPosition();
        }
    }

    private void CameraPosition()
    {
        float X = (NeuronModel[0][0].transform.localPosition.x + NeuronModel[NeuronModel.Length - 1][0].transform.localPosition.x) / 2F;
        float Z = (NeuronModel[NeuronModel.Length - 1][0].transform.localPosition.x - NeuronModel[0][0].transform.localPosition.x) / 2F;
        float minY = 0;
        float maxY = 0;
        int i = 0;
        while (i < NeuronModel.Length)
        {
            minY = Mathf.Min(minY, NeuronModel[i][NeuronModel[i].Length - 1].transform.localPosition.y);
            maxY = Mathf.Max(maxY, NeuronModel[i][0].transform.localPosition.y);
            i++;
        }
        float Y = (minY + maxY) / 2F;
        Z = Mathf.Max(Z, (maxY - minY) / 2F) * 1.25F;
        Cam.transform.eulerAngles = new Vector3(0, 0, 0);
        Cam.transform.localPosition = new Vector3(X, Y, -Z);
        Cam.nearClipPlane = Z - 0.2F;
        Cam.farClipPlane = Z + 0.2F;
    }
}
