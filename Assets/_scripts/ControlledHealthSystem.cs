using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControlledHealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Damage Settings")]
    public float damagePerSecond = 2f;
    public GameObject bloodPrefab;
    public Transform bloodSpawnPoint;

    [Header("External Control Bools")]
    public bool detection = false;
    public bool totalHealthZero = false;

    [Header("UI")]
    public Slider healthBar;
    public GameObject failPanel;
    public GameObject joystick;
    public GameObject fire;
    public GameObject Crosshair;
    public GameObject Pausebutton;

    [Header("Animator")]
    public Animator animator;

    [Header("Sounds")]
    public AudioSource audioSource;
    public AudioClip hurtSound;
    public AudioClip deathSound;

    [Header("Hurt Sound Settings")]
    public float hurtSoundInterval = 0.4f;
    float nextHurtSoundTime;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        if (failPanel != null)
            failPanel.SetActive(false);
    }

    void Update()
    {
        if (detection && !totalHealthZero)
        {
            TakeDamage(damagePerSecond * Time.deltaTime);
        }

        if (healthBar != null)
            healthBar.value = currentHealth;
    }

    public void TakeDamage(float damage)
    {
        if (totalHealthZero) return;

        // Reduce health
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // 🔴 Blood effect
        if (bloodPrefab != null)
        {
            Vector3 spawnPos = bloodSpawnPoint != null ? bloodSpawnPoint.position : transform.position;
            Quaternion rot = Quaternion.Euler(Random.Range(-30, 30), Random.Range(0, 360), 0);
            GameObject blood = Instantiate(bloodPrefab, spawnPos, rot);
            blood.transform.localScale *= Random.Range(0.8f, 1.2f);
        }

        // 🔊 Hurt sound (interval based)
        if (hurtSound && audioSource && Time.time >= nextHurtSoundTime && currentHealth > 0)
        {
            audioSource.PlayOneShot(hurtSound);
            nextHurtSoundTime = Time.time + hurtSoundInterval;
        }

        // Death check
        if (currentHealth <= 0)
        {
            KillPlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Die"))
        {
            KillPlayer();
        }
    }

    public void KillPlayer()
    {
        if (totalHealthZero) return;

        // 🔊 Death sound (one time)
        if (deathSound && audioSource)
            audioSource.PlayOneShot(deathSound);

        currentHealth = 0;
        animator.SetBool("isDead", true);
        totalHealthZero = true;

        StartCoroutine(ShowFailPanelAfterDelay());
        Debug.Log("Player Dead");
    }

    IEnumerator ShowFailPanelAfterDelay()
    {
        yield return new WaitForSeconds(6f);

        if (failPanel != null)
            failPanel.SetActive(true);
        joystick.SetActive(false);
        fire.SetActive(false);
        Pausebutton.SetActive(false);
        Crosshair.SetActive(false);
    }
}
