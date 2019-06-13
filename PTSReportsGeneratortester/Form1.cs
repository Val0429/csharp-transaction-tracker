using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PTSReportsGeneratortester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var viewer = new PTSReportsGenerator.PTSReportViewer();

            var a = viewer.CreateReport(
                "Exception"
                , "<Result endUTC=\"1518451199000\" startUTC=\"1518364800000\" status=\"200\" timeZone=\"28800000\"><Store id=\"1\"><Date date=\"2018-02-12\" dateUTC=\"1518393600000\"><Exception name=\"DISCOUNT\">2</Exception><Exception name=\"ORDER CANCEL\">2</Exception></Date></Store></Result>"
                ,""
                ,"PDF"
                ,"pre"
                ,"siffix"
                );
        }
    }
}
