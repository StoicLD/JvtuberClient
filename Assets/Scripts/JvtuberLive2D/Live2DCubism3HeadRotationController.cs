using Live2D.Cubism.Core;
using JvutberNetwork;
using UnityEngine;

namespace JvtuberLive2D
{
    /// <summary>
    /// 开头六步的第四步，更新Live2D模型的头部转动，使用到了Live2D的SDK
    /// </summary>
    public class Live2DCubism3HeadRotationController : CVVTuberProcess
    {
    
        [Header ("[Setting]")]

        public Vector3 offsetAngle;

        public bool invertXAxis;

        public bool invertYAxis;

        public bool invertZAxis;

        public bool rotateXAxis;

        public bool rotateYAxis;

        public bool rotateZAxis;

        public bool leapAngle;

        [Range (0, 1)]
        public float leapT = 0.6f;

        [Header ("[Target]")]

        public CubismModel target;

        public TcpHandler tcpHandler;

        protected CubismParameter paramAngleX;

        protected CubismParameter paramAngleY;

        protected CubismParameter paramAngleZ;

        protected Vector3 headEulerAngles;

        protected Vector3 oldHeadEulerAngle;


        #region CVVTuberProcess

        public override string GetDescription ()
        {
            return "Update head rotation of Live2DCubism3Model using HeadRotationGetter.";
        }

        public override void Setup ()
        {
            oldHeadEulerAngle = Vector3.zero;
            NullCheck(tcpHandler, "tcpHandler");
            NullCheck (target, "target");
            //应该是x，y，z轴的一些参数
            paramAngleX = target.Parameters.FindById("ParamAngleX");
            paramAngleY = target.Parameters.FindById("ParamAngleY");
            paramAngleZ = target.Parameters.FindById("ParamAngleZ");
        }

        public override void LateUpdateValue ()
        {
            if (target == null)
                return;
            

            if (tcpHandler.GetHeadEulerAngles () != Vector3.zero) {
                headEulerAngles = tcpHandler.GetHeadEulerAngles ();

                headEulerAngles = new Vector3 (headEulerAngles.x + offsetAngle.x, headEulerAngles.y + offsetAngle.y, headEulerAngles.z + offsetAngle.z);
                headEulerAngles = new Vector3 (invertXAxis ? -headEulerAngles.x : headEulerAngles.x, invertYAxis ? -headEulerAngles.y : headEulerAngles.y, invertZAxis ? -headEulerAngles.z : headEulerAngles.z);
                headEulerAngles = Quaternion.Euler (rotateXAxis ? 90 : 0, rotateYAxis ? 90 : 0, rotateZAxis ? 90 : 0) * headEulerAngles;
            }
            
            if (leapAngle) {
                headEulerAngles = new Vector3 (Mathf.LerpAngle (oldHeadEulerAngle.x, headEulerAngles.x, leapT), Mathf.LerpAngle (oldHeadEulerAngle.y, headEulerAngles.y, leapT), Mathf.LerpAngle (oldHeadEulerAngle.z, headEulerAngles.z, leapT));
            }

            float _x = (headEulerAngles.x > 180) ? headEulerAngles.x - 360 : headEulerAngles.x;
            float _y = (headEulerAngles.y > 180) ? headEulerAngles.y - 360 : headEulerAngles.y;
            float _z = (headEulerAngles.z > 180) ? headEulerAngles.z - 360 : headEulerAngles.z;

            float x = -_y * 2f;          
            float y = _x * 2f;
            float z = -_z * 2f;

            paramAngleX.Value = x;
            paramAngleY.Value = y;
            paramAngleZ.Value = z;

            oldHeadEulerAngle = headEulerAngles;
        }

        #endregion
        
    }
}