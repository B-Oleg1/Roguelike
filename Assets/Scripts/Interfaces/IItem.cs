using Assets.Scripts.Enums;

namespace Assets.Scripts.Interfaces
{
    interface IItem
    {
        public TypeItems TypeItem { get; }
        public RarityItems RarityItem { get; }
    }
}
