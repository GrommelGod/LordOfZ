using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleScript : MonoBehaviour
{
    [SerializeField] private HumanScript humanScript;
    [SerializeField] private GameObject muzzle;
    [SerializeField] private GameObject gunPoint;

    public void Shoot()
    {
        switch (humanScript.humanEnum)
        {
            case (HumanEnum)1:
                AudioManager.Instance.Play("BatSound");
                break;
            case (HumanEnum)2:
                AudioManager.Instance.Play("Shooting");
                break;
        }
        GameObject instMuzzle = Instantiate(muzzle, gunPoint.transform.position, gunPoint.transform.rotation, gunPoint.transform);
        Destroy(instMuzzle, 1f);
    }
}
