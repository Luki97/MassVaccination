using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MassVaccination.Algorithms
{
    class BruteForce
    {
        // N
        private double Population;

        // S
        private double Susceptible;

        // I
        private double Infected = 1;

        // R
        private double Recovered = 0;

        // V
        private double Vaccinated = 0;

        public BruteForce()
        {
            this.Population = 1000;
            this.Susceptible = this.Population - this.Infected - this.Recovered;
        }

        public BruteForce(double population, double infected, double vaccinated)
        {
            this.Population = population;
            this.Infected = infected;
            this.Vaccinated = vaccinated;
            this.Susceptible = this.Population - this.Infected - this.Recovered;
        }

        public (double[] S, double[] I, double[] R) SimpleSIR(double beta, double gamma, int days)
        {
            // dS / dt = - (beta * S * I) / N
            // dI / dt = (beta * S * I) / N - (gamma * I)
            // dR / dt = gamma * I

            int span = days + 1;

            double[] S = new double[span];
            double[] I = new double[span];
            double[] R = new double[span];

            S[0] = this.Susceptible;
            I[0] = this.Infected;
            R[0] = this.Recovered;

            for (var i = 1; i < span; i++)
            {
                int dayBefore = i - 1;

                S[i] = S[dayBefore] - beta * S[dayBefore] * I[dayBefore] / this.Population;
                I[i] = I[dayBefore] + beta * S[dayBefore] * I[dayBefore] / this.Population - gamma * I[dayBefore];
                R[i] = R[dayBefore] + gamma * I[dayBefore];

                //Debug.WriteLine($"S: {S[i]}, I: {I[i]}, R: {R[i]}");
            }


            return (S, I, R);
        }

        public (double[] S, double[] V, double[] I, double[] R) SimpleSVIR(double beta, double omega, double gamma, int days, int startOfVaccineProgram)
        {
            // dS / dt = - (beta * S * I) / N - omega * S
            // dV / dt = omega * S
            // dI / dt = (beta * S * I) / N - (gamma * I)
            // dR / dt = gamma * I

            int span = days + 1;

            double[] S = new double[span];
            double[] V = new double[span];
            double[] I = new double[span];
            double[] R = new double[span];

            S[0] = this.Susceptible;
            V[0] = this.Vaccinated;
            I[0] = this.Infected;
            R[0] = this.Recovered;

            for (var i = 1; i < span; i++)
            {
                int dayBefore = i - 1;
                bool isStartOfVaccinationProgram = i > startOfVaccineProgram;

                S[i] = S[dayBefore] - beta * S[dayBefore] * I[dayBefore] / this.Population - (isStartOfVaccinationProgram ? omega * S[dayBefore] : 0);
                V[i] = V[dayBefore] + (isStartOfVaccinationProgram ? omega * S[dayBefore] : 0);
                I[i] = I[dayBefore] + beta * S[dayBefore] * I[dayBefore] / this.Population - gamma * I[dayBefore];
                R[i] = R[dayBefore] + gamma * I[dayBefore];

                //Debug.WriteLine($"{i} - S: {S[i]}, V: {V[i]},  I: {I[i]}, R: {R[i]}");
            }


            return (S, V, I, R);
        }
    }
}
 