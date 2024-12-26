using UnityEngine;

namespace AfterimageSample
{
    public class AfterImage
    {
        RenderParams[] _params;
        Mesh[] _meshes;
        Matrix4x4[] _matrices;

        /// <summary>
        /// �`�悳�ꂽ��.
        /// </summary>
        public int FrameCount { get; private set; }

        /// <summary>
        /// �R���X�g���N�^.
        /// </summary>
        /// <param name="meshCount">�`�悷�郁�b�V���̐�.</param>
        public AfterImage(int meshCount)
        {
            _params = new RenderParams[meshCount];
            _meshes = new Mesh[meshCount];
            _matrices = new Matrix4x4[meshCount];
            Reset();
        }

        /// <summary>
        /// �`��O�������͌�Ɏ��s����.
        /// </summary>
        public void Reset()
        {
            FrameCount = 0;
        }

        /// <summary>
        /// ���b�V�����ƂɎg�p����}�e���A����p�ӂ��A���݂̃��b�V���̌`����L��������.
        /// </summary>
        /// <param name="material">�g�p����}�e���A��. </param>
        /// <param name="layer">�`�悷�郌�C���[.</param>
        /// <param name="renderers">�L��������SkinnedMeshRendere�̔z��.</param>
        public void Setup(Material material, int layer, SkinnedMeshRenderer[] renderers)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                // �}�e���A����null���n���ꂽ��I�u�W�F�N�g�̃}�e���A�������̂܂܎g��.
                if (material == null)
                {
                    material = renderers[i].material;
                }
                if (_params[i].material != material)
                {
                    _params[i] = new RenderParams(material);
                }
                // ���C���[��ݒ肷��.
                if (_params[i].layer != layer)
                {
                    _params[i].layer = layer;
                }
                // ���݂̃��b�V���̏�Ԃ��i�[����.
                if (_meshes[i] == null)
                {
                    _meshes[i] = new Mesh();
                }
                renderers[i].BakeMesh(_meshes[i]);
                _matrices[i] = renderers[i].transform.localToWorldMatrix;
            }
        }

        /// <summary>
        /// �L���������b�V����S�ĕ`�悷��.
        /// </summary>
        public void RenderMeshes()
        {
            for (int i = 0; i < _meshes.Length; i++)
            {
                Graphics.RenderMesh(_params[i], _meshes[i], 0, _matrices[i]);
            }
            FrameCount++;
        }
    }
}
