using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MassVaccination.Algorithms;
using System.Text.RegularExpressions;

namespace MassVaccination
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Parameters _params = new Parameters
        {
            Population = 1000,
            InitialInfected = 1,
            InitialVaccinated = 0,
            Beta = 0.5,
            Gamma = 0.01,
            Omega = 0.1,
            StartOfVaccinationProgram = 0,
            Days = 365,
            Models = new string[] { "Simple SIR", "Simple SVIR" }
        };

        public MainWindow()
        {   
            
            InitializeComponent();
            this.DataContext = this._params;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[0-9]+([.,][0-9]+)?$");
            e.Handled = regex.IsMatch(e.Text);
        }

        void Plot(object sender, RoutedEventArgs e)
        {
            switch (models.SelectedIndex)
            {
                case 0:
                    PlotSIR();
                    break;
                case 1:
                    PlotSVIR();
                    break;
                default:
                    throw new ArgumentException("Unknown model");
            }

            plot.Render();
        }

        private void PlotSIR()
        {
            plot.plt.Clear();

            var bf = new BruteForce(
                this._params.Population,
                this._params.InitialInfected,
                this._params.InitialVaccinated
            );

            var (S, I, R) = bf.SimpleSIR(this._params.Beta, this._params.Gamma, this._params.Days);
            var xs = DataGen.Consecutive(this._params.Days + 1);


            plot.plt.PlotScatter(xs, S, label: "Susceptible");
            plot.plt.PlotScatter(xs, I, label: "Infected");
            plot.plt.PlotScatter(xs, R, label: "Recovered");
            plot.plt.Legend();

            plot.plt.Title("Simple SIR");
            plot.plt.YLabel("Number of people");
            plot.plt.XLabel("Days");
        }

        private void PlotSVIR()
        {
            plot.plt.Clear();

            var bf = new BruteForce(
                this._params.Population,
                this._params.InitialInfected,
                this._params.InitialVaccinated
            );

            var (S, V, I, R) = bf.SimpleSVIR(
                this._params.Beta,
                this._params.Omega,
                this._params.Gamma, 
                this._params.Days, 
                this._params.StartOfVaccinationProgram
            );
            var xs = DataGen.Consecutive(this._params.Days + 1);


            plot.plt.PlotScatter(xs, S, label: "Susceptible");
            plot.plt.PlotScatter(xs, V, label: "Vaccinated");
            plot.plt.PlotScatter(xs, I, label: "Infected");
            plot.plt.PlotScatter(xs, R, label: "Recovered");
            plot.plt.Legend();

            plot.plt.Title("Simple SVIR");
            plot.plt.YLabel("Number of people");
            plot.plt.XLabel("Days");
        }
    }


    class Parameters
    {
        public int Population { get; set; }
        public double InitialInfected { get; set; }
        public double InitialVaccinated { get; set; }
        public double Beta { get; set; }
        public double Gamma { get; set; }
        public double Omega { get; set; }
        public int Days{ get; set; }
        public int StartOfVaccinationProgram { get; set; }
        public string[] Models { get; set; }
    }
}
