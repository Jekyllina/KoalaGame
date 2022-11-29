using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Healthbar : MonoBehaviour
{
    [SerializeField] private List<Image> healthPoints;

    [SerializeField] private int healthPointsValue = 3;

    private void Update()
    {
        for (int i = 0; i < healthPoints.Count; i++)
        {
            healthPoints[i].enabled = i < healthPointsValue;
        }
    }
}
