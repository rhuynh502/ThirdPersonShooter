using System.Numerics;

using Vector3 = UnityEngine.Vector3;

namespace ThirdPersonShooter.Entities
{
	public interface IEntity
	{
		public ref Stats Stats { get; }
		public Vector3 Position { get; }
	}
}