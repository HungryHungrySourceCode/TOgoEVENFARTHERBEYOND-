using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formulas
{
    public float Randomizer(float r)
    {
        r = Random.Range(-r, r);
        return r;
    }

    public float StringToFloat(string S)
    {
        float I;
        if (float.TryParse(S, out I) == false)
        {
            I = 0;
        }
        return I;
    }

    public int StringToInt(string S)
    {
        int I;
        if (int.TryParse(S, out I) == false)
        {
            I = 0;
        }
        return I;
    }

    public bool StringToBool(string S)
    {
        bool I;
        if (bool.TryParse(S, out I) == false)
        {
            I = false;
        }
        return I;
    }

    public float[][][] FromArray(float[][][] From)
    {
        float[][][] To = new float[From.Length][][];
        int l = 0;
        while (l < From.Length)
        {
            To[l] = new float[From[l].Length][];
            int k = 0;
            while (k < From[l].Length)
            {
                To[l][k] = new float[From[l][k].Length];
                int j = 0;
                while (j < From[l][k].Length)
                {
                    To[l][k][j] = From[l][k][j];
                    j++;
                }
                k++;
            }
            l++;
        }
        return To;
    }

    public float[] FromArray(float[] From)
    {
        float[] To = new float[From.Length];
        int i = 0;
        while (i < From.Length)
        {
            To[i] = From[i];
            i++;
        }
        return To;
    }

    public int[] FromArray(int[] From)
    {
        int[] To = FromArray(From, 0);
        return To;
    }

    public int[] FromArray(int[] From, int Corection)
    {
        int[] To = new int[From.Length];
        int i = 0;
        while (i < From.Length)
        {
            To[i] = From[i]+ Corection;
            i++;
        }
        return To;
    }

    public void FromArray(float[] From, float[] To)
    {
        int i = 0;
        while (i < From.Length)
        {
            To[i] = From[i];
            i++;
        }
    }
}
