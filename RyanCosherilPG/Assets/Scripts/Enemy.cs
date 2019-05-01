using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
    private bool isTargeted;
    private Health enemyHealth;
    private TargetingIndicator indicator;
    public Slider healthBar;
    EnemyAI ai;

	// Use this for initialization
	void Start () {
        GameManager.targets.Add(this);

        isTargeted = false;

        ai = gameObject.GetComponent<EnemyAI>();

        enemyHealth = gameObject.AddComponent<Health>();
        healthBar.value = enemyHealth.calculateHealth();
        healthBar.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

		if(isTargeted)
        {
            healthBar.GetComponentInParent<Canvas>().transform.rotation = Quaternion.LookRotation((-Camera.main.transform.position + healthBar.transform.position).normalized);// rotates the canvas of the healthbar to always look at the camera

            if (!indicator)
            {
                healthBar.gameObject.SetActive(true);
                indicator = gameObject.AddComponent<TargetingIndicator>();
            }
        }
        else
        {
            if (indicator)
            {
                healthBar.gameObject.SetActive(false);
                Destroy(gameObject.GetComponent<TargetingIndicator>());
            }             
        }
	}

    public void setIsTargeted(bool targeted)
    {
        this.isTargeted = targeted;
    }

    public void damage(float damage)
    {
        enemyHealth.adjustHealth(-damage);
        healthBar.value = enemyHealth.calculateHealth();
        ai.setCurrentState(EnemyAI.Transitions.seeSomething);

        if (enemyHealth.currentHealth <= 0)
        {
            GameObject.Find("player").SendMessage("breakLock");
            death();
        }       
    }

    void death()
    {
        GameObject goldDrop = Resources.Load<GameObject>("prefabs/goldCoins");
        GameManager.targets.Remove(this);
        Instantiate(goldDrop, gameObject.transform.position, goldDrop.transform.rotation);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        
    }
}
