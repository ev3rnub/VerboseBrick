//Course: GPE104 
//Prof: Matthew Henry 
//Student: Chad V

using UnityEngine;

public abstract class Pawn : MonoBehaviour
{
    public MyController someController; 

    public abstract void Move(Vector3 someMoveVector);
    public abstract void Rotate(float someAngle);
    public abstract void Shoot(bool someBool);
    public abstract void Start();
}