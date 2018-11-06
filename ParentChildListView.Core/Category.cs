using System.Collections.Generic;
using ParentChildListView.Core.TreeNodes;

namespace ParentChildListView.Core
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

        public override bool Equals(object obj)
        {
            if(obj is Category other) {
                return Equals(other);
            }
            return false;
        }

        private bool Equals(Category other)
        {
            return (ParentId, Id, Name).Equals((other.ParentId, other.Id, other.Name));
        }

        public override int GetHashCode()
        {
            return (ParentId, Id, Name).GetHashCode();
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

        public static IEnumerable<Category> CreateFlatDummyCategories()
        {
            yield return new Category(-1, 0, "Alle Themen");
            yield return new Category(0, 1, "Foobar");
            yield return new Category(0, 2, "Foobar");
            yield return new Category(0, 3, "Foobar");
            yield return new Category(0, 4, "Foobar");
            yield return new Category(0, 5, "Foobar");
            yield return new Category(0, 6, "Foobar");
            yield return new Category(0, 7, "Foobar");
            yield return new Category(0, 8, "Foobar");
            yield return new Category(0, 9, "Foobar");
            yield return new Category(0, 10, "Foobar");
            yield return new Category(0, 11, "Foobar");
            yield return new Category(0, 12, "Foobar");
            yield return new Category(0, 13, "Foobar");
            yield return new Category(0, 14, "Foobar");
            yield return new Category(0, 15, "Foobar");
        }
    }
}