using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Brain : MonoBehaviour
{
    public GameObject neuronPrefab,inputNeuronPrefab;
    private List<Neuron> neurons;
    private List<InputNeuron> inputNeurons;
    private List<GameObject> Neurons;
    public int numberOfNeurons = 10;
    public float timepassing = 2f;

    private int activity = 0;
    
    
    // Start is called before the first frame update

    private void Awake()
    {
        Time.fixedDeltaTime = timepassing;
        neurons = new List<Neuron>();
        Neurons = new List<GameObject>();

        inputNeurons = new List<InputNeuron>();
    }

    public List<GameObject> GetNeurons()
    {
        return this.Neurons;
    }

    public void DecreaseActivity()
    {
        activity--;
    }
    public void IncreaseActivity()
    {
        activity++;
    }
    public void GetActivity(GameObject neu) 
    {
        neu.SendMessage("SetGlobalActivity", activity);
    }



    void Start()
    {

        for (int i = 0; i < numberOfNeurons; i++)
        {
            float x, y, z;
            x = Random.Range(1, 10)*2;
            y = Random.Range(1, 10)*2;
            z = Random.Range(1, 10);
            GameObject N = Instantiate(neuronPrefab, new Vector3(x, y, z), Quaternion.identity,gameObject.transform);
            Neuron n = N.GetComponent<Neuron>();
            neurons.Add(n);
            Neurons.Add(N);
        }
        //input neurons
        for (int i = 0; i < 2; i++)
        {
            float x, y, z;
            x = Random.Range(1, 10) * 2;
            y = Random.Range(1, 10) * 2;
            z = Random.Range(1, 10);
            GameObject N = Instantiate(inputNeuronPrefab, new Vector3(x, y, z), Quaternion.identity, gameObject.transform);
            InputNeuron inputNeuron = N.GetComponent<InputNeuron>();
            inputNeurons.Add(inputNeuron);
            Neurons.Add(N);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(activity);
        Time.fixedDeltaTime = timepassing;
      
    }
}
