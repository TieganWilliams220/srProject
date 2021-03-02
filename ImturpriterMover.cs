using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Timers;
public class ImturpriterMover : MonoBehaviour
{
    public globalVar.State state;
    public Obj obj;
    public DLMovement mover;
    GameObject beacon;
    public globalVar.Tree.Node root;
    public globalVar.Tree results = new globalVar.Tree();
    private  System.Timers.Timer aTimer;
    public int count = 0;
    public int count2 = 0;
    public CommandInterface int_ = null;

     public Vector3 gData;
     int tickerTwo = 0;
     int num = 40;
    public bool doneExe = false;
    void Start()
    {  state = globalVar.State.WAITING; 
        int_ = GameObject.Find("PlayControles").GetComponent<CommandInterface>();
    }
    void Update()
    {
        if(obj.finished == false && int_.isOn == false)
        {
            if(state == globalVar.State.RUNNING && obj.state == globalVar.State.WAITING)
            {
                if( !obj.rest.IsMoving)
                {
                    if(count < mover.thisList.Count)
                    {
                        if(tickerTwo < 0)
                        {
                            if (count2 < mover.thisList[count].step.Count)
                            {
                                globalVar.steps c = mover.thisList[count].step[count2];
                                obj.GetComponentsInChildren<HingeJoint>()[c.joint].GetComponent<HingeJoint>().axis = ConvertToVector(c.vector);
                                JointSpring s = new JointSpring();
                                s.spring = 90;
                                s.damper = 10;
                                s.targetPosition = c.target; //position
                                obj.GetComponentsInChildren<HingeJoint>()[c.joint].GetComponent<HingeJoint>().spring = s;
                                obj.GetComponentsInChildren<HingeJoint>()[c.joint].GetComponent<HingeJoint>().useSpring = true;
                                tickerTwo = num;
                                count2++;
                            }
                            else if (count2 == mover.thisList[count].step.Count)
                            {
                                float hold = obj.GetDistDiference();
                                if(count >=0 && count < mover.thisList.Count)
                                {
                                    root = results.InternalInsert(root,new float[2] {hold, count});
                                    Debug.Log("index: "+count+" size:"+mover.thisList[count].step.Count+" Dist:"+ hold );
                                }
                                else
                                    Debug.Log("*********************************Error*********************************\n"+
                                    "Count is out of range, index: "+count+" Dist:"+ hold +" was not added");

                                obj.ResetObj();
                                tickerTwo = num;
                                count2++;
                            }
                            else
                            {
                                count2 = 0;
                                count++; 
                                obj.Header.movementCount= count;
                                obj.GetPos();
                            }
                        }else{tickerTwo --;}
                    }
                    else{
                        Debug.Log("*****************************writing file***************************");
                        int_.wrFile();
                        Debug.Log("*****************************file written***************************");
                        obj.memory.LogGeneration(mover.thisList, obj.Header.generation);
                        CullMovementList2();
                        Debug.Log(" Done ExecuteMovementList time:"+ DateTime.Now + " count:"+ count);
                        count = 0;
                        obj.ResetObj();
                        Debug.Log("Done cull move list:"+ DateTime.Now);
                    }
                }
            }
        }else
        {
            if(tickerTwo < 0)
            {
                if (count2 < mover.thisList[count].step.Count)
                {
                    globalVar.steps c = mover.thisList[count].step[count2];
                    obj.GetComponentsInChildren<HingeJoint>()[c.joint].GetComponent<HingeJoint>().axis = ConvertToVector(c.vector);
                    JointSpring s = new JointSpring();
                    s.spring = 90;
                    s.damper = 10;
                    s.targetPosition = c.target; //position
                    obj.GetComponentsInChildren<HingeJoint>()[c.joint].GetComponent<HingeJoint>().spring = s;
                    obj.GetComponentsInChildren<HingeJoint>()[c.joint].GetComponent<HingeJoint>().useSpring = true;
                    tickerTwo = num;
                    count2++;
                }
                else if (count2 == mover.thisList[count].step.Count)
                {
                    if(count >=0 && count < mover.thisList.Count)
                    {
                    }
                    else
                        Debug.Log("*********************************Error*********************************\n");
                    tickerTwo = num;
                    count2++;
                }
                else
                {
                    count2 = 0;
                    obj.Header.movementCount= count;
                    obj.GetPos();
                }
            }else{tickerTwo --;}
        }
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
/*************  List<List<int[]>> are arrays of movements     **********************
    list<sequwnce in list of movements 
        list<sequwnce in movement 
            int[JOINT, TARG_POSITION, X_, Y_, Z_ ]>>
*************************************************************************************************/

     public  void ExecuteMovementList(List<globalVar.movement> a)//exe full list
    {
            //mover.thisList =  a;
        obj.state = globalVar.State.WAITING;
    }

    /*************  void CullMovementList(List<List<int[]>> a)     **********************
     * 
     * 
     * 
    *************************************************************************************************/
    // public List<globalVar.movement> CullMovementList(List<globalVar.movement> a)
    // {
    //     if(results.size > 0)
    //     {
    //         List<float[]> soarted = new List<float[]>();
    //         // soarted = results.traverse(root,(int)(20));
    //         if(results.size > 100)
    //         soarted = results.traverse(root,(int)(results.size*0.5));
    //         List<globalVar.movement> temp = new List<globalVar.movement>();
    //         foreach(float[] f in soarted)
    //         {
    //             if((int)f[1] >=0 && (int)f[1]< a.Count)
    //             temp.Add(a[(int)f[1]]);
    //         }

    //         mover.thisList = temp;
    //         results.InternalClear(root);
    //         state = globalVar.State.WAITING;
    //         obj.state = globalVar.State.RUNNING;
    //         mover.state = globalVar.State.RUNNING;
    //         //mover.thisList = temp;
    //         return temp;
    //     }else 
    //     {
    //         state = globalVar.State.RUNNING;
    //         obj.state = globalVar.State.WAITING;
    //         mover.state = globalVar.State.WAITING;
    //         return a;
    //     }

    // }
    public List<float[]> GetData()
    {
        List<float[]> soarted = new List<float[]>();
        soarted = results.traverse(root, obj.Header.movementCount);
        return soarted;
    }
    public void CullMovementList2()
    {
        if(results.size > 0)
        {
            List<float[]> soarted = new List<float[]>();
            soarted = results.traverse(root, (int)(10));

            List<globalVar.movement> temp = new List<globalVar.movement>();
            foreach(float[] f in soarted)
            {
                if((int)f[1] > 0 && (int)f[1] < mover.thisList.Count)
                    temp.Add(mover.thisList[(int)f[1]-1]);
                else
                    Debug.Log("*********************************Error*********************************\n"+
                    "Count is out of range, index: "+((int)f[1] -1)+" was not added during cull Movements");
            }

            mover.thisList = temp;
           // mover.thisList = temp;
            results.InternalClear(root);
            root = null;
            state = globalVar.State.WAITING;
            obj.state = globalVar.State.RUNNING;
            mover.state = globalVar.State.RUNNING;
        }else 
        {
            state = globalVar.State.RUNNING;
            obj.state = globalVar.State.WAITING;
            mover.state = globalVar.State.WAITING;
        }

    }

    Vector3 ConvertToVector(int a)
    {
        Vector3 m_NewPosition = new Vector3();
        switch (a)
        {
            case 0:
                // m_NewPosition.Set(1, 0, 0);
                 m_NewPosition.Set(0, 1, 0);
                break;
            case 1:
                m_NewPosition.Set(1,0,0);//z
                break;
            case 2:
                m_NewPosition.Set(0, 0, 1);
                break;
            case 3:
                m_NewPosition.Set(-1,0,0);
                //m_NewPosition.Set(1, 1, 0);
                break;
            case 4:
                m_NewPosition.Set(1, 0, 1);
                break;
            case 5:
                m_NewPosition.Set(0, 1, 1);
                break;
            case 6:
                m_NewPosition.Set(1, 1, 1);
                break;
            default:
                m_NewPosition.Set(0, 0, 0);
                break;
        }
        return m_NewPosition;
        //return m_NewPosition.Set(0, 0, 0);
    }

    
//    public List<List<int[]>> BlindCull(List<List<int[]>>  a)
//     {
//         List<List<int[]>> temp = new List<List<int[]>>();
//         for(int i = 0; i< a.Count*(.01) ; i++)
//         {
//             temp.Add(a[i]);
//         }
//         return temp;
//     }

}
    