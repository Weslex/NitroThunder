using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private string targetTag = "car";
    [SerializeField] public TextMeshProUGUI finishText; 

    private int count;

    private void Start(){
        //set count to 0
        count = 0; 
        
    }

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log(other.name);
        //key array of postfix to add to the position the user gets in the game
        string[] key = {"st", "nd", "rd", "th", "th", "th", "th"};  

        //if the name of the collider touching the finish line is, display the place the player got on the overlay
        if (other.name == "car")
        {
            count++;
            //count = count/2 + 1; 
            finishText.text = $"{count}{key[count-1]} Place!!";
        }
        //if the name of the collider is "Body" it means that the car is AI so just increment count
        else if(other.name != "Body"){
            count++;
        }
        Debug.Log(count);
    }
}