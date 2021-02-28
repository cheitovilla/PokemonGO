using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokeball : MonoBehaviour
{
    public static Pokeball instance;
    public int rdnNum;
    public GameObject pokeball;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        pokeball = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (pokeball.gameObject.transform.position.y <= -50)
        {
            Destroy(pokeball);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Pokemon")
        {
            StartCoroutine("CatchPokemon", other.gameObject);
        }
    }

    public IEnumerator CatchPokemon(GameObject Pokemon)
    {
        transform.Translate(Vector3.up * 1, Space.World);
        this.GetComponent<Rigidbody>().useGravity = false;
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
      //  Destroy(Pokemon.gameObject);
        Pokemon.SetActive(false);

        yield return new WaitForSeconds(1);
        this.GetComponent<Rigidbody>().useGravity = true;

        yield return new WaitForSeconds(1.3f);
        rdnNum = Random.Range(1, 11);
        Debug.Log("probabilidad: " + rdnNum);

        randomCatched();
        if (randomCatched())
        {
            Debug.Log("lo atrape");
        }
        else
        {
            Debug.Log("F");
            Destroy(this.gameObject);
            Pokemon.SetActive(true);
            AR.instance.InstanciatePokeball();
        }

    }

    

    public bool randomCatched()
    {
        if (rdnNum >= 5)
        {
            return true;
        }
        return false;
    }


  
}
