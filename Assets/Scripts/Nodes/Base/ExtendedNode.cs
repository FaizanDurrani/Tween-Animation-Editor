using System.Collections.Generic;
using Graphs;
using UnityEditor;
using UnityEngine;
using XNode;

namespace Base
{
    public abstract class ExtendedNode : Node
    {
        #region Private Variables

        [HideInInspector] public bool Validate = false;
        private const float UpdateDelay = 1f;
        private double _timer;
        private bool _update;
        private Queue<NodePort> _nodesToUpdate;

        #endregion

        #region Protected Methods

        protected void UpdateOutputNode(NodePort port)
        {
            _nodesToUpdate = _nodesToUpdate ?? new Queue<NodePort>();
            if (!_nodesToUpdate.Contains(port))
                _nodesToUpdate.Enqueue(port);
        }

        protected void UpdateOutputNode(string port)
        {
            UpdateOutputNode(GetOutputPort(port));
        }

        protected override void Init()
        {
            _nodesToUpdate = _nodesToUpdate ?? new Queue<NodePort>();
            EditorApplication.update += EditorUpdate;
            
            var nodeName = GetNodeName();
            if (!string.IsNullOrEmpty(nodeName))
            {
               name = nodeName;
            }
            
            GetGraph<ExtendedGraph>().NodeInitialized(this);
            Debug.Log("Init called on extended node for " + GetNodeName());
        }
        
        #endregion

        #region Private Methods

        private void EditorUpdate()
        {
            if (Validate)
            {
                OnValidate();
                Validate = false;
            }

            if (_timer > EditorApplication.timeSinceStartup) return;

            if (_update)
            {
                OnValidateEnd();
                _update = false;
            }

            SendNodeUpdate();
        }

        private void SendNodeUpdate()
        {
            while (_nodesToUpdate.Count > 0)
            {
                var port = _nodesToUpdate.Dequeue();
                if (port.IsConnected)
                {
                    for (int i = 0; i < port.ConnectionCount; i++)
                    {
                        ExtendedNode node = port.GetConnection(i).node as ExtendedNode;
                        if (node != null)
                        {
                            node.OnInputUpdate(port.GetConnection(i));
                        }
                    }
                }
            }
        }

        #endregion

        private void OnValidate()
        {
            _timer = EditorApplication.timeSinceStartup + UpdateDelay;
            _update = true;
        }

        private void OnDestroy()
        {
            GetGraph<ExtendedGraph>().NodeRemoved(this);
        }

        protected T GetGraph<T>() where T : NodeGraph
        {
            return graph as T;
        }

        protected virtual void OnValidateEnd(){}
        protected virtual void OnInputUpdate(NodePort port){}
        protected abstract string GetNodeName();
    } 
}