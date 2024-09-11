// ----------------------------------------------------------------------------------------------------------
// Copyright 2015-2022 - RoboDK Inc. - https://robodk.com/
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------------------------------

// ----------------------------------------------------------------------------------------------------------
// This file (RoboDK.cs) implements the RoboDK API for C#
// This file defines the following classes:
//     Mat: Matrix class, useful pose operations
//     RoboDK: API to interact with RoboDK
//     RoboDK.Item: Any item in the RoboDK station tree
//
// These classes are the objects used to interact with RoboDK and create macros.
// An item is an object in the RoboDK tree (it can be either a robot, an object, a tool, a frame, a program, ...).
// Items can be retrieved from the RoboDK station using the RoboDK() object (such as RoboDK.GetItem() method) 
//
// In this document: pose = transformation matrix = homogeneous matrix = 4x4 matrix
//
// More information about the RoboDK API for Python here:
//     https://robodk.com/doc/en/CsAPI/index.html
//     https://robodk.com/doc/en/RoboDK-API.html
//     https://robodk.com/doc/en/PythonAPI/index.html
//
// More information about RoboDK post processors here:
//     https://robodk.com/help#PostProcessor
//
// Visit the Matrix and Quaternions FAQ for more information about pose/homogeneous transformations
//     http://www.j3d.org/matrix_faq/matrfaq_latest.html
//
// This library includes the mathematics to operate with homogeneous matrices for robotics.
// ----------------------------------------------------------------------------------------------------------

#region Namespaces

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RoboDk.API.Exceptions;

#endregion

namespace RoboDk.API
{
    /// <summary>
    /// Matrix class for robotics.
    /// Simple matrix class for homogeneous operations.
    /// </summary>
    public class Matrix 
    {
        #region Fields

        private int _rows;
        private int _cols;
        private readonly double[,] _mat;

        public Matrix _l;
        public Matrix _u;

        #endregion

        #region Constructors

        /// <summary>
        ///     Matrix class constructor for any size matrix
        /// </summary>
        /// <param name="rows">dimension 1 size (rows)</param>
        /// <param name="cols">dimension 2 size (columns)</param>
        public Matrix(int rows, int cols) // Matrix Class constructor
        {
            _rows = rows;
            _cols = cols;
            _mat = new double[_rows, _cols];
        }


        /// <summary>
        /// Matrix class constructor for a double array. The array will be set as a column matrix.
        /// Example:
        ///     RDK.AddCurve(new Mat(new double[6] {{0,0,0, 0,0,1}}));
        /// </summary>
        /// <param name="point">Column array</param>
        /// <param name="isPose">if isPose is True then convert vector into a 4x4 Pose Matrix.</param>
        public Matrix(double[] point, bool isPose = false)
        {
            if (isPose)
            {
                _cols = 4;
                _rows = 4;
                if (point.GetLength(0) < 16)
                {
                    throw new MatrixException("Invalid array size to create a pose Mat"); //raise Exception('Problems running function');
                }

                // Convert a double array of arrays to a Mat object:
                _mat = new double[_rows, _cols];
                for (var r = 0; r < _rows; r++)
                {
                    for (var c = 0; c < _cols; c++)
                    {
                        _mat[r, c] = point[r + c * 4];
                    }
                }
            }
            else
            {
                _cols = 1;
                _rows = point.GetLength(0);

                // Convert a double array of arrays to a Mat object:
                _mat = new double[_rows, _cols];
                for (int r = 0; r < _rows; r++)
                {
                    _mat[r, 0] = point[r];
                }
            }

        }

        /// <summary>
        ///     Matrix class constructor for a 4x4 homogeneous matrix
        /// </summary>
        /// <param name="nx">Position [0,0]</param>
        /// <param name="ox">Position [0,1]</param>
        /// <param name="ax">Position [0,2]</param>
        /// <param name="tx">Position [0,3]</param>
        /// <param name="ny">Position [1,0]</param>
        /// <param name="oy">Position [1,1]</param>
        /// <param name="ay">Position [1,2]</param>
        /// <param name="ty">Position [1,3]</param>
        /// <param name="nz">Position [2,0]</param>
        /// <param name="oz">Position [2,1]</param>
        /// <param name="az">Position [2,2]</param>
        /// <param name="tz">Position [2,3]</param>
        public Matrix(double nx, double ox, double ax, double tx, double ny, double oy, double ay, double ty, double nz,
            double oz, double az, double tz) // Matrix Class constructor
        {
            _rows = 4;
            _cols = 4;
            _mat = new double[_rows, _cols];
            _mat[0, 0] = nx;
            _mat[1, 0] = ny;
            _mat[2, 0] = nz;
            _mat[0, 1] = ox;
            _mat[1, 1] = oy;
            _mat[2, 1] = oz;
            _mat[0, 2] = ax;
            _mat[1, 2] = ay;
            _mat[2, 2] = az;
            _mat[0, 3] = tx;
            _mat[1, 3] = ty;
            _mat[2, 3] = tz;
            _mat[3, 0] = 0.0;
            _mat[3, 1] = 0.0;
            _mat[3, 2] = 0.0;
            _mat[3, 3] = 1.0;
        }

        /// <summary>
        ///     Matrix class constructor for a 4x4 homogeneous matrix as a copy from another matrix
        /// </summary>
        public Matrix(Matrix pose)
        {
            _rows = pose._rows;
            _cols = pose._cols;
            _mat = new double[_rows, _cols];
            for (var i = 0; i < _rows; i++)
            for (var j = 0; j < _cols; j++)
                _mat[i, j] = pose[i, j];
        }

        /// <summary>
        ///     Matrix class constructor for a 4x1 vector [x,y,z,1]
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <param name="z">z coordinate</param>
        public Matrix(double x, double y, double z)
        {
            _rows = 4;
            _cols = 1;
            _mat = new double[_rows, _cols];
            _mat[0, 0] = x;
            _mat[1, 0] = y;
            _mat[2, 0] = z;
            _mat[3, 0] = 1.0;
        }

        /// <summary>
        /// Matrix class constructor for a double array of arrays or a single column array (list of points)
        /// Example:
        ///     RDK.AddCurve(new Mat(new double[4, 6] {{0,0,0, 0,0,1}, { 500, 0, 0, 0, 0, 1 }, { 500, 500, 0, 0, 0, 1 }, { 0, 0, 0, 0, 0, 1 } }));
        ///     RDK.AddPoints(new Mat(new double[6] {{0,0,0, 0,0,1}}));
        /// </summary>
        /// <param name="pointList">List of points (array of array of doubles)</param>
        public Matrix(double[,] pointList)
        {
            if (pointList.Rank == 2)
            {
                _cols = pointList.GetLength(0);
                _rows = pointList.GetLength(1);

                // Convert a double array of arrays to a Mat object:
                _mat = new double[_rows, _cols];
                for (int c = 0; c < _cols; c++)
                {
                    for (int r = 0; r < _rows; r++)
                    {
                        _mat[r, c] = pointList[c, r];
                    }
                }
            } else if (pointList.Rank == 1)
            {
                _cols = 1;
                _rows = pointList.GetLength(0);

                // Convert a double array of arrays to a Mat object:
                _mat = new double[_rows, _cols];
                for (int r = 0; r < _rows; r++)
                {
                    _mat[r, 1] = pointList[0,r];
                }
            } else
            {
                throw new MatrixException("Invalid rank size. Provide a 1-dimensional or 2-dimensional double array to create a Mat.");
            }
        }

        #endregion

        #region Properties

        public int Rows => _rows;

        public int Cols => _cols;

        public double this[int iRow, int iCol] // Access this matrix as a 2D array
        {
            get { return _mat[iRow, iCol]; }
            set { _mat[iRow, iCol] = value; }
        }

        #endregion

        #region Public Methods

        //----------------------------------------------------
        //--------     Generic matrix usage    ---------------
        /// <summary>
        ///     Return a translation matrix
        ///     |  1   0   0   X |
        ///     transl(X,Y,Z) = |  0   1   0   Y |
        ///     |  0   0   1   Z |
        ///     |  0   0   0   1 |
        /// </summary>
        /// <param name="x">translation along X (mm)</param>
        /// <param name="y">translation along Y (mm)</param>
        /// <param name="z">translation along Z (mm)</param>
        /// <returns></returns>
        public static Matrix Transl(double x, double y, double z)
        {
            var mat = IdentityMatrix(4, 4);
            mat.SetPos(x, y, z);
            return mat;
        }

        /// <summary>
        ///     Return a X-axis rotation matrix
        ///     |  1  0        0        0 |
        ///     rotx(rx) = |  0  cos(rx) -sin(rx)  0 |
        ///     |  0  sin(rx)  cos(rx)  0 |
        ///     |  0  0        0        1 |
        /// </summary>
        /// <param name="rx">rotation around X axis (in radians)</param>
        /// <returns></returns>
        public static Matrix Rotx(double rx)
        {
            var cx = Math.Cos(rx);
            var sx = Math.Sin(rx);
            return new Matrix(1, 0, 0, 0, 0, cx, -sx, 0, 0, sx, cx, 0);
        }

        /// <summary>
        ///     Return a Y-axis rotation matrix
        ///     |  cos(ry)  0   sin(ry)  0 |
        ///     roty(ry) = |  0        1   0        0 |
        ///     | -sin(ry)  0   cos(ry)  0 |
        ///     |  0        0   0        1 |
        /// </summary>
        /// <param name="ry">rotation around Y axis (in radians)</param>
        /// <returns></returns>
        public static Matrix Roty(double ry)
        {
            var cy = Math.Cos(ry);
            var sy = Math.Sin(ry);
            return new Matrix(cy, 0, sy, 0, 0, 1, 0, 0, -sy, 0, cy, 0);
        }

        /// <summary>
        ///     Return a Z-axis rotation matrix
        ///     |  cos(rz)  -sin(rz)   0   0 |
        ///     rotz(rx) = |  sin(rz)   cos(rz)   0   0 |
        ///     |  0         0         1   0 |
        ///     |  0         0         0   1 |
        /// </summary>
        /// <param name="rz">rotation around Z axis (in radians)</param>
        /// <returns></returns>
        public static Matrix Rotz(double rz)
        {
            var cz = Math.Cos(rz);
            var sz = Math.Sin(rz);
            return new Matrix(cz, -sz, 0, 0, sz, cz, 0, 0, 0, 0, 1, 0);
        }

        /// <summary>
        /// Matrix class constructor for a 3x3 homogeneous matrix
        /// </summary>
        /// <param name="nx"></param>
        /// <param name="ox"></param>
        /// <param name="ax"></param>
        /// <param name="ny"></param>
        /// <param name="oy"></param>
        /// <param name="ay"></param>
        /// <param name="nz"></param>
        /// <param name="oz"></param>
        /// <param name="az"></param>
        public Matrix(double nx, double ox, double ax, double ny, double oy, double ay, double nz, double oz, double az)
        {
            _rows = 3;
            _cols = 3;
            _mat = new double[_rows, _cols];
            _mat[0, 0] = nx; _mat[1, 0] = ny; _mat[2, 0] = nz;
            _mat[0, 1] = ox; _mat[1, 1] = oy; _mat[2, 1] = oz;
            _mat[0, 2] = ax; _mat[1, 2] = ay; _mat[2, 2] = az;
        }

        /// <summary>
        /// Returns the sub 3x3 matrix that represents the pose rotation
        /// </summary>
        /// <returns></returns>
        public Matrix Rot3X3()
        {
            if (!IsHomogeneous())
            {
                throw new MatrixException("It is not possible to retrieve a sub 3x3 rotation mat"); //raise Exception('Problems running function');
            }

            return new Matrix(_mat[0, 0], _mat[0, 1], _mat[0, 2], 
                           _mat[1, 0], _mat[1, 1], _mat[1, 2], 
                           _mat[2, 0], _mat[2, 1], _mat[2, 2]);
        }

        /// <summary>
        /// Check if it is a Homogeneous Identity matrix
        /// </summary>
        /// <returns></returns>
        public bool IsIdentity()
        {
            if (this.Cols != 4 || this.Rows != this.Cols)
                return false;

            for (int i = 0; i < Rows; i++)
            {
                if (_mat[i, i] != 1.0)
                    return false;

            }
            if (_mat[0, 3] != 0.0 || _mat[1, 3] != 0.0 || _mat[2, 3] != 0.0)
                return false;

            return true;
        }


        //----------------------------------------------------
        //------ Pose to xyzrpw and xyzrpw to pose------------
        /// <summary>
        ///     Calculates the equivalent position and euler angles ([x,y,z,r,p,w] vector) of the given pose
        ///     Note: transl(x,y,z)*rotz(w*pi/180)*roty(p*pi/180)*rotx(r*pi/180)
        ///     See also: FromXYZRPW()
        /// </summary>
        /// <returns>XYZWPR translation and rotation in mm and degrees</returns>
        public double[] ToXYZRPW()
        {
            var xyzwpr = new double[6];
            var x = _mat[0, 3];
            var y = _mat[1, 3];
            var z = _mat[2, 3];
            double w, p, r;
            if (_mat[2, 0] > 1.0 - 1e-6)
            {
                p = -Math.PI * 0.5;
                r = 0;
                w = Math.Atan2(-_mat[1, 2], _mat[1, 1]);
            }
            else if (_mat[2, 0] < -1.0 + 1e-6)
            {
                p = 0.5 * Math.PI;
                r = 0;
                w = Math.Atan2(_mat[1, 2], _mat[1, 1]);
            }
            else
            {
                p = Math.Atan2(-_mat[2, 0], Math.Sqrt(_mat[0, 0] * _mat[0, 0] + _mat[1, 0] * _mat[1, 0]));
                w = Math.Atan2(_mat[1, 0], _mat[0, 0]);
                r = Math.Atan2(_mat[2, 1], _mat[2, 2]);
            }
            xyzwpr[0] = x;
            xyzwpr[1] = y;
            xyzwpr[2] = z;
            xyzwpr[3] = r * 180.0 / Math.PI;
            xyzwpr[4] = p * 180.0 / Math.PI;
            xyzwpr[5] = w * 180.0 / Math.PI;
            return xyzwpr;
        }

        /// <summary>
        ///     Calculates the pose from the position and euler angles ([x,y,z,r,p,w] vector)
        ///     The result is the same as calling: H = transl(x,y,z)*rotz(w*pi/180)*roty(p*pi/180)*rotx(r*pi/180)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="r"></param>
        /// <param name="p"></param>
        /// <param name="w"></param>
        /// <returns>Homogeneous matrix (4x4)</returns>
        public static Matrix FromXYZRPW(double x, double y, double z, double r, double p, double w)
        {
            var a = r * Math.PI / 180.0;
            var b = p * Math.PI / 180.0;
            var c = w * Math.PI / 180.0;
            var ca = Math.Cos(a);
            var sa = Math.Sin(a);
            var cb = Math.Cos(b);
            var sb = Math.Sin(b);
            var cc = Math.Cos(c);
            var sc = Math.Sin(c);
            return new Matrix(cb * cc, cc * sa * sb - ca * sc, sa * sc + ca * cc * sb, x, cb * sc, ca * cc + sa * sb * sc,
                ca * sb * sc - cc * sa, y, -sb, cb * sa, ca * cb, z);
        }

        /// <summary>
        ///     Calculates the pose from the position and euler angles ([x,y,z,r,p,w] vector)
        /// </summary>
        /// <param name="xyzwpr"></param>
        /// <returns>Homogeneous matrix (4x4)</returns>
        public static Matrix FromXyzrpw(double[] xyzwpr)
        {
            if (xyzwpr.Length < 6)
                return null;
            return FromXYZRPW(xyzwpr[0], xyzwpr[1], xyzwpr[2], xyzwpr[3], xyzwpr[4], xyzwpr[5]);
        }

        /// <summary>
        ///     Calculates the pose from the position and euler angles ([x,y,z,rx,ry,rz] array)
        ///     The result is the same as calling: H = transl(x,y,z)*rotx(rx*pi/180)*roty(ry*pi/180)*rotz(rz*pi/180)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="rx"></param>
        /// <param name="ry"></param>
        /// <param name="rz"></param>
        /// <returns>Homogeneous matrix (4x4)</returns>
        public static Matrix FromTxyzRxyz(double x, double y, double z, double rx, double ry, double rz)
        {
            var a = rx * Math.PI / 180.0;
            var b = ry * Math.PI / 180.0;
            var c = rz * Math.PI / 180.0;
            var crx = Math.Cos(a);
            var srx = Math.Sin(a);
            var cry = Math.Cos(b);
            var sry = Math.Sin(b);
            var crz = Math.Cos(c);
            var srz = Math.Sin(c);
            return new Matrix(cry * crz, -cry * srz, sry, x, crx * srz + crz * srx * sry, crx * crz - srx * sry * srz,
                -cry * srx, y, srx * srz - crx * crz * sry, crz * srx + crx * sry * srz, crx * cry, z);
        }

        /// <summary>
        ///     Calculates the pose from the position and euler angles ([x,y,z,rx,ry,rz] array)
        ///     The result is the same as calling: H = transl(x,y,z)*rotx(rx*pi/180)*roty(ry*pi/180)*rotz(rz*pi/180)
        /// </summary>
        /// <returns>Homogeneous matrix (4x4)</returns>
        public static Matrix FromTxyzRxyz(double[] xyzwpr)
        {
            if (xyzwpr.Length < 6)
                return null;
            return FromTxyzRxyz(xyzwpr[0], xyzwpr[1], xyzwpr[2], xyzwpr[3], xyzwpr[4], xyzwpr[5]);
        }

        /// <summary>
        ///     Calculates the equivalent position and euler angles ([x,y,z,rx,ry,rz] array) of a pose
        ///     Note: Pose = transl(x,y,z)*rotx(rx*pi/180)*roty(ry*pi/180)*rotz(rz*pi/180)
        ///     See also: FromTxyzRxyz()
        /// </summary>
        /// <returns>XYZWPR translation and rotation in mm and degrees</returns>
        public double[] ToTxyzRxyz()
        {
            var xyzwpr = new double[6];
            var x = _mat[0, 3];
            var y = _mat[1, 3];
            var z = _mat[2, 3];
            double rx1 = 0;
            double ry1 = 0;
            double rz1 = 0;


            var a = _mat[0, 0];
            var b = _mat[0, 1];
            var c = _mat[0, 2];
            var d = _mat[1, 2];
            var e = _mat[2, 2];

            if (c == 1)
            {
                ry1 = 0.5 * Math.PI;
                rx1 = 0;
                rz1 = Math.Atan2(_mat[1, 0], _mat[1, 1]);
            }
            else if (c == -1)
            {
                ry1 = -Math.PI / 2;
                rx1 = 0;
                rz1 = Math.Atan2(_mat[1, 0], _mat[1, 1]);
            }
            else
            {
                var sy = c;
                var cy1 = +Math.Sqrt(1 - sy * sy);

                var sx1 = -d / cy1;
                var cx1 = e / cy1;

                var sz1 = -b / cy1;
                var cz1 = a / cy1;

                rx1 = Math.Atan2(sx1, cx1);
                ry1 = Math.Atan2(sy, cy1);
                rz1 = Math.Atan2(sz1, cz1);
            }

            xyzwpr[0] = x;
            xyzwpr[1] = y;
            xyzwpr[2] = z;
            xyzwpr[3] = rx1 * 180.0 / Math.PI;
            xyzwpr[4] = ry1 * 180.0 / Math.PI;
            xyzwpr[5] = rz1 * 180.0 / Math.PI;
            return xyzwpr;
        }

		/// <summary>
		/// Calculates the equivalent position and euler angles ([x,y,z,u,v,w] vector) of the given pose in Universal Robots format
		/// The uvw values are the rotation vector
		/// </summary>
		/// <returns>XYZWPR translation and rotation in mm and radians</returns>
        public double[] ToUr()
        {
			double[] xyzwpr = new double[6];
			double x = _mat[0, 3];
			double y = _mat[1, 3];
			double z = _mat[2, 3];
			double rx = _mat[2, 1] - _mat[1, 2];
			double ry = _mat[0, 2] - _mat[2, 0];
			double rz = _mat[1, 0] - _mat[0, 1];
			double angle = Math.Acos(Math.Min(Math.Max(  (_mat[0, 0] + _mat[1, 1] + _mat[2, 2] - 1) * 0.5, -1.0), 1.0));
			if (angle < 1e-8){
				rx = 0;
				ry = 0;
				rz = 0;		
			} else {
				double sinAngle = Math.Sin(angle);
				double rxyzNormSq = rx*rx + ry*ry + rz*rz;
				if (Math.Abs(sinAngle) < 1e-8 || rxyzNormSq < 1e-4){
					double mx = 0;
					if (_mat[0, 0] > _mat[1, 1] && _mat[0, 0] > _mat[2, 2]){
						rx = _mat[0, 0] + 1;
						ry = _mat[1, 0];
						rz = _mat[2, 0];
						mx = _mat[0, 0];
					} else if (_mat[1, 1] > _mat[2, 2]) {
						rx = _mat[0, 1];
						ry = _mat[1, 1] + 1;
						rz = _mat[2, 1];
						mx = _mat[1, 1];
					} else {
						rx = _mat[0, 2];
						ry = _mat[1, 2];
						rz = _mat[2, 2] + 1;
						mx = _mat[2, 2];
					}
					double multFactor = angle / (Math.Sqrt(Math.Max(0, 2 * (1 + mx))));				
				} else {
					double multFactor = angle / Math.Sqrt(rxyzNormSq);
					rx = rx * multFactor;
					ry = ry * multFactor;
					rz = rz * multFactor;				
				}
			}
			xyzwpr[0] = x;
			xyzwpr[1] = y;
			xyzwpr[2] = z;
			xyzwpr[3] = rx;
			xyzwpr[4] = ry;
			xyzwpr[5] = rz;
			return xyzwpr;
        }

		/// <summary>
		/// Calculates the pose from the position and uvw rotation angles ([x,y,z,u,v,w] vector)
		/// </summary>
		/// <param name="xyzwpr">The position and euler angles array</param>
		/// <returns>Homogeneous matrix (4x4)</returns>
        public static Matrix FromUr(double[] xyzwpr)
        {
			double x = xyzwpr[0];
			double y = xyzwpr[1];
			double z = xyzwpr[2];
			double w = xyzwpr[3];
			double p = xyzwpr[4];
			double r = xyzwpr[5];
			double angle = Math.Sqrt(w * w + p * p + r * r);
			Matrix pose = Matrix.Identity4X4(); // Mat.IdentityMatrix(4, 4);
			if (angle < 1e-6) {
				// no rotation
			} else {
				double cosang = Math.Cos(0.5 * angle);			
				double ratio = Math.Sin(0.5 * angle) / angle;
				double q2 = w * ratio;
				double q3 = p * ratio;
				double q4 = r * ratio;
				double[] q1234 = new double[] { cosang, q2, q3, q4 };
				pose = Matrix.FromQuaternion(q1234);
			}
			pose.SetPos(x, y, z);
			return pose;
        }

        /// <summary>
        ///     Converts a matrix into a one-dimensional array of doubles
        /// </summary>
        /// <returns>one-dimensional array</returns>
        public double[] ToDoubles()
        {
            var cnt = 0;
            var array = new double[_rows * _cols];
            for (var j = 0; j < _cols; j++)
            for (var i = 0; i < _rows; i++)
            {
                array[cnt] = _mat[i, j];
                cnt = cnt + 1;
            }
            return array;
        }

        /// <summary>
        ///     Check if the matrix is square
        /// </summary>
        public bool IsSquare()
        {
            return _rows == _cols;
        }

        public bool Is4X4()
        {
            if (_cols != 4 || _rows != 4)
                return false;
            return true;
        }

        /// <summary>
        ///     Check if the matrix is homogeneous (4x4)
        /// </summary>
        public bool IsHomogeneous()
        {
            if (!Is4X4())
                return false;
            return true;
            /*
        test = self[0:3,0:3];
        test = test*test.tr()
        test[0,0] = test[0,0] - 1.0
        test[1,1] = test[1,1] - 1.0
        test[2,2] = test[2,2] - 1.0
        zero = 0.0
        for x in range(3):
            for y in range(3):
                zero = zero + abs(test[x,y])
        if zero > 1e-4:
            return False
        return True
        */
        }

        /// <summary>
        ///     Returns the inverse of a homogeneous matrix (4x4 matrix)
        /// </summary>
        /// <returns>Homogeneous matrix (4x4)</returns>
        public Matrix Inv()
        {
            if (!IsHomogeneous())
                throw new MatrixException("Can't invert a non-homogeneous matrix");
            var xyz = Pos();
            var matXyz = new Matrix(xyz[0], xyz[1], xyz[2]);
            var hinv = Duplicate();
            hinv.SetPos(0, 0, 0);
            hinv = hinv.Transpose();
            var newPos = Rotate(hinv, matXyz);
            hinv[0, 3] = -newPos[0, 0];
            hinv[1, 3] = -newPos[1, 0];
            hinv[2, 3] = -newPos[2, 0];
            return hinv;
        }

        /// <summary>
        ///     Rotate a vector given a matrix (rotation matrix or homogeneous matrix)
        /// </summary>
        /// <param name="pose">4x4 homogeneous matrix or 3x3 rotation matrix</param>
        /// <param name="vector">4x1 or 3x1 vector</param>
        /// <returns></returns>
        public static Matrix Rotate(Matrix pose, Matrix vector)
        {
            if (pose._cols < 3 || pose._rows < 3 || vector._rows < 3)
                throw new MatrixException("Invalid matrix size");
            var pose3X3 = pose.Duplicate();
            var vector3 = vector.Duplicate();
            pose3X3._rows = 3;
            pose3X3._cols = 3;
            vector3._rows = 3;
            return pose3X3 * vector3;
        }

        /// <summary>
        ///     Returns the XYZ position of the Homogeneous matrix
        /// </summary>
        /// <returns>XYZ position</returns>
        public double[] Pos()
        {
            if (!Is4X4())
                return null;
            var xyz = new double[3];
            xyz[0] = _mat[0, 3];
            xyz[1] = _mat[1, 3];
            xyz[2] = _mat[2, 3];
            return xyz;
        }

        /// <summary>
        ///     Sets the 4x4 position of the Homogeneous matrix
        /// </summary>
        /// <param name="xyz">XYZ position</param>
        public void SetPos(double[] xyz)
        {
            if (!Is4X4() || xyz.Length < 3)
                return;
            _mat[0, 3] = xyz[0];
            _mat[1, 3] = xyz[1];
            _mat[2, 3] = xyz[2];
        }

        /// <summary>
        ///     Sets the 4x4 position of the Homogeneous matrix
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="z">Z position</param>
        public void SetPos(double x, double y, double z)
        {
            if (!Is4X4())
                return;
            _mat[0, 3] = x;
            _mat[1, 3] = y;
            _mat[2, 3] = z;
        }

        /// <summary>
        /// Returns the VX orientation vector of the Homogeneous matrix
        /// </summary>
        /// <returns>VX orientation vector</returns>
        public double[] Vx()
        {
            if (!Is4X4())
            {
                return null;
            }
            double[] xyz = new double[3];
            xyz[0] = _mat[0, 0]; xyz[1] = _mat[1, 0]; xyz[2] = _mat[2, 0];
            return xyz;
        }

        /// <summary>
        /// Sets the VX orientation vector of the Homogeneous matrix
        /// </summary>
        /// <param name="xyz">VX orientation vector</param>
        public void SetVx(double[] xyz)
        {
            if (!Is4X4() || xyz.Length < 3)
            {
                return;
            }
            _mat[0, 0] = xyz[0]; _mat[1, 0] = xyz[1]; _mat[2, 0] = xyz[2];
        }
        /// <summary>
        /// Returns the VY orientation vector of the Homogeneous matrix
        /// </summary>
        /// <returns>VY orientation vector</returns>
        public double[] Vy()
        {
            if (!Is4X4())
            {
                return null;
            }
            double[] xyz = new double[3];
            xyz[0] = _mat[0, 1]; xyz[1] = _mat[1, 1]; xyz[2] = _mat[2, 1];
            return xyz;
        }

        /// <summary>
        /// Sets the VY orientation vector of the Homogeneous matrix
        /// </summary>
        /// <param name="xyz">VY orientation vector</param>
        public void SetVy(double[] xyz)
        {
            if (!Is4X4() || xyz.Length < 3)
            {
                return;
            }
            _mat[0, 1] = xyz[0]; _mat[1, 1] = xyz[1]; _mat[2, 1] = xyz[2];
        }
        /// <summary>
        /// Returns the VZ orientation vector of the Homogeneous matrix
        /// </summary>
        /// <returns>VZ orientation vector</returns>
        public double[] Vz()
        {
            if (!Is4X4())
            {
                return null;
            }
            double[] xyz = new double[3];
            xyz[0] = _mat[0, 2]; xyz[1] = _mat[1, 2]; xyz[2] = _mat[2, 2];
            return xyz;
        }

        /// <summary>
        /// Sets the VZ orientation vector of the Homogeneous matrix
        /// </summary>
        /// <param name="xyz">VZ orientation vector</param>
        public void SetVz(double[] xyz)
        {
            if (!Is4X4() || xyz.Length < 3)
            {
                return;
            }
            _mat[0, 2] = xyz[0]; _mat[1, 2] = xyz[1]; _mat[2, 2] = xyz[2];
        }


        public Matrix GetCol(int k)
        {
            var m = new Matrix(_rows, 1);
            for (var i = 0; i < _rows; i++)
                m[i, 0] = _mat[i, k];
            return m;
        }

        public void SetCol(Matrix v, int k)
        {
            for (var i = 0; i < _rows; i++)
                _mat[i, k] = v[i, 0];
        }

        public Matrix Duplicate() // Function returns the copy of this matrix
        {
            var matrix = new Matrix(_rows, _cols);
            for (var i = 0; i < _rows; i++)
            for (var j = 0; j < _cols; j++)
                matrix[i, j] = _mat[i, j];
            return matrix;
        }

        public static Matrix ZeroMatrix(int iRows, int iCols) // Function generates the zero matrix
        {
            var matrix = new Matrix(iRows, iCols);
            for (var i = 0; i < iRows; i++)
            for (var j = 0; j < iCols; j++)
                matrix[i, j] = 0;
            return matrix;
        }

        public static Matrix IdentityMatrix(int iRows, int iCols) // Function generates the identity matrix
        {
            var matrix = ZeroMatrix(iRows, iCols);
            for (var i = 0; i < Math.Min(iRows, iCols); i++)
                matrix[i, i] = 1;
            return matrix;
        }

        /// <summary>
        ///     Returns an identity 4x4 matrix (homogeneous matrix)
        /// </summary>
        /// <returns></returns>
        public static Matrix Identity4X4()
        {
            return IdentityMatrix(4, 4);
        }

        /*
    public static Mat Parse(string ps)                        // Function parses the matrix from string
    {
        string s = NormalizeMatrixString(ps);
        string[] rows = Regex.Split(s, "\r\n");
        string[] nums = rows[0].Split(' ');
        Mat matrix = new Mat(rows.Length, nums.Length);
        try
        {
            for (int i = 0; i < rows.Length; i++)
            {
                nums = rows[i].Split(' ');
                for (int j = 0; j < nums.Length; j++) matrix[i, j] = double.Parse(nums[j]);
            }
        }
        catch (FormatException exc) { throw new MatException("Wrong input format!"); }
        return matrix;
    }*/

        public override string ToString() // Function returns matrix as a string
        {
            return ToString(true);
        }

        /// <summary>
        /// Returns the Matrix string (XYZWPR using the functino ToTxyzRxyz() or matrix values)
        /// </summary>
        /// <param name="stringAsXyzabc"></param>
        /// <returns></returns>
        public string ToString(bool stringAsXyzabc = true)                           // Function returns matrix as a string
        {
            string s = "";
            stringAsXyzabc = stringAsXyzabc && IsHomogeneous();
            if (stringAsXyzabc)
            {
                var letter = new List<string>() { "X=", "Y=", "Z=", "Rx=", "Ry=", "Rz=" };
                var units = new List<string>() { "mm", "mm", "mm", "deg", "deg", "deg" };

                var values = this.ToTxyzRxyz();
                for (int j = 0; j < 6; j++) s += letter[j] + String.Format("{0,6:0.000}", values[j]) + " " + units[j] + " , ";
                s = s.Remove(s.Length - 3);
            }
            else
            {
                for (var i = 0; i < _rows; i++)
                {
                    for (var j = 0; j < _cols; j++)
                        s += string.Format("{0,5:0.00}", _mat[i, j]) + " ";
                    s += "\r\n";
                }
            }
            return s;
        }

        /// <summary>
        ///     Transpose a matrix
        /// </summary>
        /// <returns></returns>
        public Matrix Transpose()
        {
            return Transpose(this);
        }

        public static Matrix Transpose(Matrix m) // Matrix transpose, for any rectangular matrix
        {
            var t = new Matrix(m._cols, m._rows);
            for (var i = 0; i < m._rows; i++)
            for (var j = 0; j < m._cols; j++)
                t[j, i] = m[i, j];
            return t;
        }

        public static Matrix MultiplyMatSimple(Matrix m1, Matrix m2)
        {
            if (m1._cols != m2._rows)
                throw new MatrixException("Wrong dimensions of matrix!");

            var result = ZeroMatrix(m1._rows, m2._cols);
            for (var i = 0; i < result._rows; i++)
            for (var j = 0; j < result._cols; j++)
            for (var k = 0; k < m1._cols; k++)
                result[i, j] += m1[i, k] * m2[k, j];
            return result;
        }

        public static string NormalizeMatrixString(string matStr) // From Andy - thank you! :)
        {
            // Remove any multiple spaces
            while (matStr.IndexOf("  ") != -1)
                matStr = matStr.Replace("  ", " ");

            // Remove any spaces before or after newlines
            matStr = matStr.Replace(" \r\n", "\r\n");
            matStr = matStr.Replace("\r\n ", "\r\n");

            // If the data ends in a newline, remove the trailing newline.
            // Make it easier by first replacing \r\n’s with |’s then
            // restore the |’s with \r\n’s
            matStr = matStr.Replace("\r\n", "|");
            while (matStr.LastIndexOf("|") == matStr.Length - 1)
                matStr = matStr.Substring(0, matStr.Length - 1);

            matStr = matStr.Replace("|", "\r\n");
            return matStr.Trim();
        }


        // Operators
        public static Matrix operator -(Matrix m)
        {
            return Multiply(-1, m);
        }

        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            return Add(m1, m2);
        }

        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            return Add(m1, -m2);
        }

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            return Multiply(m1, m2);
        }

        public static Matrix operator *(double n, Matrix m)
        {
            return Multiply(n, m);
        }

        public static double[] operator *(Matrix m, double[] n)
        {
            return Multiply(m, n);
        }

        /// <summary>
        /// Returns the norm of a 3D vector
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        static public double Norm(double[] p)
        {
            return Math.Sqrt(p[0] * p[0] + p[1] * p[1] + p[2] * p[2]);
        }

        /// <summary>
        /// Returns the unitary vector
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        static public double[] Normalize3(double[] p)
        {
            double norminv = 1.0 / Norm(p);
            return new double[] { p[0] * norminv, p[1] * norminv, p[2] * norminv };
        }

        /// <summary>
        /// Returns the cross product of two 3D vectors
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        static public double[] Cross(double[] a, double[] b)
        {
            return new double[] {a[1]* b[2] - a[2]* b[1],
          a[2]* b[0] - a[0]* b[2],
           a[0]* b[1] - a[1]* b[0]};
        }
        /// <summary>
        /// Returns the dot product of two 3D vectors
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        static public double Dot(double[] a, double[] b)
        {
            return a[0] * b[0] + a[1] * b[1] + a[2] * b[2];
        }

        /// <summary>
        /// Returns the angle in radians of two 3D vectors
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        static public double Angle3(double[] a, double[] b)
        {
            return Math.Acos(Dot(Normalize3(a), Normalize3(b)));
        }

        /// <summary>
        /// Convert a point XYZ and IJK vector (Z axis) to a pose given a hint for the Y axis
        /// </summary>
        /// <param name="point"></param>
        /// <param name="zaxis"></param>
        /// <param name="reference"></param>
        /// <param name="yaxisHint"></param>
        /// <returns></returns>
        static public Matrix xyzijk_2_pose(double[] point, double[] zaxis, double[] yaxisHint = null)
        {
            Matrix pose = Matrix.Identity4X4();
            if (yaxisHint == null)
            {
                yaxisHint = new double[] { 0, 0, 1 };
            }
            pose.SetPos(point);
            pose.SetVz(zaxis);
            if (Matrix.Angle3(zaxis, yaxisHint) < 2 * Math.PI / 180)
            {
                yaxisHint = new double[] { 0, 1, 1 };
            }
            double[] xaxis = Matrix.Normalize3(Matrix.Cross(yaxisHint, zaxis));
            double[] yaxis = Matrix.Cross(zaxis, xaxis);
            pose.SetVx(xaxis);
            pose.SetVy(yaxis);
            return pose;
        }

        public Matrix ConcatenateHorizontal(Matrix matrix)
        {
            return ConcatenateHorizontal(this, matrix);
        }

        public Matrix ConcatenateVertical(Matrix matrix)
        {
            return ConcatenateVertical(this, matrix);
        }

        public Matrix TranslationPose()
        {
            double[] pos = Pos();
            return Transl(pos[0], pos[1], pos[2]);
        }

        public Matrix RotationPose()
        {
            Matrix result = new Matrix(this);
            result.SetPos(0.0, 0.0, 0.0);
            return result;
        }

        public void SaveCsv(string filename)
        {
            Transpose().SaveMat(filename);
        }

        public void SaveMat(string filename, string separator = ",")
        {
            using (var writer = new StreamWriter(filename))
            {
                for (int row = 0; row < _rows; row++)
                {
                    for (int col = 0; col < _cols; col++)
                    {
                        writer.Write("{0:0.000000}{1}", this[row, col], separator);
                    }
                    writer.WriteLine();
                }
            }
        }

        /// <summary>
        ///     Calculates a relative target with respect to the tool coordinates.
        ///     This procedure has exactly the same behavior as ABB's RelTool instruction.
        /// </summary>
        /// <param name="targetPose">Reference pose</param>
        /// <param name="x">Translation along the Tool X axis (mm)</param>
        /// <param name="y">Translation along the Tool Y axis (mm)</param>
        /// <param name="z">Translation along the Tool Z axis (mm)</param>
        /// <param name="rx">Rotation around the Tool X axis (deg) (optional)</param>
        /// <param name="ry">Rotation around the Tool Y axis (deg) (optional)</param>
        /// <param name="rz">Rotation around the Tool Z axis (deg) (optional)</param>
        /// <returns>Returns relative target</returns>
        public Matrix RelTool(Matrix targetPose, double x, double y, double z, double rx = 0.0, double ry = 0.0, double rz = 0.0)
        {
            return targetPose * Transl(x, y, z) * Rotx(rx * Math.PI / 180) * Roty(ry * Math.PI / 180) * Rotz(rz * Math.PI / 180);
        }

        /// <summary>
        ///     Calculates a relative target with respect to the reference frame coordinates.
        /// </summary>
        /// <param name="targetPose">Reference pose</param>
        /// <param name="x">Translation along the Tool X axis (mm)</param>
        /// <param name="y">Translation along the Tool Y axis (mm)</param>
        /// <param name="z">Translation along the Tool Z axis (mm)</param>
        /// <param name="rx">Rotation around the Tool X axis (deg) (optional)</param>
        /// <param name="ry">Rotation around the Tool Y axis (deg) (optional)</param>
        /// <param name="rz">Rotation around the Tool Z axis (deg) (optional)</param>
        /// <returns>Returns relative target</returns>
        public Matrix Offset(Matrix targetPose, double x, double y, double z, double rx = 0.0, double ry = 0.0, double rz = 0.0)
        {
            if (!targetPose.IsHomogeneous())
            {
                throw new MatrixException("Pose matrix is not homogeneous");
            }

            return Transl(x, y, z) * Rotx(rx * Math.PI / 180.0) * Roty(ry * Math.PI / 180.0) * Rotz(rz * Math.PI / 180.0) * targetPose;
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///     Returns the quaternion of a pose (4x4 matrix)
        /// </summary>
        /// <param name="ti"></param>
        /// <returns></returns>
        private static double[] ToQuaternion(Matrix ti)
        {
            const double tolerance0 = 1e-9;
            const double tolerance180 = 1e-7;
            double[] q = new double[4];

            double cosangle = Math.Min(Math.Max(((ti[0, 0] + ti[1, 1] + ti[2, 2] - 1.0) * 0.5), -1.0), 1.0);  // Calculate the rotation angle
            if (cosangle > 1.0 - tolerance0)
            {
                // Identity matrix
                q[0] = 1.0;
                q[1] = 0.0;
                q[2] = 0.0;
                q[3] = 0.0;
            }
            else if (cosangle < -1.0 + tolerance180)
            {
                // 180 rotation around an axis
                double[] diag = new[] { ti[0, 0], ti[1, 1], ti[2, 2] };
                int k = Array.IndexOf(diag, diag.Max());
                double[] col = new[] { ti[0, k], ti[1, k], ti[2, k] };
                col[k] = col[k] + 1.0;
                double[] rotvector = col.Select(n => n / SqrtA(2.0 * (1.0 + diag[k]))).ToArray();

                double SqrtA(double d)
                {
                    if (d <= 0.0)
                    {
                        return 0.0;
                    }
                    return Math.Sqrt(d);
                }

                q[0] = 0.0;
                q[1] = rotvector[0];
                q[2] = rotvector[1];
                q[3] = rotvector[2];
            }
            else
            {
                // No edge case, normal calculation
                double a = ti[0, 0];
                double b = ti[1, 1];
                double c = ti[2, 2];
                double sign2 = 1.0;
                double sign3 = 1.0;
                double sign4 = 1.0;
                if (ti[2, 1] - ti[1, 2] < 0)
                {
                    sign2 = -1.0;
                }
                if (ti[0, 2] - ti[2, 0] < 0)
                {
                    sign3 = -1.0;
                }
                if (ti[1, 0] - ti[0, 1] < 0)
                {
                    sign4 = -1.0;
                }
                q[0] =         Math.Sqrt(Math.Max( a + b + c + 1.0, 0.0)) / 2.0;
                q[1] = sign2 * Math.Sqrt(Math.Max( a - b - c + 1.0, 0.0)) / 2.0;
                q[2] = sign3 * Math.Sqrt(Math.Max(-a + b - c + 1.0, 0.0)) / 2.0;
                q[3] = sign4 * Math.Sqrt(Math.Max(-a - b + c + 1.0, 0.0)) / 2.0;
            }
            return q;
        }

        /// <summary>
        ///     Returns the pose (4x4 matrix) from quaternion data
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        private static Matrix FromQuaternion(double[] qin)
        {
            var qnorm = Math.Sqrt(qin[0] * qin[0] + qin[1] * qin[1] + qin[2] * qin[2] + qin[3] * qin[3]);
            var q = new double[4];
            q[0] = qin[0] / qnorm;
            q[1] = qin[1] / qnorm;
            q[2] = qin[2] / qnorm;
            q[3] = qin[3] / qnorm;
            var pose = new Matrix(1 - 2 * q[2] * q[2] - 2 * q[3] * q[3], 2 * q[1] * q[2] - 2 * q[3] * q[0],
                2 * q[1] * q[3] + 2 * q[2] * q[0], 0, 2 * q[1] * q[2] + 2 * q[3] * q[0],
                1 - 2 * q[1] * q[1] - 2 * q[3] * q[3], 2 * q[2] * q[3] - 2 * q[1] * q[0], 0,
                2 * q[1] * q[3] - 2 * q[2] * q[0], 2 * q[2] * q[3] + 2 * q[1] * q[0], 1 - 2 * q[1] * q[1] - 2 * q[2] * q[2],
                0);
            return pose;
        }

        /// <summary>
        ///     Converts a pose to an ABB target
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        private static double[] ToAbb(Matrix h)
        {
            var q = ToQuaternion(h);
            double[] xyzq1234 = {h[0, 3], h[1, 3], h[2, 3], q[0], q[1], q[2], q[3]};
            return xyzq1234;
        }

        private static void SafeAplusBintoC(Matrix a, int xa, int ya, Matrix b, int xb, int yb, Matrix c, int size)
        {
            for (var i = 0; i < size; i++) // rows
            for (var j = 0; j < size; j++) // cols
            {
                c[i, j] = 0;
                if (xa + j < a._cols && ya + i < a._rows)
                    c[i, j] += a[ya + i, xa + j];
                if (xb + j < b._cols && yb + i < b._rows)
                    c[i, j] += b[yb + i, xb + j];
            }
        }

        private static void SafeAminusBintoC(Matrix a, int xa, int ya, Matrix b, int xb, int yb, Matrix c, int size)
        {
            for (var i = 0; i < size; i++) // rows
            for (var j = 0; j < size; j++) // cols
            {
                c[i, j] = 0;
                if (xa + j < a._cols && ya + i < a._rows)
                    c[i, j] += a[ya + i, xa + j];
                if (xb + j < b._cols && yb + i < b._rows)
                    c[i, j] -= b[yb + i, xb + j];
            }
        }

        private static void SafeACopytoC(Matrix a, int xa, int ya, Matrix c, int size)
        {
            for (var i = 0; i < size; i++) // rows
            for (var j = 0; j < size; j++) // cols
            {
                c[i, j] = 0;
                if (xa + j < a._cols && ya + i < a._rows)
                    c[i, j] += a[ya + i, xa + j];
            }
        }

        private static void AplusBintoC(Matrix a, int xa, int ya, Matrix b, int xb, int yb, Matrix c, int size)
        {
            for (var i = 0; i < size; i++) // rows
            for (var j = 0; j < size; j++)
                c[i, j] = a[ya + i, xa + j] + b[yb + i, xb + j];
        }

        private static void AminusBintoC(Matrix a, int xa, int ya, Matrix b, int xb, int yb, Matrix c, int size)
        {
            for (var i = 0; i < size; i++) // rows
            for (var j = 0; j < size; j++)
                c[i, j] = a[ya + i, xa + j] - b[yb + i, xb + j];
        }

        private static void ACopytoC(Matrix a, int xa, int ya, Matrix c, int size)
        {
            for (var i = 0; i < size; i++) // rows
            for (var j = 0; j < size; j++)
                c[i, j] = a[ya + i, xa + j];
        }

        private static Matrix Multiply(Matrix a, Matrix b) // Smart matrix multiplication
        {
            if (a._cols != b._rows)
                throw new MatrixException("Wrong dimension of matrix!");

            Matrix r;

            var msize = Math.Max(Math.Max(a._rows, a._cols), Math.Max(b._rows, b._cols));

            if (msize < 32)
            {
                r = ZeroMatrix(a._rows, b._cols);
                for (var i = 0; i < r._rows; i++)
                for (var j = 0; j < r._cols; j++)
                for (var k = 0; k < a._cols; k++)
                    r[i, j] += a[i, k] * b[k, j];
                return r;
            }

            var size = 1;
            var n = 0;
            while (msize > size)
            {
                size *= 2;
                n++;
            }
            ;
            var h = size / 2;


            var mField = new Matrix[n, 9];

            /*
         *  8x8, 8x8, 8x8, ...
         *  4x4, 4x4, 4x4, ...
         *  2x2, 2x2, 2x2, ...
         *  . . .
         */

            int z;
            for (var i = 0; i < n - 4; i++) // rows
            {
                z = (int) Math.Pow(2, n - i - 1);
                for (var j = 0; j < 9; j++)
                    mField[i, j] = new Matrix(z, z);
            }

            SafeAplusBintoC(a, 0, 0, a, h, h, mField[0, 0], h);
            SafeAplusBintoC(b, 0, 0, b, h, h, mField[0, 1], h);
            StrassenMultiplyRun(mField[0, 0], mField[0, 1], mField[0, 1 + 1], 1, mField); // (A11 + A22) * (B11 + B22);

            SafeAplusBintoC(a, 0, h, a, h, h, mField[0, 0], h);
            SafeACopytoC(b, 0, 0, mField[0, 1], h);
            StrassenMultiplyRun(mField[0, 0], mField[0, 1], mField[0, 1 + 2], 1, mField); // (A21 + A22) * B11;

            SafeACopytoC(a, 0, 0, mField[0, 0], h);
            SafeAminusBintoC(b, h, 0, b, h, h, mField[0, 1], h);
            StrassenMultiplyRun(mField[0, 0], mField[0, 1], mField[0, 1 + 3], 1, mField); //A11 * (B12 - B22);

            SafeACopytoC(a, h, h, mField[0, 0], h);
            SafeAminusBintoC(b, 0, h, b, 0, 0, mField[0, 1], h);
            StrassenMultiplyRun(mField[0, 0], mField[0, 1], mField[0, 1 + 4], 1, mField); //A22 * (B21 - B11);

            SafeAplusBintoC(a, 0, 0, a, h, 0, mField[0, 0], h);
            SafeACopytoC(b, h, h, mField[0, 1], h);
            StrassenMultiplyRun(mField[0, 0], mField[0, 1], mField[0, 1 + 5], 1, mField); //(A11 + A12) * B22;

            SafeAminusBintoC(a, 0, h, a, 0, 0, mField[0, 0], h);
            SafeAplusBintoC(b, 0, 0, b, h, 0, mField[0, 1], h);
            StrassenMultiplyRun(mField[0, 0], mField[0, 1], mField[0, 1 + 6], 1, mField); //(A21 - A11) * (B11 + B12);

            SafeAminusBintoC(a, h, 0, a, h, h, mField[0, 0], h);
            SafeAplusBintoC(b, 0, h, b, h, h, mField[0, 1], h);
            StrassenMultiplyRun(mField[0, 0], mField[0, 1], mField[0, 1 + 7], 1, mField); // (A12 - A22) * (B21 + B22);

            r = new Matrix(a._rows, b._cols); // result

            /// C11
            for (var i = 0; i < Math.Min(h, r._rows); i++) // rows
            for (var j = 0; j < Math.Min(h, r._cols); j++) // cols
                r[i, j] = mField[0, 1 + 1][i, j] + mField[0, 1 + 4][i, j] - mField[0, 1 + 5][i, j] + mField[0, 1 + 7][i, j];

            /// C12
            for (var i = 0; i < Math.Min(h, r._rows); i++) // rows
            for (var j = h; j < Math.Min(2 * h, r._cols); j++) // cols
                r[i, j] = mField[0, 1 + 3][i, j - h] + mField[0, 1 + 5][i, j - h];

            /// C21
            for (var i = h; i < Math.Min(2 * h, r._rows); i++) // rows
            for (var j = 0; j < Math.Min(h, r._cols); j++) // cols
                r[i, j] = mField[0, 1 + 2][i - h, j] + mField[0, 1 + 4][i - h, j];

            /// C22
            for (var i = h; i < Math.Min(2 * h, r._rows); i++) // rows
            for (var j = h; j < Math.Min(2 * h, r._cols); j++) // cols
                r[i, j] = mField[0, 1 + 1][i - h, j - h] - mField[0, 1 + 2][i - h, j - h] + mField[0, 1 + 3][i - h, j - h] +
                          mField[0, 1 + 6][i - h, j - h];

            return r;
        }

        // function for square matrix 2^N x 2^N

        private static void
            StrassenMultiplyRun(Matrix a, Matrix b, Matrix c, int l, Matrix[,] f) // A * B into C, level of recursion, matrix field
        {
            var size = a._rows;
            var h = size / 2;

            if (size < 32)
            {
                for (var i = 0; i < c._rows; i++)
                for (var j = 0; j < c._cols; j++)
                {
                    c[i, j] = 0;
                    for (var k = 0; k < a._cols; k++)
                        c[i, j] += a[i, k] * b[k, j];
                }
                return;
            }

            AplusBintoC(a, 0, 0, a, h, h, f[l, 0], h);
            AplusBintoC(b, 0, 0, b, h, h, f[l, 1], h);
            StrassenMultiplyRun(f[l, 0], f[l, 1], f[l, 1 + 1], l + 1, f); // (A11 + A22) * (B11 + B22);

            AplusBintoC(a, 0, h, a, h, h, f[l, 0], h);
            ACopytoC(b, 0, 0, f[l, 1], h);
            StrassenMultiplyRun(f[l, 0], f[l, 1], f[l, 1 + 2], l + 1, f); // (A21 + A22) * B11;

            ACopytoC(a, 0, 0, f[l, 0], h);
            AminusBintoC(b, h, 0, b, h, h, f[l, 1], h);
            StrassenMultiplyRun(f[l, 0], f[l, 1], f[l, 1 + 3], l + 1, f); //A11 * (B12 - B22);

            ACopytoC(a, h, h, f[l, 0], h);
            AminusBintoC(b, 0, h, b, 0, 0, f[l, 1], h);
            StrassenMultiplyRun(f[l, 0], f[l, 1], f[l, 1 + 4], l + 1, f); //A22 * (B21 - B11);

            AplusBintoC(a, 0, 0, a, h, 0, f[l, 0], h);
            ACopytoC(b, h, h, f[l, 1], h);
            StrassenMultiplyRun(f[l, 0], f[l, 1], f[l, 1 + 5], l + 1, f); //(A11 + A12) * B22;

            AminusBintoC(a, 0, h, a, 0, 0, f[l, 0], h);
            AplusBintoC(b, 0, 0, b, h, 0, f[l, 1], h);
            StrassenMultiplyRun(f[l, 0], f[l, 1], f[l, 1 + 6], l + 1, f); //(A21 - A11) * (B11 + B12);

            AminusBintoC(a, h, 0, a, h, h, f[l, 0], h);
            AplusBintoC(b, 0, h, b, h, h, f[l, 1], h);
            StrassenMultiplyRun(f[l, 0], f[l, 1], f[l, 1 + 7], l + 1, f); // (A12 - A22) * (B21 + B22);

            /// C11
            for (var i = 0; i < h; i++) // rows
            for (var j = 0; j < h; j++) // cols
                c[i, j] = f[l, 1 + 1][i, j] + f[l, 1 + 4][i, j] - f[l, 1 + 5][i, j] + f[l, 1 + 7][i, j];

            /// C12
            for (var i = 0; i < h; i++) // rows
            for (var j = h; j < size; j++) // cols
                c[i, j] = f[l, 1 + 3][i, j - h] + f[l, 1 + 5][i, j - h];

            /// C21
            for (var i = h; i < size; i++) // rows
            for (var j = 0; j < h; j++) // cols
                c[i, j] = f[l, 1 + 2][i - h, j] + f[l, 1 + 4][i - h, j];

            /// C22
            for (var i = h; i < size; i++) // rows
            for (var j = h; j < size; j++) // cols
                c[i, j] = f[l, 1 + 1][i - h, j - h] - f[l, 1 + 2][i - h, j - h] + f[l, 1 + 3][i - h, j - h] +
                          f[l, 1 + 6][i - h, j - h];
        }

        private static Matrix Multiply(double n, Matrix m) // Multiplication by constant n
        {
            var r = new Matrix(m._rows, m._cols);
            for (var i = 0; i < m._rows; i++)
            for (var j = 0; j < m._cols; j++)
                r[i, j] = m[i, j] * n;
            return r;
        }

        private static double[] Multiply(Matrix m1, double[] p1)         // Add matrix
        {
            double[] p2 = new double[p1.Length];
            if (m1._cols == 4 && m1._rows == 4 && p1.Length == 3)
            {
                // multiply a homogeneous matrix and a 3D vector
                p2[0] = m1[0, 0] * p1[0] + m1[0, 1] * p1[1] + m1[0, 2] * p1[2] + m1[0, 3];
                p2[1] = m1[1, 0] * p1[0] + m1[1, 1] * p1[1] + m1[1, 2] * p1[2] + m1[1, 3];
                p2[2] = m1[2, 0] * p1[0] + m1[2, 1] * p1[1] + m1[2, 2] * p1[2] + m1[2, 3];
                return p2;
            }
            if (m1._cols != p1.Length) throw new MatrixException("Matrices must have the same dimensions!");
            for (var i = 0; i < m1._rows; i++)
            {
                double vi = 0;
                for (var j = 0; j < m1._cols; j++)
                {
                    vi = vi + m1[i, j] * p1[j];
                }
                p2[i] = vi;
            }
            return p2;
        }

        private static Matrix Add(Matrix m1, Matrix m2) // Add matrix
        {
            if (m1._rows != m2._rows || m1._cols != m2._cols)
                throw new MatrixException("Matrices must have the same dimensions!");
            var r = new Matrix(m1._rows, m1._cols);
            for (var i = 0; i < r._rows; i++)
            for (var j = 0; j < r._cols; j++)
                r[i, j] = m1[i, j] + m2[i, j];
            return r;
        }

        private static Matrix ConcatenateHorizontal(Matrix m1, Matrix m2)
        {
            if (m1._rows != m2._rows)
            {
                throw new MatrixException("Vertical size of matrices does not match");
            }

            var result = new Matrix(m1._rows, m1._cols + m2._cols);

            for (int row = 0; row < m1._rows; row++)
            {
                for (int col = 0; col < m1._cols; col++)
                {
                    result[row, col] = m1[row, col];
                }

                for (int col = 0; col < m2._cols; col++)
                {
                    result[row, m1._cols + col] = m2[row, col];
                }
            }

            return result;
        }

        private static Matrix ConcatenateVertical(Matrix m1, Matrix m2)
        {
            if (m1._cols != m2._cols)
            {
                throw new MatrixException("Horizontal size of matrices does not match");
            }

            var result = new Matrix(m1._rows + m2._rows, m1._cols);

            for (int col = 0; col < m1._cols; col++)
            {
                for (int row = 0; row < m1._rows; row++)
                {
                    result[row, col] = m1[row, col];
                }

                for (int row = 0; row < m2._rows; row++)
                {
                    result[m1._rows + row, col] = m2[row, col];
                }
            }

            return result;
        }

        #endregion
    }
}
