using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Mayın_Tarlası
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Aşağı.Enabled = false;
            Yukari.Enabled = false;
            Sağa.Enabled = false;
            Sola.Enabled = false;
        }

        Random rnd = new Random();
        int[] mayinlar = new int[80];
        int[,] panel = new int[20, 20];
        int mayinSayi;
        int dakika, saniye;
        void mayinAtama()                       
        {
            mayinSayi = 0;
            if (radioButton1.Checked == true)       //radioButtonların hangisi seçildisyse ona göre
            {                                       //mayın sayısını belirliyoruz.
                mayinSayi = 40;
            }
            if (radioButton2.Checked == true)
            {
                mayinSayi = 50;
            }
            if (radioButton3.Checked == true)
            {
                mayinSayi = 80;
            }

            for (int i = 0; i < mayinSayi; i++)
            {
                int secilen = rnd.Next(0, 400);     //secilen değişkenine 0-400 arası değer atıyoruz
                if (mayinlar.Contains(secilen))     //mayinlar dizisinde secilen değişkeni var mı diye bakıyoruz
                {                                   //varsa tekrar atama yapmasını sağlıyoruz
                    i--;
                    continue;
                }
                mayinlar[i] = secilen;              //diziyi 0-400 arasında random değerle dolduruyoruz

            }

            int x = 0;
            for (int i = 0; i < 400; i++)           //tek boyutlu mayınlar dizisini 2 boyutlu diziye çeviriyoruz
            {
                if (i < 20)                        
                {
                    if (mayinlar.Contains(i))       //i değişkeni mayinlar dizinde var mı diye kontrol ediyoruz
                    {                               //varsa 0.satırda i. 1 yani mayın atıyoruz. 
                        panel[i, 0] = 1;
                    }
                }
                else                                //eğer i>20 ise burda işlem yapıyoruz
                {
                    if (i % 20 == 0)                //i 20 nin katıysa x e 20 ekliyoruz
                    {
                        x += 20;
                    }
                    if (mayinlar.Contains(i))       //i değişkeni mayinlar dizinde var mı diye kontrol ediyoruz
                    {
                        panel[i - x, i / 20] = 1;   //(i-x) işlemi sıra sıra sütunlara i/20 işlemi satırlara
                                                    //ulaşmamızı sağlıyor ve bu şekilde mayınları 2 boyutlu
                    }                               //diziye atamış oluyoruz
                }

            }
            for (int i = 0; i < mayinSayi; i++)     //mayinlar dizisini sıfırlıyoruz
            {
                mayinlar[i] = 0;
            }
            /*  for (int i = 0; i < 20; i++)        //burdaki yorum satırlarını istediğimiz zaman açıp mayınları
              {                                     //görüyoruz . Programı test aşamasında mayınları görmemizi sağlıyor.
                  for (int j = 0; j < 20; j++)
                  {
                      if (panel[i, j] == 1)
                      {

                          TablePanel.Controls.Add(new Label() { Text = "1" }, i, j);
                      }
                  }
              }*/

            PictureBox picture = new PictureBox();                  //burda picture box oluşturup başlangıç yerine
            picture.BackColor = Color.White;                        //resmimizi atıyoruz
            picture.BackgroundImageLayout = ImageLayout.Zoom;
            TablePanel.Controls.Add(picture, 10, 19);
            TablePanel.GetControlFromPosition(10, 19).BackgroundImage = Properties.Resources.lion;
            picture.Margin = new Padding(1);

        }


        void yeniOyun()                         //yeni oyun oluşturma metodu
        {
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    panel[i, j] = 0;                //mayınları temizliyor
                    TablePanel.Controls.Clear();    //paneli temizliyor
                }
            }
            label5.Text = "00 : 00";
            satir = 19; sutun = 10;
            MayinKontrol();                         
            this.BackColor = Color.WhiteSmoke;

        }
        List<string> oyuncu = new List<string>();

        int satir = 19, sutun = 10;

        void MayinKontrol()           //etraftaki mayınları bulmamızı sağlayan fonksiyon
        {
            int mayinVarMi = 0;

            if (sutun != 0)
            {
                if (panel[sutun - 1, satir] == 1)
                {
                    mayinVarMi++;
                }
            }
            if (sutun != 19)
            {
                if (panel[sutun + 1, satir] == 1)
                {
                    mayinVarMi++;
                }
            }
            if (satir != 0)
            {
                if (panel[sutun, satir - 1] == 1)
                {
                    mayinVarMi++;
                }
            }
            if (satir != 19)
            {
                if (panel[sutun, satir + 1] == 1)
                {
                    mayinVarMi++;
                }
            }

            label3.Text = "Mayınlara Yakın :" + mayinVarMi.ToString();
        }

        void mayinlariGoster()                  //kazandığımızda veya kaybettiğimizde mayınların
        {                                       //yerlerini gösteren fonksiyon
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    if (panel[i, j] == 1)
                    {

                        TablePanel.GetControlFromPosition(i, j).BackgroundImage = Properties.Resources.bomba;

                    }
                }
            }
        }
        void mayinaBasti()          //mayına bastıldığında bu işlemler yapılıyor
        {
            timer1.Stop();          //zaman durduruldu

            oyuncu.Add(textBox1.Text + "    " + dakika.ToString() + " : " + saniye.ToString());
            File.WriteAllLines(AppDomain.CurrentDomain.BaseDirectory + @"\" + "kayitlar.txt", oyuncu);  //kişi ve skor dosyaya kaydedildi

            dakika = 0; saniye = 0;

            TablePanel.GetControlFromPosition(satir, sutun).BackgroundImage = Properties.Resources.bomba;
            this.BackColor = Color.Yellow;

            mayinlariGoster();

            DialogResult seçim = MessageBox.Show("Mayına Bastınız\nYeni oyun açılsın mı?", "KAYBETTİNİZ"
            , MessageBoxButtons.YesNo);
            //kullanıcının yeni oyun isteyip istemediği soruldu.
            if (seçim == DialogResult.Yes)
            {
                yeniOyun();
            }
            Aşağı.Enabled = false;
            Yukari.Enabled = false;
            Sağa.Enabled = false;
            Sola.Enabled = false;
        }



        private void Yukari_Click(object sender, EventArgs e)   //yukarı hareket etme
        {
            satir--;
            timer1.Start();

            if (panel[sutun, satir] == 1)       //çıktığımız yerde mayın varsa mayinaBasti fonskiyonunu çağırıyoruz
            {
                mayinaBasti();
            }
            else
            {
                MayinKontrol();
                TablePanel.GetControlFromPosition(sutun, satir).BackgroundImage = Properties.Resources.lion;//gidilen yere aslan resmi koyuyoruz
                TablePanel.GetControlFromPosition(sutun, satir + 1).BackColor = Color.White;//geçilen yolu beyaz yapıyoruz
                TablePanel.GetControlFromPosition(sutun, satir + 1).BackgroundImage = null;//arkamızdaki resmi siliyoruz
            }
            if (satir == 0)         //üst sınıra ulaşırsa kazandırıyoruz
            {
                timer1.Stop();
                mayinlariGoster();
                MessageBox.Show("Kazandınız Tebrikler");
                
                Aşağı.Enabled = false;
                Yukari.Enabled = false;
                Sağa.Enabled = false;
                Sola.Enabled = false;

                oyuncu.Add(textBox1.Text + "    " + dakika.ToString() + " : " + saniye.ToString());
                File.WriteAllLines(AppDomain.CurrentDomain.BaseDirectory + @"\" + "kayitlar.txt", oyuncu);//kazananı dosyaya ekliyoruz
                dakika = 0; saniye = 0;
            }
        }

        private void Sağa_Click(object sender, EventArgs e)
        {
            sutun++;
            timer1.Start();

            if (sutun > 19)
            {
                MessageBox.Show("Sağa çok gittiniz.");
                sutun--;
            }
            else
            {
                MayinKontrol();

                if (panel[sutun, satir] == 1)
                {
                    mayinaBasti();
                }
                else
                {
                    TablePanel.GetControlFromPosition(sutun, satir).BackgroundImage = Properties.Resources.lion;
                    TablePanel.GetControlFromPosition(sutun - 1, satir).BackColor = Color.White;
                    TablePanel.GetControlFromPosition(sutun - 1, satir).BackgroundImage = null;

                }
            }
        }

        private void Aşağı_Click(object sender, EventArgs e)
        {
            satir++;
            timer1.Start();

            if (satir > 19)
            {
                MessageBox.Show("Aşağıya çok gittiniz.");
                satir--;
            }
            else
            {
                MayinKontrol();

                if (panel[sutun, satir] == 1)
                {
                    mayinaBasti();
                }
                else
                {
                    TablePanel.GetControlFromPosition(sutun, satir).BackgroundImage = Properties.Resources.lion;
                    TablePanel.GetControlFromPosition(sutun, satir - 1).BackColor = Color.White;
                    TablePanel.GetControlFromPosition(sutun, satir - 1).BackgroundImage = null;
                }
            }
        }

        private void Sola_Click(object sender, EventArgs e)
        {
            sutun--;
            timer1.Start();

            if (sutun < 0)
            {
                MessageBox.Show("Sola çok gittiniz.");
                sutun++;
            }
            else
            {
                MayinKontrol();

                if (panel[sutun, satir] == 1)
                {
                    mayinaBasti();
                }
                else
                {
                    TablePanel.GetControlFromPosition(sutun, satir).BackgroundImage = Properties.Resources.lion;
                    TablePanel.GetControlFromPosition(sutun + 1, satir).BackColor = Color.White;
                    TablePanel.GetControlFromPosition(sutun + 1, satir).BackgroundImage = null;

                }
            }
        }

        private void KişiyiKaydet_Click(object sender, EventArgs e)
        {

            if (radioButton2.Checked == false && radioButton2.Checked == false && //zorluk seçilmedi ve isim girilmedi ise
                radioButton2.Checked == false && textBox1.Text == "")              //hata verilmesini sağlıyoruz
            {
                MessageBox.Show("Lütfen Oynun Seviyesini Seçin ve Adınızı Girin", "Mayın Tarlası",
                    MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }

            else
            {
                MessageBox.Show(textBox1.Text + " Kaydedildi.", "Mayın Tarlası");

                Aşağı.Enabled = true;
                Yukari.Enabled = true;
                Sağa.Enabled = true;
                Sola.Enabled = true;
                yeniOyun();
                mayinAtama();

                for (int i = 0; i < 400; i++)                   //her hücreyi picturebox ile dolduruyoruz
                {                                               //böylelikle daha sonra bu pictureboxlar üzerinde
                    PictureBox picture = new PictureBox();      //işlem yapabiliyoruz
                    picture.BackColor = Color.LightSkyBlue;
                    picture.BackgroundImageLayout = ImageLayout.Zoom;
                    TablePanel.Controls.Add(picture);
                    picture.Margin = new Padding(1);

                }
            }
        }
        private void SkorlarıGöster_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Directory.GetCurrentDirectory() + "\\kayitlar.txt");//txt dosyasını açar
        }

        private void Yardım_Click(object sender, EventArgs e)
        {
            MessageBox.Show("-----------GELİŞTİREN----------\n\nAtatürk Üniversitesi Bilgisayar Mühendisliği" +
                "  Hamza AYDEMİR\n\nOyunun Oynanması:\n" +
                "Oyunun başlaması için öncelikle seviye seçilmeli ve oyuncu kaydedilmelidir. " +
                "Seviye seçilip oyuncu kaydı yapıldığında seçilen seviyeye göre mayınlar tarlamıza yerleşir. " +
                "Yön tuşlarına basıldığı zaman süre aktif hale gelir. Üst tarafa ulaştığınızda oyunu kazanmış " +
                "olursunuz. Oyun otamatik olarak sağ sol ön ve arkanızda mayın varsa üstte sizi uyarır. " +
                "Mayınlara yakınlığa göre hareket edilir ve üst tarafa ulaşmak hedeflenir. Eğer mayına basarsanız" +
                "mayınların yerleri gözükür ve yeni oyun oynamak isteyip istemediğiniz sorulur.\n\n" +
                ".....BOL ŞANS.....", "Mayın Tarlası", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void timer1_Tick_1(object sender, EventArgs e)      //kronometre
        {
            if (saniye == 60)
            {
                saniye = 0;
                dakika++;
            }
            label5.Text = dakika.ToString() + " : " + saniye.ToString();
            saniye++;
        }
    }
}
