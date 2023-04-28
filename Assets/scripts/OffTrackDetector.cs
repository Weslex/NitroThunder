using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CarController; 


public class OffTrackDetector : MonoBehaviour
{
    private int terrainLayer;
    public event Action OnCarOffTrack;
    public event Action OnCarOnTrack;

private void Start()
{
    //intialize terrainLayer variable
    terrainLayer = LayerMask.NameToLayer("Terrain");
}

//method to be called when the car enters a collider
private void OnTriggerEnter(Collider other)
{
    Debug.Log("Trigger entered: " + other.gameObject.name);
    //if the car enters the terrain call car off track
    if (other.gameObject.name == "Terrain")
    {
        OnCarOffTrack?.Invoke(); 
    }
    //else call car on track
    else{
        OnCarOnTrack?.Invoke(); 
        
    }
}

//method to be called when the car exits a collider
private void OnTriggerExit(Collider other)
{
    Debug.Log("Trigger exited: " + other.gameObject.name);
    //if the car is not exiting the terrain call off track method
    if(!(other.gameObject.name == "Terrain"))
    {
        OnCarOffTrack?.Invoke(); 
    }
}



}
