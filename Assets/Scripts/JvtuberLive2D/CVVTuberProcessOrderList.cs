using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JvtuberLive2D
{
    [DisallowMultipleComponent]
    public class CVVTuberProcessOrderList : MonoBehaviour
    {
        //CVVTuberProcess是一个抽象类，基本上所有的主要模块都集成自这个类
        [SerializeField]
        List<CVVTuberProcess> processOrderList = default(List<CVVTuberProcess>);

        public List<CVVTuberProcess> GetProcessOrderList ()
        {
            return processOrderList;
        }
    }
}
