using OxyPlot.Series;
using OxyPlot;
using System.Collections.Generic;
using System.Windows;

namespace ReceiptManager
{
    public partial class StatisticsTypeByCountWindow : Window
    {
        private readonly List<StatModel> dataForStat;

        public PlotModel TypeByCountModel { get; set; }

        public StatisticsTypeByCountWindow(List<StatModel> dataForStat)
        {
            InitializeComponent();
            this.dataForStat = dataForStat;

            DataContext = this;

            InitializePlot();
            LoadData();
        }

        private void InitializePlot()
        {
            TypeByCountModel = new PlotModel { Title = "Cтатистика по типам за кількістю" };
        }

        private void LoadData()
        {
            var pieSeries = new PieSeries
            {
                StrokeThickness = 1.0,
                InsideLabelPosition = 0.5,
                AngleSpan = 360,
                StartAngle = 0
            };

            foreach (var item in dataForStat)
            {
                pieSeries.Slices.Add(new PieSlice(item.Type.ToString(), item.Count));
            }

            TypeByCountModel.Series.Add(pieSeries);
        }
    }
}