using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] GameObject BalaPrefab;
	private GameObject gmBala;
	private GameObject Pistola;
	[SerializeField] float rayLength;
	Tween CameraShake;
	Tween PistolaShake;
	[SerializeField] float InitialTimer;
	private float timer;

    private void Awake()
    {
		Pistola = GameObject.Find("ArmaHolder");
    }
    private void Start()
    {
        CameraShake = Camera.main.DOShakeRotation(.2f, Random.Range(1f, 5f), 10, 45, true);

		CameraShake.SetEase(Ease.OutSine);
		CameraShake.SetRelative(true);
		CameraShake.SetRecyclable(true);
		CameraShake.SetAutoKill(false);

		PistolaShake = Pistola.transform.DOShakeRotation(.2f, Random.Range(1f, 5f), 3, 45, true);

		PistolaShake.SetEase(Ease.OutSine);
		PistolaShake.SetRelative(true);
		PistolaShake.SetRecyclable(true);
		PistolaShake.SetAutoKill(false);

	}
    private void Update()
    {
        if (Input.GetButton("Fire1"))
        {
			timer += Time.deltaTime;

			if (timer >= InitialTimer)
            {
				Shoot();
				timer = 0;
			}
        }

    }

    private void Shoot()
    {
		RaycastHit hit;

		CameraShake.Restart();
		PistolaShake.Restart();

		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayLength))
		{
			if (hit.transform.gameObject.CompareTag("Enemy"))
			{
				Debug.Log("Enemigo alcanzado");
				int i = hit.collider.gameObject.GetComponent<AgentController>().EN.VidaEnemigoGS--;
				Debug.Log("Vida Enemigo: " + i);
			}

			gmBala = Instantiate(BalaPrefab, hit.point, Quaternion.LookRotation(hit.normal));
			Destroy(gmBala, .5f);
		}
	}
}
