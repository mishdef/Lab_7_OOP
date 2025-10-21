using OOPLabWPF;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ProgramClassTest
{ 
    [TestClass()]
    [DoNotParallelize]
    public sealed class ProgramTest
    {
        private const int TotalSeedItems = 5;

        [TestInitialize()]
        public void Initialize()
        {
            OOPLabWPF.MainWindow.carLimit = 100;
            OOPLabWPF.MainWindow.cars = new List<Car>();
            Car.Count = 0;
            Car.ResetCountres();
        }

        [TestCleanup()]
        public void Cleanup()
        {
            OOPLabWPF.MainWindow.cars.Clear();
            OOPLabWPF.MainWindow.carLimit = 0;
            Car.Count = 0;
            Car.ResetCountres();
        }

        [TestMethod()]
        public void AddSeedData_AddsAllFiveCars()
        {
            OOPLabWPF.MainWindow.carLimit = 5;
            int expectedCarsAdded = TotalSeedItems;

            OOPLabWPF.MainWindow.AddSeedData();

            int finalCount = OOPLabWPF.MainWindow.cars.Count;
            Assert.AreEqual(expectedCarsAdded, finalCount);
            Assert.AreEqual(expectedCarsAdded, Car.Count);
        }

        [TestMethod()]
        public void AddSeedData_RespectsMaxCapacity()
        {
            OOPLabWPF.MainWindow.carLimit = 3;
            int expectedFinalCount = 3;

            OOPLabWPF.MainWindow.AddSeedData();

            int finalCount = OOPLabWPF.MainWindow.cars.Count;
            Assert.AreEqual(expectedFinalCount, finalCount);
            Assert.AreEqual(expectedFinalCount, Car.Count);
        }

        [TestMethod()]
        public void AddSeedData_WhenFull_NoMoreAdded()
        {
            OOPLabWPF.MainWindow.carLimit = 2;

            OOPLabWPF.MainWindow.AddSeedData();

            int initialCount = OOPLabWPF.MainWindow.cars.Count;
            Assert.AreEqual(2, initialCount);

            OOPLabWPF.MainWindow.AddSeedData();

            int finalCount = OOPLabWPF.MainWindow.cars.Count;
            int expectedFinalCount = initialCount;

            Assert.AreEqual(expectedFinalCount, finalCount);
            Assert.AreEqual(expectedFinalCount, Car.Count);
        }
    }
}
