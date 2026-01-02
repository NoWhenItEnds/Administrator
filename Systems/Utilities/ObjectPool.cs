using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Administrator.Utilities
{
    /// <summary> A caching system for pulling objects from a pool of initialised objects on command. </summary>
    /// <typeparam name="T"> The kind of node in the pool. </typeparam>
    public class ObjectPool<T> where T : Node
    {
        /// <summary> A pool that holds objects that are in the game world. A nullable datetime shows if an object is currently in use and when it was pulled. </summary>
        private Dictionary<T, DateTime?> _objectPool = new Dictionary<T, DateTime?>();


        /// <summary> The node to use as the spawned nodes' parent. </summary>
        private readonly Node PARENT_NODE;

        /// <summary> The prefab to spawn additional objects from. </summary>
        private readonly PackedScene PREFAB;

        /// <summary> How many item nodes the item pool should initially contain. </summary>
        private readonly Int32 INITIAL_SIZE = 100;

        /// <summary> Whether the object pool should resize itself if it reaches its initial size. </summary>
        private readonly Boolean DO_RESIZE = false;


        /// <summary> A caching system for pulling objects from a pool of initialised objects on command. </summary>
        /// <param name="parentNode"> The node to use as the spawned nodes' parent. </param>
        /// <param name="objectPrefab"> The prefab to spawn additional objects from. </param>
        /// <param name="initialSize"> How many item nodes the item pool should initially contain. </param>
        /// <param name="doResize"> Whether the object pool should resize itself if it reaches its initial size. </param>
        public ObjectPool(Node parentNode, PackedScene objectPrefab, Int32 initialSize = 100, Boolean doResize = false)
        {
            PARENT_NODE = parentNode;
            PREFAB = objectPrefab;
            INITIAL_SIZE = initialSize;
            DO_RESIZE = doResize;

            // Add any existing objects.
            var children = PARENT_NODE.GetChildren();
            foreach (Node child in children)
            {
                if (child is T node)
                {
                    _objectPool.Add(node, DateTime.UtcNow);
                }
            }

            // Build the pool.
            for (Int32 i = 0; i < INITIAL_SIZE; i++)
            {
                BuildItem();
            }
        }


        /// <summary> Create a new object for the pool. </summary>
        /// <returns> The created node. </returns>
        /// <exception cref="ArgumentException"> If the given prefab cannot be created as type T. </exception>
        private T BuildItem()
        {
            T obj = PREFAB.InstantiateOrNull<T>();
            if (obj == null)
            {
                throw new ArgumentException($"The given prefab cannot be instantiated into type {typeof(T)}.");
            }

            PARENT_NODE.AddChild(obj);
            _objectPool.Add(obj, null);
            FreeObject(obj);

            return obj;
        }


        /// <summary> Get a new object by pulling from the pool. </summary>
        /// <returns> The now active object. </returns>
        public T GetAvailableObject()
        {
            T? result = null;

            // Attempt to find a free object in the pool.
            foreach (KeyValuePair<T, DateTime?> obj in _objectPool)
            {
                if (obj.Value == null)    // If the obj is free.
                {
                    result = obj.Key;
                    break;
                }
            }

            // If an item was not found...
            if (result == null)
            {
                if (DO_RESIZE)  // Expand the pool.
                {
                    result = BuildItem();
                }
                else            // Grab earliest.
                {
                    result = _objectPool.OrderBy(x => x.Value).First().Key;
                }
            }

            _objectPool[result] = DateTime.UtcNow;

            // Make the pulled object visible.
            switch (result)
            {
                case Node2D node2D:
                    node2D.Visible = true;
                    break;
                case Node3D node3D:
                    node3D.Visible = true;
                    break;
                case Control control:
                    control.Visible = true;
                    break;
            }

            return result;
        }


        /// <summary> Free all the currently in use items. </summary>
        public void FreeAll()
        {
            T[] activeObjects = GetActiveObjects();
            foreach (T obj in activeObjects)
            {
                FreeObject(obj);
            }
        }


        /// <summary> Free an object and return it to the pool. </summary>
        /// <param name="obj"> The object to return. </param>
        public void FreeObject(T obj)
        {
            _objectPool[obj] = null;

            // Depending upon the base type, getting it out of the way may be difficult.
            switch (obj)
            {
                case Node2D node2D:
                    node2D.Visible = false;
                    break;
                case Node3D node3D:
                    node3D.Visible = false;
                    break;
                case Control control:
                    control.Visible = false;
                    break;
            }
        }


        /// <summary> Get an array of all the currently active objects. </summary>
        /// <returns> All the objects that are currently in use. </returns>
        public T[] GetActiveObjects() => _objectPool.Where(x => x.Value != null).Select(x => x.Key).ToArray();


        /// <summary> Get an array of all the pool's objects. </summary>
        /// <returns> An array of all the objects in the pool. </returns>
        public T[] GetAllObjects() => _objectPool.Select(x => x.Key).ToArray();
    }
}
