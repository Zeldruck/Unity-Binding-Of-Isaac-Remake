using UnityEngine;

namespace Interfaces
{
    public interface IDamageable
    {
        void TakeDamage(int _damages);
        void TakeDamage(int _damages, Vector2 _point, float _force);
    }
}
