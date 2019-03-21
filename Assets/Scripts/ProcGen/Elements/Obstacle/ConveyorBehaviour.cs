﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBehaviour : LocalUpdater<Rigidbody>
{
    public float speed;

    private void FixedUpdate()
    {
        AddForceToRBs();
    }

    void AddForceToRBs()
    {
        foreach(Rigidbody rb in currentlyContained)
        {
            rb.AddForce(transform.right * speed * Time.deltaTime, ForceMode.Impulse);
        }
    }

    protected override void PlayerEnters(GameObject player)
    {
        base.PlayerEnters(player);
    }

    protected override void PlayerExits(GameObject player)
    {
        base.PlayerExits(player);
    }
}