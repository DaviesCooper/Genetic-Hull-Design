using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// This will be the physics class which when placed on the parent object of the rocket, will perform the simulation.
/// </summary>
class SimulationPhysics : MonoBehaviour
{
    List<GameObject> children;
    List<GameObject> thruster;
    float fuelAmount;
    float fuelForce;
    float fuelBurn;
    Individual myParent;
    float maxheight;
    Rigidbody me;

    public void Init(Individual parentIndiv)
    {
        myParent = parentIndiv;
        fuelAmount = myParent.fuelVolume;
        children = parentIndiv.exteriorIndex;
        thruster = parentIndiv.thrusterIndex;
        fuelForce = IndividualManager.Instance.fuelForce;
        fuelBurn = IndividualManager.Instance.fuelConsumption;
    }

    public void Start()
    {
        me = GetComponent<Rigidbody>();
        me.drag = 1;
        me.AddForce(new Vector3(0,1000,0));
    }

    /// <summary>
    /// Fixed update because I do not want to be dependant on frame-rate.
    /// Every tick, we do the following
    /// if height > -100
    ///     1: For each block in children
    ///             Find velocity, and raycast from it to the block.
    ///             if the block is hit (i.e. nothing was in the way)
    ///                 find the velocity in the axis perpendicular to the normal hit.
    ///                 calculate the drag of that velocity
    ///                 apply that force opposite the normal
    ///     2: For each block in thrusters
    ///             If fuel amount > 0
    ///                 add force to facing = fuelForce;
    ///                 fuel amount -= fuel burn
    ///    height = max(height, transform.position)
    /// else
    ///     set individuals height to our maxheight
    ///     kill this script 
    /// </summary>
    public void FixedUpdate()
    {
        if (this.Equals(null))
        {
            return;
        }
        if (transform.position.y > -50)
        {
            foreach (GameObject body in children)
            {
                Vector3 v = me.GetRelativePointVelocity(body.transform.position);
                RaycastHit hit;
                ///if we hit something (pretty much impossible not to)
                if (Physics.Raycast(v * 30, -v, out hit))
                {
                    ///Calculating the force of drag on the collider
                    Vector3 norm = hit.normal;
                    float theta = Vector3.Angle(norm, v);
                    Vector3 dragV = v * (Mathf.Cos(theta));
                    dragV *= dragV.magnitude;
                    Vector3 oppForce = -dragV / 2;

                    ///Applying that force opposite to the normal
                    me.AddForceAtPosition(oppForce, body.transform.position);
                }
            }
            ///For each thruster, we add a force
            foreach (GameObject body in thruster)
            {
                if (fuelAmount > 0)
                {
                    PropelThruster(body); 
                    fuelAmount -= fuelBurn;
                }
            }
            maxheight = Mathf.Max(transform.position.y, maxheight);
            if (this.Equals(null))
            {
                return;
            }
            if (myParent.Equals(null))
            {
                return;
            }
            myParent.timeInAir += 1;
        }
        ///If we're done, we signal to kill ourselves to the event handler
        else
        {
            if (this.Equals(null))
            {
                return;
            }
            if (myParent.Equals(null))
            {
                return;
            }
            myParent.maxHeight = maxheight;
            IndividualFunctions.ScoreIndividual(myParent);
            CustomEventHandler.SelfDestruct(this.gameObject);
        }

    }


    private void PropelThruster(GameObject obj)
    {
        me.AddForceAtPosition((transform.rotation * obj.transform.forward) * fuelForce, obj.transform.position);
    }
}
