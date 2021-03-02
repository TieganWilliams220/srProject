using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Memory : MonoBehaviour
{
public List<List<globalVar.movement>> hist;
    public Memory()
    {
        hist = new List<List<globalVar.movement>>();
    }
    public void LogGeneration(List<globalVar.movement> generation, int gen)
    {
        List<globalVar.movement> hold = new List<globalVar.movement>();

        foreach (globalVar.movement l in generation)
        {
            hold.Add(l);
        }
        if(gen >= hist.Count || hist.Count ==0 )
        {
            hist.Add(hold);
        }else
        {
            hist[gen]= hold;
        }

        

    }
    public List<globalVar.movement> GetGeneration(int ind)
    {
        if(ind>=0 && ind <hist.Count )
        return hist[ind];
        return null;
    }


}