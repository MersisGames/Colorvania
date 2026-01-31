using UnityEngine;
using UnityEngine.Events;

namespace PLAYERTWO.PlatformerProject
{
	[RequireComponent(typeof(BoxCollider))]
	[AddComponentMenu("PLAYER TWO/Platformer Project/Misc/Item Box")]
	public class ItemBox : MonoBehaviour, IEntityContact
	{
		public Collectible[] collectibles;
		public MeshRenderer itemBoxRenderer;
		public Material emptyItemBoxMaterial;

		[Space(15)]
		public UnityEvent onCollect;
		public UnityEvent onDisable;

		protected int m_index;
		protected bool m_enabled = true;
		protected Vector3 m_initialScale;

		protected BoxCollider m_collider;

		protected virtual void InitializeCollectibles()
		{
			foreach (var collectible in collectibles)
			{
				collectible.collectOnContact = false;
				collectible.gameObject.SetActive(false);
			}
		}

		public virtual void Collect(Player player)
		{
			if (m_enabled)
			{
				if (m_index < collectibles.Length)
				{
					collectibles[m_index].gameObject.SetActive(true);

					if (collectibles[m_index].TryGetComponent(out CollectibleHidden _))
						collectibles[m_index].Collect(player);
					else
						collectibles[m_index].collectOnContact = true;

					m_index = Mathf.Clamp(m_index + 1, 0, collectibles.Length);
					onCollect?.Invoke();
				}

				if (m_index == collectibles.Length)
				{
					Disable();
				}
			}
		}

		public virtual void Disable()
		{
			if (m_enabled)
			{
				m_enabled = false;
				itemBoxRenderer.sharedMaterial = emptyItemBoxMaterial;
				onDisable?.Invoke();
			}
		}

		protected virtual void Start()
		{
			m_collider = GetComponent<BoxCollider>();
			m_initialScale = transform.localScale;
			InitializeCollectibles();
		}

		public void OnEntityContact(Entity entity)
		{
			if (entity is Player player)
			{
				var offset = entity.height * 0.5f - entity.radius;
				var head =
					entity.position + entity.transform.up * (offset - Physics.defaultContactOffset);

				if (entity.verticalVelocity.y > 0 && BoundsHelper.IsAbovePoint(m_collider, head))
				{
					Collect(player);
					entity.verticalVelocity = Vector3.zero;
				}
			}
		}
	}
}
