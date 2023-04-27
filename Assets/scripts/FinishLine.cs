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
        count = 0; 
        
    }

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log(other.name);
        string[] key = {"st", "nd", "rd", "th", "th", "th", "th"};  
        if (other.name == "car")
        {
            count++;
            //count = count/2 + 1; 
            finishText.text = $"{count}{key[count-1]} Place!!";
            // Your functionality here, e.g., load the next level or display a win message
        }
        else if(other.name != "Body"){
            count++;
        }
        Debug.Log(count);
    }
}