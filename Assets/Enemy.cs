using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector]
    public int vida = 100;

    [HideInInspector]
    public int ataque = 20;

    [HideInInspector]
    public float velocidad = 5f;

    [HideInInspector]
    public GameObject target;
}
