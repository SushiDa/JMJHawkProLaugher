using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : AbstractItem
{
    [SerializeField] internal GameObject projectile;

    private float shotDelayTimer;
    private bool isFiring = false;

    internal override bool CanInteractImpl(PlayerGameController player)
    {
        return false;
    }


    protected override void InteractImpl()
    {
        isFiring = true;
    }

    void Update()
    {
        
        if (isFiring && shotDelayTimer < 1) 
        {
            shotDelayTimer += Time.deltaTime;
            if (shotDelayTimer >= 1) 
            {
                isFiring = false;
                shotDelayTimer = 0;
                AudioBridge.PlaySFX("Canon");
                //Animation du tir
                GetComponentInChildren<Animator>().SetBool("Fire", true);
                //Create projectile and add speed
                GameObject tarte = Instantiate(projectile, transform.position, Quaternion.identity);
                tarte.GetComponent<Rigidbody>().velocity = new Vector3(-10, 10, 0);
                tarte.GetComponent<Rigidbody>().rotation = Quaternion.LookRotation(Vector3.up, Vector3.left);
                Destroy(gameObject, 2.0f);
            }
            
        }
    }
}
