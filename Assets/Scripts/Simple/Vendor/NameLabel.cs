namespace Assets.Scripts.Simple.Vendor
{
    public class NameLabel : TextLabel
    {
        public override void OnGUI()
        {
            Text = transform.name;
            base.OnGUI();
        }
    }
}