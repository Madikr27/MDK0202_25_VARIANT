using System;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Globalization;

namespace _25_VARIANT
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double totalCost = 0;
        private int extraMinutes = 0;
        private string selectedTariff = "";
        private int minutes = 0;
        private int freeMinutes = 0;
        private double baseRate = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void rschitatButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверка ввода количества минут
            if (!int.TryParse(minutesTextbox.Text, out minutes) || minutes < 0)
            {
                MessageBox.Show("Пожалуйста, введите корректное количество минут (целое неотрицательное число).", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Определение выбранного тарифа
            RadioButton selectedRadioButton = null;
            foreach (var child in ((Grid)Content).Children)
            {
                if (child is RadioButton radioButton && radioButton.IsChecked == true)
                {
                    selectedRadioButton = radioButton;
                    break;
                }
            }

            if (selectedRadioButton == null)
            {
                MessageBox.Show("Пожалуйста, выберите тариф.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Расчет стоимости в зависимости от тарифа
            if (selectedRadioButton.Content.ToString().Contains("Тариф1"))
            {
                selectedTariff = "Тариф1";
                freeMinutes = 200;
                baseRate = 0.7;
                CalculateCost(minutes, freeMinutes, baseRate, 1.6);
            }
            else if (selectedRadioButton.Content.ToString().Contains("Тариф2"))
            {
                selectedTariff = "Тариф2";
                freeMinutes = 100;
                baseRate = 0.3;
                CalculateCost(minutes, freeMinutes, baseRate, 1.6);
            }

            // Вывод результатов на форму
            resultOrderLabel.Content = $"Сумма к оплате: {totalCost:F2} руб.";
            countMinutesLabel.Content = $"Количество минут сверх нормы: {extraMinutes}";
        }

        private void CalculateCost(int actualMinutes, int freeMinutes, double baseRate, double extraRate)
        {
            if (actualMinutes <= freeMinutes)
            {
                totalCost = actualMinutes * baseRate;
                extraMinutes = 0;
            }
            else
            {
                extraMinutes = actualMinutes - freeMinutes;
                totalCost = (freeMinutes * baseRate) + (extraMinutes * extraRate);
            }
        }

        private void checkButton_Click(object sender, RoutedEventArgs e)
        {
            if (totalCost == 0 || string.IsNullOrEmpty(selectedTariff))
            {
                MessageBox.Show("Сначала выполните расчет стоимости.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Создание текстового файла 
                CreateTextReceipt();
                MessageBox.Show("Квитанция успешно создана на рабочем столе!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании квитанции: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CreateTextReceipt()
        {
            // Генерация номера чека
            string receiptNumber = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();

            // Формирование содержимого чека
            string receiptContent = $@"ООО «Телефонная компания»

Тариф: {selectedTariff}
Количество минут: {minutes}
Минут сверх нормы: {extraMinutes}
Итого: {totalCost:F2} руб.
Дата: {DateTime.Now:dd.MM.yyyy HH:mm:ss}

Спасибо за использование наших услуг!
";

            string fileName = $"чек_{DateTime.Now:ddMMyyyy_HHmmss}.txt";
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(desktopPath, fileName);

            File.WriteAllText(filePath, receiptContent, System.Text.Encoding.UTF8);
            System.Diagnostics.Process.Start("notepad.exe", filePath);

        }

    }
}
