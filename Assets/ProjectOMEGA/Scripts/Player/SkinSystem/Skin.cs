using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skin : MonoBehaviour
{
    [SerializeField] private string skinID;
    [SerializeField] private Animator animator;

    public string GetID()
    {
        return skinID;
    }

    public Animator GetAnimator()
    {
        return animator;
    }
}
