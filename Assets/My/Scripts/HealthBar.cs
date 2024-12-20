using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteInEditMode]
public class HealthBar : MonoBehaviour
{
    [Range(0, 1)]
    public float percentage;
    public TextMeshProUGUI text;
    public Material curMat;
    public Color colorStart, colorEnd, colorText;
    private Color color;

    private PlayerManager player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        color = Color.Lerp(colorStart, colorEnd, percentage);
        curMat.SetFloat("_percentage", percentage);
        curMat.SetColor("_Color", color);

        text.color = colorText;
        percentage = (float)(player.getHp()/100f);
        int val = (int)(percentage*100f);
        text.text = val.ToString() + "%";
    }
}
