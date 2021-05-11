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
    [SerializeField] private int m_MagazineSize;
    [SerializeField] private float m_Reloadtime;

    [Space]
    [SerializeField] private Transform m_Muzzle;

    [Space]
    [SerializeField] private CinemachineImpulseSource m_ShakeSource;
    [SerializeField] private ParticleSystem[] m_ShootFXs;

    [Space]
    [SerializeField] private CustomCursor m_CursorObject;
    [SerializeField] private Sprite m_CursorImage;
    [SerializeField] private Sprite m_CursorReloadImage;

    private int m_CurrentMagazine;
    private float m_NextFireTime;
    private bool m_IsReloading;

    public GameObject ProjectilePrefab => m_ProjectilePrefab;
    public int ProjectileCount => m_ProjectileCount;
    public int BurstCount => m_BurstCount;
    public float BurstRate => m_BurstRate;
    public float FireRate => m_Firerate;
    public bool IsSingleFire => m_Singlefire;
    public float Spray => m_Spray;
    public int MagazineSize => m_MagazineSize;
    public int CurrentMagazine => m_CurrentMagazine;
    public float ReloadDuration => m_Reloadtime;

    private IWeaponInputProvider InputProvider { get; set; }

    private void Awake()
    {
        // Gets the input provider from parental chain.
        InputProvider = GetComponentInParent<IWeaponInputProvider>();

        // Subscribes to nessecary events.
        InputProvider.FireEvent += (bool down) =>
        {
            if (m_Singlefire == down && enabled && gameObject.activeSelf)
            {
                Shoot();
            }
        };
        InputProvider.ReloadEvent += Reload;

        // Set the current magazine so the player does not have to initially reload.
        m_CurrentMagazine = m_MagazineSize;
    }

    private void OnEnable()
    {
        if (InputProvider.UseCursor)
        {
            m_CursorObject.FillPercent = 1f;
            m_CursorObject.CursorIcon = m_CursorImage;
        }
    }

    private void OnDisable()
    {
        // Sets the reloading flag, otherwise when re-enabled the weapon will still be 'reloading' forever,
        // as the coroutine was canceled but the flag wasn't set.
        m_IsReloading = false;
    }

    private void Update()
    {
        if (InputProvider == null)
        {
            // If we dont have an input provider, keep looking.
            InputProvider = GetComponentInParent<IWeaponInputProvider>();
        }
    }

    public void Shoot ()
    {
        // Only shoot if enough time has passed since the last shot.
        if (Time.time > m_NextFireTime)
        {
            StartCoroutine(ShootRoutine());

            // set the next fire time based on the firerate, scales on how many bullets are in the burst function.
            m_NextFireTime = Time.time + (60f / m_Firerate) * m_BurstCount;
        }
    }

    private IEnumerator ShootRoutine()
    {
        // Itterates for each burst count
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
            // If we have run out of bullets in our magazine, reload and cancel the routine.
            else
            {
                Reload();
                yield break;
            }
        }
    }

    public void Reload ()
    {
        // Only reload if the magazine isnt full, we arent already reloading, we are enabled
        if (m_CurrentMagazine < m_MagazineSize && !m_IsReloading && isActiveAndEnabled)
        {
            StartCoroutine(ReloadRoutine());
        }
    }

    private IEnumerator ReloadRoutine()
    {
        if (InputProvider.UseCursor)
        {
            m_CursorObject.FillPercent = 0f;
            m_CursorObject.CursorIcon = m_CursorReloadImage;
        }

        m_IsReloading = true;
        m_CurrentMagazine = 0;

        ReloadEvent?.Invoke();

        float percent = 0f;
        while (percent < 1f)
        {
            if (InputProvider.UseCursor)
            {
                m_CursorObject.FillPercent = percent;
            }

            percent += Time.deltaTime / m_Reloadtime;
            yield return null;
        }

        m_CurrentMagazine = m_MagazineSize;
        m_IsReloading = false;

        if (InputProvider.UseCursor)
        {
            m_CursorObject.FillPercent = 1f;
            m_CursorObject.CursorIcon = m_CursorImage;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (m_Muzzle)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, Quaternion.Euler(0f, 0f, m_Spray) * m_Muzzle.right);
            Gizmos.DrawRay(transform.position, Quaternion.Euler(0f, 0f, -m_Spray) * m_Muzzle.right);
            Gizmos.color = Color.white;
        }
    }
}
