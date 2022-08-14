using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private float damageTimeLeft; // amount of time left the damage indicator displays
    private const float DISPLAY_DURATION = 1.0f;

    // Update is called once per frame
    void Update()
    {
        if(damageTimeLeft > 0) {
            damageTimeLeft -= Time.deltaTime;
            if(damageTimeLeft <= 0) {
                transform.GetChild(2).gameObject.SetActive(false); // turn off damage indicator
                gameObject.SetActive(false);
            }
        }
    }

    public void Increase(uint amount, float newPercentage) {
        gameObject.SetActive(true);
        GameObject damageDisplay = transform.GetChild(2).gameObject;
        damageDisplay.SetActive(true);
        damageTimeLeft = DISPLAY_DURATION;
        UpdateHealthBar(newPercentage);

        TMPro.TMP_Text textBox = damageDisplay.GetComponent<TMPro.TMP_Text>();
        textBox.color = Color.green;
        textBox.text = "+" + amount;
    }

    public void Reduce(uint amount, float newPercentage) {
        gameObject.SetActive(true);
        GameObject damageDisplay = transform.GetChild(2).gameObject;
        damageDisplay.SetActive(true);
        damageTimeLeft = DISPLAY_DURATION;
        UpdateHealthBar(newPercentage);

        TMPro.TMP_Text textBox = damageDisplay.GetComponent<TMPro.TMP_Text>();
        textBox.color = Color.red;
        textBox.text = "-" + amount;
    }

    private void UpdateHealthBar(float percentage) {
        GameObject hpBar = transform.GetChild(0).gameObject;
        GameObject hpBack = transform.GetChild(1).gameObject;
        Vector3 newScale = hpBack.transform.localScale;
        newScale.x *= percentage;
        float shift = (hpBack.transform.localScale.x - newScale.x) / 2; // keep left-justified
        hpBar.transform.localScale = newScale;
        Vector3 newPos = hpBack.transform.position;
        newPos.x -= shift;
        newPos.z = -1; // make sure color lies over back
        hpBar.transform.position = newPos;
    }
}
