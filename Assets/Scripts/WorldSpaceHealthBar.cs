using UnityEngine;
using UnityEngine.UI;

public class WorldSpaceHealthBar : MonoBehaviour
{
    [SerializeField] private Health targetHealth; // the player's or enemy's Health
    [SerializeField] private Image fillImage; // the fill image
    [SerializeField] private Vector3 offset = new Vector3(0, 1.2f, 0);
    private Camera mainCam;

    void Start()
    {
        if (targetHealth == null)
        {
            var parentHealth = GetComponentInParent<Health>();
            if (parentHealth != null)
                targetHealth = parentHealth;
        }

        if (fillImage == null)
            Debug.LogWarning("Fill image not assigned on WorldSpaceHealthBar.");

        mainCam = Camera.main;
    }

    void LateUpdate()
    {
        if (targetHealth == null || fillImage == null) return;

        // Update fill
        fillImage.fillAmount = Mathf.Clamp01((float)targetHealth.currentHealth / targetHealth.maxHealth);

        // Position: keep above parent
        if (transform.parent != null)
            transform.position = transform.parent.position + offset;

        // Billboard toward camera
        if (mainCam != null)
        {
            Vector3 dir = transform.position - mainCam.transform.position;
            dir.y = 0; // optional: lock vertical rotation so it doesn't tilt weirdly
            if (dir.sqrMagnitude > 0.001f)
                transform.rotation = Quaternion.LookRotation(dir);
        }
    }
}

