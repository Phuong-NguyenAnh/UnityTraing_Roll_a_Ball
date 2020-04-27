using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class Player : MonoBehaviour
{
    public enum PlayerState
    {
        MainMenu,
        Init,
        Idle,
        Running,
        Die,
        Pause,
        GameOver,
    }
    public float speed;
    public GameObject pickUps;
    public Animator anim;

    public PlayerState currentState = PlayerState.MainMenu;
    private PlayerState previousState = PlayerState.MainMenu;

    private int count;

    public MainMenu mainMenu;

    public void SetPlayerState(PlayerState state)
    {
        if (currentState == state) return;

        anim.enabled = state != PlayerState.Pause;
        previousState = currentState == PlayerState.Pause ? previousState : currentState;
        currentState = state;

        switch (state)
        {
            case PlayerState.Init:
                PlayerStats stats = GetComponent<PlayerStats>();
                stats.Init();
                transform.position = Vector3.zero;
                SetPlayerState(PlayerState.Idle);
                break;
            case PlayerState.Idle:
                anim.SetTrigger("Idle");
                break;
            case PlayerState.Running:
                anim.SetTrigger("Run");
                break;
            case PlayerState.Die:
                anim.SetTrigger("Die");
                break;
            case PlayerState.GameOver:
                mainMenu.gameObject.SetActive(true);
                break;
            case PlayerState.Pause:
            default:
                break;
        }
    }

    public void Init()
    {
        SetPlayerState(PlayerState.Init);
    }

    public void resume()
    {
        SetPlayerState(previousState);
    }
    public void OnDieEnd()
    {
        SetPlayerState(PlayerState.GameOver);
    }
    public bool IsAlive()
    {
        return (currentState == PlayerState.Idle || currentState == PlayerState.Running);
    }

    public bool IsPlaying()
    {
        return (
            currentState == PlayerState.Idle
            || currentState == PlayerState.Running
            || currentState == PlayerState.Die
            );
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
            if (script.isRed)
            {
                PlayerStats stats = GetComponent<PlayerStats>();
                stats.TakeDamage(script.redDamage);
            }
        }
    }
}
