using UnityEngine;
 
namespace Models
{
	public class DeathScreen : MonoBehaviour
	{
		[SerializeField] GameObject deathscreenImage;
		[SerializeField] GameObject PlayerPrefab;

		HealthModel health;

		void Start()
		{
			deathscreenImage.SetActive(false);
			health = GetComponent<HealthModel>();
		}

		// Update is called once per frame
		void Update()
		{
			if (health.health <= 0)
			{
				 
				deathscreenImage.SetActive(true);
				Cursor.visible = true;
			}
			else
			{
				deathscreenImage.SetActive(false);
			}
		}

		/// <summary>
		/// Кнопка Респауна 
		/// </summary>
		public void RespawnButton()
		{
			var CheckPoint = GameObject.FindGameObjectWithTag("Player").GetComponent<CheckPoints>();
			if(health.health <= 0)
			{
                CheckPoint.CanRespawn = true;
				health.health = 100;
			}
			deathscreenImage.SetActive(false);		 
		}
	}
}
