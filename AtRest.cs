using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AtRest 
{
    //public Obj obj;
    private Transform[] objectTransfom;
    private float noMovementThreshold = 0.01f;
    private const int noMovementFrames = 5;
    Vector3[][] previousLocations = null;
    private bool isMoving;
  
    public AtRest(Obj obj)
    {
        // Debug.Log("Inside Obj Awake");
        objectTransfom = obj.GetComponentsInChildren<Transform>();
        isMoving = false;
        previousLocations = new Vector3[objectTransfom.Length][];
        for(int i = 1; i < objectTransfom.Length;i++)
        {
            previousLocations[i] = new Vector3[noMovementFrames];
        }
        
        //For good measure, set the previous locations
        for(int i = 1; i < previousLocations.Length ; i++)
            for(int j = 0; j < previousLocations[i].Length-1; j++)
                previousLocations[i][j] = Vector3.zero;
    }

    
    public bool IsMoving
    {
        get{ return isMoving; }
        // get{ Debug.Log("obj is moving: "+ isMoving); return isMoving; }
    }

    public void UpdatePosition()
    {

        //Store the newest vector at the end of the list of vectors
        for(int i = 1; i < previousLocations.Length; i++)
        {
            for(int j = 0; j < previousLocations[i].Length-1; j++)
            {
                previousLocations[i][j] = previousLocations[i][j+1];
            }
        }

        for(int i = 1; i < previousLocations.Length; i++)
        {
            previousLocations[i][previousLocations[i].Length - 1] = objectTransfom[i].position;
        }
        //Debug.Log("obj position: "+ objectTransfom.position);
    
        //Check the distances between the points in your previous locations
        //If for the past several updates, there are no movements smaller than the threshold,
        //you can most likely assume that the object is not moving
        for(int j = 1; j < previousLocations.Length; j++)
        {
            for(int i = 0; i < previousLocations[j].Length - 1; i++)
            {
                if(Vector3.Distance(previousLocations[j][i], previousLocations[j][i + 1]) >= noMovementThreshold)
                {
                    //The minimum movement has been detected between frames
                    isMoving = true;
                    return;
                }
                else
                {
                    isMoving = false;
                }
            }
        }
//printRestList();
    }
    void printRestList()
    {
        string hold= "";
        for(int i = 1; i < previousLocations.Length; i++)
        {
            for(int j = 0; j < previousLocations[i].Length-1; j++)
            {
                //hold += (previousLocations[i][j] + " "); 
            }
            hold += (" objseg "+ i+" position "+objectTransfom[i].position +"\n");
        }
        Debug.Log(hold);
    }

}