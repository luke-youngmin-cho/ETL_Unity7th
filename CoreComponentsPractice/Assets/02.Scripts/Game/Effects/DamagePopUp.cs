using TMPro;
using UnityEngine;

namespace DiceGame.Game.Effects
{
    public abstract class DamagePopUp : MonoBehaviour
    {
        private TMP_Text _damage;
        private float _fadeOutSpeed = 0.8f;
        private Vector3 _velocity = new Vector3(0.0f, 0.3f, 0.0f);
        private Color _color;


        public virtual void Show(float damage)
        {
            _damage.text = Mathf.CeilToInt(damage).ToString();
        }

        private void Awake()
        {
            _damage = GetComponent<TMP_Text>();
            _color = _damage.color;
        }

        private void Update()
        {
            transform.position += _velocity * Time.deltaTime;
            _color.a -= _fadeOutSpeed * Time.deltaTime;
            _damage.color = _color;

            if (_color.a <= 0.0f)
            {
                Destroy(gameObject);
            }
        }
    }
}