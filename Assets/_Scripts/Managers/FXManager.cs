using UnityEngine;

public class FXManager : MonoBehaviour
{
    [SerializeField] ParticleSystem breakableEffect;
    [SerializeField] ParticleSystem destroyEffect;
    [SerializeField] ParticleSystem holeEffect;

    public static FXManager Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }

    public void ShowBreakableEffect(Vector3 position) {
        transform.position = position;
        breakableEffect.Play();
    }

    public void ShowDestroyEffect(Vector3 position) {
        transform.position = position;
        destroyEffect.Play();
    }
    public void ShowHoleEffect(Vector3 position) {
        transform.position = position;
        holeEffect.Play();
    }
}
