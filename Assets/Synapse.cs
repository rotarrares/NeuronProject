using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class Synapse : MonoBehaviour 
{

    GameObject presynaptic, postsynaptic;
    Neuron preSynaptic, postSynaptic;
    float receiver;
    float modulation;
    int strength = 10;

    private void Awake()
    {
        receiver = 21f;
        modulation = 1f;
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        float v = Random.value;

        if (v < preSynaptic.probabilityOfInhibitory)
        {
            
            modulation = -1f;
        }
    }

    private void FixedUpdate()
    {
        
    }
    private void OnDestroy()
    {
        Debug.DrawLine(presynaptic.transform.position,
                postsynaptic.transform.position,
                Color.blue,
                3f);
    }



    public void Connect(GameObject presynaptic, GameObject postsynaptic)
    {
        this.presynaptic = presynaptic;
        this.postsynaptic = postsynaptic;
        this.preSynaptic = presynaptic.GetComponent<Neuron>();
        if (this.preSynaptic == null)
            this.preSynaptic = preSynaptic.GetComponent<InputNeuron>();
        this.postSynaptic = postsynaptic.GetComponent<Neuron>();
    }
    


    //sends the signal to the postsynaptic neuron
    public float GetSignal()
    {
        if (modulation * receiver > 2)
            Debug.DrawLine(presynaptic.transform.position,
                postsynaptic.transform.position,
                Color.green,
                Time.fixedDeltaTime);
        else
            Debug.DrawLine(presynaptic.transform.position,
                postsynaptic.transform.position,
                Color.red,
                Time.fixedDeltaTime);
        if (preSynaptic.isFiring())
        {
            return this.receiver * this.modulation;
        }
        return 0f;
    }


}
