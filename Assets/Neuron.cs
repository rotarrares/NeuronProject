using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;


public class Neuron:MonoBehaviour
{
    //Neuron Parameters
    public int firingSteps = 5;
    public int afterFiringSteps = 2;
    public int maxConnections = 5;
    public float probabilityOfInhibitory = 0.1f;
    public int desiredActivity = 4;
    
    protected List<Synapse> dentrites;
    
    private bool firing = false;
    private bool afterfiring = false;
    private int[] stepCounter;
    private float potential = -70f;
    private int globalActivity = 0;

    private GameObject brain;
    public GameObject synapsePrefab;
    protected GameObject cube;

    //===============================================================================
    //Neuron Related Tasks
    public virtual bool isFiring()
    {
        return firing;
    }


    //Asks the brain what is the global activity
    // *Parameter = -1 -> send message to brain
    // otherwise it means brain responded
    public void SetGlobalActivity(int activity = -1)
    {
        if (activity > -1)
            globalActivity = activity;
        else
            brain.SendMessage("GetActivity", gameObject);
    }


    //Sums the signals coming from the dentrites
    public virtual float SumOfSignals()
    {
        float sum = 0f;
        if (dentrites.Count > 0)
            foreach (Synapse dentrite in dentrites)
            {
                sum = +dentrite.GetSignal();
            }

        return sum;
    }



    // create synapse with random neuron in vecinity
    public virtual void CreateConnection(List<GameObject> vecinity)
    {
        float restructureProbability = (1f - ((float)globalActivity / (float)desiredActivity))/40f;
        if (dentrites.Count < maxConnections && Random.value < restructureProbability)
        {
            int randomIndex = Random.Range(0, vecinity.Count);
            if (vecinity[randomIndex] != gameObject)
            {
                GameObject synapseGameObject = Instantiate(synapsePrefab, new Vector3(0, 0, 0), Quaternion.identity,gameObject.transform);
                synapseGameObject.transform.parent = transform;
                Synapse synapse = synapseGameObject.GetComponent<Synapse>();
                synapse.Connect(vecinity[randomIndex], gameObject);
                dentrites.Add(synapse);
            }
        }
    }

    public virtual void DeleteConnection(List<GameObject> vecinity)
    {
        float restructureProbability = (1f - ((float)globalActivity / (float)desiredActivity));
        if(restructureProbability < 0)
            Debug.Log("Activity: " + globalActivity + " / " + desiredActivity + " Restructure probability:" + restructureProbability);
        if (dentrites.Count > 0 && Random.value < -restructureProbability)
        {
            int randomIndex = Random.Range(0, dentrites.Count - 1);
            Destroy(dentrites[randomIndex]);
            dentrites.RemoveAt(randomIndex);
        }
    }

    //=========================================================================================
    //Draw Neuron
    public virtual void drawCube()
    {
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.parent = gameObject.transform;
        cube.transform.position = gameObject.transform.position;
    }
    //Sets the color of the neuron
    protected virtual void ColorCube(int col)
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
                cube.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                break;
        }
    }

    // =========================================================================================
    // Unity Timeline Functions
    void Awake()
    {

        stepCounter = new int[5];
        dentrites = new List<Synapse>();
    }

    
  
    private void Start()
    {

        this.brain = gameObject.transform.parent.gameObject;
        drawCube();
        
    }

    void FixedUpdate()
    {
        SetGlobalActivity();
        // update synapses

        potential = -70f + SumOfSignals();

        // at treshold
        if (potential > -50f)
        {
            if (firing == false)
            {
                //start spike
                firing = true;
                brain.SendMessage("IncreaseActivity");
                ColorCube(1);
                stepCounter[0] = firingSteps;
            }
        }
        else
        {
            DeleteConnection(brain.GetComponent<Brain>().GetNeurons());
        }

        // on firing;
        if (stepCounter[0] > 0)
        {
            if (stepCounter[0] == 1)
            {
                afterfiring = true;
                brain.SendMessage("DecreaseActivity");
                ColorCube(2);
                stepCounter[1] = afterFiringSteps;
            }
            stepCounter[0]--;
        }

        //after firing;
        if (stepCounter[1] > 0)
        {
            if (stepCounter[1] == 1)
            {
                afterfiring = false;
                firing = false;
                ColorCube(3);

            }
            stepCounter[1]--;
        }
        CreateConnection(brain.GetComponent<Brain>().GetNeurons());
        

    }

    // =========================================================================================


   



}

