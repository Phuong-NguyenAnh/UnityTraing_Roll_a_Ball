using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class MAWPlayer : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Running,
        Die,
        Spawn,
        Pause
    }
    public float speed;
    public GameObject pickUps;
    public Animator anim;

    private PlayerState currentState = PlayerState.Idle;
    private PlayerState previousState = PlayerState.Idle;

    private int count;

    private void Start()
    {
        count = 0;
        SetCountText();
        SetPlayerState(PlayerState.Idle);
    }

    public void SetPlayerState(PlayerState state)
    {
        if (currentState == state) return;
        switch (state)
        {
            case PlayerState.Running:
                anim.SetTrigger("Run");
                break;
            case PlayerState.Die:
                anim.SetTrigger("Die");
                break;
            case PlayerState.Spawn:
                anim.SetTrigger("Spawn");
                break;
            case PlayerState.Idle:
                anim.SetTrigger("Idle");
                break;
            case PlayerState.Pause:
            default:
                break;
        }
        anim.enabled = state != PlayerState.Pause;
        previousState = currentState == PlayerState.Pause ? previousState : currentState;
        currentState = state;
    }

    public void resume()
    {
        SetPlayerState(previousState);
    }

    public void OnDieEnd()
    {
        SetPlayerState(PlayerState.Spawn);
    }
    public void OnSpawnEnd()
    {
        SetPlayerState(PlayerState.Idle);
    }

    public bool IsAlive()
    {
        return (currentState == PlayerState.Idle || currentState == PlayerState.Running);
    }

    private Transform currentTarget = null;
    private void FixedUpdate()
    {
        if (IsAlive())
        {
            for (int i = 0; i < pickUps.transform.childCount && currentTarget == null; i++)
            {
                var tempTarget = pickUps.transform.GetChild(i);
                var script = tempTarget.GetComponent<PickUp>();
                if (!script.isDie)
                {
                    currentTarget = tempTarget;
                    break;
                }
            }

            if (currentTarget != null)
            {
                SetPlayerState(PlayerState.Running);
                transform.LookAt(currentTarget);
                float step = speed * Time.deltaTime;
                var pos = currentTarget.position;
                pos.y = 0;
                transform.position = Vector3.MoveTowards(transform.position, pos, step);
                transform.localRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            if (other.transform == currentTarget)
            {
                currentTarget = null;
            }
            var script = other.GetComponent<PickUp>();
            script.Die();
            count++;
            SetCountText();
            if (script.isRed)
            {
                SetPlayerState(PlayerState.Die);
            }
            else
            {
                bool haveTarget = false;
                for (int i = 0; i < pickUps.transform.childCount && !haveTarget; i++)
                {
                    var tempTarget = pickUps.transform.GetChild(i);
                    PickUp targetScript = tempTarget.GetComponent<PickUp>();
                    if (!targetScript.isDie)
                    {
                        haveTarget = true;
                        break;
                    }
                }
                SetPlayerState(haveTarget ? PlayerState.Running : PlayerState.Idle);
            }
        }
    }

    void SetCountText()
    {
        // countText.text = "Count: " + count.ToString();
    }
}
