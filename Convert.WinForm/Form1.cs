using Convert.WinForm.Wcf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Convert.WinForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            double amount;
            txtUserNameConverted.Text = "";
            txtAmountConverted.Text = "";
            var addr = txtAddr.Text;

            if (!double.TryParse(txtAmount.Text, out amount))
            {
                MessageBox.Show("Please input valid amount.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                var httpBinding = new BasicHttpBinding();
                httpBinding.Security.Mode = BasicHttpSecurityMode.None;
                var endpoint = new EndpointAddress("http://localhost/convert.wcf/ConvertService.svc");
                ConvertServiceClient svc = new ConvertServiceClient(httpBinding, endpoint);
                var model = new ChequeDataModel()
                {
                    UserName = txtUserName.Text,
                    Amount = amount
                };

                var result = svc.Translate(model);

                txtUserNameConverted.Text = result.UserName;
                txtAmountConverted.Text = result.AmountString;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
    }
}
