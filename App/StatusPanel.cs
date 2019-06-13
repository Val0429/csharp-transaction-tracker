namespace App
{
    public partial class AppClient
    {
        protected virtual void InitializeStatePanel()
        {
            StatePanel = ApplicationForms.StatusPanel();

            Form.Controls.Add(StatePanel);
        }
    }
}
