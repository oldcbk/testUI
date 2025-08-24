using Unity.Properties;
using UnityEngine;

namespace ShowPropDetail
{
    [CreateAssetMenu(fileName = "name_SO", menuName = "Tests/ShowPropDetail-SO")]
    public class SO: ScriptableObject
    {
        [SerializeField, DontCreateProperty]
        private string m_str;

        [CreateProperty]
        public string str
        {
            get => m_str;
            set => m_str = value;
        }

        [field: SerializeField, DontCreateProperty]
        [CreateProperty]
        public string str2 { get; set; }

        public string str3;
    }
}