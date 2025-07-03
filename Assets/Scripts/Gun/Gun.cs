using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GunConfig config;
    public Transform firePos;
    public Transform shootDirection;
    public int currentAmmo;
    public float nextTimeToFire;
    public bool isReloading;
    public GameObject shootingVFX;
    Vector3 gunBasePos;
    public BaseCharacter gunOwner;

    // Start is called before the first frame update
    void Start()
    {
        gunBasePos = transform.localPosition;
        shootDirection = Camera.main.transform;
        currentAmmo = config.magazineSize;
    }

    public void Shoot(Animator reloadAnim)
    {
        if (isReloading || Time.time < nextTimeToFire) return;
        if(currentAmmo <= 0 && !isReloading)
        {
            StartCoroutine(Reload(reloadAnim));
            return;
        }

        nextTimeToFire = Time.time + 1f / config.fireRate;
        currentAmmo--;       
        ApplyRecoil();
        if (config.typeOfGun == GunType.Shotgun)
        {
            for (int i = 0; i < 5; i++)
            {
                SetBullet();
            }
        }
        else
        {
            SetBullet();
        }
            
        var flash = Instantiate(shootingVFX, firePos.position, firePos.rotation);       
        //config.shotSFX.Play();             
    } 
    public IEnumerator Reload(Animator reloadAnim)
    {
        if (isReloading || currentAmmo == config.magazineSize) yield break;
        isReloading = true;
        //config.reloadSFX.Play();
        reloadAnim?.SetTrigger("isReloading");
        yield return new WaitForSeconds(config.reloadTime);
        currentAmmo = config.magazineSize;
        isReloading = false;
        yield break;
    }
    void SetBullet()
    {
        if (BulletPool.Instance == null || firePos == null)
        {
            Debug.LogError("Missing BulletPool, or firePos");
            return;
        }

        Vector3 bulletDirection;
        Vector3 spreadOffset = Random.insideUnitSphere * config.spreadOffset;
        Vector3 raycastDirection = (shootDirection.forward + spreadOffset).normalized;      

        if (Physics.Raycast(shootDirection.position, raycastDirection, out RaycastHit hit, config.range, config.hitLayer))
        {
            //Debug.Log("Hit: " + hit.collider.name);
            bulletDirection = (hit.point - firePos.position).normalized;
        }
        else
        {
            bulletDirection = raycastDirection;
        }

        GameObject bullet = BulletPool.Instance.GetPooledObject();
        if (bullet != null)
        {
            bullet.GetComponent<Bullet>().Initialize(config, bulletDirection, firePos, gunOwner);
            bullet.SetActive(true);
        }       
    }
    void ApplyRecoil()
    {     
        Vector3 newPos = transform.localPosition;
        newPos.z -= config.recoil;
        transform.localPosition = newPos;
        StartCoroutine(RecoverGunPosition(0.2f));
    }

    IEnumerator RecoverGunPosition(float duration)
    {
        Vector3 startPos = transform.localPosition;
        Vector3 targetPos = new Vector3(transform.localPosition.x, transform.localPosition.y, gunBasePos.z);
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
        
            Vector3 lerpedPos = transform.localPosition;
            lerpedPos.z = Mathf.Lerp(startPos.z, targetPos.z, t);
            transform.localPosition = lerpedPos;
            yield return null;
        }

        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, targetPos.z);
    }
}
