using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PokemonDisplay : MonoBehaviour
{

    [SerializeField] Pokemon pokemon;
    [SerializeField] TextMeshProUGUI pokemonName;
    [SerializeField] TextMeshProUGUI shortDescription;
    [SerializeField] Image homeImage;

    [SerializeField] Image focusedImage;
    [SerializeField] TextMeshProUGUI focusedName;
    [SerializeField] TextMeshProUGUI longDescription;
    [SerializeField] GameObject focusedDisplay;

    [SerializeField] Sprite notFoundImage;
    [SerializeField] string notFoundName;
    [SerializeField] string notFoundDescription;

    private bool found = false;
    PokedexBehavior pokedexBehaviorScript;

    void Start()
    {
        homeImage.sprite = notFoundImage;
        pokemonName.text = notFoundName;
        shortDescription.text = notFoundDescription;

        focusedImage.sprite = notFoundImage;
        focusedName.text = notFoundName;
        longDescription.text = notFoundDescription;
    }

    public void foundPokemon()
    {
        found = true;
        if (found == true)
        {
            Debug.Log(found);
            homeImage.sprite = pokemon.image;
            pokemonName.text = pokemon.name;
            shortDescription.text = pokemon.shortDescription;
            
        }
       
    }

    public void FocusPokemon()
    {

        pokedexBehaviorScript = PokedexBehavior.instance;
        if (found == true)
        {
            focusedImage.sprite = pokemon.image;
            focusedName.text = pokemon.name;
            longDescription.text = pokemon.longDescription;
            
        }
        pokedexBehaviorScript.OpenFocusDisplay();



    }
}
