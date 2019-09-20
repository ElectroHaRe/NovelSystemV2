using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static WFControlLibrary.PointExtension;

namespace BaseWFControlNovelLibraryModulTests
{
    [TestClass]
    public class PointWorkerTests
    {
        #region GetIntersectionPoint
        [TestMethod]
        public void GetIntersectionPoint_p1_00_p2_11_g1_10_g2_01_result_10()
        {
            var p1 = new Point(0, 0);
            var p2 = new Point(1, 1);
            var g1 = new Point(1, 0);
            var g2 = new Point(0, 1);
            var expect = new Point(1, 0);

            var result = GetIntersectionPoint(p1, g1, p2, g2);

            Assert.AreEqual(expect, result);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void GetIntersectionPoint_p1_And_p2_Anything_g1_00_g2_01_result_ArgumentException_ZeroGuideVector()
        {
            var p1 = new Point(0, 0);
            var p2 = new Point(1, 1);
            var g1 = new Point(0, 0);
            var g2 = new Point(0, 1);

            GetIntersectionPoint(p1, g1, p2, g2);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void GetIntersectionPoint_p1_And_p2_Anything_g1_11_g2_11_result_ArgumentException_LinesAreParallel()
        {
            var p1 = new Point(0, 0);
            var p2 = new Point(1, 1);
            var g1 = new Point(1, 1);
            var g2 = new Point(1, 1);

            GetIntersectionPoint(p1, g1, p2, g2);
        }
        #endregion

        #region NormalizeVector
        [TestMethod]
        public void NormalizeVector_vector_010_result_01()
        {
            var vector = new Point(0, 10);
            var expect = new Point(0, 1);

            var temp_result = vector.Normalize();
            var result = temp_result.ToPoint();

            Assert.AreEqual(expect, result);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void NormalizeVector_vector_00_result_ArgumentException_ZeroVector()
        {
            var vector = new Point(0, 0);

            vector.Normalize();
        }
        #endregion

        #region WhereIsMyPoint
        [TestMethod]
        public void WhereIsMyPoint_rect_0022_point_12_result_Down()
        {
            var rect = new Rectangle(0, 0, 2, 2);
            var point = new Point(1, 2);
            var expect = Side.Down;

            var result = point.WhereIAm(rect);

            Assert.AreEqual(expect, result);
        }

        [TestMethod]
        public void WhereIsMyPoint_rect_1122_point_00_result_Left()
        {
            var rect = new Rectangle(1, 1, 2, 2);
            var point = new Point(0, 0);
            var expect = Side.Left;

            var result = point.WhereIAm(rect);

            Assert.AreEqual(expect, result);
        }

        [TestMethod]
        public void WhereIsMyPoint_rect_0022_point_22_result_Right()
        {
            var rect = new Rectangle(0, 0, 2, 2);
            var point = new Point(2, 2);
            var expect = Side.Right;

            var result = point.WhereIAm(rect);

            Assert.AreEqual(expect, result);
        }

        [TestMethod]
        public void WhereIsMyPoint_rect_0122_point_10_result_Up()
        {
            var rect = new Rectangle(0, 1, 2, 2);
            var point = new Point(1, 0);
            var expect = Side.Up;

            var result = point.WhereIAm(rect);

            Assert.AreEqual(expect, result);
        }

        [TestMethod]
        public void WhereIsMyPoint_rect_0000_point_Anything_result_Nowhere()
        {
            var rect = new Rectangle(0, 0, 0, 0);
            var point = new Point(1, 2);
            var expect = Side.Nowhere;

            var result = point.WhereIAm(rect);

            Assert.AreEqual(expect, result);
        }
        #endregion
    }
}
