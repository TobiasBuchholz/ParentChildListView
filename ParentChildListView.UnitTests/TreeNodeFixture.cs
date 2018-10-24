using System;
using System.Collections.Generic;
using ParentChildListView.UI;
using PressMatrix.Utility.TreeNodes;
using Xunit;

namespace ParentChildListView.UnitTests
{
    public sealed class TreeNodeFixture
    {
        public static IEnumerable<Category> CreateDummyNodes()
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
        }
        
        [Theory]
        [InlineData(0, new[] { 2, 3, 4, 5, 6, 7, 8 }, new[] { 2, 3 })]
        [InlineData(1, new[] { 1, 3, 4, 5, 6, 7, 8 }, new[] { 2, 3 })]
        [InlineData(2, new[] { 1, 2, 4, 5, 6, 7, 8 }, new[] { 2, 3 })]
        [InlineData(3, new[] { 1, 2, 3, 5, 6, 7, 8 }, new int[] {})]
        [InlineData(4, new[] { 1, 2, 3, 4, 6, 7, 8 }, new[] { 2 })]
        public void calculates_difference_between_root_level_and_first_level_node(int selectedNodeIndex, int[] expectedRemovedIndexes, int[] expectedAddedIndexes)
        {
            var rootNode = CreateDummyNodes().ToRootTreeNodes()[0];
            var firstLevelNode = rootNode.ChildNodes[selectedNodeIndex];

            var diff = rootNode.CalculateDiff(firstLevelNode);
            Assert.Equal(expectedRemovedIndexes, diff.RemovedIndexes);           
            Assert.Equal(expectedAddedIndexes, diff.AddedIndexes);           
        }

        [Theory]
        [InlineData(0, new[] { 3 }, new[] { 3 })]
        [InlineData(1, new[] { 2 }, new int[] {})]
        public void calculates_difference_between_first_level_and_second_level_node(int selectedNodeIndex, int[] expectedRemovedIndexes, int[] expectedAddedIndexes)
        {
            var rootNode = CreateDummyNodes().ToRootTreeNodes()[0];
            var firstLevelNode = rootNode.ChildNodes[2];
            var secondLevelNode = firstLevelNode.ChildNodes[selectedNodeIndex];

            var diff = firstLevelNode.CalculateDiff(secondLevelNode);
            Assert.Equal(expectedRemovedIndexes, diff.RemovedIndexes);
            Assert.Equal(expectedAddedIndexes, diff.AddedIndexes);
        }

        [Theory]
        [InlineData(0, new[] { 2, 3 }, new[] { 2, 3, 4, 5, 6, 7, 8 })]
        [InlineData(1, new[] { 2, 3 }, new[] { 1, 3, 4, 5, 6, 7, 8 })]
        [InlineData(2, new[] { 2, 3 }, new[] { 1, 2, 4, 5, 6, 7, 8 })]
        [InlineData(3, new int[] {}, new[] { 1, 2, 3, 5, 6, 7, 8 })]
        [InlineData(4, new[] { 2 }, new[] { 1, 2, 3, 4, 6, 7, 8 })]
        public void calculates_difference_between_first_level_and_root_level_node(int selectedNodeIndex, int[] expectedRemovedIndexes, int[] expectedAddedIndexes)
        {
            var rootNode = CreateDummyNodes().ToRootTreeNodes()[0];
            var firstLevelNode = rootNode.ChildNodes[selectedNodeIndex];

            var diff = firstLevelNode.CalculateDiff(rootNode);
            Assert.Equal(expectedRemovedIndexes, diff.RemovedIndexes);           
            Assert.Equal(expectedAddedIndexes, diff.AddedIndexes);           
        }

        [Fact]
        public void calculates_difference_between_second_level_and_root_level()
        {
            var rootNode = CreateDummyNodes().ToRootTreeNodes()[0];
            var firstLevelNode = rootNode.ChildNodes[2];
            var secondLevelNode = firstLevelNode.ChildNodes[0];

            var diff = secondLevelNode.CalculateDiff(rootNode);
            Assert.Equal(new[] { 1, 2, 3 }, diff.RemovedIndexes);
            Assert.Equal(new[] { 1, 2, 3, 4, 5, 6, 7, 8 }, diff.AddedIndexes);
        }

        [Fact]
        public void calculates_difference_between_third_level_and_root_level()
        {
            var rootNode = CreateDummyNodes().ToRootTreeNodes()[0];
            var firstLevelNode = rootNode.ChildNodes[2];
            var secondLevelNode = firstLevelNode.ChildNodes[0];
            var thirdLevelNode = secondLevelNode.ChildNodes[0];

            var diff = thirdLevelNode.CalculateDiff(rootNode);
            Assert.Equal(new[] { 1, 2, 3 }, diff.RemovedIndexes);
            Assert.Equal(new[] { 1, 2, 3, 4, 5, 6, 7, 8 }, diff.AddedIndexes);
        }

        [Fact]
        public void calculate_difference_between_third_level_and_first_level()
        {
            var rootNode = CreateDummyNodes().ToRootTreeNodes()[0];
            var firstLevelNode = rootNode.ChildNodes[2];
            var secondLevelNode = firstLevelNode.ChildNodes[0];
            var thirdLevelNode = secondLevelNode.ChildNodes[0];

            var diff = thirdLevelNode.CalculateDiff(firstLevelNode);
            throw new NotImplementedException();
            Assert.Equal(new[] { 1, 2, 3 }, diff.RemovedIndexes);
            Assert.Equal(new[] { 1, 2, 3, 4, 5, 6, 7, 8 }, diff.AddedIndexes);
        }
    }
}