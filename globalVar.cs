using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class globalVar : MonoBehaviour
{ 
    public enum State { RUNNING, WAITING };
    public enum MovementAtt { JOINT, TARG_POSITION, XYZ };
    public enum JointAtt { MAX, MIN, XYZ, xyz };
    // [Serializable]
[System.Serializable]
    public class movement
    {
        public movement()
        {step = new List<steps>();}
        public List<steps> step = null;

    }
[System.Serializable]
        public class steps
        {
            public steps()
            {
                joint = 0;
                target = 0;
                vector = 0;

            }
            public steps(int a, int b, int c)
            {
                joint = a;
                target = b;
                vector = c;

            }
            public int joint;
            public int target;
            public int vector;

        }
    public class header
    {
        public header()
        {
            objID = count;
            count ++;   
        }
        public header(string n)
        {
            ObjName = n;
            objID = count;
            count ++;
        }
        public string ObjName = "NullName";
        public int objID = 0;
        public int generation = 0;
        public int movementCount = 0;
        static int count = 0;
        
    }


public class Tree
    {
        public class Node
        {
            public Node(float[] v)
            {value = v;}
            ~Node()
            {
                value = null;
            }
            public float[] value;
            public Node left;
            public Node right;
        } 

        public int size =0;
        public List<float[]> soarted = new List<float[]>();
        // Node Root;
        // public void insert( float[] v)
        // {
        //      InternalInsert(Root, v);
        // }
        public Node InternalInsert( Node root, float[] v)
        {
            if (root == null)
            {
                root = new Node(v);
                //root.value = v;
                size++;
            }
            else if (v[1] == root.value[1])
            {
                Debug.Log("*********************************Error*********************************\n"+
                "matching index, values " + v[1] +" " +v[0] + " was not added");
            }
            else if (v[0] > root.value[0])
            {
                root.left = InternalInsert(root.left, v);
            }
            else
            {
                root.right = InternalInsert(root.right, v);
            }

            return root;
        }

        public List<float[]> traverse(Node root, int c)
        {
            soarted.Clear();
            List<float[]> hold = new List<float[]>();
            soarted = new List<float[]>();
            traverse2(root);
            if(c > size)
            c = size;
            for(int i =0; i< c;i++)
                hold.Add(soarted[i]);
            return hold;
        }
        public void traverse2(Node root)
        {
            if (root == null)
            {
                return;
            }

            traverse2(root.left);
            soarted.Add(root.value);
            traverse2(root.right);
        }
        // public void Clear()
        // {
        //     Root = null;
        //     InternalClear(Root);

        // }
        public void InternalClear(Node root)
        {
            size = 0;
            if (root == null)
            {
                return;
            }
            traverse2(root.left);
            traverse2(root.right);
            root = null;
        }
    }
}