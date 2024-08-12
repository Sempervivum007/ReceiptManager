using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ReceiptManager
{
    public partial class GetStatisticsWindow : Window
    {
        public GetStatisticsWindow(JsonDatabase _database)
        {
            InitializeComponent();
            Database = _database;
        }

        public JsonDatabase Database { get; }
        public List<Receipt> FilteredReceipt { get; set; }

        public bool IsTypeByAmount { get; set; }
        public bool IsTypeBySum { get; set; }
        public bool IsSumByDate { get; set; }

        private bool isCheckedDateFilter = false;
        private bool isCheckedTypeFilter = false;
        private bool isCheckedSumFilter = false;

        private void DateCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            isCheckedDateFilter = true;
        }

        private void TypeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            isCheckedTypeFilter = true;
        }

        private void SumCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            isCheckedSumFilter = true;
        }

        private void GetFilteredReceipt()
        {
            var dateFrom = DatePickerFrom.SelectedDate;
            var dateTo = DatePickerTo.SelectedDate;

            bool isSumFrom = decimal.TryParse(AmountTextBoxFrom.Text, out var sumFrom);
            bool isSumTo = decimal.TryParse(AmountTextBoxTo.Text, out var sumTo);

            bool isSelectedType = false;
            ExpenseType type = new ExpenseType();
            var selectedType = (ComboBoxItem)TypeComboBox.SelectedItem;
            if (selectedType != null)
                isSelectedType = Enum.TryParse(selectedType.Content.ToString(), out type);

            var receipts = Database.ReadReceipts();

            if (isCheckedDateFilter)
            {
                if(dateFrom != null)
                    receipts = receipts.Where(r => r.Date >= dateFrom).ToList();

                if (dateTo != null)
                    receipts = receipts.Where(r => r.Date < dateTo).ToList();
            }

            if (isCheckedTypeFilter)
            {
                if (selectedType != null && isSelectedType)
                    receipts = receipts.Where(r => r.Type == type).ToList();
            }

            if (isCheckedSumFilter)
            {
                if(isSumFrom)
                    receipts = receipts.Where(r => r.Amount >= sumFrom).ToList();

                if (isSumTo)
                    receipts = receipts.Where(r => r.Amount < sumTo).ToList();
            }

            FilteredReceipt = receipts;

            this.DialogResult = true;
            this.Close();
        }

        private void GetStatBySumButton_Click(object sender, RoutedEventArgs e)
        {
            IsTypeBySum = true;
            GetFilteredReceipt();
        }

        private void GetStatByCountButton_Click(object sender, RoutedEventArgs e)
        {
            IsTypeByAmount = true;
            GetFilteredReceipt();
        }

        private void GetStatByDateButton_Click(object sender, RoutedEventArgs e)
        {
            IsSumByDate = true;
            GetFilteredReceipt();
        }
    }
}