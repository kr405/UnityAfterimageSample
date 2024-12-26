using UnityEngine;

namespace AfterimageSample
{
    public class AfterImage
    {
        RenderParams[] _params;
        Mesh[] _meshes;
        Matrix4x4[] _matrices;

        /// <summary>
        /// 描画された回数.
        /// </summary>
        public int FrameCount { get; private set; }

        /// <summary>
        /// コンストラクタ.
        /// </summary>
        /// <param name="meshCount">描画するメッシュの数.</param>
        public AfterImage(int meshCount)
        {
            _params = new RenderParams[meshCount];
            _meshes = new Mesh[meshCount];
            _matrices = new Matrix4x4[meshCount];
            Reset();
        }

        /// <summary>
        /// 描画前もしくは後に実行する.
        /// </summary>
        public void Reset()
        {
            FrameCount = 0;
        }

        /// <summary>
        /// メッシュごとに使用するマテリアルを用意し、現在のメッシュの形状を記憶させる.
        /// </summary>
        /// <param name="material">使用するマテリアル. </param>
        /// <param name="layer">描画するレイヤー.</param>
        /// <param name="renderers">記憶させるSkinnedMeshRendereの配列.</param>
        public void Setup(Material material, int layer, SkinnedMeshRenderer[] renderers)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                // マテリアルにnullが渡されたらオブジェクトのマテリアルをそのまま使う.
                if (material == null)
                {
                    material = renderers[i].material;
                }
                if (_params[i].material != material)
                {
                    _params[i] = new RenderParams(material);
                }
                // レイヤーを設定する.
                if (_params[i].layer != layer)
                {
                    _params[i].layer = layer;
                }
                // 現在のメッシュの状態を格納する.
                if (_meshes[i] == null)
                {
                    _meshes[i] = new Mesh();
                }
                renderers[i].BakeMesh(_meshes[i]);
                _matrices[i] = renderers[i].transform.localToWorldMatrix;
            }
        }

        /// <summary>
        /// 記憶したメッシュを全て描画する.
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
