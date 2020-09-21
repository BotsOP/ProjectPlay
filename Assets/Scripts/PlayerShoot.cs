using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [SerializeField] float coolDownShoot = 1f;
    Camera mainCamera;
    float lastTime;
    Rigidbody rb;
    bool lerp;
    private IEnumerator coroutine;
    Vector3 knockback;
    float speed2;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;

        lastTime = Time.time + coolDownShoot;
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }

    private void Shoot()
    {
        if(lastTime <= Time.time && Input.GetMouseButton(0))
        {
            coroutine = LerpKnockback(coolDownShoot);
            StartCoroutine(coroutine);
            
            knockback = new Vector3(mainCamera.transform.forward.x, mainCamera.transform.forward.y / 100, mainCamera.transform.forward.z);
            lastTime = Time.time + coolDownShoot;
        }

        if(lerp)
        {
            speed2 = Mathf.Lerp(0,1000,lastTime - Time.time);
        }

        Vector3 zMov2 = knockback * speed2 * Time.deltaTime * -1;
        rb.AddForce(zMov2.x, zMov2.y, zMov2.z, ForceMode.Impulse);
    }

    private IEnumerator LerpKnockback(float waitTime)
    {
        lerp = true;
        yield return new WaitForSeconds(waitTime);
        lerp = false;
        speed2 = 0;
    }
}
