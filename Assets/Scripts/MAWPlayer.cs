using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class MAWPlayer : MonoBehaviour
{
public enum PlayerState {
    Idle,
    Running,
    Die,
    Spawn
}
    public float speed;
    public Text countText;
    public Text winText;
    public GameObject pickUps;
    public Animator anim;

    private PlayerState currentState = PlayerState.Idle;

    private int count;
    
    private void Start() {
        count = 0;
        SetCountText();
        winText.text = "";
        SetPlayerState(PlayerState.Idle);
    }

    public void SetPlayerState(PlayerState state) {
        if (currentState == state) return;
        currentState = state;
        switch(state) {
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
            default:
            anim.SetTrigger("Idle");
            break;
        }
    }

    public void Spawn() {
        transform.position = Vector3.zero;
        SetPlayerState(PlayerState.Spawn);
    }

    public void SpawnFinish() {
        SetPlayerState(PlayerState.Idle);
    }

    private void FixedUpdate() {
        if (currentState == PlayerState.Idle || currentState == PlayerState.Running) {
            Transform target = null;
            for (int i = 0; i < pickUps.transform.childCount; i++) {
                if (target == null) {
                    var tempTarget = pickUps.transform.GetChild(i);
                    var script = tempTarget.GetComponent<PickUp>();
                    if (!script.isDie) {
                        target = tempTarget;
                    }
                }else {
                    break;
                }
            }

            if (target != null) {
                SetPlayerState(PlayerState.Running);
                transform.LookAt(target);
                float step = speed * Time.deltaTime;
                var pos = target.position;
                pos.y = 0;
                transform.position = Vector3.MoveTowards(transform.position, pos, step);
                transform.localRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Pick Up")) {
            var script = other.GetComponent<PickUp>();
            script.Die();
            count++;
            SetCountText();
            if (script.isRed) {
                SetPlayerState(PlayerState.Die);
            } else {
                SetPlayerState(PlayerState.Idle);
            }
        }
    }

    void SetCountText() {
        countText.text = "Count: " + count.ToString();
    }
}
