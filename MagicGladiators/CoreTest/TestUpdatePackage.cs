using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MagicGladiators;
using Microsoft.Xna.Framework;

namespace CoreTest
{
    [TestClass]
    public class TestUpdatePackage
    {
        #region First Constructor
        [TestMethod]
        public void Constructor0_ZeroInput()
        {
            UpdatePackage test = new UpdatePackage(new Vector2(0,0));
            Vector2[] actual = new Vector2[2] {test.position, test.velocity};
            Vector2[] expected = new Vector2[2] {Vector2.Zero, Vector2.Zero};
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Constructor0_OneInput()
        {
            UpdatePackage test = new UpdatePackage(new Vector2(1,1));
            Vector2[] actual = new Vector2[2] {test.position, test.velocity};
            Vector2[] expected = new Vector2[2] {new Vector2(1,1), Vector2.Zero};
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Constructor0_MinusOneInput()
        {
            UpdatePackage test = new UpdatePackage(new Vector2(-1,-1));
            Vector2[] actual = new Vector2[2] {test.position, test.velocity};
            Vector2[] expected = new Vector2[2] {new Vector2(-1, -1), Vector2.Zero};
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Constructor0_MaxInput()
        {
            UpdatePackage test = new UpdatePackage(new Vector2(float.MaxValue,float.MaxValue));
            Vector2[] actual = new Vector2[2] {test.position, test.velocity};
            Vector2[] expected = new Vector2[2] {new Vector2(float.MaxValue, float.MaxValue), Vector2.Zero};
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Constructor0_MinInput()
        {
            UpdatePackage test = new UpdatePackage(new Vector2(float.MinValue,float.MinValue));
            Vector2[] actual = new Vector2[2] {test.position, test.velocity};
            Vector2[] expected = new Vector2[2] {new Vector2(float.MinValue, float.MinValue), Vector2.Zero};
            CollectionAssert.AreEqual(expected, actual);
        }
        #endregion

        #region Second Constructor
        [TestMethod]
        public void Constructor1_ZeroInput()
        {
            UpdatePackage test = new UpdatePackage(new Vector2(0,0), new Vector2(0,0));
            Vector2[] actual = new Vector2[2] {test.position, test.velocity};
            Vector2[] expected = new Vector2[2] {Vector2.Zero, Vector2.Zero};
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Constructor1_OneInput()
        {
            UpdatePackage test = new UpdatePackage(new Vector2(1,1), new Vector2(1,1));
            Vector2[] actual = new Vector2[2] {test.position, test.velocity};
            Vector2[] expected = new Vector2[2] {new Vector2(1,1), new Vector2(1,1)};
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Constructor1_MinusOneInput()
        {
            UpdatePackage test = new UpdatePackage(new Vector2(-1,-1), new Vector2(-1,-1));
            Vector2[] actual = new Vector2[2] {test.position, test.velocity};
            Vector2[] expected = new Vector2[2] {new Vector2(-1,-1), new Vector2(-1,-1)};
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Constructor1_MaxInput()
        {
            UpdatePackage test = new UpdatePackage(new Vector2(float.MaxValue,float.MaxValue), new Vector2(float.MaxValue,float.MaxValue));
            Vector2[] actual = new Vector2[2] {test.position, test.velocity};
            Vector2[] expected = new Vector2[2] {new Vector2(float.MaxValue, float.MaxValue), new Vector2(float.MaxValue, float.MaxValue) };
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Constructor1_MinInput()
        {
            UpdatePackage test = new UpdatePackage(new Vector2(float.MinValue,float.MinValue), new Vector2(float.MinValue,float.MinValue));
            Vector2[] actual = new Vector2[2] {test.position, test.velocity};
            Vector2[] expected = new Vector2[2] {new Vector2(float.MinValue, float.MinValue), new Vector2(float.MinValue, float.MinValue) };
            CollectionAssert.AreEqual(expected, actual);
        }
        #endregion
    }
}
