using UnityEngine;
using System.Collections;

public class TimedDoor : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private Vector3 openOffset = new Vector3(0, 3, 0);
    [SerializeField] private float openSpeed = 2f;
    [SerializeField] private float openDuration = 5f;
    [SerializeField] private bool autoClose = true;
    [SerializeField] private PressurePlate pressurePlateReference;

    [Header("Audio")]
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;

    [Header("Visual")]
    [SerializeField] private GameObject openEffect;
    [SerializeField] private GameObject closeEffect;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpen = false;
    private bool isMoving = false;
    private Coroutine closeCoroutine;
    private AudioSource audioSource;

    private void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + openOffset;
        audioSource = GetComponent<AudioSource>();
        pressurePlateReference.OnActivated += PressurePlate_OnActivated;
        pressurePlateReference.OnDeactivated += PressurePlate_OnDeactivated;
    }
    
    private void PressurePlate_OnActivated(object sender, System.EventArgs e)
    {
        OpenDoor();
    }
    private void PressurePlate_OnDeactivated(object sender, System.EventArgs e)
    {
        CloseDoor();
    }

    public void OpenDoor()
    {
        if (isOpen || isMoving) return;

        if (closeCoroutine != null)
        {
            StopCoroutine(closeCoroutine);
        }

        StartCoroutine(MoveDoor(openPosition, true));

        if (autoClose && openDuration > 0f)
        {
            closeCoroutine = StartCoroutine(AutoClose());
        }
    }

    public void CloseDoor()
    {
        if (!isOpen || isMoving) return;

        if (closeCoroutine != null)
        {
            StopCoroutine(closeCoroutine);
        }

        StartCoroutine(MoveDoor(closedPosition, false));
    }

    private IEnumerator MoveDoor(Vector3 targetPosition, bool opening)
    {
        isMoving = true;

        if (opening)
        {
            PlaySound(openSound);
            SpawnEffect(openEffect);
        }
        else
        {
            PlaySound(closeSound);
            SpawnEffect(closeEffect);
        }

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                openSpeed * Time.deltaTime
            );
            yield return null;
        }

        transform.position = targetPosition;
        isOpen = opening;
        isMoving = false;
        StopSound();
    }

    private IEnumerator AutoClose()
    {
        yield return new WaitForSeconds(openDuration);
        CloseDoor();
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void StopSound()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    private void SpawnEffect(GameObject effect)
    {
        if (effect != null)
        {
            Instantiate(effect, transform.position, Quaternion.identity);
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 startPos = Application.isPlaying ? closedPosition : transform.position;
        Vector3 endPos = startPos + openOffset;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(startPos, endPos);
        Gizmos.DrawWireCube(endPos, transform.localScale);
    }
}
