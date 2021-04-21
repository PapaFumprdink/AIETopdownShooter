using System.Collections;
using UnityEngine;
using Cinemachine;

[DisallowMultipleComponent]
public class ProjectileGun : MonoBehaviour
{
    public event System.Action ShootEvent;
    public event System.Action ReloadEvent;

    [SerializeField] private GameObject m_ProjectilePrefab;
    [SerializeField] private int m_ProjectileCount;
    [SerializeField] private int m_BurstCount;
    [SerializeField] private float m_BurstRate;
    [SerializeField] private float m_Firerate;
    [SerializeField] private bool m_Singlefire;
    [SerializeField] private float m_Spray;

    [Space]
    [SerializeField] private float m_AimDownSightsTime;
    [SerializeField] private AnimationCurve m_AimDownSightsCurve;

    [Space]
    [SerializeField] private int m_MagazineSize;
    [SerializeField] private float m_Reloadtime;

    [Space]
    [SerializeField] private Transform m_Muzzle;

    [Space]
    [SerializeField] private CinemachineImpulseSource m_ShakeSource;
    [SerializeField] private ParticleSystem[] m_ShootFXs;

    private int m_CurrentMagazine;
    private float m_NextFireTime;
    private bool m_IsReloading;
    private bool m_IsAimDownSights;
    private float m_AimDownSightsPercent;

    public GameObject ProjectilePrefab => m_ProjectilePrefab;
    public int ProjectileCount => m_ProjectileCount;
    public int BurstCount => m_BurstCount;
    public float BurstRate => m_BurstRate;
    public float FireRate => m_Firerate;
    public bool IsSingleFire => m_Singlefire;
    public float Spray => m_Spray;
    public float AimDownSightsTime => m_AimDownSightsTime;
    public int MagazineSize => m_MagazineSize;
    public int CurrentMagazine => m_CurrentMagazine;
    public float ReloadDuration => m_Reloadtime;

    private IWeaponInputProvider InputProvider { get; set; }

    private void Awake()
    {
        InputProvider = GetComponentInParent<IWeaponInputProvider>();

        InputProvider.FireEvent += () =>
        {
            if (m_Singlefire && enabled && gameObject.activeSelf)
            {
                Shoot();
            }
        };
        InputProvider.ReloadEvent += Reload;

        InputProvider.CycleWeaponEvent += (index) =>
        {
            var clampedIndex = Util.Loop(index, transform.parent.childCount);
            gameObject.SetActive(transform.GetSiblingIndex() == clampedIndex);
            print($"{index} || {clampedIndex}");
        };

        gameObject.SetActive(transform.GetSiblingIndex() == 0);
        m_CurrentMagazine = m_MagazineSize;
    }

    private void OnDisable()
    {
        m_IsReloading = false;
    }

    private void Update()
    {
        if (InputProvider != null)
        {
            if (InputProvider.WantsToFire && !m_Singlefire)
            {
                Shoot();
            }

            m_IsAimDownSights = InputProvider.IsAimingDownSights;

        }
        else
        {
            InputProvider = GetComponentInParent<IWeaponInputProvider>();
        }
    }

    public void Shoot ()
    {
        if (Time.time > m_NextFireTime)
        {
            StartCoroutine(ShootRoutine());
            m_NextFireTime = Time.time + (60f / m_Firerate) * m_BurstCount;
        }
    }

    private IEnumerator ShootRoutine()
    {
        for (int i = 0; i < m_BurstCount; i++)
        {
            if (m_CurrentMagazine > 0)
            {
                for (int j = 0; j < m_ProjectileCount; j++)
                {
                    Instantiate(m_ProjectilePrefab, m_Muzzle.position, Quaternion.Euler(0, 0, Random.Range(-m_Spray, m_Spray) / 2f) * m_Muzzle.rotation);
                }

                foreach (ParticleSystem fx in m_ShootFXs)
                {
                    fx.Play();
                }

                m_ShakeSource.GenerateImpulse();
                ShootEvent?.Invoke();

                m_CurrentMagazine--;
                yield return new WaitForSeconds(60f / m_BurstRate);
            }
            else
            {
                Reload();
                yield break;
            }
        }
    }

    public void Reload ()
    {
        if (m_CurrentMagazine < m_MagazineSize && !m_IsReloading && enabled && gameObject.activeSelf)
        {
            StartCoroutine(ReloadRoutine());
        }
    }

    private IEnumerator ReloadRoutine()
    {
        m_IsReloading = true;
        m_CurrentMagazine = 0;

        ReloadEvent?.Invoke();

        yield return new WaitForSeconds(m_Reloadtime);

        m_CurrentMagazine = m_MagazineSize;
        m_IsReloading = false;
    }
}
