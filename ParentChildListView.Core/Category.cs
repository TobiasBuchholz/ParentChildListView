using System.Collections.Generic;
using PressMatrix.Utility.TreeNodes;

namespace ParentChildListView.UI
{
    public sealed class Category : ITreeNodeData
    {
        public Category(long parentId, long id, string name)
        {
            ParentId = parentId;
            Id = id;
            Name = name;
        }

        public override string ToString()
        {
            return $"[Category: Name={Name}]";
        }

        public long ParentId { get; }
        public long Id { get; }
        public string Name { get; }

        public static IEnumerable<Category> CreateDummyCategories()
        {
            yield return new Category(-1, 0, "Alle Themen");
            yield return new Category(0, 1, "Development");
            yield return new Category(0, 2, "Rezepte");
            yield return new Category(0, 3, "Sport");
            yield return new Category(0, 4, "Musik");
            yield return new Category(0, 5, "Autos");
            yield return new Category(0, 6, "Wissen");
            yield return new Category(0, 7, "Sonstiges");
            yield return new Category(0, 8, "Test Dummy");
            yield return new Category(1, 9, "Android Development");
            yield return new Category(1, 10, "iOS Development");
            yield return new Category(2, 11, "Vegetarisch");
            yield return new Category(2, 12, "Mit Fleisch");
            yield return new Category(3, 13, "Fußball");
            yield return new Category(3, 14, "Formel 1");
            yield return new Category(5, 15, "AutoBild");
            yield return new Category(13, 16, "Kicker");
            yield return new Category(16, 17, "Sub-Kicker 1");
            yield return new Category(16, 18, "Sub-Kicker 2");
            yield return new Category(16, 19, "Sub-Kicker 3");
        }
    }
}