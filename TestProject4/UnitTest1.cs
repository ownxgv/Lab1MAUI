
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
        public void test1()
        {
            mainPage.setCell(0, 0, "5");


            mainPage.CalculateCell(0, 0);

            Assert.Equal("5", mainPage.getCell(0, 0));
        }

        [Fact]
        public void test2()
        {
            mainPage.setCell(0, 0, "2+3");

            mainPage.CalculateCell(0, 0);

            Assert.Equal("5", mainPage.getCell(0, 0));
        }
        [Fact]
        public void test3()
        {
            mainPage.setCell(0, 0, "2+2*2");

            mainPage.CalculateCell(0, 0);

            Assert.Equal("6", mainPage.getCell(0, 0));
        }


        [Fact]
        public void test4()
        {
            mainPage.setCell(0, 0, "2+2/0");

            mainPage.CalculateCell(0, 0);

            Assert.Equal("ERROR", mainPage.getCell(0, 0));
        }

        [Fact]
        public void test5()
        {
            mainPage.setCell(0, 0, "(2+2) // 3");

            mainPage.CalculateCell(0, 0);

            Assert.Equal("1", mainPage.getCell(0, 0));
        }

        [Fact]
        public void test6()
        {
            mainPage.setCell(0, 0, "(2+2+1) % 3");

            mainPage.CalculateCell(0, 0);

            Assert.Equal("2", mainPage.getCell(0, 0));
        }
    }
}
