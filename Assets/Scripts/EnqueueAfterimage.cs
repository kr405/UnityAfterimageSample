using UnityEngine;

namespace AfterimageSample
{
    public class EnqueueAfterimage : MonoBehaviour
    {
        [SerializeField] AfterimageRenderer _afterimageRenderer;
        [SerializeField] int _intervalFrames;

        Vector3 _oldPosition;
        int _count = 0;

        void Awake()
        {
            _oldPosition = _afterimageRenderer.transform.position;
        }

        void Update()
        {
            if (_afterimageRenderer.transform.position == _oldPosition)
            {
                _count = 0;
                return;
            }
            if (_count >= _intervalFrames)
            {
                _count = 0;
                _afterimageRenderer.Enqueue();
            }
            _oldPosition = _afterimageRenderer.transform.position;
            _count++;
        }
    }
}

