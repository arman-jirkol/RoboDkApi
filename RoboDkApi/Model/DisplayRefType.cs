using System;


namespace RoboDk.API.Model
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum DisplayRefType
    {
        /// <summary>
        /// Set the default behavior
        /// </summary>
        Default = -1,

        /// <summary>
        /// No axis to display
        /// </summary>
        None = 0,

        /// <summary>
        /// Display all reference frames
        /// </summary>
        All = 0x1FF, /*0b111111111,*/
        
        /// <summary>
        /// Display the translation/rotation along the XY plane (holds Z axis the same)
        /// </summary>
        TxyRz = 0x63, /*0b001100011,*/

        /// <summary>
        /// Display the translation X axis
        /// </summary>
        Tx = 0x1, /*0b001,*/

        /// <summary>
        /// Display the translation Y axis
        /// </summary>
        Ty = 0x2, /*0b010,*/

        /// <summary>
        /// Display the translation Z axis
        /// </summary>
        Tz = 0x4, /*0b100,*/

        /// <summary>
        /// Display the rotation X axis
        /// </summary>
        Rx = 0x8, /*0b001000,*/

        /// <summary>
        /// Display the rotation Y axis
        /// </summary>
        Ry = 0x10, /*0b010000,*/

        /// <summary>
        /// Display the rotation Z axis
        /// </summary>
        Rz = 0x20, /*0b100000,*/

        /// <summary>
        /// Display the plane translation along XY plane
        /// </summary>
        Pxy = 0x40, /*0b001000000,*/

        /// <summary>
        /// Display the plane translation along XZ plane
        /// </summary>
        Pxz = 0x80, /*0b010000000,*/

        /// <summary>
        /// Display the plane translation along YZ plane
        /// </summary>
        Pyz = 0x100, /*0b100000000*/

    }
}
