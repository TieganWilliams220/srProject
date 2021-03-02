using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


// using System.Object;
// using Newtonsoft.Json.Linq.JObject;

public class ObjectHandler : MonoBehaviour
{

    public List<globalVar.movement> RdJsonFile(int gen, int m, globalVar.header h, globalVar.Tree  d, globalVar.Tree.Node b )
    {
        string[] words;
        List<globalVar.movement> load = new List<globalVar.movement>();
        globalVar.movement move = new globalVar.movement();
        globalVar.steps st = new globalVar.steps();
        int i = 0 ;
        using (StreamReader r = new StreamReader(Application.dataPath + @"_"+gen+ "_"+h.ObjName+"results.txt"))
        {
           words = r.ReadToEnd().Split(' ');
        }
        
        //hdr += h.ObjName +" " + h.objID +" " + h.generation+" " + h.movementCount;
        if(h.objID == Int32.Parse(words[1]))
        {
            h.ObjName = words[0];
            h.objID= Int32.Parse(words[1]);
            h.generation= Int32.Parse(words[2]);
            h.movementCount= Int32.Parse(words[3]);
            i+=4;
            if(words[4] != "eofHeader")
            {
            }
            else
            {
                i++;
                //d.InternalClear(b);
                b = null;
                while(words[i+1]!= "eofDistData")
                {
                    b = d.InternalInsert(b, new float[2] {float.Parse(words[i]), float.Parse(words[i+1])});
                    i+=2;
                }
                i+=2;
                while(words[i] != "")
                {
                    if(words[i] == ";")
                    {
                            load.Add(move);
                            move = new globalVar.movement();
                            i++;
                    }
                    else if (words[i] != "")
                    {
                        st = new globalVar.steps( Int32.Parse(words[i]),
                        Int32.Parse(words[i+1]),
                        Int32.Parse(words[i+2]));
                        i+=3;
                        move.step.Add(st);
                    }
                }
            }
        }
        return load;
        
    }
    public void WrJsonFile(List<globalVar.movement> a, List<float[]> b, globalVar.header h)
    {
        string json = WrHeaderData(h);
        string exc = WrExclDistData(b);
        json += WrDistData(b);
        foreach ( globalVar.movement l in a)
        {
            foreach ( globalVar.steps m  in l.step)
                    json += (m.joint +" " + m.target  +" "+ m.vector+ " ");
            json +="; " ;
        }
        File.WriteAllText(Application.dataPath + @"_"+h.generation+ "_"+h.ObjName+"results.txt", json);
        File.WriteAllText(Application.dataPath + @"_"+h.generation+ "_"+h.ObjName+"excelData.txt", exc);
    }
    string WrHeaderData(globalVar.header h)
    {
        string hdr = "";
        hdr += h.ObjName +" " + h.objID +" " + h.generation+" " + h.movementCount;
        hdr += " eofHeader ";
        return hdr;
    }
    string WrDistData(List<float[]> b)
    {
        string hdr = "";
        foreach(float[] a in b)
        {
            hdr += a[0]+ " "+a[1]+" ";
        }
        hdr += " eofDistData ";
        return hdr;
    }
        string WrExclDistData(List<float[]> b)
    {
        string hdr = "";
        foreach(float[] a in b)
        {
            hdr += a[0] + "\n";
        }
        return hdr;
    }
}
