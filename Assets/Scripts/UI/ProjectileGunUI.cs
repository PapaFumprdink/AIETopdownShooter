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
    [SerializeField] private TMP_Text m_FiremodeText;

    private void Awake()
    {
        m_TargetGun.ReloadEvent += () => StartCoroutine(ReloadRoutine());
    }

    private void Update()
    {
        if (m_AmmoCounter)
        {
            m_AmmoCounter.text = $"{m_TargetGun.CurrentMagazine}/{m_TargetGun.MagazineSize}";
        }

        if (m_FiremodeText)
        {
            if (!m_TargetGun.IsSingleFire)
                m_FiremodeText.text = "Fullauto";
            else if (m_TargetGun.BurstCount > 1)
                m_FiremodeText.text = $"{m_TargetGun.BurstCount} Round Burst";
            else
                m_FiremodeText.text = "Singlefire";
        }
    }

    private IEnumerator ReloadRoutine()
    {
        var percent = 0f;
        while (percent < 1f)
        {
            if (m_ReloadProgressBar)
            {
                m_ReloadProgressBar.fillAmount = percent;
            }

            percent += Time.deltaTime / m_TargetGun.ReloadDuration;
            yield return null;
        }
    }
}
