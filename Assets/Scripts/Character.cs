using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Character : MonoBehaviour{
    public enum GroundCheck { Ray, Sphere, Cube }

    protected bool IsGrounded { get; private set; } = false;
    protected bool WasGrounded { get; set; } = false;

    Vector3 FeetsCenterPos{
        get => Vector3X.IgnoreY(characterCollider.bounds.center, characterCollider.bounds.min.y);
    }

    float GroundRadius{
        get => characterCollider.bounds.size.x / 2f;
    }

    public float groundDistance = .02f;
    public GroundCheck groundCheck;
    public LayerMask groundMask;
    public Collider characterCollider;

    public virtual void Update(){
        IsGrounded = false;

        if (groundCheck == GroundCheck.Ray)
            IsGrounded = IsGroundedCheckRay();
        else if (groundCheck == GroundCheck.Sphere)
            IsGrounded = IsGroundedCheckSphere();
        else if (groundCheck == GroundCheck.Cube)
            IsGrounded = IsGroundedCheckSphere();
    }

    public virtual void FixedUpdate(){
        WasGrounded = IsGrounded;
    }

    private RaycastHit RaycastHitX(Vector3 origin, Vector3 direction, LayerMask layerMask, float maxDistance = 10f, bool debug = false){
        Ray ray = new Ray(origin, direction);
        RaycastHit hit = new RaycastHit();

        Physics.Raycast(ray.origin, ray.direction, out hit, maxDistance, layerMask);
        return hit;
    }

    protected bool IsGroundedCheckRay(){
        Vector3[] origins = new Vector3[] {
            characterCollider.bounds.center,  
            characterCollider.bounds.center + Vector3.right * -GroundRadius, 
            characterCollider.bounds.center + Vector3.right * GroundRadius,  
            characterCollider.bounds.center + Vector3.forward * -GroundRadius,
            characterCollider.bounds.center + Vector3.forward * GroundRadius, 
        };

        float maxDistance = groundDistance + characterCollider.bounds.center.y - characterCollider.bounds.min.y;
        List<RaycastHit> hits = new List<RaycastHit>();

        foreach (var el in origins)
            hits.Add(RaycastHitX(el, Vector3.down, groundMask, maxDistance));
        return hits.Select(el => el.collider != null).Contains(true);
    }

    protected bool IsGroundedCheckSphere(){
        if(groundCheck == GroundCheck.Sphere)
            return Physics.CheckSphere(FeetsCenterPos, GroundRadius, groundMask);
        else if (groundCheck == GroundCheck.Cube)
            return Physics.CheckBox(FeetsCenterPos, Vector3.one * GroundRadius, Quaternion.identity, groundMask);

        return false;
    }
}