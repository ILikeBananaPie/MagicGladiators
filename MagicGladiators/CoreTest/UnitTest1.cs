using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MagicGladiators;
using Microsoft.Xna.Framework;

namespace CoreTest
{
    //[ExpectedException(typeof(System.OverflowException))]

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetVectorTestPositiveValuesNegativeResult()
        {
            Physics physics = new Physics(new GameObject());
            Vector2 actual = physics.GetVector(new Vector2(1, 1), new Vector2(2, 2));
            Vector2 expected = new Vector2(-1, -1);
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void GetVectorTestPositiveValuesPositiveResult()
        {
            Physics physics = new Physics(new GameObject());
            Vector2 actual = physics.GetVector(new Vector2(2, 2), new Vector2(1, 1));
            Vector2 expected = new Vector2(1, 1);
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void GetVectorTestNegativeValuesNegativeResult()
        {
            Physics physics = new Physics(new GameObject());
            Vector2 actual = physics.GetVector(new Vector2(-2, -2), new Vector2(-1, -1));
            Vector2 expected = new Vector2(-1, -1);
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void GetVectorTestNegativeValuesPositiveResult()
        {
            Physics physics = new Physics(new GameObject());
            Vector2 actual = physics.GetVector(new Vector2(-1, -1), new Vector2(-2, -2));
            Vector2 expected = new Vector2(1, 1);
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void GetVectorTestEqualValues()
        {
            Physics physics = new Physics(new GameObject());
            Vector2 actual = physics.GetVector(new Vector2(1, 1), new Vector2(1, 1));
            Vector2 expected = new Vector2(0, 0);
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void GetVectorTestMaxPositiveValuePostitiveResult()
        {
            Physics physics = new Physics(new GameObject());
            Vector2 actual = physics.GetVector(new Vector2(int.MaxValue, int.MaxValue), new Vector2(0, 0));
            Vector2 expected = new Vector2(int.MaxValue, int.MaxValue);
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void GetVectorTestMaxPositiveValueNegativeResult()
        {
            Physics physics = new Physics(new GameObject());
            Vector2 actual = physics.GetVector(new Vector2(0, 0), new Vector2(int.MaxValue, int.MaxValue));
            Vector2 expected = new Vector2(-int.MaxValue, -int.MaxValue);
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void GetVectorTestMinPositiveValueNegativeResult()
        {
            Physics physics = new Physics(new GameObject());
            Vector2 actual = physics.GetVector(new Vector2(int.MinValue, int.MinValue), new Vector2(0, 0));
            Vector2 expected = new Vector2(-int.MaxValue, -int.MaxValue);
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void GetVectorTestMinPositiveValuePositiveResult()
        {
            Physics physics = new Physics(new GameObject());
            Vector2 actual = physics.GetVector(new Vector2(0, 0), new Vector2(int.MinValue, int.MinValue));
            Vector2 expected = new Vector2(int.MaxValue, int.MaxValue);
            Assert.AreEqual(actual, expected);
        }




        [TestMethod]
        public void PhysicsBreakTestIf()
        {
            Physics physics = new Physics(new GameObject());
            Vector2 actual = physics.Friction(0.05F, new Vector2(1, 1));
            Vector2 expected = new Vector2(-0.05F, -0.05F);
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void PhysicsBreakTestElse()
        {
            Physics physics = new Physics(new GameObject());
            Vector2 actual = physics.Friction(0.05F, new Vector2(0.0025F, 0.0025F));
            Vector2 expected = new Vector2(0, 0);
            //Assert.AreEqual(actual, expected);
        }



        [TestMethod]
        public void UpdateVelocityTestPositiveValues()
        {
            Physics physics = new Physics(new GameObject());
            Vector2 actual = physics.UpdateVelocity(new Vector2(1, 1), new Vector2(1, 1));
            Vector2 expected = new Vector2(2, 2);
            Assert.AreEqual(actual, expected);
        }
         
        [TestMethod]
        public void UpdateVelocityTestNegativeValues()
        {
            Physics physics = new Physics(new GameObject());
            Vector2 actual = physics.UpdateVelocity(new Vector2(-1, -1), new Vector2(-1, -1));
            Vector2 expected = new Vector2(-2, -2);
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void UpdateVelocityTestMaxValue()
        {
            Physics physics = new Physics(new GameObject());
            Vector2 actual = physics.UpdateVelocity(new Vector2(float.MaxValue, float.MaxValue), new Vector2(-1, -1));
            Vector2 expected = new Vector2(float.MaxValue, float.MaxValue);
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void UpdateVelocityTestMinValue()
        {
            Physics physics = new Physics(new GameObject());
            Vector2 actual = physics.UpdateVelocity(new Vector2(float.MinValue, float.MinValue), new Vector2(0, 0));
            Vector2 expected = new Vector2(float.MinValue, float.MinValue);
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void TestingFloatMaxValue()
        {

            float actual = float.MaxValue - 1;
            //actual--;
            float expected = float.MaxValue;

            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void TestingIntMaxValue()
        {

            int actual = int.MaxValue - 1;
            int expected = int.MaxValue;

            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void TestingDoubleMaxValue()
        {

            double actual = double.MaxValue - 1;
            double expected = double.MaxValue;

            Assert.AreEqual(actual, expected);
        }
    }
}