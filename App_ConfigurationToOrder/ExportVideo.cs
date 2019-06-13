using ExportVideoForm.CTO;

namespace App_ConfigurationToOrder
{
    public class ExportVideoForm : App.ExportVideoForm
    {
        public ExportVideoForm()
        {
            _exportVideo = new ExportVideo();
        }
    }
}
