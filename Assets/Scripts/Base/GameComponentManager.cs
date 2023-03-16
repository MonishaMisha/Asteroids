using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    /// <summary>
    /// Manager for all the basic scene components
    /// </summary>
    public class GameComponentManager : MonoBehaviour, IGameComponentManager
    {
        [SerializeField]
        List<BaseGameComponent> sceneComponents = new List<BaseGameComponent>();

        void Start()
        {

            foreach (BaseGameComponent sceneComponent in GetComponentsInChildren<BaseGameComponent>())
            {
                if (!sceneComponents.Contains(sceneComponent))
                {
                    sceneComponents.Add(sceneComponent);
                }
            }

            sceneComponents.ForEach(sc => sc.Init(this));

        }

        /// <summary>
        /// Get attached component of specific type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetSceneComponent<T>()
        {
            T obj = default(T);

            if (typeof(IBaseGameComponent).IsAssignableFrom(typeof(T)))
            {
                foreach (BaseGameComponent service in sceneComponents)
                {
                    if (service.TryGetComponent<T>(out obj))
                    {
                        return obj;
                    }
                }
            }
            Debug.LogError("Could not find the basicSceneComponent of type <b>" + typeof(T) + "</b>");
            return obj;
        }
        
    /// <summary>
    /// Method to add manual reference(Use this if only necessary)
    /// </summary>
    /// <param name="reference"></param>
        public void SetReference(BaseGameComponent reference)
        {
            if (!sceneComponents.Contains(reference))
            {
                sceneComponents.Add(reference);
            }
            else
            {
                Debug.LogError("Reference <b>" + reference.GetType() + "</b> already present in scenemanager");
            }
        }
    }

    public interface IGameComponentManager
    {
        T GetSceneComponent<T>();

        void SetReference(BaseGameComponent reference);
    }