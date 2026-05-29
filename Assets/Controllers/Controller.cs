using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    public Pawn pawn;
    public abstract void MakeDecisions();
 }
