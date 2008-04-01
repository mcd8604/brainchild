using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace OctreeCulling
{
    class Node
    {
        private List<Node> _nodes;
        public List<Node> Nodes
        {
            get { return _nodes; }
            set { _nodes = value; }
        }

        public Node()
        {
            _nodes = new List<Node>();
        }

        public void AddNode(Node newNode)
        {
            _nodes.Add(newNode);
        }

        public virtual void Draw(GameTime gameTime)
        {
            foreach (Node node in _nodes)
            {
                node.Draw(gameTime);
            }
        }
    }
}
