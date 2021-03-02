using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj : MonoBehaviour
{
    public globalVar.State state;
    public DLMovement mover =null;
    public ImturpriterMover intMov = null;
    public AtRest rest = null;
    public CommandInterface int_ = null;
    public globalVar.header Header = null;
    public Memory memory = null;

    public bool finished = false;
    Vector3[] pos = null;
    Quaternion[] rot = null; 
       Vector3[] pos2 = null;
    Quaternion[] rot2 = null;
    Transform[] h = null;
    //int count = 0;
    
    public int GetCount(){return intMov.count;}
    

    // Start is called before the first frame update
    void Start()
    {
        int_ = GameObject.Find("PlayControles").GetComponent<CommandInterface>();
        Header = new globalVar.header();
        GetPos();
                h = GetComponentsInChildren<Transform>();
        pos2 = new Vector3[h.Length];
        rot2 = new Quaternion[h.Length];
        for(int i = 0; i < h.Length;i++)
        {
            pos2[i] = h[i].position;
            rot2[i] = h[i].rotation;
        }
        memory = new Memory();
        rest = new AtRest(this);
        state = globalVar.State.WAITING;
        CheckConections();
        mover.StartState();
        state = globalVar.State.WAITING;

    }
    public void GetPos()
    {
        
        h = GetComponentsInChildren<Transform>();
        pos = new Vector3[h.Length];
        rot = new Quaternion[h.Length];
        for(int i = 0; i < h.Length;i++)
        {
            pos[i] = h[i].position;
            rot[i] = h[i].rotation;
        }
    }

    public void Update()
    {
        rest.UpdatePosition();
        //CheckConections();
        if(finished == false)
        {

        if(state == globalVar.State.RUNNING)
        {
            if (mover.state == globalVar.State.WAITING && intMov.state == globalVar.State.RUNNING)
            {
                Debug.Log("*****************************ExeMovements***************************");
                state = globalVar.State.WAITING;
                intMov.ExecuteMovementList(mover.thisList);

            }
            else if(intMov.state == globalVar.State.WAITING && mover.state == globalVar.State.RUNNING)
            {
                state = globalVar.State.WAITING;
                
                Debug.Log("*****************************Cluster***************************");
                mover.Cluster();
                Header.generation ++;
                mover.state = globalVar.State.WAITING;
                state = globalVar.State.RUNNING;
                intMov.state = globalVar.State.RUNNING;
            }
            else
            {
               // Debug.Log("*****************************Error in states***************************");
            }
        }
        }else
        {

                            state = globalVar.State.WAITING;
                intMov.ExecuteMovementList(mover.thisList);
        }
    }

public void ExeMove()
{
    if (mover.state == globalVar.State.WAITING 
    && intMov.state == globalVar.State.WAITING
    && state == globalVar.State.WAITING)
    {

        
    }
}
    void CheckConections()
    {
        if(mover == null)
            mover = GetComponent<DLMovement>();
        if(mover.obj == null)
            mover.obj = this;
        if(intMov == null)
            intMov = GetComponent<ImturpriterMover>();
        if(intMov.obj == null)
            intMov.obj = this;
        if(intMov.mover == null)
            intMov.mover = mover;
        if(mover.intMov == null)
            mover.intMov =intMov;

    }
    public void ResetObj()
    {
        Transform[] hold = GetComponentsInChildren<Transform>();
        for (int i = 0; i < hold.Length; i++)
        {
            hold[i].position = pos2[i];
            hold[i].rotation = rot2[i];
        }
        //Debug.Log("Inside reset Obj***********************");
        HingeJoint[] b = GetComponentsInChildren<HingeJoint>();
        JointSpring s = new JointSpring();
        s.targetPosition = 0;
        for (int i = 0; i < b.Length; i++)
        {
            b[i].spring = s;
        }
    }
    public int IncrementSequence()
    {
        //count++;
        //return count;
        return 0;
    }
    public float GetDistDiference()
    {
        float dif = 0.01f;
        Transform[] hold = GetComponentsInChildren<Transform>();
        for (int i = 0; i < hold.Length; i++)
        {
            dif += Vector3.Distance(hold[i].position, pos[i]);
            //Debug.Log("*****************************distence sum***************************"+ dif);
        }
            dif /= hold.Length;
           // Debug.Log("*****************************distence ava***************************"+dif);

        
        return dif;
    }

    public void ChangeState()
    {
        if(state == globalVar.State.RUNNING)
            state = globalVar.State.WAITING;
        else if(state == globalVar.State.WAITING)
            state = globalVar.State.RUNNING;
        else
            state = globalVar.State.WAITING;

    }


    public int GetGeneration()    {return Header.generation;}
    public int GetListSize()    {return mover.thisList.Count;}
    public bool GetIsMoving(){return rest.IsMoving;}
   // public void SetCount(int i){count += i;}

    // List<List<int[]>> TestJoints()
    // {
    //     List<List<int[]>> test = new List<List<int[]>>();
    //     List<int[]> testSequ = new List<int[]>();
    //     //joint: 0 targetPosition: -90 Vector: 1 ~ joint: 0 targetPosition: -90 Vector: 0 ~
    //     //"joint: "+m[0] + " targetPosition: " + m[1] + " Vector: " + m[2]+ " ~ "

        
    //     testSequ.Add(new int[3]{1,90,0});
    //     test.Add(testSequ);
    //     testSequ = new List<int[]>();
        
    //     testSequ.Add(new int[3]{2,90,0});
    //     test.Add(testSequ);
    //     testSequ = new List<int[]>();

    //     // testSequ.Add(new int[3]{1,-90,0});
    //     testSequ.Add(new int[3]{1,-90,0});
    //     test.Add(testSequ);
    //     testSequ = new List<int[]>();

    //     // testSequ.Add(new int[3]{2,-90,0});
    //     testSequ.Add(new int[3]{2,-90,0});
    //     test.Add(testSequ);
    //     testSequ = new List<int[]>();

    //     // testSequ.Add(new int[3]{1,-90,0});
    //     // test.Add(testSequ);
    //     // testSequ = new List<int[]>();

    //     // testSequ.Add(new int[3]{2,-90,0});
    //     // test.Add(testSequ);
    //     // testSequ = new List<int[]>();

    //     // testSequ.Add(new int[3]{1, -90,0});
    //     // test.Add(testSequ);
    //     // testSequ = new List<int[]>();

    //     // testSequ.Add(new int[3]{2,-90,0});
    //     // test.Add(testSequ);
    //     // testSequ = new List<int[]>();

    //     testSequ.Add(new int[3]{1,90,0});
    //     test.Add(testSequ);
    //     testSequ = new List<int[]>();

    //     testSequ.Add(new int[3]{2,90,0});

    //     test.Add(testSequ);
    //     testSequ = new List<int[]>(); 
    //     testSequ.Add(new int[3]{1,-90,0});

    //     test.Add(testSequ);
    //     testSequ = new List<int[]>();
    //     testSequ.Add(new int[3]{2,-90,0});

    //     test.Add(testSequ);
    //     testSequ = new List<int[]>();
    //     testSequ.Add(new int[3]{1,90,0});
    //     test.Add(testSequ);
    //     testSequ = new List<int[]>();

    //     testSequ.Add(new int[3]{2,90,0});
    //     test.Add(testSequ);

    //     return test; 
    // }
}


// testSequ.Add(new int[3]{1,90,3});
//         test.Add(testSequ);
//         testSequ = new List<int[]>();
        
//         testSequ.Add(new int[3]{2,90,3});
//         test.Add(testSequ);
//         testSequ = new List<int[]>();

//           testSequ.Add(new int[3]{3,90,3});
//         testSequ.Add(new int[3]{1,-90,3});
//         test.Add(testSequ);
//         testSequ = new List<int[]>();
//           testSequ.Add(new int[3]{4,90,3});
//         //testSequ.Add(new int[3]{1,-90,3});
//         testSequ.Add(new int[3]{2,-90,3});
//         test.Add(testSequ);
//         testSequ = new List<int[]>();
//           testSequ.Add(new int[3]{5,90,3});
//         testSequ.Add(new int[3]{3,-90,3});
//         testSequ.Add(new int[3]{1,90,3});
//         test.Add(testSequ);
//         testSequ = new List<int[]>();
//         testSequ.Add(new int[3]{1,-90,3});
//         testSequ.Add(new int[3]{4,-90,3});
//         testSequ.Add(new int[3]{2,90,3});
//         test.Add(testSequ);
//         testSequ = new List<int[]>();
//         testSequ.Add(new int[3]{2,-90,3});
//         testSequ.Add(new int[3]{5,-90,3});
//         testSequ.Add(new int[3]{3,90,3});
//         testSequ.Add(new int[3]{1,-90,3});

//         test.Add(testSequ);
//         testSequ = new List<int[]>();
//         testSequ.Add(new int[3]{3,-90,3});
//         testSequ.Add(new int[3]{1,90,3});
//         testSequ.Add(new int[3]{4,90,3});
//         testSequ.Add(new int[3]{2,-90,3});

//         test.Add(testSequ);
//         testSequ = new List<int[]>(); 
//         testSequ.Add(new int[3]{4,-90,3});
//         testSequ.Add(new int[3]{2,90,3});
//         testSequ.Add(new int[3]{1,90,3});
//         testSequ.Add(new int[3]{3,-90,3});
//         testSequ.Add(new int[3]{1,-90,3});

//         test.Add(testSequ);
//         testSequ = new List<int[]>();

//         test.Add(testSequ);
//         testSequ = new List<int[]>();
//         testSequ.Add(new int[3]{4,-90,3});
//         testSequ.Add(new int[3]{1,90,3});
//         test.Add(testSequ);
//         testSequ = new List<int[]>();
//         testSequ.Add(new int[3]{5,-90,3});
//         testSequ.Add(new int[3]{2,90,3});
//         test.Add(testSequ);
