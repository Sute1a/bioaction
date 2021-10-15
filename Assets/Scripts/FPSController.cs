using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSController : MonoBehaviour
{
    //移動用の変数を作成
    float x, z;

    //スピード調整用の変数を作成
    float speed = 0.1f;

    //変数の宣言
    public GameObject cam;
    Quaternion cameraRot, characterRot;

    float Xsensityvity = 3f, Ysensityvity = 3f;


    bool cursorLock = true;

    float minX = -90f, maxX = 90f;

    public Animator animator;

    int ammunition = 50, maxAmmunition = 50, ammoClip = 10, maxAmmoClip = 10;

    int playerHP = 100, maxPlayerHP = 100;
    public Slider hpBer;
    public Text ammoText;

    public GameObject mainCamera, subCamera;

    public AudioSource playerFootStep;
    public AudioClip WalkFootStepSE,RunFootStepSE;

    public AudioSource voice, impact;
    public AudioClip hitVoiceSE, HitImpactSE;




    // Start is called before the first frame update
    void Start()
    {
        cameraRot = cam.transform.localRotation;
        characterRot = transform.localRotation;

        GameState.canShoot = true;

        hpBer.value = playerHP;
        ammoText.text = ammoClip + "/" + ammunition;
    }

    // Update is called once per frame(毎フレーム）
    void Update()
    {
        float xRot = Input.GetAxis("Mouse X") * Ysensityvity;
        float yRot = Input.GetAxis("Mouse Y") * Xsensityvity;

        cameraRot *= Quaternion.Euler(-yRot, 0, 0);
        characterRot *= Quaternion.Euler(0, xRot, 0);

        cameraRot = ClampRotation(cameraRot);

        cam.transform.localRotation = cameraRot;
        transform.localRotation = characterRot;

        UpdateCursorLock();

        if (Input.GetMouseButton(0) && GameState.canShoot)
        {
            if (ammoClip > 0)
            {

                animator.SetTrigger("Fire");
                GameState.canShoot = false;

                ammoClip--;
                ammoText.text = ammoClip + "/" + ammunition;
            }
            else
            {
                // Debug.Log("弾がないよ");

                Weapon.instance.TriggerSE();
            }
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            int amountNeed = maxAmmoClip - ammoClip;
            int ammouAvailble = amountNeed < ammunition ? amountNeed : ammunition;

            if (amountNeed != 0 && ammunition!=0)
            {
                animator.SetTrigger("Reload");

                ammunition -= ammouAvailble;
                ammoClip += ammouAvailble;
                ammoText.text = ammoClip + "/" + ammunition;
            }

            
        }

        if (Mathf.Abs(x)>0||Mathf.Abs(z)>0)
        {
            if(!animator.GetBool("walk"))
            {
                animator.SetBool("walk", true);

                PlayerWalkFootStep(WalkFootStepSE);
            }
        }
        else if (animator.GetBool("walk"))
        {
            animator.SetBool("walk", false);

            StopFootStep();
        }

        if(z>0 && Input.GetKey(KeyCode.LeftShift))
        {
            if (!animator.GetBool("Run"))
            {
                animator.SetBool("Run", true);
                speed = 0.25f;

                PlayerRunFootStep(RunFootStepSE);
            }
        }
        else if (animator.GetBool("Run"))
        {
            animator.SetBool("Run", false);
            speed = 0.1f;

            StopFootStep();
        }

        if (Input.GetMouseButton(1))
        {
            subCamera.SetActive(true);
            mainCamera.GetComponent<Camera>().enabled = false;
        }
        else if (subCamera.activeSelf)
        {
            subCamera.SetActive(false);
            mainCamera.GetComponent<Camera>().enabled = true;
        }

    }

    //(0.02秒ごと)
    private void FixedUpdate()
    {
        x = 0;
        z = 0;


        x = Input.GetAxisRaw("Horizontal") * speed;
        //Horizonntal=水平
        z = Input.GetAxisRaw("Vertical") * speed;
        //Vertical=垂直

        //transform.position += new Vector3(x,0,z);
        transform.position += cam.transform.forward * z + cam.transform.right * x;
    }

    public void UpdateCursorLock()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cursorLock = false;
        }
        else if(Input.GetMouseButton(0))
        {
            cursorLock = true;
        }
        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (!cursorLock)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public Quaternion ClampRotation(Quaternion q)
    {
        q.x /=q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1f;

        float angleX = Mathf.Atan(q.x) * Mathf.Rad2Deg * 2f;

        angleX = Mathf.Clamp(angleX,minX,maxX);

        q.x = Mathf.Tan(angleX * Mathf.Deg2Rad * 0.5f);

        return q;
    }

    public void PlayerWalkFootStep(AudioClip clip)
    {
        playerFootStep.loop = true;

        playerFootStep.pitch = 1f;

        playerFootStep.clip = clip;

        playerFootStep.Play();
    }

    public void PlayerRunFootStep(AudioClip clip)
    {
        playerFootStep.loop = true;

        playerFootStep.pitch = 1.3f;

        playerFootStep.clip = clip;

        playerFootStep.Play();
    }

    public void StopFootStep()
    {
        playerFootStep.Stop();

        playerFootStep.loop = false;

        playerFootStep.pitch = 1f;
    }

    public void TakeHit(float damage)
    {
        playerHP = (int)Mathf.Clamp(playerHP - damage, 0, playerHP);

        hpBer.value = playerHP;

        ImpactSE();

        if (Random.Range(0,10)<6)
        {
            VoiceSE(hitVoiceSE);
        }

        if(playerHP<=0 && !GameState.GameOver)
        {
            GameState.GameOver = true;
        }
    }

    public void VoiceSE(AudioClip clip)
    {
        voice.Stop();

        voice.clip = clip;
        voice.Play();
    }

    public void ImpactSE()
    {
        voice.clip = HitImpactSE;
        voice.Play();
    }
}
