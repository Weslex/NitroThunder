using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Speedometer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private GameObject car; // Add this line to define the car object

    private CarController carController;

    void Start()
    {
        if (car != null)
        {
            carController = car.GetComponent<CarController>();
            CheckSpeedometerActive(); 
        }
    }

    void Update()
    {
        if (carController != null)
        {
            float speed = carController.GetSpeedMPH();
            speedText.text = $"{Mathf.RoundToInt(speed)} MPH";
            //Debug.Log($"{Mathf.RoundToInt(speed)} MPH");
        }
        else
        {
            Debug.Log("Speedometer: CarController reference not set!");
        }
    }
    private void CheckSpeedometerActive()
        {
            string currentScene = SceneManager.GetActiveScene().name;

            if (currentScene == "MenuScreen") // Replace "Menu" with the name of your menu scene
            {
                gameObject.SetActive(false); // Disable the speedometer
            }
            else
            {
                gameObject.SetActive(true); // Enable the speedometer
            }
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            CheckSpeedometerActive();
        }
}