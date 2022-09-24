using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class Disconnect : MonoBehaviour
{

    public NetworkManager _nm;
    public GameObject _nmGO;
    private void Start()
    {
        _nm = _nmGO.GetComponent<NetworkManager>();
    }

    // Start is called before the first frame update
    public void DisconnectFromNetwork()
    {
        _nm.Shutdown(true);
        Destroy(_nmGO);

        SceneManager.LoadScene("Entrance");
    }
}
