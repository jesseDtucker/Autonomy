using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

using Autonomy.Model.Utility.Geometry;

namespace Autonomy.Model.UnitTests
{
    [TestClass]
    public class Vector2DTests
    {
        [TestMethod]
        [TestCategory("Vector2DTests")]
        public void VectorAddition()
        {
            Vector2D v1 = new Vector2D(5.0f, 7.0f);
            Vector2D v2 = new Vector2D(4.0f, 2.0f);

            Vector2D result = v1 + v2;

            Assert.IsTrue(AreFloatsEqual(result.X, 9.0f));
            Assert.IsTrue(AreFloatsEqual(result.Y, 9.0f));

            v1 = new Vector2D(1.0f, 2.0f);
            v2 = new Vector2D(-5.0f, 9.0f);

            result = v1 + v2;

            Assert.IsTrue(AreFloatsEqual(result.X, -4.0f));
            Assert.IsTrue(AreFloatsEqual(result.Y, 11.0f));
        }

        [TestMethod]
        [TestCategory("Vector2DTests")]
        public void VectorSubtraction()
        {
            Vector2D v1 = new Vector2D(5.0f, 7.0f);
            Vector2D v2 = new Vector2D(4.0f, 2.0f);

            Vector2D result = v1 - v2;

            Assert.IsTrue(AreFloatsEqual(result.X, 1.0f));
            Assert.IsTrue(AreFloatsEqual(result.Y, 5.0f));

            v1 = new Vector2D(1.0f, 2.0f);
            v2 = new Vector2D(-5.0f, 9.0f);

            result = v1 - v2;

            Assert.IsTrue(AreFloatsEqual(result.X, 6.0f));
            Assert.IsTrue(AreFloatsEqual(result.Y, -7.0f));
        }

        [TestMethod]
        [TestCategory("Vector2DTests")]
        public void VectorScaleUp()
        {
            Vector2D v1 = new Vector2D(3.0f, 4.0f);

            Vector2D result = v1 * 3;

            Assert.IsTrue(AreFloatsEqual(result.X, 9.0f));
            Assert.IsTrue(AreFloatsEqual(result.Y, 12.0f));
            Assert.IsTrue(AreFloatsEqual(result.Magnitude, 15.0f));
        }

        [TestMethod]
        [TestCategory("Vector2DTests")]
        public void VectorScaleDown()
        {
            Vector2D v1 = new Vector2D(3.0f, 4.0f);

            Vector2D result = v1 / 3;

            Assert.IsTrue(AreFloatsEqual(result.X, 1.0f));
            Assert.IsTrue(AreFloatsEqual(result.Y, 1.33f));
            Assert.IsTrue(AreFloatsEqual(result.Magnitude, 1.67f));
        }

        [TestMethod]
        [TestCategory("Vector2DTests")]
        public void GetUnitVector()
        {
            Vector2D v1 = new Vector2D(3.0f, 4.0f);

            Vector2D result = v1.UnitVector;

            Assert.IsTrue(AreFloatsEqual(result.X, 0.6f));
            Assert.IsTrue(AreFloatsEqual(result.Y, 0.8f));
            Assert.IsTrue(AreFloatsEqual(result.Magnitude, 1.0f));
        }

        #region Helpers

        public const float EPSILON = 0.01f;

        protected bool AreFloatsEqual(float f1, float f2)
        {
            float left = f1 - EPSILON;
            float right = f1 + EPSILON;

            if (f2 > left && f2 < right)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
