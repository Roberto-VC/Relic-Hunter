using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public GameObject closed;
    public GameObject open;
    public KeyManager Keys;
    private bool used = false;

    void Update()
    {
        if (!used && Input.GetKey(KeyCode.M) && Keys.doorkey)
        {
            Replace(closed, open);
            used = true;
        }
    }

    void Replace(GameObject obj1, GameObject obj2)
    {
        Debug.Log(obj1.name);
        obj1.SetActive(false); // Deactivate the 'open' GameObject
        obj2.SetActive(true);  // Activate the 'closed' GameObject
    }


}