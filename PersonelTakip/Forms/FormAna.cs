using PersonelTakip.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PersonelTakip
{
    public partial class FormAna : Form
    {
        CalisanDAL calisanDAL = new CalisanDAL();
        public FormAna()
        {
            InitializeComponent();
        }

        private void FormAna_Load(object sender, EventArgs e)
        {
            dgvCalisanlar.DataSource = calisanDAL.GetAll("WHERE Departman='Muhasebe'");
        }

        void ClearTools()
        {
            foreach (Control control in pnlPersonelBilgi.Controls)
            {
                if (control.GetType()!=typeof(Label)) //Sıradaki control label değilse 
                {
                    control.Text = string.Empty;
                }   
            }
            //Çalışma sorusu:
            // buraya nasıl kodlar yazmalıyız ki datetime picker nesneleri içi boşalsın
        }
        private void btnTemizle_Click(object sender, EventArgs e)
        {
            /* txtAd.ResetText();
             txtSoyad.ResetText();
             txtTcNo.ResetText();
             txtPersonelNo.ResetText();*/
            ClearTools();
        }
    }
}
