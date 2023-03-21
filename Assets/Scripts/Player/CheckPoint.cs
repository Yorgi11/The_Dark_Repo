using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour
{

    static GameObject checkPoint;
    public GameObject game;
    static GameObject player;
    public GameObject regroup;
    public List<GameObject> enemyObjects;
    static List<bool> enemiesAlive;
    public List<GameObject> doorObjects;
    public List<bool> defaults;
    static List<bool> doorOpen;
    public int doorCounter;
    public int enemyCount;

    // Start is called before the first frame update
    void Start()
    {
        player = this.gameObject;
        if (checkPoint != null)
        {
            player.transform.position = checkPoint.transform.position;

            for(int i = 0; i < doorObjects.Count; i++)
            {
                if (doorOpen[i].Equals(true))
                {
                    Vector3 position = new Vector3(doorObjects[i].transform.position.x, -5f, doorObjects[i].transform.position.z);
                    doorObjects[i].transform.position = position;
                }
            }
            for (int i = 0; i < enemyObjects.Count; i++)
            {
                if(enemiesAlive[i] == true)
                {
                    enemyObjects[i].SetActive(true);
                }
            }

        }
        else
        {
            doorOpen = new List<bool>();
            enemiesAlive = new List<bool>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void checkPointSystem(GameObject respawn)
    {
        checkPoint = respawn;
        DontDestroyOnLoad(checkPoint);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "CheckPoint")
        {
            checkPointSystem(other.transform.gameObject);
            other.transform.gameObject.SetActive(false);
            for (int i = 0; i < enemyObjects.Count; i++)
            {
                try
                {
                    if (GameObject.Find(enemyObjects[i].name).activeSelf)
                    {
                        defaults[i] = true;
                    }
                }
                catch
                {
                    defaults[i] = false;
                }
                enemiesAlive.Add(defaults[i]);
            }
            for (int i = 0; i < doorObjects.Count; i++)
            {
                if (doorObjects[i].transform.position.y != 0)
                {
                    defaults[i] = true;
                }
                else
                {
                    defaults[i] = false;
                }
                doorOpen.Add(defaults[i]);
            }
        }
    }
}
