using UnityEngine;

public class BatAI : MonoBehaviour
{
	[SerializeField] private GameObject projectilePrefab;
	[SerializeField] private Transform shootPoint;
	[SerializeField] private float shootInterval = 2f;
	[SerializeField] private float projectileSpeed = 6f;
	[SerializeField] private float projectileLifetime = 5f;

	private float shootTimer;

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
