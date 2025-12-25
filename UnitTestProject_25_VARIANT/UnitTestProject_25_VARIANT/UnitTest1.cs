using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _25_VARIANT.Tests
{
    [TestClass]
    public class PhoneBillCalculatorTests
    {
        // метод CalculateCost для тестирования
        private (double totalCost, int extraMinutes) CalculatePhoneBill(int actualMinutes, int freeMinutes, double baseRate, double extraRate)
        {
            if (actualMinutes <= freeMinutes)
            {
                return (actualMinutes * baseRate, 0);
            }
            else
            {
                int extraMinutes = actualMinutes - freeMinutes;
                double totalCost = (freeMinutes * baseRate) + (extraMinutes * extraRate);
                return (totalCost, extraMinutes);
            }
        }

        // ТЕСТ 1: Большие числа (максимально большое по длине поля)
        [TestMethod]
        public void Test1_LargeNumbers_Tariff1()
        {
            // максимальное значение 
            int maxInt = int.MaxValue;
            int freeMinutes = 200;
            double baseRate = 0.7;
            double extraRate = 1.6;

            // выполняем расчет для тарифа 1
            var result = CalculatePhoneBill(maxInt, freeMinutes, baseRate, extraRate);

            // проверяем, что расчет выполнен
            Assert.IsTrue(result.totalCost > 0,
                "Для большого числа должна быть положительная стоимость");

            // проверка на правильность расчета сверхнормативных минут
            Assert.AreEqual(maxInt - 200, result.extraMinutes,
                "Сверхнормативные минуты для большого числа рассчитаны неверно");
        }

        [TestMethod]
        public void Test1_LargeNumbers_Tariff2()
        {
            // максимальное значение 
            int maxInt = int.MaxValue;
            int freeMinutes = 100;
            double baseRate = 0.3;
            double extraRate = 1.6;

            // выполняем расчет для тарифа 2
            var result = CalculatePhoneBill(maxInt, freeMinutes, baseRate, extraRate);

            // проверяем, что расчет выполнен
            Assert.IsTrue(result.totalCost > 0,
                "Для большого числа должна быть положительная стоимость");

            // проверка на правильность расчета сверхнормативных минут
            Assert.AreEqual(maxInt - 100, result.extraMinutes,
                "Сверхнормативные минуты для большого числа рассчитаны неверно");
        }

        // ТЕСТ 2: Отрицательные числа
        [TestMethod]
        public void Test2_NegativeNumbers_Tariff1()
        {
            // отрицательное значение
            int negativeMinutes = -150;
            int freeMinutes = 200;
            double baseRate = 0.7;
            double extraRate = 1.6;

            // выполняем расчет для тарифа 1
            var result = CalculatePhoneBill(negativeMinutes, freeMinutes, baseRate, extraRate);

            // проверяем результаты
            Assert.AreEqual(-105, result.totalCost, 0.01,
                "Отрицательные минуты должны давать отрицательную стоимость");

            Assert.AreEqual(0, result.extraMinutes,
                "При отрицательных минутах не может быть сверхнормы");
        }

        [TestMethod]
        public void Test2_NegativeNumbers_Tariff2()
        {
            // отрицательное значение
            int negativeMinutes = -50;
            int freeMinutes = 100;
            double baseRate = 0.3;
            double extraRate = 1.6;

            // выполняем расчет для тарифа 2
            var result = CalculatePhoneBill(negativeMinutes, freeMinutes, baseRate, extraRate);

            // проверяем результаты
            Assert.AreEqual(-15, result.totalCost, 0.01,
                "Отрицательные минуты должны давать отрицательную стоимость");

            Assert.AreEqual(0, result.extraMinutes,
                "При отрицательных минутах не может быть сверхнормы");
        }

        // ТЕСТ 3: Пустые поля ввода
        [TestMethod]
        public void Test3_EmptyInputFields()
        {

            Assert.IsTrue(string.IsNullOrEmpty(""), "Пустая строка должна быть пустой");
            Assert.IsTrue(string.IsNullOrWhiteSpace("   "), "Строка с пробелами должна считаться пустой");


            int result;
            bool parseEmpty = int.TryParse("", out result);
            Assert.IsFalse(parseEmpty, "Пустая строка не должна парситься в число");

            bool parseSpaces = int.TryParse("   ", out result);
            Assert.IsFalse(parseSpaces, "Строка с пробелами не должна парситься в число");
        }


    }
}