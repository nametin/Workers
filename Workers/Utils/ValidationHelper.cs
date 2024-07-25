
using System.Text.RegularExpressions;
using Workers.Models;

namespace Workers.Util
{
    public class ValidationHelper
    {
        /*isim soyisim alfabetik olmalı
         experience (text veya boş gönderildiğinde -1 gelir) number olmalı
        experience, verilen pozisyona göre belirli aralıklarda olmalı
        kontrollerini yaptıktan sonra bir hata mesajı (msg değişkeni) döner. mesaj boş ise hata yoktur. 
         */
        public static string IsValidInput(Position selectedPosition, int experience, string firstName, string lastName)
        {
            bool isExperienceValid = (selectedPosition == Position.SoftwareEngineer || selectedPosition == Position.TestEngineer) && (experience >= 0 && experience <= 3) ||
                   (selectedPosition == Position.SeniorSoftwareEngineer || selectedPosition == Position.SeniorTestEngineer) && (experience >= 4 && experience <= 7) ||
                   (selectedPosition == Position.LeadEngineer) && (experience >= 8 && experience <= 11) ||
                   (selectedPosition == Position.SeniorLeadEngineer) && (experience >= 12 && experience <= 15);

            string msg = "";
            if (!IsAlphabetic(firstName))
            {
                msg += "İsim yalnızca alfabetik karakterler içerebilir\n\n";
            }
            if (!IsAlphabetic(lastName))
            {
                msg += "Soyisim yalnızca alfabetik karakterler içerebilir\n";
            }
            if (experience == -1)
            {
                msg += "Lütfen deneyim alanına bir sayı giriniz.";
            }
            else if(!isExperienceValid)
            {
                msg += "\nTecrübe düzeyleri:\nYazılım Mühendisi, Test Mühendisi için [0,3]\nKıdemli Yazılım Mühendisi, Kıdemli Test Mühendisi için [4,7]\nLider Mühendis için [8,11]\nKıdemli Lider Mühendis için [12,15] \nolabilir. ";

            }
            
            return msg;
        }

        private static bool IsAlphabetic(string input)
        {
            return Regex.IsMatch(input, @"^[a-zA-ZğüşıöçĞÜŞİÖÇ ]+$");
        }
    }
}
