using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    
    public TMP_InputField Name;
    public Button StartButton;
    public static string UserName;
    private static StartGame Instance;
    // Start is called before the first frame update
    void Start()
    {
        //Name = gameObject.GetComponent<TMP_InputField>();
        Debug.Log(Name);
       // StartButton = gameObject.GetComponent<Button>();
        StartButton.onClick.AddListener(StartPlaying);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartPlaying()
    {
        if(UserName == null)
            UserName = Name.text;
        Debug.Log("Pressed..."+ UserName);
        SceneManager.LoadScene("Main");
    }

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
    }
}
