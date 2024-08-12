using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ReceiptManager
{
    public partial class AddReceiptWindow : Window
    {
        public JsonDatabase Database { get; }
        public Receipt SelectedReceipt { get; }

        public AddReceiptWindow(JsonDatabase database, Receipt selectedReceipt)
        {
            InitializeComponent();
            Database = database;
            SelectedReceipt = selectedReceipt;

            if (selectedReceipt != null)
            {
                NameTextBox.Text = selectedReceipt.Description;
                DateDatePicker.SelectedDate = selectedReceipt.Date;
                AmountTextBox.Text = selectedReceipt.Amount.ToString();

                var selectedItem = TypeComboBox.Items.Cast<ComboBoxItem>()
                       .FirstOrDefault(item => (string)item.Content == selectedReceipt.Type.ToString());

                TypeComboBox.SelectedItem = selectedItem;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            DateTime? date = DateDatePicker.SelectedDate;
            decimal amount;

            if (string.IsNullOrEmpty(name) || date == null || !decimal.TryParse(AmountTextBox.Text, out amount))
            {
                MessageBox.Show("Будь ласка, заповніть всі поля коректно.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var selectedType = (ComboBoxItem)TypeComboBox.SelectedItem;
            if (selectedType == null)
            {
                MessageBox.Show("Будь ласка, вкажіть тип витрат.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Enum.TryParse(selectedType.Content.ToString(), out ExpenseType type);

            var receipt = new Receipt
            {
                Id = Guid.NewGuid(),
                Type = type,
                Description = name,
                Date = date ?? DateTime.Now,
                Amount = amount
            };

            if (SelectedReceipt != null)
            {
                List<Receipt> receipts = Database.ReadReceipts();

                var index = receipts.FindIndex(r => r.Id == SelectedReceipt.Id);
                if (index != -1)
                {
                    receipts[index] = receipt;
                    Database.SaveReceipts(receipts);
                }
            }
            else
            {
                Database.AddReceipt(receipt);
            }

            MessageBox.Show("Чек успішно збережено!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            this.DialogResult = true;
            this.Close();
        }
    }
}