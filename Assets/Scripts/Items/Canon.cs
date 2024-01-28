using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : AbstractItem
{
    [SerializeField] internal GameObject projectile;

    private float shotDelayTimer;
    private bool isFiring = false;
    private bool fireAnim = false;

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
        
        if (isFiring) 
        {
            if (!fireAnim && shotDelayTimer >= 0.5f)
            {
                fireAnim = true;
                //Animation du tir
                GetComponentInChildren<Animator>().SetBool("Fire", true);
            }
            shotDelayTimer += Time.deltaTime;
            if (shotDelayTimer >= 1) 
            {
                isFiring = false;
                shotDelayTimer = 0;
                AudioBridge.PlaySFX("Canon");
                
                
                //Create projectile and add speed
                GameObject tarte = Instantiate(projectile, transform.position, Quaternion.identity);
                tarte.GetComponent<Rigidbody>().velocity = transform.rotation * new Vector3(-10, 10, 0) * 3f;
                tarte.GetComponent<Rigidbody>().rotation = Quaternion.LookRotation(Vector3.up, Vector3.left);
                Destroy(gameObject, 2.0f);
            }
            
        }
    }
}
