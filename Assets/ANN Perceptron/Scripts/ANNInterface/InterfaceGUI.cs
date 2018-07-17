using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceGUI
{
    public bool Button(int Column, int Line, string NameTrue)
    {
        bool Temp = Button(Column, Line, NameTrue, NameTrue, false);
        return Temp;
    }

    public bool Button(int Column, int Line, string NameFalse, string NameTrue, bool Parameter, ref bool Activate)
    {
        bool Temp = Button(Column, Line, NameFalse, NameTrue, Parameter);
        if (Temp != Parameter)
            Activate = true;
        return Temp;
    }

    public bool Button(int Column, int Line, string NameFalse, string NameTrue, bool Parameter)
    {
        string TempString = NameFalse;
        if (Parameter)
            TempString = NameTrue;
        if (GUI.Button(new Rect(10 * Column + (Column - 1) * 180, 10 + (Line - 1) * 30, 180, 30), TempString))
        {
            if (Parameter)
                Parameter = false;
            else
                Parameter = true;
        }
        return Parameter;
    }

    public float HorizontalSlider(int Column, int Line, string Name, float Parameter, float Min, float Max, ref bool Activate)
    {
        float Temp = HorizontalSlider(Column, Line, Name, Parameter, Min, Max);

        if (Temp != Parameter)
            Activate = true;
        return Temp;
    }

    public float HorizontalSlider(int Column, int Line, string Name, float Parameter, float Min, float Max)
    {
        GUI.Box(new Rect(10 * Column + (Column - 1) * 180, 10 + (Line - 1) * 30, 180, 30),"");

        Parameter = GUI.HorizontalSlider(new Rect(10 * Column + (Column - 1) * 180, 10 + (Line - 1) * 30 + 18, 180, 12), Parameter, Min, Max);
        GUI.Label(new Rect(10 * Column + (Column - 1) * 180, 10 + (Line - 1) * 30, 180, 22), Name + ": " + Parameter.ToString("f2"));
        return Parameter;
    }

    public int IntArrows(int Column, int Line, string Name, bool OnePlus, int Parameter, int Min, bool Infinity, int Max, ref bool Activate)
    {
        int Temp = IntArrows(Column, Line, Name, OnePlus, Parameter, Min, Infinity, Max);
        if (Temp != Parameter)
            Activate = true;
        return Temp;
    }

    public int IntArrows(int Column, int Line, string Name, bool OnePlus, int Parameter, int Min, bool Infinity, int Max)
    {
        GUI.Box(new Rect(10 * Column + (Column - 1) * 180, 10 + (Line - 1) * 30, 180, 30), "");

        GUI.Label(new Rect(10 * Column + (Column - 1) * 180 + 20, 10 + (Line - 1) * 30, 100, 22), Name + ": ");
        if (GUI.Button(new Rect(10 * Column + (Column - 1) * 180, 10 + (Line - 1) * 30, 20, 30), "<"))
        {
            Parameter--;
        }
        if (GUI.Button(new Rect(10 * Column + (Column - 1) * 180 + 160, 10 + (Line - 1) * 30, 20, 30), ">"))
        {
            Parameter++;
        }

        if (Parameter < Min)
            Parameter = Min;
        if (Parameter > Max && !Infinity)
            Parameter = Max;
        Name = Parameter.ToString();
        if (OnePlus)
            Name = (Parameter + 1).ToString();
        GUI.Label(new Rect(10 * Column + (Column - 1) * 180 + 120, 10 + (Line - 1) * 30, 30, 30), Name);
        return Parameter;
    }

    public void Info(int Column, int Line, string Name, float Parameter)
    {
        GUI.Box(new Rect(10 * Column + (Column - 1) * 180, 10 + (Line - 1) * 30, 180, 30), "");
        GUI.Label(new Rect(10 * Column + (Column - 1) * 180, 10 + (Line - 1) * 30, 180, 22), Name + " :" + Parameter);
    }

    public void InfoF2(int Column, int Line, string Name, float Parameter)
    {
        GUI.Box(new Rect(10 * Column + (Column - 1) * 180, 10 + (Line - 1) * 30, 180, 30), "");
        GUI.Label(new Rect(10 * Column + (Column - 1) * 180, 10 + (Line - 1) * 30, 180, 22), Name + " :" + Parameter.ToString("f2"));
    }
}
