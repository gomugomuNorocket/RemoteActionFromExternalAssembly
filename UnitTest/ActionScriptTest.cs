using System;
using System.Windows.Forms;
using ActionsScriptLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class ActionScriptTest
    {
        ActionScript ActionScript = new ActionScript();

        [TestMethod]
        public void MoveMouse()
        {
            int newXPos = -500;
            int newYPos = 500;
            try
            {
                ActionScript.MoveMouse(newXPos, newYPos);

            }
            catch (Exception ex)
            {
                Assert.IsTrue(false);
            }

            long currentX = Cursor.Position.X;
            long currentY = Cursor.Position.Y;

            Assert.IsTrue((newXPos == currentX && newYPos == currentY) ? true : false);
        }

        [TestMethod]
        public void SelectWindow()
        {
            try
            {
                //ActionScript.SelectWindow("WINWORD");
                ActionScript.SelectWindow("Chrome");
            }
            catch(Exception ex)
            {
                Assert.IsTrue(false);
            }
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ClickMouse()
        {
            try
            {
                ActionScript.ClickMouse();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false);
            }
            Assert.IsTrue(true);
        }
    }
}
