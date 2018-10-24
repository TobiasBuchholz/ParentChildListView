using System;
using System.Collections.Generic;
using System.Linq;
using ParentChildListView.UI.TreeNodes;

namespace PressMatrix.Utility.TreeNodes
{
    public static class TreeNodeExtensions
    {
        public static IList<TreeNode<T>> ToRootTreeNodes<T>(this IEnumerable<T> @this) where T : ITreeNodeData
        {
            var nodesDict = @this.ToDictionary(x => x.Id, x => new TreeNode<T>(x));
            var rootNodes = new List<TreeNode<T>>();

            foreach(var pair in nodesDict) {
                var node = pair.Value;
                if(node.ParentId == TreeNode<T>.ParentIdNone) {
                    rootNodes.Add(node);
                } else {
                    nodesDict[node.ParentId].AddChildNode(node);
                }
            }
            return rootNodes;
        }
    }
}