using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUI : MonoBehaviour
{
    private Transform healthBar;
    private Canvas canvas;
    public Image image;
    private Vector3 _playerPos;
    private MeshRenderer _renderer;

    private void Awake()
    {
        healthBar = gameObject.transform.Find("healthBar");
        canvas = gameObject.transform.GetComponentInChildren<Canvas>();      
    }

    private void Start()
    {
        image = Instantiate(healthBar, canvas.transform).GetComponent<Image>();
        image.transform.localScale = new Vector3(0.02f, 0.02f);
    }

    private void Update()
    {
        image.transform.position = transform.position + new Vector3(0, 2, 0); //привязка шкалы к главной камере
        image.transform.LookAt(Camera.main.transform);
    }  
    
}
