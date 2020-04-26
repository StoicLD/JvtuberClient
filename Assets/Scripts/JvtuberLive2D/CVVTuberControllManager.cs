using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JvtuberLive2D
{
    /// <summary>
    /// 主要的控制类，控制整个面部捕捉的过程
    /// </summary>
    [DisallowMultipleComponent, RequireComponent (typeof(CVVTuberProcessOrderList))]
    public class CVVTuberControllManager : MonoBehaviour
    {
        //六个process组件，对应六个步骤
        protected List<CVVTuberProcess> processOrderList;

        // Use this for initialization
        protected virtual IEnumerator Start ()
        {
            enabled = false;

            yield return null;

            processOrderList = GetComponent<CVVTuberProcessOrderList> ().GetProcessOrderList ();
            if (processOrderList == null)
                yield break;

            foreach (var item in processOrderList) {
                if (item == null)
                    continue;

                //Debug.Log("Setup : "+item.gameObject.name);
                //启动每一个Process，也就是Readme中的那几个步骤
                item.Setup ();
            }

            enabled = true;
        }

        // Update is called once per frame
        protected virtual void Update ()
        {
            if (processOrderList == null)
                return;

            foreach (var item in processOrderList) {
                if (item == null)
                    continue;

                if (!item.gameObject.activeInHierarchy || !item.enabled)
                    continue;

                //Debug.Log("UpdateValue : " + item.gameObject.name);

                item.UpdateValue ();
            }
        }

        // Update is called once per frame
        protected virtual void LateUpdate ()
        {
            if (processOrderList == null)
                return;

            foreach (var item in processOrderList) {
                if (item == null)
                    continue;

                if (!item.gameObject.activeInHierarchy || !item.enabled)
                    continue;

                //Debug.Log("LateUpdateValue : " + item.gameObject.name);

                item.LateUpdateValue ();
            }
        }

        protected virtual void OnDestroy ()
        {
            Dispose ();
        }

        public virtual void Dispose ()
        {
            
        }
    }
}