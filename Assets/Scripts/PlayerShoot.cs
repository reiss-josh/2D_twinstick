
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Transform Arrow;
    private AudioSource audioSource;
    public AudioClip shootSound;
    public float volume = 0.5f;
    // Start is called before the first frame update
    void Awake()
    {
        FindObjectOfType<PlayerController>().shootEvent += Shoot;
        audioSource = GetComponent<AudioSource>();
    }

    public void Shoot(Vector3 arrowPos, Quaternion arrowAngle)
    {
        //Debug.Log(arrowPos);
        float pitch = Random.Range(0.5f, 1.5f);
        audioSource.pitch = pitch;
        audioSource.PlayOneShot(shootSound, volume);
        Instantiate(Arrow, arrowPos, arrowAngle);
    }
}