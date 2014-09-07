using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

using Autonomy.Model.Utility.Geometry;

namespace Autonomy.Model.UnitTests
{
    [TestClass]
    public class LineTests
    {
        [TestMethod]
        [TestCategory("Vector2DTests")]
        public void LineIntersection()
        {
            Line horizontalLine = new Line(new Vector2D(0.0f, 1.0f), new Vector2D(2.0f, 1.0f));
            Line verticalLine = new Line(new Vector2D(1.0f, 0.0f), new Vector2D(1.0f, 2.0f));

            Assert.IsTrue(horizontalLine.TestIntersection(verticalLine));
            Assert.IsTrue(horizontalLine.TestIntersection(horizontalLine));
            Assert.IsTrue(verticalLine.TestIntersection(verticalLine));

            Line offsetHorizontalLine = new Line(new Vector2D(0.0f, 3.0f), new Vector2D(2.0f, 3.0f));

            Assert.IsFalse(offsetHorizontalLine.TestIntersection(horizontalLine));

            Line reverseHorizontalLine = new Line(new Vector2D(2.0f, 1.0f), new Vector2D(0.0f, 1.0f));
            Line reverseVerticalLine = new Line(new Vector2D(1.0f, 2.0f), new Vector2D(1.0f, 0.0f));
            Line reverseOffsetHorizontalLine = new Line(new Vector2D(0.0f, 3.0f), new Vector2D(2.0f, 3.0f));

            Assert.IsTrue(reverseHorizontalLine.TestIntersection(reverseVerticalLine));
            Assert.IsTrue(reverseHorizontalLine.TestIntersection(verticalLine));
            Assert.IsTrue(reverseVerticalLine.TestIntersection(verticalLine));
            Assert.IsTrue(reverseVerticalLine.TestIntersection(reverseVerticalLine));

            Assert.IsFalse(reverseHorizontalLine.TestIntersection(reverseOffsetHorizontalLine));
            Assert.IsFalse(reverseHorizontalLine.TestIntersection(offsetHorizontalLine));
        }

        [TestMethod]
        [TestCategory("Vector2DTests")]
        public void LineCircleIntersection()
        {
            BoundaryCircle circleOnOrigin = new BoundaryCircle(1.0f, new Vector2D(0.0f,0.0f));

            Line intersectingLine = new Line(new Vector2D(-1.0f, -1.0f), new Vector2D(1.0f, 1.0f));
            Line nonIntersectingLine = new Line(new Vector2D(-1.0f, 1.0f), new Vector2D(1.0f, 2.0f));
            Line boundedNonIntersectingLine = new Line(new Vector2D(1.1f, 1.1f), new Vector2D(2.6f, 2.6f));
            Line internalLine = new Line(new Vector2D(0.5f, 0.5f), new Vector2D(0.4f, 0.4f));

            Assert.IsTrue(circleOnOrigin.TestIntersection(intersectingLine));
            Assert.IsFalse(circleOnOrigin.TestIntersection(nonIntersectingLine));
            Assert.IsFalse(circleOnOrigin.TestIntersection(boundedNonIntersectingLine));
            Assert.IsTrue(circleOnOrigin.TestIntersection(internalLine));
        }
    }
}
