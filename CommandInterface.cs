using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using System;

public class CommandInterface : MonoBehaviour
{
    ObjectHandler handl;
    public Obj focused = null;

    Settings set;
    Text t = null;
    Text GenCount = null;
    Text ListSize = null;
    Text IsMoving = null;
    Toggle loop = null;
    public bool isOn = false;
    GameObject Graph = null;
    // public Dropdown myDropdown;
      
                                    //x,y,w,h
    // public Rect windowRect = new Rect(0, 0,850, 300);
    // public Rect SettingsRect = new Rect(0, 0,850, 300);
    // //GUI set_ ;
    //ToolBar tool;
    // Canvas can;

    public void Update()
    {
        Button play =  GameObject.Find("Play").GetComponent<Button>();
        if(focused.state == globalVar.State.WAITING)
            play.GetComponentInChildren<Text>().text = "||";
         else
            play.GetComponentInChildren<Text>().text = ">";
        t.GetComponentInChildren<Text>().text =  (focused.GetCount()).ToString();  
        GenCount.GetComponentInChildren<Text>().text =  (focused.GetGeneration()+1).ToString();  
        ListSize.GetComponentInChildren<Text>().text =  (focused.GetListSize()).ToString();  
        IsMoving.GetComponentInChildren<Text>().text =  (focused.rest.IsMoving).ToString();
       // isOn = focused.GetComponent<Toggle>().isOn;
       //Toggle tog = GameObject.Find("Loop").GetComponent<Toggle>();
        if(loop.isOn)
            isOn = true;
        else
            isOn = false;
//        UpdateGraph(focused.intMov.gData);

                    // Debug.Log("inside comand update");


    }
    void Start()
    {
        handl = GameObject.Find("ProtoObj").GetComponent<ObjectHandler>();
        focused = GameObject.Find("ProtoObj").GetComponent<Obj>();
         t = GameObject.Find("count").GetComponent<Text>();
         GenCount = GameObject.Find("GenCount").GetComponent<Text>();
         ListSize = GameObject.Find("SizeCount").GetComponent<Text>();
         IsMoving = GameObject.Find("IsMoving").GetComponent<Text>();
         loop = GameObject.Find("Loop").GetComponent<Toggle>();
        focused.state = globalVar.State.WAITING;
        focused.intMov.state = globalVar.State.WAITING;
        focused.mover.state = globalVar.State.WAITING;
        Graph = GameObject.Find("Graph");

    }
    public void onPlayClick()
    {
        //  Button go = (Button)GameObject.Find("Play");
            focused.ChangeState();
            focused.intMov.ChangeState();

    }
    public void wrFile()
    {

        handl.WrJsonFile(focused.mover.thisList, 
        focused.intMov.GetData(),
        focused.Header);
            //         focused.ChangeState();
            // focused.intMov.ChangeState();
    }
    public void onNextClick()
    {
        if(focused.state == globalVar.State.RUNNING){
            
        }
    }
    public void onNext100Click()
    {
        if(focused.state == globalVar.State.RUNNING){
            // focused.SetCount(100);
        }
    }
    public void onRepeatClick()
    {
        if(focused.state == globalVar.State.RUNNING){
            //focused.ResetObj();
            //focused.IncrementSequence();
            // focused.ExeMove();
        }
    }
    public void onSaveClick()
    {
        if(focused.state == globalVar.State.RUNNING){
            handl.WrJsonFile(focused.mover.thisList, 
                focused.intMov.GetData(),
                focused.Header);
        }

    }
    public void onLoadClick()
    {
        if(focused.state == globalVar.State.RUNNING){

            //InputField gen =  GameObject.Find("Gen").GetComponent<InputField>();
            //InputField move =  GameObject.Find("Move").GetComponent<InputField>();
            focused.mover.thisList = handl.RdJsonFile(0, 0, focused.Header, focused.intMov.results, focused.intMov.root);
            focused.ResetObj();
            focused.intMov.count = focused.Header.movementCount;
            //focused.intMov.exeList = focused.mover.thisList;
           // focused.mover.PrintList(focused.mover.thisList);
        }

    }
    public void onLoadGenClick()
    {
        if(focused.state == globalVar.State.RUNNING){

            InputField gen =  GameObject.Find("Gen").GetComponent<InputField>();
            InputField move =  GameObject.Find("Move").GetComponent<InputField>();
            if(gen.text != null)
            {
            List<globalVar.movement> possList = focused.memory.GetGeneration(Int32.Parse(gen.text));
            if( possList != null)
            {
                focused.mover.thisList = possList;
                if(move.text != "")
                focused.intMov.count = Int32.Parse(move.text);
            }

            }
            //focused.intMov.exeList = focused.mover.thisList;
           // focused.mover.PrintList(focused.mover.thisList);
        }

    }

    public void onNextGenerationClick()
    {
        if(focused.state == globalVar.State.WAITING){}
        // focused.ExecuteCluster();
    }
    public void onRestartClick()
    {
        if(focused.state == globalVar.State.WAITING){}
        // focused.Restart();

    }
    public void onMenuClick()
    {


    }

        public void UpdateGraph(Vector3 v)
    {
        GameObject gameObject = new GameObject("Child");
        gameObject.transform.SetParent(Graph.transform);
        if (Graph != null)
        {
            Text dote =  gameObject.AddComponent<Text>();
        // Text dote = new Text(); 
            if (dote != null)
            {
            dote.transform.SetParent(gameObject.transform);
                //Text dote =  Graph.AddComponent<Text>();
                //dote = Graph.AddComponent<Text>();
                //Vector3 pos = new Vector3(dist, size);
                if(v != null)
                {
                    dote.transform.position = v;
                    dote.text = ".";
                    Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
                    dote.font = ArialFont;
                    dote.material = ArialFont.material;
                }
            }
        }


    }
    void OnSettings(int id)
    {
            print("2Settings Got a click");
    }



}
