using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

//speedometer class
public class Speedometer : MonoBehaviour
{
    //fields that can be populated in Unity editor
    [SerializeField] private TextMeshProUGUI speedText; //the overlay 
    [SerializeField] private GameObject car; //the car object

    //car controll object
    private CarController carController;

    void Start()
    {
        if (car != null)
        {
            //set carController object to the instance in the game
            carController = car.GetComponent<CarController>();
            //check if the scene is not the menu screen
            CheckSpeedometerActive(); 
        }
    }

    void Update()
    {
        if (carController != null)
        {
            //call method in carController to get speed of the car
            float speed = carController.GetSpeedMPH();
            //round to whole number and set the overlay text to the speed
            speedText.text = $"{Mathf.RoundToInt(speed)} MPH";
            //Debug.Log($"{Mathf.RoundToInt(speed)} MPH");
        }
        else
        {
            Debug.Log("Speedometer: CarController reference not set!");
        }
    }

    //method to ensure overlay does not appear in the menu screen
    private void CheckSpeedometerActive()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "MenuScreen") 
        {
            gameObject.SetActive(false); 
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    //method to apply the OnSceneLoaded method to SceneManger object
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    //method to remove the OnSceneLoaded method from SceneManager object
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //method to be called when a new scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckSpeedometerActive();
    }
}