using System.Collections;
using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
	[AddComponentMenu("PLAYER TWO/Platformer Project/Enemy/Enemy Respawn")]
	public class EnemyRespawn : MonoBehaviour
	{
		[Header("Respawn Settings")]
		[Tooltip("The delay in seconds before the enemy respawns after dying.")]
		public float respawnDelay = 5f;

		protected Enemy m_enemy;

		protected WaitForSeconds m_respawnWait;
		protected Coroutine m_respawnRoutine;

		protected virtual void Start()
		{
			InitializeEnemy();
			InitializeCallbacks();

			m_respawnWait = new WaitForSeconds(respawnDelay);
		}

		protected virtual void InitializeEnemy()
		{
			m_enemy = GetComponent<Enemy>();
		}

		protected virtual void InitializeCallbacks()
		{
			m_enemy.enemyEvents.OnDie.AddListener(OnDie);
		}

		protected virtual void OnDie()
		{
			if (m_respawnRoutine != null)
				StopCoroutine(m_respawnRoutine);

			m_respawnRoutine = StartCoroutine(RespawnRoutine());
		}

		protected IEnumerator RespawnRoutine()
		{
			yield return m_respawnWait;

			m_enemy.Respawn();
			m_respawnRoutine = null;
		}
	}
}
