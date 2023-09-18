using UnityEngine;

namespace VaVarm.Utils {

    public static class HierarchyNavigation
    {
        public static Transform FindChildByNameRecursively(Transform parent, string name)
        {
            Transform result = null;
            foreach (Transform child in parent)
            {
                if (child.name == name)
                {
                    result = child;
                    break;
                }
                else
                {
                    result = FindChildByNameRecursively(child, name);
                    if (result != null)
                        break;
                }
            }
            return result;
        }
    }
}
