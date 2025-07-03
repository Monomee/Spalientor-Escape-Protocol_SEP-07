using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public BaseCharacter gunOwner;
    private float speed;
    private float lifetime = 0.5f;
    private Vector3 direction; 
    private GunConfig config;
    private Transform firePos;

    private LineRenderer lineRenderer;
    private float trailDuration = 0.1f; 
    private List<Vector3> trailPoints = new List<Vector3>(); 
    private List<float> pointLifetimes = new List<float>();
    private const int maxTrailPoints = 8;
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    public void Initialize(GunConfig gunConfig, Vector3 bulletDirection, Transform startPosition, BaseCharacter owner)
    {
        config = gunConfig;
        speed = config.bulletSpeed;
        direction = bulletDirection.normalized;
        transform.position = startPosition.transform.position; 
        transform.rotation = Quaternion.LookRotation(direction); 
        firePos = startPosition;
        gunOwner = owner;

        trailPoints.Clear();
        pointLifetimes.Clear();
        trailPoints.Add(firePos.position);
        pointLifetimes.Add(trailDuration);
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, firePos.position);
    }
    void OnEnable()
    {
        Invoke("Deactivate", lifetime);
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, firePos.position) > config.range)
            Deactivate();
        DrawTrail();   
    }
    void Deactivate()
    {
        CancelInvoke();
        trailPoints.Clear();
        pointLifetimes.Clear();
        lineRenderer.positionCount = 0;
        lineRenderer.SetPositions(new Vector3[0]);
        gameObject.SetActive(false);
    }
    void DrawTrail()
    {
        // Thêm điểm trail mới
        trailPoints.Add(transform.position);
        pointLifetimes.Add(trailDuration);

        // Giới hạn số điểm trail tối đa 8
        while (trailPoints.Count > maxTrailPoints)
        {
            trailPoints.RemoveAt(0);
            pointLifetimes.RemoveAt(0);
        }

        // Cập nhật thời gian sống của các điểm trail
        for (int i = pointLifetimes.Count - 1; i >= 0; i--)
        {
            pointLifetimes[i] -= Time.deltaTime;
            if (pointLifetimes[i] <= 0)
            {
                trailPoints.RemoveAt(i);
                pointLifetimes.RemoveAt(i);
            }
        }

        // Cập nhật LineRenderer
        lineRenderer.positionCount = trailPoints.Count;
        if (trailPoints.Count > 0)
        {
            lineRenderer.SetPositions(trailPoints.ToArray());

            // Cập nhật alpha theo thời gian sống của mỗi điểm
            Gradient gradient = new Gradient();
            GradientColorKey[] colorKeys = new GradientColorKey[] {
            new GradientColorKey(Color.white, 0f),
            new GradientColorKey(Color.white, 1f)
        };
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[Mathf.Min(trailPoints.Count, maxTrailPoints)];
            for (int i = 0; i < alphaKeys.Length; i++)
            {
                float alpha = pointLifetimes[i] / trailDuration;
                alphaKeys[i] = new GradientAlphaKey(alpha, (float)i / (alphaKeys.Length - 1));
            }
            gradient.SetKeys(colorKeys, alphaKeys);
            lineRenderer.colorGradient = gradient;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("canBeAttacked") ||
            other.CompareTag("Player"))
        {
            BaseCharacter hitCharacter = other.GetComponent<BaseCharacter>();
            if (hitCharacter == null || hitCharacter == gunOwner)
                return; // ko tan cong chinh minh

            HealthStatus healthStatus = hitCharacter.GetBehavior<HealthStatus>(BehaviorType.Health);

            // dan cua Player chi ban enemy
            if (gunOwner.characterType == CharacterType.Player && hitCharacter.characterType != CharacterType.Player)
            {
                if (other is SphereCollider)
                {
                    Debug.Log("HeadShot");
                    hitCharacter.isAlive = healthStatus.TakeDamage(config.damage * 2f);
                }
                else if (other is BoxCollider)
                {
                    Debug.Log("Hit");
                    hitCharacter.isAlive = healthStatus.TakeDamage(config.damage);
                }
                Deactivate();
            }
            // dan cua SpecialForce chi tan cong Player
            else if (gunOwner.characterType != CharacterType.Player && hitCharacter.characterType == CharacterType.Player)
            {
                Debug.Log("Hit Player");
                hitCharacter.isAlive = healthStatus.TakeDamage(config.damage);
                if (!hitCharacter.isAlive)
                {
                    UIManagerMap2.Instance.OpenDeadText();
                }
                Deactivate();
            }
            // con lai thi bo qua
            else
            {
                //Debug.Log("Bullet hit but no damage dealt");
                Deactivate();
            }
        }
        Deactivate();
    }
}
