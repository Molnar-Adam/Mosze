using UnityEngine;
using TMPro;

/// Egy visszaszámláló rendszert vezérlő osztály, amely időtúllépéskor a kezdőpontra juttatja a játékost.
public class Timer : MonoBehaviour
{
        /// Az időt mutató szöveges UI elem.
        [SerializeField] private TextMeshProUGUI TimerText;
        
        /// A visszaszámlálás időtartama másodpercben.
        [SerializeField] private float countdownSeconds;
        
        /// Az a transzform, ahová az idő lejárta után a játékos kerül.
        [SerializeField] private Transform respawnLocation;
        
        /// A játékos címkéje.
        [SerializeField] private string playerTag = "Player";

        /// A hátralévő idő mennyisége.
        private float remainingTime;
        
        /// Jelzi, hogy a visszaszámlálás jelenleg aktív-e.
        private bool timerRunning;
        
        /// A követett játékos transzformja, akit visszateleportál.
        private Transform trackedPlayer;

        /// Visszaadja, hogy az időzítő fut-e.
        public bool IsTimerRunning => timerRunning;

        /// Induláskor feltölti az időt és alapértelmezetten elrejti a UI-t.
        private void Start()
        {
                remainingTime = Mathf.Max(0f, countdownSeconds);
                UpdateTimerText();
                SetTimerVisible(false);
        }

        /// Képkockánként frissíti a hátralévő időt, és ha lejár, triggereli az újraéledést.
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

        /// Amikor a játékos belép a zónába, elindítja és láthatóvá teszi a visszaszámlálót.
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
        /// Leállítja az időzítőt, teleportálja a játékost, és elrejti a kijelzőt. Kézzel is meghívható külső eventeknél.
                UpdateTimerText();
        }

        public void TriggerTimerEnd(Transform playerTransform = null)
        {
                if (playerTransform != null)
                {
                        trackedPlayer = playerTransform;
                }

                remainingTime = 0f;
        /// Ténylegesen áthelyezi a játékost az újraéledési pontra és kinullázza a sebességét.
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
        /// Frissíti a UI komponens szövegét a hátralévő, felkerekített másodpercekkel.
                {
                        rb.linearVelocity = Vector2.zero;
                        rb.angularVelocity = 0f;
                }
        }

        private void UpdateTimerText()
        /// Megjeleníti vagy elrejti a visszaszámláló szöveget a képernyőn.
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
