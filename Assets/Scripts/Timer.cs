using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
        [SerializeField] private TextMeshProUGUI TimerText;
        [SerializeField] private float countdownSeconds;
        [SerializeField] private Transform respawnLocation;
        [SerializeField] private string playerTag = "Player";

        private float remainingTime;
        private bool timerRunning;
        private Transform trackedPlayer;

        public bool IsTimerRunning => timerRunning;

        private void Start()
        {
                remainingTime = Mathf.Max(0f, countdownSeconds);
                UpdateTimerText();
                SetTimerVisible(false);
        }

        private void Update()
        {
                if (!timerRunning)
                {
                        return;
                }

                remainingTime -= Time.deltaTime;
                if (remainingTime <= 0f)
                {
                        TriggerTimerEnd();
                        return;
                }

                UpdateTimerText();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
                if (timerRunning || !collision.CompareTag(playerTag))
                {
                        return;
                }

                trackedPlayer = collision.transform;
                remainingTime = Mathf.Max(0f, countdownSeconds);
                timerRunning = true;
                SetTimerVisible(true);
                UpdateTimerText();
        }

        public void TriggerTimerEnd(Transform playerTransform = null)
        {
                if (playerTransform != null)
                {
                        trackedPlayer = playerTransform;
                }

                remainingTime = 0f;
                UpdateTimerText();
                TeleportPlayerToRespawn();
                timerRunning = false;
                SetTimerVisible(false);
        }

        private void TeleportPlayerToRespawn()
        {

                trackedPlayer.position = respawnLocation.position;

                Rigidbody2D rb = trackedPlayer.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                        rb.linearVelocity = Vector2.zero;
                        rb.angularVelocity = 0f;
                }
        }

        private void UpdateTimerText()
        {
                if (TimerText == null)
                {
                        return;
                }

                TimerText.text = Mathf.CeilToInt(remainingTime).ToString();
        }

        private void SetTimerVisible(bool isVisible)
        {
                if (TimerText == null)
                {
                        return;
                }

                TimerText.gameObject.SetActive(isVisible);
        }

}
