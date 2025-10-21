using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOPLabWPF; // Переконайся, що є посилання на основний проект
using System;
using System.Collections.Generic;

namespace CarClassTest
{
    [TestClass]
    public class CarSerializationTests
    {
        private List<Car> _testCars;

        [TestInitialize]
        public void Initialize()
        {
            _testCars = new List<Car>
            {
                new Car("Audi", "A4", Color.Red, 150f, 1550m, 12000.0, 60.0, new DateTime(2022, 1, 1), 10.0, 4),
                new Car("BMW", "M3", Color.Blue, 420f, 1600m, 85000.0, 63.0, new DateTime(2008, 5, 17), 16.0, 2)
            };
        }

        [TestMethod]
        public void Serialize_ToCsv_ReturnsCorrectFormat()
        {
            // Arrange
            string expectedHeader = "Mark;Model;Color;HorsePower;Weight;Milage;FuelCapacity;ProductionDate;FuelConsumptionPer100km;NumberOfDoors";
            string expectedCar1 = _testCars[0].ToString();
            string expectedCar2 = _testCars[1].ToString();

            // Act
            string result = MainWindow.ToCsv(_testCars);

            // Assert
            Assert.IsTrue(result.StartsWith(expectedHeader));
            Assert.IsTrue(result.Contains(expectedCar1));
            Assert.IsTrue(result.Contains(expectedCar2));
        }

        [TestMethod]
        public void Deserialize_FromCsv_CorrectData()
        {
            // Arrange
            string csvContent = MainWindow.ToCsv(_testCars);

            // Act
            List<Car> deserializedCars = MainWindow.FromCsv(csvContent);

            // Assert
            Assert.AreEqual(2, deserializedCars.Count);
            Assert.AreEqual("Audi", deserializedCars[0].MarkAndModel.Split(' ')[0]);
            Assert.AreEqual(420, deserializedCars[1].HorsePower);
            Assert.AreEqual(Color.Blue, deserializedCars[1].Color);
        }

        [TestMethod]
        public void Deserialize_FromCsv_WithBrokenLines_SkipsInvalid()
        {
            // Arrange
            string brokenCsvContent = "Mark;Model;Color;HorsePower;Weight;Milage;FuelCapacity;ProductionDate;FuelConsumptionPer100km;NumberOfDoors\r\n" +
                                      "Ford;Focus;3;120;1300;50000;55;2018-01-01;7;4\r\n" +
                                      "VW;Golf;4;abc;1250;70000;50;2017-01-01;6.5;4\r\n" + 
                                      "Toyota;Camry;5;180;1500;30000;2019-01-01;8;4"; 

            // Act
            List<Car> deserializedCars = MainWindow.FromCsv(brokenCsvContent);

            // Assert
            Assert.AreEqual(1, deserializedCars.Count);
            Assert.AreEqual("Ford", deserializedCars[0].MarkAndModel.Split(' ')[0]);
        }

        [TestMethod]
        public void Serialize_ToJson_ReturnsValidJson()
        {
            // Arrange

            // Act
            string result = MainWindow.ToJson(_testCars);

            // Assert
            StringAssert.Contains(result, "\"Private Mark\": \"Audi\"");
        }

        [TestMethod]
        public void Deserialize_FromJson_CorrectData()
        {
            // Arrange
            string jsonContent = MainWindow.ToJson(_testCars);

            // Act
            List<Car> deserializedCars = MainWindow.FromJson(jsonContent);

            // Assert
            Assert.AreEqual(2, deserializedCars.Count);
            Assert.AreEqual("BMW", deserializedCars[1].MarkAndModel.Split(' ')[0]);
            Assert.AreEqual(150, deserializedCars[0].HorsePower);
        }

        [TestMethod]
        public void Deserialize_FromJson_WithInvalidData_SkipsInvalid()
        {
            // Arrange
            string brokenJsonContent = @"
            [
              {
                'Mark': 'ValidCar', 'Model': 'V1', 'Color': 1, 'HorsePower': 100.0, 'Weight': 1200, 
                'Milage': 1000, 'FuelCapacity': 50, 'ProductionDate': '2020-01-01', 
                'FuelConsumptionPer100km': 8, 'NumberOfDoors': 4
              },
              {
                'Mark': 'InvalidCar', 'Model': 'IV1', 'Color': 2, 'HorsePower': -50.0, 'Weight': 1300, 
                'Milage': 2000, 'FuelCapacity': 55, 'ProductionDate': '2021-01-01', 
                'FuelConsumptionPer100km': 9, 'NumberOfDoors': 2
              }
            ]";
            brokenJsonContent = brokenJsonContent.Replace('\'', '\"');

            // Act
            List<Car> deserializedCars = MainWindow.FromJson(brokenJsonContent);

            // Assert
            Assert.AreEqual("ValidCar", deserializedCars[0].MarkAndModel.Split(' ')[0]);
        }
    }
}