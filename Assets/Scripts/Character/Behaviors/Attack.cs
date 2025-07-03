using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Attack : IBehavior
{
    private BaseCharacter owner;
    private Gun currentGun;

    public Gun CurrentGun { get => currentGun; set => currentGun = value; }

    public void Initialize(BaseCharacter owner)
    {
        this.owner = owner;

        //GameObject found = GameManager.Instance.FindChildWithTag(owner.gameObject, "weapon");
        currentGun = GameManager.Instance.FindGunInChild(owner.transform);
        if (currentGun != null)
        {
            //currentGun = found.GetComponent<Gun>();
            currentGun.enabled = true;
            currentGun.gunOwner = owner;
            if (owner.characterType != CharacterType.Player)
            {
                Transform shootDir = currentGun.transform;
                shootDir.position = currentGun.firePos.position;
                currentGun.shootDirection = shootDir;
            }
            else
            {
                currentGun.shootDirection = Camera.main.transform;
                UIManagerMap2.Instance.bulletNumber.text = currentGun.config.magazineSize + "/"+currentGun.config.magazineSize;
            }
        }
    }

    public void UpdateBehavior(Transform target = null)
    {
        if (currentGun == null) return;
        if (owner.characterType != CharacterType.Player && target != null)
        {
            Vector3 direction = (target.position - owner.transform.position).normalized;
            direction.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 60f, 0);//do model lech
            owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, lookRotation, Time.deltaTime * 10f);

            CurrentGun.Shoot(owner.gameObject.GetComponent<Animator>());
            return;
        }
        if (owner.characterType == CharacterType.Player)
        {
            if ((CurrentGun.config.isAutomatic ? Input.GetMouseButton(0) : Input.GetMouseButtonDown(0)) && !CurrentGun.isReloading)
            {
                CurrentGun.Shoot(CurrentGun.GetComponent<Animator>());
                UIManagerMap2.Instance.bulletNumber.text = CurrentGun.currentAmmo + "/" + CurrentGun.config.magazineSize;
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                CurrentGun.StartCoroutine(CurrentGun.Reload(CurrentGun.GetComponent<Animator>()));
                UIManagerMap2.Instance.bulletNumber.text = CurrentGun.currentAmmo + "/" + CurrentGun.config.magazineSize;
            }
        }
    }

}
