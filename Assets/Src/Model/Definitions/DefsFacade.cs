using UnityEngine;

namespace Model.Definitions
{
    [CreateAssetMenu(fileName = "DefsFacade", menuName = "Defs/DefsFacade")]
    public class DefsFacade : ScriptableObject
    {
        [SerializeField] private InventoryItemsDef items;
        [SerializeField] private PlayerDef player;

        public InventoryItemsDef Items => items;
        public PlayerDef Player => player;

        private static DefsFacade _instance;
        public static DefsFacade Instance => _instance == null ? LoadDefs() : _instance;

        private static DefsFacade LoadDefs()
        {
            return _instance = Resources.Load<DefsFacade>("DefsFacade");
        }
    }
}