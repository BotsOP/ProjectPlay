using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] float coolDownShoot = 1f;
    [SerializeField] float knockbackTime = 5f;
    float lastTime;
    bool lerp;
    private IEnumerator coroutine;
    Vector3 knockback;
    float speedKnockback;
    int ammo = 1;
    public Text txt;
    BasicMovement basicMovement;
    Camera mainCamera;
    Rigidbody rb;
    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        lastTime = Time.time + coolDownShoot;
        basicMovement = GetComponent<BasicMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
        if(basicMovement.isGrounded)
        {
            ammo = 1;
        }
        txt.text = "Ammo: " + ammo;
    }

    private void Shoot()
    {
        if(lastTime <= Time.time && Input.GetMouseButton(0) && ammo > 0)
        {
            Debug.Log("shot");
            ammo--;
            coroutine = LerpKnockback(knockbackTime);
            StartCoroutine(coroutine);
            
            knockback = new Vector3(mainCamera.transform.forward.x, mainCamera.transform.forward.y / 30, mainCamera.transform.forward.z);
            lastTime = Time.time + coolDownShoot;
        }
        

        if(lerp)
        {
            speedKnockback = Mathf.Lerp(0,1000, (lastTime - Time.time + knockbackTime - coolDownShoot) / knockbackTime);
        }

        Vector3 velocity = knockback * speedKnockback * Time.deltaTime * -1 * 20;
        rb.AddForce(velocity.x, velocity.y, velocity.z, ForceMode.Impulse);
    }

    private IEnumerator LerpKnockback(float waitTime)
    {
        lerp = true;
        yield return new WaitForSeconds(waitTime);
        lerp = false;
        speedKnockback = 0;
    }
}
