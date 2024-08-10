using UnityEngine;

namespace RTS.Core
{
    public static class RectTransformExtensions
    {
        const float ZERO = 0.0f;
        const float HALF = 0.5f;
        const float FULL = 1.0f;
        public static void SetLeft(this RectTransform rt, float left)
        {
            rt.offsetMin = new Vector2(left, rt.offsetMin.y);
        }

        public static void SetRight(this RectTransform rt, float right)
        {
            rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
        }

        public static void SetTop(this RectTransform rt, float top)
        {
            rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
        }

        public static void SetBottom(this RectTransform rt, float bottom)
        {
            rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
        }

        public static Canvas GetCanvas(this RectTransform rectTransform)
        {
            if (rectTransform == null)
            {
                Debug.LogError("Object Not Assigned To Canvas");
                return null;
            }

            Canvas canvas = rectTransform.GetComponent<Canvas>();
            if (canvas != null)
                return canvas;
            return GetCanvas(rectTransform.transform.parent.GetComponent<RectTransform>());
        }

        public static Vector2 GetSize(this RectTransform rectTransform)
        {
            Rect size = RectTransformUtility.PixelAdjustRect(rectTransform, rectTransform.GetCanvas());
            return new Vector2(size.width, size.height);
        }
        public static Vector2 GetHorizontalAnchor(this RectTransform rectTransform) => GetAnchorValue(rectTransform.GetHorizontalAnchorType());
        public static Vector2 GetVerticalAnchor(this RectTransform rectTransform) => GetAnchorValue(rectTransform.GetVerticalAnchorType());

        public static AnchorType GetHorizontalAnchorType(this RectTransform rectTransform) => GetAnchorType(rectTransform.anchorMin.x, rectTransform.anchorMax.x);
        public static AnchorType GetVerticalAnchorType(this RectTransform rectTransform) => GetAnchorType(rectTransform.anchorMin.y, rectTransform.anchorMax.y);

        private static AnchorType GetAnchorType(float min, float max)
        {
            bool minZero = min == ZERO;
            bool minHalf = min == HALF;
            bool minFull = min == FULL;

            bool maxZero = max == ZERO;
            bool maxHalf = max == HALF;
            bool maxFull = max == FULL;

            bool Center = minHalf && maxHalf; //0.5 0.5 | Width
            bool CenterFromLeft = minZero && maxZero; // 0 0 | Width
            bool CenterFromRight = minFull && maxFull; // 1 1 | Width
            bool Left = minZero && maxHalf; // 0 .5  | halfSize - width
            bool Right = minHalf && maxZero; // .5 1 | halfSize - width
            bool Fill = minZero && maxZero; // 0 1 | size - width

            if (Center)
                return AnchorType.Center;
            if (CenterFromLeft)
                return AnchorType.CenterFromLeft;
            if (CenterFromRight)
                return AnchorType.CenterFromRight;
            if (Left)
                return AnchorType.Left;
            if (Right)
                return AnchorType.Right;
            return AnchorType.Fill;
        }

        public static void SetHorizontalAnchor(this RectTransform rectTransform, AnchorType anchorType)
        {
            Vector2 anchor = GetAnchorValue(anchorType);
            rectTransform.SetAnchor(
                new Vector2(anchor.x, rectTransform.anchorMin.y),
                new Vector2(anchor.y, rectTransform.anchorMax.y));
        }

        public static void Move(this RectTransform rect, Vector2 normalized)
        {
            float xPos = Mathf.Clamp01(normalized.x);
            float yPos = Mathf.Clamp01(normalized.y);

            Vector2 parentSize = rect.GetParent().GetSize();
            Vector2 size = rect.GetSize();
            Vector2 hor = rect.GetHorizontalAnchor();
            Vector2 ver = rect.GetVerticalAnchor();

            Vector2 newPosition = new Vector2(
                            (float)Formula(xPos, size.x, parentSize.x, hor.x),
                            (float)Formula(yPos, size.y, parentSize.y, ver.y)
                        );
            Debug.Log($"{newPosition}");
            rect.localPosition = newPosition;

            static float Formula(float pos, float currentSize, float parentSize, float pivot)
            {
                float value = (parentSize * pos) - (pivot * currentSize);
                return value;
            }
        }

        public static void SetVerticalAnchor(this RectTransform rectTransform, AnchorType anchorType)
        {
            Vector2 anchor = GetAnchorValue(anchorType);
            rectTransform.SetAnchor(
                new Vector2(rectTransform.anchorMin.x, anchor.x),
                new Vector2(rectTransform.anchorMax.x, anchor.y));
        }

        private static Vector2 GetAnchorValue(AnchorType anchorType)
        {
            Vector2 anchorVal = new();
            switch (anchorType)
            {
                case AnchorType.Center:
                    anchorVal.x = HALF;
                    anchorVal.y = HALF;
                    break;
                case AnchorType.CenterFromLeft:
                    anchorVal.x = ZERO;
                    anchorVal.y = ZERO;
                    break;
                case AnchorType.CenterFromRight:
                    anchorVal.x = FULL;
                    anchorVal.y = FULL;
                    break;
                case AnchorType.Left:
                    anchorVal.x = ZERO;
                    anchorVal.y = HALF;
                    break;
                case AnchorType.Right:
                    anchorVal.x = HALF;
                    anchorVal.y = FULL;
                    break;
                case AnchorType.Fill:
                    anchorVal.x = ZERO;
                    anchorVal.y = FULL;
                    break;
            }
            return anchorVal;
        }
        public static void SetAnchors(this RectTransform rectTransform, AnchorType horizontal, AnchorType vertical)
        {
            rectTransform.SetHorizontalAnchor(horizontal);
            rectTransform.SetVerticalAnchor(vertical);
        }
        public static void SetAnchor(this RectTransform rectTransform, Vector2 min, Vector2 max)
        {
            rectTransform.anchorMin = min;
            rectTransform.anchorMax = max;
        }
        public static void SetPivot(this RectTransform rectTransform, Vector2 pivot) => rectTransform.pivot = pivot;
        public static Vector2 GetPivot(this RectTransform rectTransform) => rectTransform.pivot;

        public static RectTransform GetParent(this RectTransform rectTransform)
        {
            if (rectTransform.parent == null)
                return null;
            return rectTransform.parent.gameObject.GetOrAddComponent<RectTransform>();
        }

        public static void SetSize(this RectTransform rectTransform, Vector2 size)
        {
            Vector2 currentSize = rectTransform.GetSize();
            Vector2 parentSize = rectTransform.GetParent().GetSize();
            AnchorType horizontal = rectTransform.GetHorizontalAnchorType();
            AnchorType vertical = rectTransform.GetVerticalAnchorType();

            Vector2 WidthMinMax = GetValues(horizontal, currentSize.x, size.x, parentSize.x);
            Vector2 HeightMinMax = GetValues(vertical, currentSize.y, size.y, parentSize.y);

            Vector3 position = new Vector3(WidthMinMax.x, HeightMinMax.x, rectTransform.localPosition.z);

            Vector2 sizeDelta = new Vector2(WidthMinMax.y, HeightMinMax.y);

            rectTransform.localPosition = position;
            rectTransform.sizeDelta = sizeDelta;
        }

        private static Vector2 GetValues(AnchorType anchorType, float currentSize, float desiredSize, float parentSize)
        {
            float min;
            float max;
            switch (anchorType)
            {
                default:
                case AnchorType.Center:
                case AnchorType.CenterFromLeft:
                case AnchorType.CenterFromRight:
                    // Width
                    min = 0;
                    max = desiredSize;
                    break;
                case AnchorType.Left:
                case AnchorType.Right:
                    // HalfSize - width
                    min = parentSize / 2f;
                    max = (parentSize / 2f) - desiredSize;
                    break;
                case AnchorType.Fill:
                    // Margins
                    float Margin = -(parentSize - desiredSize);
                    min = 0;
                    max = Margin;
                    break;
            }

            return new Vector2(min, max);
        }

        public static void SetRectByHeight(this RectTransform rectTransform, float height)
        {
            Vector2 rect = rectTransform.GetSize();
            Vector2 proportionNorm = new(rect.x / rect.y, rect.y / rect.x);
            Vector2 newSizeDelta = new(height * proportionNorm.x, height);
            rectTransform.sizeDelta = newSizeDelta;
        }

        public enum AnchorType
        {
            Center, // 0.5, 0.5 | SetWidth
            CenterFromLeft, // 0, 0 | SetWidth
            CenterFromRight, // 1, 1 | SetWidth
            Left, // 0, 0.5
            Right, // 0.5 1
            Fill // 0 1
        }
    }
}