using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    //Tutorial followed: https://www.youtube.com/watch?v=BLfNP4Sc_iA&t=50s by Brackeys

    public Slider slider;
    public Gradient gradient;
    public Image fill;

    [SerializeField] private float _health = 100f; // Private backing field

    public float Health //Single source of truth principle
    {
        get { return _health; }
        set {
                _health = Mathf.Clamp(value, 0f, 100f); 
                UpdateHealthBar();

                if (_health <= 0)
                {
                    Debug.Log("Player is dead");
                }
            } // Clamps the value between 0 and 100
    }

    void Start()
    {
        Health = 100f; // Set the health to the initial value
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void UpdateHealthBar()
    {
        slider.value = Health;
        Debug.Log("Health: " + slider.value);
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

}
