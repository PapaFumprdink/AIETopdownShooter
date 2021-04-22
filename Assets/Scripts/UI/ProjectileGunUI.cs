using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class ProjectileGunUI : MonoBehaviour
{
    [SerializeField] private ProjectileGun m_TargetGun;
    [SerializeField] private Image m_ReloadProgressBar;
    [SerializeField] private TMP_Text m_AmmoCounter;

    private void Awake()
    {
        // Subscribes to the parents gun reload event
        m_TargetGun.ReloadEvent += () => StartCoroutine(ReloadRoutine());
    }

    private void Update()
    {
        // If we have an ammo counter, set the text to the current ammo over the magazine size, eg 12/14.
        if (m_AmmoCounter)
        {
            m_AmmoCounter.text = $"{m_TargetGun.CurrentMagazine}/{m_TargetGun.MagazineSize}";
        }
    }

    private IEnumerator ReloadRoutine()
    {
        var percent = 0f;
        while (percent < 1f)
        {
            // while reloading, fill the progress bar based on how far through the reload we are.
            if (m_ReloadProgressBar)
            {
                m_ReloadProgressBar.fillAmount = percent;
            }

            percent += Time.deltaTime / m_TargetGun.ReloadDuration;
            yield return null;
        }
    }
}
