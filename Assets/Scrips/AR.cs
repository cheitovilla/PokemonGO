using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AR : MonoBehaviour
{
    public static AR instance;
    public GameObject parent;
    public GameObject pokemon;
    public GameObject pokeball;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SpawnPokemon()
    {
        Debug.Log("one time created");

        InstanciatePokeball();
    }

    public void InstanciatePokeball()
    {
        pokeball = Instantiate(Resources.Load("Pokeball", typeof(GameObject))) as GameObject;
        pokeball.transform.SetParent(parent.transform, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (DefaultTrackableEventHandler.instance.wached == true)
        {
            SpawnPokemon();
            //instance
            DefaultTrackableEventHandler.instance.wached = false;
        }
    }
}
