using UnityEngine;

namespace Assets.Scripts.Simple.Vendor
{
    public class TextLabel : MonoBehaviour
    {
        float boxW = 150f;
        float boxH = 25f;

        public string Text;
        public Vector3 Offset = Vector3.zero;

        public virtual void OnGUI()
        {
            Vector2 TextLocation = Camera.main.WorldToScreenPoint(transform.position + Offset);

            TextLocation.y = Screen.height - TextLocation.y;

            TextLocation.x -= boxW * 0.5f;
            TextLocation.y -= boxH * 0.5f;

            GUI.Box(new Rect(TextLocation.x, TextLocation.y, boxW, boxH), Text);
        }

    }
}