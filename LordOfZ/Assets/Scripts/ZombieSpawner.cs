using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ZombieSpawner : MonoBehaviour
{
    public static ZombieSpawner Instance;

    [SerializeField] private LayerMask ground;
    public List<Zombie> zombiesList;
    public GameObject selectedZombie;
    private ZombieScript zombieScript;
    public GameObject explosionPref;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = GameManager.Instance.cam.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit, 100, ground))
                {
                    if (selectedZombie != null && GameManager.Instance.playerMoney >= zombieScript.zCost)
                    {
                        Instantiate(explosionPref, hit.point, Quaternion.identity);
                        GameObject instZombie = Instantiate(selectedZombie, hit.point, Quaternion.identity);
                        switch (zombieScript.zombieEnum)
                        {
                            case 0:
                                AudioManager.Instance.Play("Groan_Walker");
                                break;
                            case (ZombieEnum)1:
                                AudioManager.Instance.Play("Groan_Runner");
                                break;
                            case (ZombieEnum)2:
                                AudioManager.Instance.Play("Groan_Tank");
                                break;
                        }
                        GameManager.Instance.playerZombies.Add(instZombie.GetComponent<ZombieScript>());
                        GameManager.Instance.BuyUnit(zombieScript.zCost);
                    }
                }
            }
        }
    }

    public void ZombieButton(int zombType)
    {
        foreach (Zombie zombie in zombiesList)
        {
            if ((int)zombie.zombieEnum == zombType)
            {
                if (GameManager.Instance.playerMoney >= zombie.cost)
                {
                    selectedZombie = zombie.zombiePref;
                    zombieScript = selectedZombie.GetComponent<ZombieScript>();
                    zombieScript.zCost = zombie.cost;
                }
            }
        }
    }
}

[Serializable]
public class Zombie
{
    public ZombieEnum zombieEnum;
    public int cost;
    public GameObject zombiePref;
}
