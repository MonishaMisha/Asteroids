using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic class to create singleton objects of any monobehaviour type
/// </summary>
/// <typeparam name="T"></typeparam>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {

        private static T instance;

        public static T Instance
        {
            get
            {
                return instance;
            }
        }


        // Start is called before the first frame update
        void Awake()
        {
          if(instance == null)
            {
                instance = this as T;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
}
