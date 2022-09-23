using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class AppManager : MonoBehaviour
{

    private int _gameMode = 0; // 0: client, 1: host(server/client)

    public string _mainScene;
    private NetworkManager _nm;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartAsHost()
    {
        Debug.Log("Starting as host");
        _gameMode = 1;
        StartCoroutine(LoadMainSceneAsync());
    }

    public void StartAsClient()
    {
        Debug.Log("Starting as client");
        _gameMode = 0;
        StartCoroutine(LoadMainSceneAsync());
    }

    IEnumerator LoadMainSceneAsync()
    {
        // Set the current Scene to be able to unload it later
        Scene currentScene = SceneManager.GetActiveScene();

        // The Application loads the Scene in the background at the same time as the current Scene.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_mainScene, LoadSceneMode.Single);

        // Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Find the network manager and start the appropriate game mode
        _nm = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();

        if(_nm != null)
        {
            _nm.SetSingleton();
            switch (_gameMode)
            {
                case 0:
                    NetworkManager.Singleton.StartClient();
                    break;
                case 1:
                    NetworkManager.Singleton.StartHost();
                    break;
            }
        }
    }
}
