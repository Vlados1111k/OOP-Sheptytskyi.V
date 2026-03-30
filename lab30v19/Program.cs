using System;
using Xunit;

namespace lab30v19
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Loan Calculator Project");
            Console.WriteLine("Run 'dotnet test' to execute unit tests.");
        }
    }

    public class LoanCalculator
    {
        public double CalculateMonthlyPayment(double principal, double annualRate, int months)
        {
            if (principal <= 0 || annualRate < 0 || months <= 0)
                throw new ArgumentException("Invalid input parameters");

            if (annualRate == 0)
                return principal / months;

            double monthlyRate = annualRate / 12 / 100;
            double payment = (principal * monthlyRate * Math.Pow(1 + monthlyRate, months)) 
                             / (Math.Pow(1 + monthlyRate, months) - 1);

            return Math.Round(payment, 2);
        }

        public bool IsEligibleForLoan(double monthlyIncome, double monthlyPayment)
        {
            if (monthlyIncome <= 0) return false;
            return (monthlyPayment / monthlyIncome) <= 0.4;
        }
    }

    public class LoanCalculatorTests
    {
        private readonly LoanCalculator _calculator;

        public LoanCalculatorTests()
        {
            _calculator = new LoanCalculator();
        }

        [Fact]
        public void CalculateMonthlyPayment_StandardCase_ReturnsCorrectValue()
        {
            var result = _calculator.CalculateMonthlyPayment(10000, 12, 12);
            Assert.Equal(888.49, result);
        }

        [Fact]
        public void CalculateMonthlyPayment_ZeroRate_ReturnsSimpleDivision()
        {
            var result = _calculator.CalculateMonthlyPayment(1200, 0, 12);
            Assert.Equal(100, result);
        }

        [Theory]
        [InlineData(0, 5, 12)]
        [InlineData(1000, -1, 12)]
        [InlineData(1000, 5, 0)]
        public void CalculateMonthlyPayment_InvalidInputs_ThrowsException(double p, double r, int m)
        {
            Assert.Throws<ArgumentException>(() => _calculator.CalculateMonthlyPayment(p, r, m));
        }

        [Theory]
        [InlineData(5000, 1500, true)]
        [InlineData(2000, 1000, false)]
        [InlineData(3000, 1200, true)]
        public void IsEligibleForLoan_TheoryTests(double income, double payment, bool expected)
        {
            var result = _calculator.IsEligibleForLoan(income, payment);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IsEligibleForLoan_ZeroIncome_ReturnsFalse()
        {
            var result = _calculator.IsEligibleForLoan(0, 500);
            Assert.False(result);
        }

        [Fact]
        public void CalculateMonthlyPayment_LargeAmount_ReturnsPreciseValue()
        {
            var result = _calculator.CalculateMonthlyPayment(1000000, 5, 360);
            Assert.Equal(5368.22, result);
        }

        [Fact]
        public void CalculateMonthlyPayment_ResultIsInRange()
        {
            var result = _calculator.CalculateMonthlyPayment(5000, 10, 24);
            Assert.InRange(result, 200, 300);
        }

        [Fact]
        public void IsEligibleForLoan_ExactLimit_ReturnsTrue()
        {
            var result = _calculator.IsEligibleForLoan(1000, 400);
            Assert.True(result);
        }

        [Fact]
        public void CalculateMonthlyPayment_NotNull()
        {
            var result = _calculator.CalculateMonthlyPayment(1000, 5, 12);
            Assert.NotNull(result);
        }

        [Fact]
        public void CalculateMonthlyPayment_IsTypeDouble()
        {
            var result = _calculator.CalculateMonthlyPayment(100, 1, 1);
            Assert.IsType<double>(result);
        }
    }
}