using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class toolTip : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 2f, 0);
    public TextMeshProUGUI tooltipText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(player.position + offset);
            transform.position = screenPos;

        }
    }
    public void Show(string toolTip)
    {
        tooltipText.text = toolTip;
        tooltipText.gameObject.SetActive(true);
    }
    public void Hide()
    {
        tooltipText.gameObject.SetActive(false);
    }
}
