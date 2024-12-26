using System.Collections.Generic;
using UnityEngine;

namespace AfterimageSample
{
    public class AfterimageRenderer : MonoBehaviour
    {
        [SerializeField] Material _material;
        [SerializeField] int _duration = 150;
        [SerializeField] int _layer = 6;

        SkinnedMeshRenderer[] _renderers;
        Stack<AfterImage> _pool = new Stack<AfterImage>();
        Queue<AfterImage> _renderQueue = new Queue<AfterImage>();

        void Awake()
        {
            _renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        }

        void Update()
        {
            Render();
        }

        /// <summary>
        /// �L���[�ɓ����Ă���AfterImage�̃��b�V����`�悷��.
        /// </summary>
        public void Render()
        {
            for (int i = 0; i < _renderQueue.Count; i++)
            {
                var afterimage = _renderQueue.Dequeue();
                afterimage.RenderMeshes();

                // �`��񐔂����x�𒴂���܂ŌJ��Ԃ��L���[�ɓ����.
                // ���x�𒴂�����v�[���ɕԂ�.
                if (afterimage.FrameCount < _duration)
                {
                    _renderQueue.Enqueue(afterimage);
                }
                else
                {
                    afterimage.Reset();
                    _pool.Push(afterimage);
                }
            }
        }

        /// <summary>
        /// �`��҂��̃L���[��Afterimage�I�u�W�F�N�g������.
        /// </summary>
        public void Enqueue()
        {
            AfterImage afterimage;
            if (_pool.Count > 0)
            {
                afterimage = _pool.Pop();
            }
            else
            {
                afterimage = new AfterImage(_renderers.Length);
            }
            afterimage.Setup(_material, _layer, _renderers);
            _renderQueue.Enqueue(afterimage);
        }        
    }
}

