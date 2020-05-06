using UnityEngine;

namespace Controllers
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(SphereCollider))]
    public class CharController : MonoBehaviour
    {
        private Rigidbody _charBody;
        [HideInInspector] public Vector3 charCenter; // Центр перса
        private CapsuleCollider _charCollider;
        public float charFallForce = 50f; // Скорость падения
        public float charHeight = 2.5f; // Высота перса
        [HideInInspector] public bool charIsGrounded;
        public float charJumpForce = 250f; // Сила прыжка
        public float charMass = 100f; // Масса игрока
        public float charDrag = 0f; //Сопротивление игрока
        public float charRadius = 0.5f; // Радиус
        public float charRollSpeed = 25000f; // Сила кувырка
        public float charSpeed = 3f; // Скорость перса

        private SphereCollider _foot;

        public Rigidbody Rigidbody { get; set; }
        public CapsuleCollider CapsuleCollider { get; set; }


        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            _charBody = GetComponent<Rigidbody>();
            Rigidbody = _charBody;
            _charBody.constraints =
                RigidbodyConstraints.FreezeRotation; // Заморозка вращения что бы не падал по сторонам
            _charBody.mass = charMass;

            _charCollider = GetComponent<CapsuleCollider>(); // Ссылка на коллайдер
            CapsuleCollider = _charCollider;
            charCenter = new Vector3(-0.15f, -0.65f, 0f); // Задаем центр (найден имперически)
            // Параметры коллайдера
            _charCollider.center = charCenter;
            _charCollider.height = charHeight;
            _charCollider.radius = charRadius;
            _charBody.drag = charDrag;
            // Коллайдер для проверки земли
            _foot = GetComponent<SphereCollider>();
            _foot.center = new Vector3(-0.15f, -_charCollider.height * 0.75f, 0f);
            _foot.radius = _charCollider.radius;
            _foot.isTrigger = true;
        }

        /// <summary>
        ///     Метод передвижения
        /// </summary>
        /// <param name="movement">Вектор движения</param>
        public void Move(Vector3 movement)
        {
            Rigidbody.MovePosition(transform.position + movement);
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.gameObject.CompareTag("Ground")) return;
            charIsGrounded = true;
          //  Debug.Log(charIsGrounded);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("Ground")) return;
            charIsGrounded = false;
          //  Debug.Log(charIsGrounded);
        }
    }
}