using PersonelTakip.DataAccess;
using PersonelTakip.Entities;
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
        Calisan calisan= null ;
        int secilenID = 0;
        public FormAna()
        {
            InitializeComponent();
        }

        void Refresh()
        {
            dgvCalisanlar.DataSource = calisanDAL.GetAll();
            //dgvCalisanlar.DataSource = calisanDAL.GetAll("WHERE Departman='Muhasebe'");
        }
        private void FormAna_Load(object sender, EventArgs e)
        {
            Refresh();
            Transfer();
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

            dtpDogumTarihi.Format = DateTimePickerFormat.Custom;
            dtpDogumTarihi.CustomFormat = " ";

            dtpIseBslmaTarihi.Format = DateTimePickerFormat.Custom;
            dtpIseBslmaTarihi.CustomFormat = " ";

            txtAd.Focus();
            Refresh();
            Cancel();
        }
        private void btnTemizle_Click(object sender, EventArgs e)
        {
            /* txtAd.ResetText();
             txtSoyad.ResetText();
             txtTcNo.ResetText();
             txtPersonelNo.ResetText();*/
            ClearTools();

        }

       

        void Transfer()
        {
            /*int personel = Convert.ToInt32(dgvCalisanlar.SelectedRows[0].Cells[0].Value.ToString());

            foreach (DataGridView satir in dgvCalisanlar.Rows)
            {
                DataGridViewCell hucre = satir.Cells[0];
                if (Convert.ToInt32(hucre.Value.ToString())==personel)
                {
                    satir.Selected = true;
                    break;
                }
            }*/

            try
            {
                secilenID = Convert.ToInt32(dgvCalisanlar.SelectedRows[0].Cells[0].Value);
                txtAd.Text = dgvCalisanlar.SelectedRows[0].Cells[1].Value.ToString();
                txtSoyad.Text = dgvCalisanlar.SelectedRows[0].Cells[2].Value.ToString();
                txtTcNo.Text = dgvCalisanlar.SelectedRows[0].Cells[3].Value.ToString();
                txtPersonelNo.Text = dgvCalisanlar.SelectedRows[0].Cells[4].Value.ToString();
               
                dtpDogumTarihi.Value = Convert.ToDateTime(dgvCalisanlar.SelectedRows[0].Cells[5].Value.ToString());
                
                dtpIseBslmaTarihi.Value = Convert.ToDateTime(dgvCalisanlar.SelectedRows[0].Cells[6].Value.ToString());
                
                cmbDepartman.Text = dgvCalisanlar.SelectedRows[0].Cells[7].Value.ToString();
                cmbUnvan.Text = dgvCalisanlar.SelectedRows[0].Cells[8].Value.ToString();
                cmbDurumu.Text = dgvCalisanlar.SelectedRows[0].Cells[9].Value.ToString();

            }
            catch (Exception)
            {

               // throw;
            }
        }

        private void dgvCalisanlar_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            Transfer();
        }

        bool BlankControl()
        {
            bool result = true;
            foreach (Control control in pnlPersonelBilgi.Controls)
            {
                if (control.Text== string.Empty || control.Text==" ")
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (!BlankControl())
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz!");
            }
            else
            {
                calisan = new Calisan
                {
                    Ad = txtAd.Text,
                    Soyad = txtSoyad.Text,
                    TcNo = txtTcNo.Text,
                    PersonelNo = txtPersonelNo.Text,
                    DogumTarihi = dtpDogumTarihi.Value,
                    IseBaslamaTarihi = dtpIseBslmaTarihi.Value,
                    Departman = cmbDepartman.SelectedItem.ToString(),
                    Unvan = cmbUnvan.SelectedItem.ToString(),
                    Durumu = cmbDurumu.SelectedItem.ToString()
                };
                bool result = calisanDAL.Insert(calisan);
                if (result)
                {
                    Refresh();
                    MessageBox.Show("Kayıt Başarılı.");
                    ClearTools();
                }
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
           /* calisanDAL.Update(calisan);
            Transfer();*/
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (secilenID==0)
            {
                MessageBox.Show("Lütfen Silmek İstediğiniz Kaydı Seçiniz!");
            }
            else
            {
                DialogResult answer = MessageBox.Show("Seçilen Kayıt Silinecektir\nEmin misiniz?","DİKKAT",MessageBoxButtons.YesNo);
                if (answer==DialogResult.Yes)//eğer yes düğmesine tıklandıysa
                {
                    bool result = calisanDAL.Delete(secilenID);
                    if (result)
                    {
                        MessageBox.Show($"{secilenID}. ID'li Silme İşleminiz gerçekleşmiştir!");
                    }
                }
            }
        }
        void Cancel()
        {

            dgvCalisanlar.ClearSelection();
            secilenID = 0;
        }
        private void btnHepsiniSil_Click(object sender, EventArgs e)
        {
            DialogResult answer = MessageBox.Show("Tablonuzdaki Tüm Kayıtlarınız silinecek\nEmin misiniz?","DİKKAT TÜM KAYITLAR SİLİNECEK",MessageBoxButtons.YesNo);
            if (answer==DialogResult.Yes)
            {
                bool result = calisanDAL.Delete();
                if (result)
                {
                    MessageBox.Show("Tüm Kayıtlarınız Silindi\nHADİ BAKALIM KOLAY GELSİN");
                    Refresh();
                }
            }
        }

        private void dgvCalisanlar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar==(char)Keys.Escape)//Basılan tus escape
            {
                ClearTools();
            }
        }
        string CreateQueryString()
        {
            string queryString = string.Empty;
            List<string> alanlar = new List<string>();
            foreach (Control control in pnlPersonelBilgi.Controls)
            {
                //Eğer label ise bir sonrakine geç
                if (control.Tag==null) continue;
                //eğer o sırada kontrol ettiğiöiz içi boşsa bir sonraki kontrole geç 
                if (String.IsNullOrEmpty(control.Text) || control.Text == " ") continue;
                string fieldName = control.Tag.ToString();
                string value = control.Text;
                value = string.Empty;

                if (control.GetType()==typeof(DateTimePicker))
                {
                   value =Convert.ToDateTime(control.Text).ToString("yyyy-MM-dd");
                    alanlar.Add($"{fieldName} = '{value}'");
                    //  // tarih eklemey

                }
                else
                {
                    value = control.Text;
                    if (cbxBenzer.Checked) 
                    { 
                        alanlar.Add($"{fieldName} LIKE '%{value}%'"); //tarih eklerken eşitlik veriyor mbox da textleri bitkaç harf ile de bulur LİKE ile
                    }
                    else
                    {
                        alanlar.Add($"{fieldName} = '{value}'");
                    }
                }//WHERE fieldname = value
            }
            if (alanlar.Count>0)
            {
                queryString = $"WHERE {string.Join(" AND ", alanlar)}";
                //listedeki değerleri birleştirir
            }
            return queryString;
        }

        private void btnBul_Click(object sender, EventArgs e)
        {
            dgvCalisanlar.DataSource = calisanDAL.GetAll(CreateQueryString());
        }

        private void dtpDogumTarihi_ValueChanged(object sender, EventArgs e)
        {
            dtpDogumTarihi.Format = DateTimePickerFormat.Long;

        }

        private void dtpIseBslmaTarihi_ValueChanged(object sender, EventArgs e)
        {
            dtpIseBslmaTarihi.Format = DateTimePickerFormat.Long;

        }
    }
}
