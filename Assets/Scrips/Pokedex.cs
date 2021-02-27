using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Pokedex : MonoBehaviour
{
    public GameObject pokedex;
    public GameObject btnPokedex;

    private string urlPokedex = "https://pokeapi.co/api/v2/pokemon?limit=10&offset=";
    private string urlComplete;
    private string offset;
    private int index;

    //images
    private string urlImagesPokemons = "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/back/"; // + 1.png
    private string urlCompleteImagesPokemons;
    private Texture2D texturePokemons;

    private DataPokemons responsePokemonsGeneral;
    public RectTransform ParentPanel;
    private GameObject[] goButtonPokemons;
    private int cantDestroy;
    private string specificPokemon;

    private void Start()
    {
        index = 0;
        offset = index.ToString();
        StartCoroutine(GetPokemons());
    }

    public void NextList10()
    {
        DestroyAllButtons();
        index += 10;
        offset = index.ToString();
        StartCoroutine(GetPokemons());
    }

    public void BackList10()
    {
        if (index == 0)
        {
            Debug.Log("no hay de este lado");
        }
        else
        {
            DestroyAllButtons();
            index -= 10;
            offset = index.ToString();
            StartCoroutine(GetPokemons());
        }
        
    }

    public IEnumerator GetPokemons()
    {
        urlComplete = urlPokedex + offset;
        UnityWebRequest www = UnityWebRequest.Get(urlComplete);
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

                goButtonPokemons = new GameObject[responsePokemonsGeneral.results.Count];

                for (int i = 0; i < responsePokemonsGeneral.results.Count; i++)
                {
                    cantDestroy += 1;
                    //   beneficioToSplit = responseRoot.value[i].Beneficio;
                    goButtonPokemons[i] = Instantiate(Resources.Load("ButtonPokems", typeof(GameObject))) as GameObject;
                    goButtonPokemons[i].GetComponentInChildren<Text>().text = responsePokemonsGeneral.results[i].name;
                    goButtonPokemons[i].transform.SetParent(ParentPanel, false);

                    string urlPokemon = responsePokemonsGeneral.results[i].url;
                    goButtonPokemons[i].GetComponent<Button>().onClick.AddListener(() => ButtonClicked(urlPokemon));

                }

                StartCoroutine(GetImagesPokemons());
            }
        }
    }

    public IEnumerator GetImagesPokemons()
    {
        for (int i = index; i < goButtonPokemons.Length + index; i++)
        {
            int aux = i + 1;
            urlCompleteImagesPokemons = urlImagesPokemons + aux + ".png";
            Debug.Log("url image: " + urlCompleteImagesPokemons);
            UnityWebRequest www = UnityWebRequest.Get(urlCompleteImagesPokemons);
            DownloadHandlerTexture textPokemon = new DownloadHandlerTexture(true);
            www.downloadHandler = textPokemon;
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
                    Debug.Log("Json Result images: " + jsonResult);

                    texturePokemons = textPokemon.texture;
                    goButtonPokemons[i - index].GetComponentInChildren<RawImage>().texture = texturePokemons;
                    goButtonPokemons[i - index].GetComponentInChildren<RawImage>().SetNativeSize();
                    goButtonPokemons[i - index].GetComponentInChildren<RawImage>().transform.localScale = new Vector2(0.5f, 0.5f);
                    
                }
            }
        }
           
        
        
    }


    public void DestroyAllButtons()
    {
        Debug.Log("Cantidad a destruir :" + cantDestroy);
        for (int i = 0; i < cantDestroy; i++)
        {
            Destroy(goButtonPokemons[i].gameObject);
        }
        cantDestroy = 0;
    }

    
    public void ButtonClicked(string _urlPokemon) //le agrego un listener dynamico al boton que se le agrega a c/u del os botones enlistados
    {
        specificPokemon = _urlPokemon;
        Debug.Log("Button clicked = " + specificPokemon);

    }
     
     

    public void OpenPokedex()
    {
        pokedex.SetActive(true);
        btnPokedex.SetActive(false);

        offset = "10";
        StartCoroutine(GetPokemons());

    }

    public void ClosePokedex()
    {
        pokedex.SetActive(false);
        btnPokedex.SetActive(true);
    }
}
