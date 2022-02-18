using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteAlways]
public class CoordinateLabel : MonoBehaviour
{
    TextMeshPro label;
    Vector2Int coordinates = new Vector2Int();

    [SerializeField]
    GameObject mesh1, mesh2;

    private void Awake()
    {
        label = GetComponent<TextMeshPro>();

        DisplayCoordinates();
    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            DisplayCoordinates();
            UpdateObjectName();
        }
    }

    void DisplayCoordinates()
    {
        coordinates.x = Mathf.RoundToInt(transform.parent.position.x / UnityEditor.EditorSnapSettings.move.x);
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z / UnityEditor.EditorSnapSettings.move.z);

        if(mesh1 != null && mesh2 != null)
        {
            if ((coordinates.x + coordinates.y) % 2 == 0)
            {
                mesh1.SetActive(true);
                mesh2.SetActive(false);
            }
            else
            {
                mesh1.SetActive(false);
                mesh2.SetActive(true);
            }
        }


        label.text = coordinates.ToString();
    }

    void UpdateObjectName()
    {
        transform.parent.name = "Tile " + coordinates.ToString();
    }
}
