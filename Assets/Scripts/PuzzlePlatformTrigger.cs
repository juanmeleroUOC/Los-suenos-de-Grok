using UnityEngine;

public class PuzzlePlatformTrigger : MonoBehaviour
{
    public PuzzleManager puzzleManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BulletBall"))
        {
            puzzleManager.RegisterPlatformHit(transform);
        }
    }
}
