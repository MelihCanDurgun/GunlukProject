using System.Data.SqlClient;

namespace gunlukOdevi
{
    internal class Program
    {
        static void Main(string[] args)

        {
            SqlConnection conn = new SqlConnection("Server=DESKTOP-3RLDHM4\\SQLEXPRESS; Database=Gunluk; Integrated Security=True ; TrustServerCertificate=Yes");
            //SQL bağlantısı
            //Console.WriteLine("Kullanıcı Giriş Ekranı");
            //Console.WriteLine("Kullanıcı Adı : ");
            //string kullaniciAdi = Console.ReadLine();
            //Console.WriteLine("Şifre : ");
            //string sifre = Console.ReadLine();
            //Thread.Sleep(2000);
            //Console.Clear();  ""


            bool anaMenuDonme = false;
            do
            {
                Console.WriteLine("Yapmak İstediğiniz İşlemi Seçiniz\n");
                Console.WriteLine("1)Yeni kayıt ekle");
                Console.WriteLine("2)Kayıtları listele");
                Console.WriteLine("3)Ana menüye dön");
                Console.WriteLine("4)Çıkış yap");
                Console.WriteLine("5)Kayıt Ara");
                Console.WriteLine("Örnek Ana menü için 3'ü seçiniz\n");

                string secim = Console.ReadLine();

                anaMenuDonme = true;
                switch (secim)
                {
                    case "1":
                        Console.Clear();
                        DateTime currentDate = DateTime.Now.Date;
                        string formattedDate = currentDate.ToString("yyyy-MM-dd");

                        using (SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Icerik WHERE CONVERT(date, DateCreated) = @formattedDate", conn))
                        {
                            checkCmd.Parameters.AddWithValue("@formattedDate", formattedDate);
                            conn.Open();
                            int adet = Convert.ToInt32(checkCmd.ExecuteScalar());
                            Thread.Sleep(1000);
                            conn.Close();

                            do
                            {
                                bool eklemeMenu = false;

                                using (SqlCommand cmd = new SqlCommand("INSERT INTO Icerik (title, description, DateCreated) VALUES (@Title, @Description, @formattedDate)", conn))
                                {
                                    eklemeMenu = true;
                                    Console.WriteLine(formattedDate);
                                    Console.WriteLine("Eklemek istediğiniz başlığı giriniz: ");
                                    string title = Console.ReadLine();

                                    Console.WriteLine("Eklemek istediğiniz açıklamayı giriniz: ");
                                    string description = Console.ReadLine();

                                    cmd.Parameters.AddWithValue("@Title", title);
                                    cmd.Parameters.AddWithValue("@Description", description);
                                    cmd.Parameters.AddWithValue("@formattedDate", currentDate);

                                    conn.Open();
                                    cmd.ExecuteNonQuery();
                                    conn.Close();

                                    Console.WriteLine("Kayıt ekleniyorr");
                                    Thread.Sleep(1500);
                                    Console.WriteLine("Kayıt başarılı bir şekilde eklendi");
                                    Thread.Sleep(1000);
                                    Console.Clear();
                                    Console.WriteLine("Bu tarihe zaten bir kayıt eklenmiş. İkinci Kayıdı Eklemek İstediğinize Eminmisiniz (E/H)");
                                    string cevap = Console.ReadLine().ToUpper();
                                    Thread.Sleep(1500);

                                    if (cevap == "E")
                                    {
                                        eklemeMenu = false;

                                    }

                                    else if (cevap == "H")
                                    {
                                        Console.WriteLine("Ana menüden Kayıtları listele bölümüne girerek günlüğünüzü düzenleye bilir veya silip yeniden yazabilirsiniz.");
                                        Thread.Sleep(2000);
                                        Console.Clear();
                                        break;
                                        anaMenuDonme = false;

                                    }

                                }
                            }
                            while (true);
                            { break; }


                        }
                        break;

                    case "2":
                        bool sonrakiDon = false;
                        int currentId = 0;
                        do
                        {
                            Console.Clear();
                            Console.WriteLine("Kayıtlar listeleniyor");


                            conn.Open();
                            string sqlQuery = $"SELECT TOP 1 * FROM Icerik WHERE Id > {currentId} ORDER BY Id ";


                            using (SqlCommand command = new SqlCommand(sqlQuery, conn))
                            using (SqlDataReader reader = command.ExecuteReader())
                                if (reader.Read())
                                {
                                    int Id = Convert.ToInt32(reader["Id"]);
                                    string title = reader["title"].ToString();
                                    string description = reader["description"].ToString();
                                    string date = reader["DateCreated"].ToString();
                                    if (Id != currentId)
                                    {
                                        currentId = Id;
                                    }
                                    conn.Close();

                                    Console.WriteLine($" Başlık: {title}\n Açıklama: {description}\n Tarih : {date}");
                                    Console.WriteLine("----------------------------------------------------------------");
                                    Console.WriteLine("1 - Sıradaki veriyi gör\n2 - Düzenle \n3 - Sil \n4 - Ana Menüye Dön");
                                    string secim2 = Console.ReadLine();
                                    Console.Clear();

                                    if (secim2 == "1")
                                    {
                                        sonrakiDon = false;

                                    }

                                    else if (secim2 == "2")
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Düzenleme Ekranı\n");
                                        Console.WriteLine("Başlık");
                                        string Title = Console.ReadLine();
                                        Console.WriteLine("Açıklama");
                                        string Description = Console.ReadLine();


                                        using (SqlCommand editCmd = new SqlCommand($"UPDATE Icerik SET Title=@Title, Description=@Description WHERE Id={currentId} ", conn))
                                        {
                                            editCmd.Parameters.AddWithValue("@Title", Title);
                                            editCmd.Parameters.AddWithValue("@Description", Description);
                                            conn.Open();
                                            editCmd.ExecuteNonQuery();
                                            conn.Close();
                                            Console.WriteLine("Düzenleme işleminiz başarılı :) ");
                                            Thread.Sleep(500);
                                            Console.WriteLine("Ana Menüye yönlendiriliyorsunuz..");
                                            anaMenuDonme = false;
                                            break;
                                        }
                                    }
                                    else if (secim2 == "3")
                                    {
                                        Console.Clear();
                                        using (SqlCommand editCmd = new SqlCommand($"DELETE FROM Icerik WHERE Id={currentId} ", conn))
                                        {
                                            conn.Open();
                                            editCmd.ExecuteNonQuery();
                                            conn.Close();
                                            Console.WriteLine("Silme işleminiz başarılı :) ");
                                            Thread.Sleep(500);
                                            Console.WriteLine("Ana Menüye yönlendiriliyorsunuz..");
                                            anaMenuDonme = false;
                                            break;
                                        }

                                    }
                                    else if (secim2 == "4")
                                    {
                                        Console.Clear();
                                        anaMenuDonme = false;
                                        break;
                                    }

                                    else
                                    {
                                        Console.WriteLine("Geçersiz seçim. Lütfen tekrar deneyin.");
                                    }
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.WriteLine("Veri bulunamadı.");
                                    Console.WriteLine("Ana Menüye yönlendiriliyorsunuz.. ");
                                    Thread.Sleep(500);
                                    anaMenuDonme = false;

                                    break;
                                }
                            conn.Close();

                        }
                        while (!sonrakiDon);
                        break;


                    case "3":

                        Console.WriteLine("Ana menüye dönülüyor");
                        Thread.Sleep(500);
                        anaMenuDonme = false;
                        Console.Clear();
                        break;


                    case "4":

                        Console.WriteLine("Çıkış yapılıyor");
                        Thread.Sleep(1500);
                        Console.Clear();
                        break;
                    case "5":
                        bool kayitAra = true;

                        do
                        {
                            Console.Clear();
                            Console.WriteLine("Kayıt Arama Ekranı");
                            Console.WriteLine("Arama İstediğiniz Tarihi Gün.Ay.Yıl Olarak Giriniz");
                            string tarihGir = Console.ReadLine();

                            if (DateTime.TryParse(tarihGir, out DateTime tarih))
                            {
                                Console.WriteLine("Girilen tarih: " + tarih);

                                conn.Open();
                                string sqlQuery = $"SELECT * FROM Icerik WHERE CONVERT(date, DateCreated) = @formattedDate";
                                using (SqlCommand command = new SqlCommand(sqlQuery, conn))
                                {
                                    command.Parameters.AddWithValue("@formattedDate", tarih);

                                    using (SqlDataReader mn = command.ExecuteReader())
                                    {
                                        while (mn.Read())
                                        {
                                            string title = mn["title"].ToString();
                                            string description = mn["description"].ToString();
                                            string date = mn["DateCreated"].ToString();
                                            Console.WriteLine($" Başlık: {title}\n Açıklama: {description}\n Tarih : {date}");
                                            Console.WriteLine("----------------------------------------------------------------");
                                        }
                                    }
                                }

                                conn.Close();
                            }
                            else
                            {
                                Console.WriteLine("Bu tarihte kayıt bulunmamaktadır.");
                            }

                            Console.WriteLine("Yeni bir arama yapmak istiyor musunuz? (E/H)");
                            string cevap = Console.ReadLine().ToUpper();
                            kayitAra = (cevap == "E");

                        } while (kayitAra);

                        break;

                }
            }
            while (!anaMenuDonme);
        }
    }
}


