using Xunit;
using Lab1MAUI;
using Microsoft.Maui.Controls;

namespace Lab1MAUITests
{
    public class MainPageTests
    {
        private MainPage mainPage;

        public MainPageTests()
        {
            mainPage = new MainPage();
        }

        [Fact]
        public void Evaluate_ConstantExpression_ReturnsCorrectValue()
        {
            // Arrange
            mainPage.setCell(0, 0, "5");
            

            // Act
            mainPage.CalculateCell(0, 0);

            // Assert
            Assert.Equal("5", mainPage.getCell(0,0));
        }

        [Fact]
        public void Evaluate_AdditionExpression_ReturnsCorrectValue()
        {
            // Arrange
            mainPage.setCell(0, 0, "2+3");

            // Act
            mainPage.CalculateCell(0, 0);

            // Assert
            Assert.Equal("5", mainPage.getCell(0,0));
        }

        
    }
}
