using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/**************************************************************************************************
 * Class: DLMovement
 * Description: 
 * 
 * varyables:
 *      public:
 *                  globalVar.State DLMstate;
 *                  int directionalThresh = 1; //or some arbritraty set #, will eventualy be set to var in settings 		
 *                  int threshold = 5;  //or some arbritraty set #, will eventualy be set to var in settings 		
 *                  GameObject obj_ ; //refference to parent object
 *                  Obj parent_ ;
 *                  ImturpriterMover intMov;
 *                  List<globalVar.movement> thisList = new List<globalVar.movement>();
 *      privat:
 *                  List<globalVar.movement> lastList = new List<globalVar.movement>();
 *                  List<globalVar.movement> possList = new List<globalVar.movement>();
 *                  globalVar.movement jointRestrictions = new globalVar.movement();
 *                  
 *        -------------------  thisList, lastList, possList are arrays of movements     ----------------
 *                list<sequwnce in list of movements 
 *                    list<sequwnce in movement 
 *                        int[JOINT, TARG_POSITION, X_, Y_, Z_ ]>>
 *        -----------------------------------------------------------------------------------------------
 *        -------------------  jointRestrictions  -------------------
 *            int[JOINT, MIN, MAX, X_, Y_, Z_ ]>>
 *        -----------------------------------------------------------
 * 
 * Methods:
 * 
 * 
 * 
 **************************************************************************************************/
public class DLMovement : MonoBehaviour
{
    public globalVar.State state;
    public int directionalThresh = 1; //or some arbritraty set #, will eventualy be set to var in settings 		
    int threshold = 1;  //or some arbritraty set #, will eventualy be set to var in settings 		
    public const int vectorCount = 0;  //or some arbritraty set #, will eventualy be set to var in settings 		
    public Obj obj = null;
    public ImturpriterMover intMov = null;
   public bool hasExStartState = false;

    /*************  thisList, lastList, possList are arrays of movements     **********************
        list<sequwnce in list of movements 
            list<sequwnce in movement 
                int[JOINT, TARG_POSITION, X_, Y_, Z_ ]>>
    *************************************************************************************************/
    public List<globalVar.movement> thisList = new List<globalVar.movement>(); 
    List<globalVar.movement> lastList = new List<globalVar.movement>();
    List<globalVar.movement> possList = new List<globalVar.movement>();

    /*************  jointRestrictions  **********************
     * int[JOINT, MIN, MAX, vector, cur position ]>>
    *********************************************************/
    List<int[]> jointRestrictions = new List<int[]>();

    public void GenerateJointRestrictions()
    {
        HingeJoint[] h = obj.GetComponentsInChildren<HingeJoint>();
        for (int i = 0; i < h.Length; i++)
        {
            //Debug.Log("vectorCount:"+vectorCount);
            for(int ii = 0; ii<=vectorCount; ii++)
            {

            jointRestrictions.Add(new int[(int)globalVar.JointAtt.xyz + 1] 
            {(int)h[i].limits.max,
                        (int)h[i].limits.min,
                           ii, 0});
            //Debug.Log("joint "+i+" max "+ h[i].limits.max + " min "+ h[i].limits.min+ " Vector " + ii);
            }
        }
    }

    /**************************************************************************************************
     * void StartState()
     * Description: 
     * 
     **************************************************************************************************/
    public void StartState()
    {
        state = globalVar.State.RUNNING;
        int count = 0;
        GenerateJointRestrictions();   
        foreach ( int[] restriction in jointRestrictions)
        {
            for (int i = 0; i <= threshold; i++)
            {
                globalVar.movement temp = new globalVar.movement();
                temp.step.Add(new globalVar.steps(count/(vectorCount+1),
                ((((Math.Abs(restriction[(int)globalVar.JointAtt.MIN]) +
                 Math.Abs(restriction[(int)globalVar.JointAtt.MAX])) / threshold) * i)
                 + restriction[(int)globalVar.JointAtt.MIN]), restriction[2] ));
                lastList.Add(temp);
            PrintList(temp);

            }
            count++;
        }
        PrintList(lastList);
        NestedCluster(lastList, lastList);

        thisList = possList;
        possList = null;
        possList = new List<globalVar.movement>();
    
        
        hasExStartState = true;
        state = globalVar.State.WAITING;
    }

    /**************************************************************************************************
     * bool Validate(List<globalVar.movement> a)
     * Description: 
     * 
     **************************************************************************************************/
    bool Validate(globalVar.movement a)
    {
        ResetCurrJointRestrictions();

        foreach(globalVar.steps b in a.step)
        {
            (jointRestrictions[(b.joint + b.vector)])[(int)globalVar.JointAtt.xyz] += b.target;
            if ((jointRestrictions[(b.joint + b.vector)])[(int)globalVar.JointAtt.MAX] < (jointRestrictions[(b.joint + b.vector)])[(int)globalVar.JointAtt.xyz]) 
            { /*Debug.Log((jointRestrictions[(b.joint + b.vector)])[(int)globalVar.JointAtt.MAX] + " < "+(jointRestrictions[(b.joint + b.vector)])[(int)globalVar.JointAtt.xyz]);*/ 
            return false; }
             if((jointRestrictions[(b.joint + b.vector)])[(int)globalVar.JointAtt.MIN] > (jointRestrictions[(b.joint + b.vector)])[(int)globalVar.JointAtt.xyz])
             { /*Debug.Log((jointRestrictions[(b.joint + b.vector)])[(int)globalVar.JointAtt.MIN] + " > " + (jointRestrictions[(b.joint + b.vector)])[(int)globalVar.JointAtt.xyz]);*/ 
             return false;}
                
        }
          return true;
    }

    void ResetCurrJointRestrictions()
    {
        for (int i = 0; i < jointRestrictions.Count; i++)
            (jointRestrictions[i])[(int)globalVar.JointAtt.xyz] = 0;

    }
    
    /**************************************************************************************************
     * void Cluster()
     * Description: 
     * 
     **************************************************************************************************/
    public void Cluster()
    {
        NestedCluster(thisList, thisList);
        //NestedCluster(thisList, lastList);
        if(possList.Count != 0)
        {

            lastList = null;
            lastList = thisList;//.ConvertAll(Converter< int[], globalVar.movement);
            thisList = null;
            thisList = possList;
        possList = new List<globalVar.movement>();
        obj.memory.LogGeneration(thisList, obj.Header.generation);
        }else
        {
            Debug.Log("*****************************FinishedExeMovements***************************");
            if(thisList.Count == 0 && lastList.Count != 0)
                if(obj.Header.movementCount == lastList.Count)
                    thisList = lastList;
            obj.int_.wrFile();
            obj.finished = true;
        }

    }
  
    
    /**************************************************************************************************
     * 
     * Description: 
     * 
     **************************************************************************************************/
    void NestedCluster(List<globalVar.movement> a, List<globalVar.movement> b)
    {
        //possList.Clear();
        foreach (globalVar.movement l in a)
        {
            foreach (globalVar.movement m in b)
            {
                globalVar.movement new_ = new globalVar.movement();
                new_.step.AddRange(m.step);
                new_.step.AddRange(l.step);
                if (Validate(new_))
                    possList.Add(new_);

            }
        }

    }

    public String PrintList(List<globalVar.movement> a)
    {
        String holder = "";
        if(a.Count != 0)
        {

        if(a.Count>1){}
           // Debug.Log("***********************size of movement list "+a.Count + " ***********************");
        foreach (globalVar.movement l in a)
        {
            foreach (globalVar.steps m in l.step)
            {
                    holder += ("joint: "+m.joint + " targetPosition: " + m.target + " Vector: " + m.vector+ " ~ ");
            }
            return holder;
        }
        }else
        {
            //Debug.Log("***********************size of movement list "+a.Count + " ***********************");

        }
            return holder;

    }
   public String PrintList(globalVar.movement a)
    {
        String holder = "";
        if(a.step.Count != 0)
        {

        if(a.step.Count>1){}
            //Debug.Log("***********************size of movement list "+a.Count + " ***********************");
      
      
            foreach (globalVar.steps m in a.step)
            {
                    holder += ("joint: "+m.joint + " targetPosition: " + m.target + " Vector: " + m.vector+ " ~ ");
            }
            return holder;
        }else
        {
            Debug.Log("***********************size of movement list "+a.step.Count + " ***********************");
            
        }
            return holder;
    }
}
