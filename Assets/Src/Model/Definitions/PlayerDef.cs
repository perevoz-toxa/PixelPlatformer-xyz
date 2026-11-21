using UnityEngine;

namespace Model.Definitions
{
    [CreateAssetMenu(fileName = "PlayerDef", menuName = "Defs/PlayerDef")]
    public class PlayerDef : ScriptableObject
    {
        [SerializeField] private int inventorySize;
        
        public int InventorySize => inventorySize;
    }
}