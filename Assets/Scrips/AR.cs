using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AR : MonoBehaviour
{
    public static AR instance;
    public GameObject parentPokeball;
    public GameObject pokeball;

    public GameObject parentPokemon;
    public GameObject pokemon;

    private string urlTenPokemons = "https://pokeapi.co/api/v2/pokemon?limit=10&offset=0";
    private DataPokemons responsePokemonsGeneral;
    private List<string> listPokemon;
    private List<string> listAct;
    private string rdnNamePokemon;
    public int countTime;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetNamePokemon());
       // InstanciatePokeballAndPokemon();
    }

    //colocar en el star del imagetracking para que se traiga cualquier pokemon
    public void SpawnPokemon()
    {
        Debug.Log("one time created");

        InstanciatePokeballAndPokemon();
    }

    public void InstanciatePokeballAndPokemon()
    {
        pokeball = Instantiate(Resources.Load("Pokeball", typeof(GameObject))) as GameObject;
        pokeball.transform.SetParent(parentPokeball.transform, false);

        pokemon = Instantiate(Resources.Load("Cube", typeof(GameObject))) as GameObject;
        pokemon.transform.SetParent(parentPokemon.transform, false);
        pokemon.GetComponentInChildren<Text>().text = rdnNamePokemon;
    }

    public IEnumerator GetNamePokemon()
    {
        UnityWebRequest www = UnityWebRequest.Get(urlTenPokemons);
        www.timeout = 10;

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError("Algo salio mal");
        }
        else
        {
            if (www.isDone)
            {
                string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                Debug.Log("Json Result pokemon: " + jsonResult);

                responsePokemonsGeneral = JsonUtility.FromJson<DataPokemons>(jsonResult);
                Debug.Log("root del pokemon: " + responsePokemonsGeneral.results.Count);

                listPokemon = new List<string>();
                for (int i = 0; i < responsePokemonsGeneral.results.Count; i++)
                {
                    Debug.Log(responsePokemonsGeneral.results[i].name);
                    listPokemon.Add(responsePokemonsGeneral.results[i].name);
                }
            }

            PickNamePokemon();
        }
    }

    public void PickNamePokemon()
    {
        countTime++;
        if (countTime == 1)
        {
            rdnNamePokemon = listPokemon[Random.Range(0, listPokemon.Count)];
            Debug.Log("rdn: " + rdnNamePokemon);
            listPokemon.Remove(rdnNamePokemon);
            listAct = listPokemon;
            InstanciatePokeballAndPokemon();
        }
        else
        {
            try
            {
                rdnNamePokemon = listAct[Random.Range(0, listAct.Count)];
                Debug.Log("rdn: " + rdnNamePokemon);
                listPokemon.Remove(rdnNamePokemon);
                Destroy(pokemon);
                Destroy(pokeball);
                InstanciatePokeballAndPokemon();
            }
            catch (System.Exception)
            {

                throw;
            }
            
            
        }
        
    }
}
