using UnityEngine;
using UnityEngine.Events;

namespace PLAYERTWO.PlatformerProject
{
	[AddComponentMenu("PLAYER TWO/Platformer Project/Level/Level Score")]
	public class LevelScore : Singleton<LevelScore>
	{
		/// <summary>
		/// Called when the collectibles list is updated with a new collectible item.
		/// </summary>
		public UnityEvent<Collectible> OnCollectibleAdded;

		/// <summary>
		/// Called when the amount of a specific collectible item is set.
		/// </summary>
		public UnityEvent<string> OnCollectibleSet;

		/// <summary>
		/// Called after the level data is fully loaded.
		/// </summary>
		public UnityEvent OnScoreLoaded;

		/// <summary>
		/// Returns the time since the current Level started.
		/// </summary>
		public float time { get; protected set; }

		/// <summary>
		/// Returns true if the time counter should be updating.
		/// </summary>
		public bool stopTime { get; set; } = true;

		protected CollectibleInstanceList m_collectibles;

		/// <summary>
		/// Returns the list of collected collectible items on the current Level.
		/// </summary>
		public CollectibleInstanceList collectibles
		{
			get
			{
				m_collectibles ??= m_level ? new(m_level.GetTrackedCollectibles()) : new();
				return m_collectibles;
			}
		}

		protected Level m_level => Level.instance;

		/// <summary>
		/// Resets the Level Score to its default values.
		/// </summary>
		public virtual void ResetScore()
		{
			time = 0;
		}

		/// <summary>
		/// Collect a given collectible item.
		/// </summary>
		/// <param name="collectible">The collectible item to collect.</param>
		public virtual void Collect(Collectible collectible)
		{
			if (!collectible)
				return;

			collectibles.AddOrStack(collectible);
			OnCollectibleAdded?.Invoke(collectible);
		}

		/// <summary>
		/// Sets the amount of collected coins in this level.
		/// </summary>
		/// <param name="coins">The amount of coins collected.</param>
		public virtual void SetCoins(int coins)
		{
			collectibles.SetAmount("coin", coins);
			OnCollectibleSet?.Invoke("coin");
		}

		/// <summary>
		/// Sends the current score to the Game Level to persist the data.
		/// </summary>
		public virtual void Consolidate()
		{
			if (!m_level)
				return;

			m_level.BeatLevel(collectibles, time);
		}

		protected virtual void Start()
		{
			OnScoreLoaded.Invoke();
		}

		protected virtual void Update()
		{
			if (!stopTime)
			{
				time += Time.deltaTime;
			}
		}
	}
}
