using UnityEngine;
using UnityEngine.UI;

namespace LabDiner.Shared
{
    [ExecuteAlways]
    [RequireComponent(typeof(Image))]
    public class ProceduralImage : MonoBehaviour
    {
        [Header("Procedural Settings")]
        [Range(0, 100)] public float radius = 20f;
        [Range(0, 50)] public float borderWidth = 5f;
        public Color backgroundColor = Color.white;
        public Color borderColor = Color.black;

        private Image _targetImage;
        private float _lastRadius, _lastBorderWidth;
        private Color _lastBgColor, _lastBdColor;

        private void OnEnable() => Refresh();

        private void Update()
        {
            if (radius != _lastRadius || borderWidth != _lastBorderWidth ||
                backgroundColor != _lastBgColor || borderColor != _lastBdColor)
            {
                Refresh();
            }
        }

        public void Refresh()
        {
            _targetImage = GetComponent<Image>();
            _lastRadius = radius;
            _lastBorderWidth = borderWidth;
            _lastBgColor = backgroundColor;
            _lastBdColor = borderColor;
            GenerateProceduralSprite();
        }

        private void GenerateProceduralSprite()
        {
            if (_targetImage == null) return;

            // 1. Tính toán Size chuẩn: Phải có đủ chỗ cho Radius, Border và 2px đệm để không bị mất viền
            // Thêm +4 pixel "đường thẳng" ở giữa để 9-slice kéo giãn không bị lỗi toán học
            int sideSize = Mathf.CeilToInt(radius + borderWidth) + 4;
            int texSize = sideSize * 2; 

            Texture2D tex = new Texture2D(texSize, texSize, TextureFormat.RGBA32, false);
            tex.filterMode = FilterMode.Bilinear;
            tex.wrapMode = TextureWrapMode.Clamp;

            Vector2 center = new Vector2(texSize / 2f, texSize / 2f);
            // Vùng an toàn để vẽ: trừ đi 1px sát rìa để tránh bị dính viền texture (gây ra shadow)
            Vector2 halfSize = new Vector2(texSize / 2f - 1.5f, texSize / 2f - 1.5f);

            Color32[] colors = new Color32[texSize * texSize];

            for (int y = 0; y < texSize; y++)
            {
                for (int x = 0; x < texSize; x++)
                {
                    Vector2 p = new Vector2(x + 0.5f, y + 0.5f) - center;
                    float dist = RoundedBoxSDF(p, halfSize, radius);

                    // Khử răng cưa cực mịn bằng SmoothStep thay vì Clamp
                    float alpha = 1.0f - Mathf.SmoothStep(0, 1.0f, dist + 0.5f);
                    float borderMask = 1.0f - Mathf.SmoothStep(0, 1.0f, dist + borderWidth + 0.5f);

                    Color finalCol = Color.Lerp(borderColor, backgroundColor, borderMask);
                    finalCol.a *= alpha;
                    colors[y * texSize + x] = finalCol;
                }
            }

            tex.SetPixels32(colors);
            tex.Apply();

            // 2. Thiết lập 9-Slice: Điểm mấu chốt để viền đều
            // Border của Sprite phải nằm đúng vào phần đường thẳng sau khi kết thúc đoạn cong
            float slice = sideSize - 2; 
            Vector4 sliceBorder = new Vector4(slice, slice, slice, slice);

            if (_targetImage.sprite != null && _targetImage.sprite.name == "ProceduralUISprite")
            {
                DestroyImmediate(_targetImage.sprite.texture);
                DestroyImmediate(_targetImage.sprite);
            }

            _targetImage.sprite = Sprite.Create(tex, new Rect(0, 0, texSize, texSize), new Vector2(0.5f, 0.5f), 100, 0, SpriteMeshType.FullRect, sliceBorder);
            _targetImage.sprite.name = "ProceduralUISprite";
            _targetImage.type = Image.Type.Sliced;
        }

        private float RoundedBoxSDF(Vector2 p, Vector2 b, float r)
        {
            Vector2 q = new Vector2(Mathf.Abs(p.x) - b.x + r, Mathf.Abs(p.y) - b.y + r);
            return Vector2.Max(q, Vector2.zero).magnitude + Mathf.Min(Mathf.Max(q.x, q.y), 0.0f) - r;
        }
    }
}