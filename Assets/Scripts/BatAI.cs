using UnityEngine;

/// A denevér ellenség egyszerű mesterséges intelligenciáját megvalósító osztály.
/// Rendszeres időközönként lövedékeket lő lefelé.
public class BatAI : MonoBehaviour
{
	/// A kilövendő lövedék prefabja.
	[SerializeField] private GameObject projectilePrefab;

	/// Az a pont, ahonnan a lövedék kiindul.
	[SerializeField] private Transform shootPoint;

	/// Két lövés között eltelt idő másodpercben.
	[SerializeField] private float shootInterval = 2f;

	/// A kilőtt lövedék mozgási sebessége.
	[SerializeField] private float projectileSpeed = 6f;

	/// A lövedék élettartama másodpercben, mielőtt automatikusan megsemmisülne.
	[SerializeField] private float projectileLifetime = 5f;

	/// Belső időzítő a lövések követéséhez.
	private float shootTimer;

	/// Képkockánként frissíti az időzítőt, és ha letelt az idő, elsüt egy lövedéket.
	private void Update()
	{
		if (projectilePrefab == null)
		{
			return;
		}

		shootTimer += Time.deltaTime;
		if (shootTimer < shootInterval)
		{
			return;
		}

		shootTimer = 0f;
		Shoot();
	}

	/// Létrehoz egy lövedéket a megadott végpontnál, beállítja a sebességét, 
	private void Shoot()
	{
		Vector3 spawnPosition = shootPoint != null ? shootPoint.position : transform.position;
		GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
		Destroy(projectile, projectileLifetime);

		if (projectile.GetComponent<BatProjectile>() == null)
		{
			projectile.AddComponent<BatProjectile>();
		}

		Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
		if (rb != null)
		{
			rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
			rb.linearVelocity = Vector2.down * projectileSpeed;
		}
	}
}
