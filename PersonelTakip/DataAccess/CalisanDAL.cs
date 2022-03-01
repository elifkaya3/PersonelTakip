using PersonelTakip.Entities;
using PersonelTakip.Tools;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PersonelTakip.DataAccess
{
    class CalisanDAL
    {
        //burada data access layer hzırlıyoruz
        // yani çalısan entitimizle ilgili CRUD vb işlemlerimizi burada gerçekleştirilecek

        /// <summary>
        /// bu metod geriye içinde çalışan tipinde değerler barındıran bir list tipinde değer döndürecek
        /// </summary>
        /// <param name="kosulCumlesi"></param>
        /// bu parametre boş bırakılırsa yani "" string.empty şeklinde kullanılırsa
        /// tüm kayıtları dönderecek.eğere buraya uygbun bir WHERE ifadesi yazılırsa koşula uygun kayıtları döndürecek
        /// <returns></returns>
        public List<Calisan> GetAll(string kosulCumlesi="")
        {
            List<Calisan> calisanlar = new List<Calisan>();
            try
            {
                //buraya yazacağımız işlemleri yapmayı dene sorun yoksa çalıştır.
                using (SqlCommand command = new SqlCommand($"SELECT * FROM tblCalisanlar {kosulCumlesi}", SQLBaglanti.Baglanti))
                {
                    //using satırında yaratılan command adlı nesne sadece bu scope ta yaşayacak.
                    //bu scope dışında yok olmuş olacak.
                    //bu nesnenin yok edilmesini garbage collector un inisiyatifine bırakmamış olduk.
                    SQLBaglanti.BaglantiAc();
                    using (SqlDataReader reader=command.ExecuteReader())
                    {
                        while (reader.Read()) //Geri dönen değerlerin hepsine bakmamızı sağlıyor
                        {
                            Calisan calisan = new Calisan
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                Ad = reader["Ad"].ToString(),
                                Soyad = reader["Soyad"].ToString(),
                                TcNo = reader["TcNo"].ToString(),
                                PersonelNo = reader["PersonelNo"].ToString(),
                                DogumTarihi=Convert.ToDateTime(reader["DogumTarihi"]),
                                IseBaslamaTarihi=Convert.ToDateTime(reader["IseBaslamaTarihi"]),
                                Departman = reader["Departman"].ToString(),
                                Unvan = reader["Unvan"].ToString(),
                                Durumu = reader["Durumu"].ToString()

                            };
                            calisanlar.Add(calisan);
                        }
                    }
                }
                return calisanlar;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            finally
            {
                SQLBaglanti.BaglantiKapat();
            }
        }
        public bool Insert(Calisan calisan)
        {
            string sorguCumlesi = $"INSERT INTO tblCalisanlar (Ad,Soyad,TcNo,PersonelNo,DogumTarihi,IseBaslamaTarihi,Departman,Unvan,Durumu)"+
                $"VALUES(@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9)";
            try
            {
                using (SqlCommand command = new SqlCommand(sorguCumlesi,SQLBaglanti.Baglanti))
                {
                    command.Parameters.AddWithValue("@p1",calisan.Ad);
                    command.Parameters.AddWithValue("@p2",calisan.Soyad);
                    command.Parameters.AddWithValue("@p3",calisan.TcNo);
                    command.Parameters.AddWithValue("@p4",calisan.PersonelNo);
                    command.Parameters.AddWithValue("@p5",calisan.DogumTarihi);
                    command.Parameters.AddWithValue("@p6",calisan.IseBaslamaTarihi);
                    command.Parameters.AddWithValue("@p7",calisan.Departman);
                    command.Parameters.AddWithValue("@p8",calisan.Unvan);
                    command.Parameters.AddWithValue("@p9",calisan.Durumu);
                    SQLBaglanti.BaglantiAc();
                    command.ExecuteNonQuery();
                }
                return true; //Kayıt başarılı anlamında
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false; //kayıt başarısız anlamında
            }
            finally
            {
                SQLBaglanti.BaglantiKapat();
            }
        }
        public bool Update(Calisan calisan)
        {
            string sorguCumlesi = $"UPDATE tblCalisanlar SET Ad=@p1, Soyad=@p2,TcNo=@p3,PersonelNo=@p4"+
                $"DogumTarihi=@p5,IseBaslamaTarihi=@p6,Departman=@p7,Unvan=@p8,Durumu=@p9 WHERE ID=@p10";
            try
            {
                using (SqlCommand command = new SqlCommand(sorguCumlesi, SQLBaglanti.Baglanti))
                {
                    command.Parameters.AddWithValue("@p1", calisan.Ad);
                    command.Parameters.AddWithValue("@p2", calisan.Soyad);
                    command.Parameters.AddWithValue("@p3", calisan.TcNo);
                    command.Parameters.AddWithValue("@p4", calisan.PersonelNo);
                    command.Parameters.AddWithValue("@p5", calisan.DogumTarihi);
                    command.Parameters.AddWithValue("@p6", calisan.IseBaslamaTarihi);
                    command.Parameters.AddWithValue("@p7", calisan.Departman);
                    command.Parameters.AddWithValue("@p8", calisan.Unvan);
                    command.Parameters.AddWithValue("@p9", calisan.Durumu);
                    command.Parameters.AddWithValue("@p10", calisan.ID);

                    SQLBaglanti.BaglantiAc();
                    command.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                SQLBaglanti.BaglantiKapat();
            }
        }
        public bool Delete(int id)
        {
            string sorguCumlesi = $"DELETE FROM tblCalisanlar WHERE ID=@p1";

            try
            {
                using (SqlCommand command = new SqlCommand(sorguCumlesi, SQLBaglanti.Baglanti))
                {
                    command.Parameters.AddWithValue("@p1", id);
                    SQLBaglanti.BaglantiAc();
                    command.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                SQLBaglanti.BaglantiKapat();
            }
        }
        public bool Delete(string kosulCumlesi=" ")
        {
            string sorguCumlesi = $"DELETE FROM tblCalisanlar {kosulCumlesi}";
            try
            {
                using (SqlCommand command = new SqlCommand(sorguCumlesi, SQLBaglanti.Baglanti))
                {
                    //command.Parameters.AddWithValue("@p1", kosulCumlesi);
                    SQLBaglanti.BaglantiAc();
                    command.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                SQLBaglanti.BaglantiKapat();
            }
        }
    }
}
