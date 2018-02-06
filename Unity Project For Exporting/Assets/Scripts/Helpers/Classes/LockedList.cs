using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using UnityEngine.Assertions;

namespace Helpers.Classes
{
    /// <summary>
    /// Using Reflection we return the actual list or a ReadOnlYcollection if we are not the owner of the list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LockedList<T> : IEnumerable<T>
    {
        private IList<T> _list = new List<T>();

        private readonly Type _ownerType;

        public LockedList()
        {
            this._ownerType = this.GetCallingType();
        }

        public IList<T> List
        {
            get
            {
                if (Utils.IsSameOrSubclass(this.GetCallingType(), this._ownerType))
                    return this._list;
                return new ReadOnlyCollection<T>(this._list);
            }
            set
            {
                Type t = this.GetCallingType();
                Assert.IsTrue(Utils.IsSameOrSubclass(t, this._ownerType),
                    String.Format("{0} is not the owner ({1})", t, this._ownerType));
                this._list = value;
            }
        }

        private Type GetCallingType()
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame[] frames = stackTrace.GetFrames();
            Type type = null;
            for (var index = 1; index < frames.Length; index++)
            {
                var frame = frames[index];
                type = frame.GetMethod().DeclaringType;
                if (this.GetType() == type ) continue;
                break;
            }
            return type;
        }

        public T this[int i] { get { return this.List[i]; } set { this.List[i] = value; } }

        public int Count(){ return this.List.Count;}

        public void Add(T item) {this.List.Add(item);}

        public void AddRange(IEnumerable<T> items) {((List<T>)this.List).AddRange(items);}


        public void Remove(T item){this.List.Remove(item);}

        public void RemoveAt(int index){this.List.RemoveAt(index);}

        public void Insert(int index,T item){this.List.Insert(index, item);}

        public int IndexOf(T item){return this.List.IndexOf(item);}

        public bool Contains(T item){return this.List.Contains(item);}

        public void Clear() {this.List.Clear();}

        public IEnumerator<T> GetEnumerator(){return this._list.GetEnumerator();}

        IEnumerator IEnumerable.GetEnumerator(){return this.GetEnumerator();}

        
    }
}