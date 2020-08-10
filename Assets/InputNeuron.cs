using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class InputNeuron : Neuron
{
    
    
    public override bool isFiring()
    {
        return true;
    }

    public override void drawCube()
    {
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.parent = gameObject.transform;
        cube.transform.position = gameObject.transform.position;
    }

    public override float SumOfSignals()
    {
        
        return Random.Range(0,24);
    }
    protected override void ColorCube(int col)
    {
        switch (col)
        {
            case 1:
                cube.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                break;
            case 2:
                cube.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                break;
            default:
                cube.GetComponent<Renderer>().material.SetColor("_Color", Color.cyan);
                break;
        }
    }

    public override void DeleteConnection(List<GameObject> vecinity)
    {

    }
    public override void CreateConnection(List<GameObject> vecinity)
    {
    }
}
